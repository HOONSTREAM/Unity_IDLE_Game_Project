using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{

    private Vector3 startPos;
    private Quaternion rotation;
    public Character_Scriptable CH_Data;
    public string CH_Name;
    public int MP;
    public GameObject Trail_Object;
    [SerializeField]
    private ParticleSystem Provocation_Effect; // 보스가 출현할 때, 머리 상단에 느낌표를 나타냅니다.
    public bool isMainCharacter = false;
    
    protected override void Start()
    {
        base.Start();

        Data_Set(Resources.Load<Character_Scriptable>("Scriptable/Character/" + CH_Name));

        Spawner.m_players.Add(this);
        Base_Manager.Stage.M_PlayEvent += OnReady;
        Base_Manager.Stage.M_BossEvent -= OnBoss;
        Base_Manager.Stage.M_BossEvent += OnBoss;
        Base_Manager.Stage.M_ClearEvent += OnClear;
        Base_Manager.Stage.M_DeadEvent += OnDead;
        Base_Manager.Stage.M_DungeonEvent += OnDungeon;
        Base_Manager.Stage.M_DungeonClearEvent += OnDungeonClear;
        
     
        startPos = transform.position;
        rotation = transform.rotation;

        Main_UI.Instance.Character_State_Check(this);

    }

    private void Update()
    {
       
        if (isDead)
        {
            return;
        }

        if (Stage_Manager.M_State == Stage_State.Play || Stage_Manager.M_State == Stage_State.BossPlay)
        {
            FindClosetTarget(Spawner.m_monsters.ToArray()); // 리스트를 배열로 형변환

            if (m_target == null)
            {
                float targetPos = Vector3.Distance(transform.position, startPos);

                if (targetPos > 0.1f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, startPos, Time.deltaTime); // time.deltatime에 speed를 곱해주면 속도가 빨라짐
                    transform.LookAt(startPos);
                    AnimatorChange("isMOVE");
                }

                else
                {
                    transform.rotation = rotation;
                    AnimatorChange("isIDLE");
                }

            }

            else
            {
                if (m_target.GetComponent<Character>().isDead) // 몬스터가 사망을하면 새로운 타겟을 찾기
                {                   
                    FindClosetTarget(Spawner.m_monsters.ToArray());
                }

                float targetDistance = Vector3.Distance(transform.position, m_target.position);

                if (targetDistance <= target_Range && targetDistance > Attack_Range && isATTACK == false) // 현재 타겟이 추적 범위 안에는 있지만, 공격범위 안에는 없을 때
                {
                    AnimatorChange("isMOVE");
                    transform.LookAt(m_target.position);
                    transform.position = Vector3.MoveTowards(transform.position, m_target.transform.position, Time.deltaTime);
                }

                else if (targetDistance <= Attack_Range && isATTACK == false)
                {
                    isATTACK = true;
                    AnimatorChange("isATTACK");                    
                    Get_MP(5);
                    Invoke("InitAttack", (1.0f / ATK_Speed));
                }
            }
        }
         
    }
    private void OnReady()
    {   
        if(this != null)
        {
            isDead = false;

            if (!Spawner.m_players.Contains(this))
            {
                Spawner.m_players.Add(this);
            }

            AnimatorChange("isIDLE");

            Set_ATK_HP_Sub_Hero();
            transform.position = startPos;
            transform.rotation = rotation;

            return;
        }
     
    }

    private void OnBoss()
    {     
        AnimatorChange("isIDLE");

        if (Provocation_Effect != null) //유효성검사
        {
            Provocation_Effect.Play();
        }
    }
       
    private void OnClear()
    {
        AnimatorChange("isVICTORY");
    }

    private void OnDead()
    {
        Spawner.m_players.Add(this);
    }

    private void OnDungeon(int Value)
    {
        if(this != null)
        {
            isDead = false;
            AnimatorChange("isIDLE");
            Spawner.m_players.Add(this);
            Set_ATK_HP_Sub_Hero();
            transform.position = startPos;
            transform.rotation = rotation;
        }
        
    }

    private void OnDungeonClear(int Value)
    {
        OnClear();
    }

    /// <summary>
    /// Scriptable Object Data를 바탕으로 기본데이터를 세팅합니다.
    /// </summary>
    /// <param name="datas"></param>
    /// 
    private void Data_Set(Character_Scriptable datas)
    {
        CH_Data = datas;
        Bullet_Name = CH_Data.Character_EN_Name;
        Attack_Range = datas.M_Attack_Range;
        ATK_Speed = datas.M_Attack_Speed;

        Set_ATK_HP_Sub_Hero();
    }

    /// <summary>
    /// 각 영웅별 ATK, HP, ATK_SPEED를 세팅합니다.
    /// Advice : X2_SPEED는 직접적인 ATK_SPEED를 수정하지않고, TimeScale을 수정합니다.
    /// </summary>
    public void Set_ATK_HP_Sub_Hero()
    {
        ATK = Base_Manager.Player.Get_ATK(CH_Data.Rarity, Base_Manager.Data.character_Holder[CH_Data.name]);
        HP = Base_Manager.Player.Get_HP(CH_Data.Rarity, Base_Manager.Data.character_Holder[CH_Data.name]);
        ATK_Speed = CH_Data.M_Attack_Speed + Base_Manager.Player.Calculate_Atk_Speed_Percentage();
    }

    public void Get_MP(int mp)
    {
        if (Use_Skill)
        {
            return;
        }

        if (isMainCharacter)
        {
            return;
        }

        
        MP += mp;

        if(MP >= CH_Data.MAX_MP)
        {

            MP = 0; // MP 초기화

            if (GetComponent<Skill_Base>() != null)
            {              
                GetComponent<Skill_Base>().Set_Skill();
            }

            Use_Skill = true;
          
        }

        Main_UI.Instance.Character_State_Check(this);

    }

    public override void GetDamage(double dmg)
    {
        if(HP <= 0 && Stage_Manager.isDungeon)
        {
            return;
        }

        base.GetDamage(dmg);

        if (Stage_Manager.isDead)
        {
            return;
        }

        Delegate_Holder.Player_hit(this);
        Get_MP(10);

        var goOBJ = Base_Manager.Pool.Pooling_OBJ("HIT_TEXT").Get((value) =>
        {
            value.GetComponent<Hit_Text>().Init(transform.position, dmg, Color.red, true);

        });

        HP -= dmg;

        if(HP <= 0)
        {
            if (Stage_Manager.isDungeon)
            {
                Spawner.m_players.Remove(this); // 자신을 더이상 추적하지 못하도록 리스트에서 삭제
                AnimatorChange("isDEAD");
                m_target = null;
                Base_Manager.Stage.State_Change(Stage_State.Dungeon_Dead);
                return;
            }

            else
            {
                isDead = true;
                DeadEvent();
            }

        }
    }
    private void DeadEvent()
    {
        Spawner.m_players.Remove(this); // 자신을 더이상 추적하지 못하도록 리스트에서 삭제

        if(Spawner.m_players.Count <= 0 && Stage_Manager.isDead == false)
        {
            Base_Manager.Stage.State_Change(Stage_State.Dead);
        }

        AnimatorChange("isDEAD");
        m_target = null;

    }

    /// <summary>
    /// 근접캐릭터의 검기를 활성화 합니다.
    /// </summary>
    protected override void Attack()
    {
        base.Attack();
        Trail_Object.gameObject.SetActive(true);

        if(m_target != null)
        {
            Delegate_Holder.Player_Attack(this, m_target.GetComponent<Monster>());
        }
        
        Invoke("TrailDisable", 1.0f);
    }

    /// <summary>
    /// 활성화 된 검기를 비활성화 합니다. 
    /// </summary>
    private void TrailDisable() => Trail_Object.gameObject.SetActive(false);

    /// <summary>
    /// 보스가 출현할 때, 캐릭터를 넉백시키는 메서드 입니다.
    /// </summary>
    public void Knock_Back()
    {
        StartCoroutine(Knock_Back_Coroutine(5.0f, 0.3f));
    }

    IEnumerator Knock_Back_Coroutine(float power, float duration)
    {
        float t = duration;
        Vector3 force = this.transform.forward * -power;
        force.y = 0f;

        while (t > 0f)
        {
            t -= Time.deltaTime;
            if (Vector3.Distance(Vector3.zero, transform.position) < 3.0f)
            {
                transform.position += force * Time.deltaTime;
            }
            yield return null;
        }
    }


    private void OnDestroy()
    {
        StopAllCoroutines();

        if (Spawner.m_players.Contains(this))
        {
            Spawner.m_players.Remove(this);            
        }
    }
}
