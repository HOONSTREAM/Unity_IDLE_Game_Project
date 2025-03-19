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
    private ParticleSystem Provocation_Effect; // ������ ������ ��, �Ӹ� ��ܿ� ����ǥ�� ��Ÿ���ϴ�.
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
            FindClosetTarget(Spawner.m_monsters.ToArray()); // ����Ʈ�� �迭�� ����ȯ

            if (m_target == null)
            {
                float targetPos = Vector3.Distance(transform.position, startPos);

                if (targetPos > 0.1f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, startPos, Time.deltaTime); // time.deltatime�� speed�� �����ָ� �ӵ��� ������
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
                if (m_target.GetComponent<Character>().isDead) // ���Ͱ� ������ϸ� ���ο� Ÿ���� ã��
                {                   
                    FindClosetTarget(Spawner.m_monsters.ToArray());
                }

                float targetDistance = Vector3.Distance(transform.position, m_target.position);

                if (targetDistance <= target_Range && targetDistance > Attack_Range && isATTACK == false) // ���� Ÿ���� ���� ���� �ȿ��� ������, ���ݹ��� �ȿ��� ���� ��
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

        if (Provocation_Effect != null) //��ȿ���˻�
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
    /// Scriptable Object Data�� �������� �⺻�����͸� �����մϴ�.
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
    /// �� ������ ATK, HP, ATK_SPEED�� �����մϴ�.
    /// Advice : X2_SPEED�� �������� ATK_SPEED�� ���������ʰ�, TimeScale�� �����մϴ�.
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

            MP = 0; // MP �ʱ�ȭ

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
                Spawner.m_players.Remove(this); // �ڽ��� ���̻� �������� ���ϵ��� ����Ʈ���� ����
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
        Spawner.m_players.Remove(this); // �ڽ��� ���̻� �������� ���ϵ��� ����Ʈ���� ����

        if(Spawner.m_players.Count <= 0 && Stage_Manager.isDead == false)
        {
            Base_Manager.Stage.State_Change(Stage_State.Dead);
        }

        AnimatorChange("isDEAD");
        m_target = null;

    }

    /// <summary>
    /// ����ĳ������ �˱⸦ Ȱ��ȭ �մϴ�.
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
    /// Ȱ��ȭ �� �˱⸦ ��Ȱ��ȭ �մϴ�. 
    /// </summary>
    private void TrailDisable() => Trail_Object.gameObject.SetActive(false);

    /// <summary>
    /// ������ ������ ��, ĳ���͸� �˹��Ű�� �޼��� �Դϴ�.
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
