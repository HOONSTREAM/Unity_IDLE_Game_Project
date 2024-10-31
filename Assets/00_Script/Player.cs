using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{

    private Vector3 startPos;
    private Quaternion rotation;
    public Character_Scriptable CH_Data;
    public string CH_Name;
    public GameObject Trail_Object;
    [SerializeField]
    private ParticleSystem Provocation_Effect; // 보스가 출현할 때, 머리 상단에 느낌표를 나타냅니다.
    

    protected override void Start()
    {
        base.Start();

        Data_Set(Resources.Load<Character_Scriptable>("Scriptable/" + CH_Name));

        Spawner.m_players.Add(this);
        Base_Manager.Stage.M_ReadyEvent += OnReady;
        Base_Manager.Stage.M_BossEvent += OnBoss;
        Base_Manager.Stage.M_ClearEvent += OnClear;
        Base_Manager.Stage.M_DeadEvent += OnDead;
        startPos = transform.position;
        rotation = transform.rotation;

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
                    Invoke("InitAttack", 1.0f);
                }
            }
        }
         
    }
    private void OnReady()
    {
        isDead = false;
        AnimatorChange("isIDLE");
        Spawner.m_players.Add(this);
        Set_ATK_HP();
        transform.position = startPos;
        transform.rotation = rotation;
    }
    private void OnBoss()
    {
        AnimatorChange("isIDLE");
        Provocation_Effect.Play();
    }
    private void OnClear()
    {
        AnimatorChange("isVICTORY");
    }
    private void OnDead()
    {
        Spawner.m_players.Add(this);
    }
    private void Data_Set(Character_Scriptable datas)
    {
        CH_Data = datas;
        Attack_Range = datas.M_Attack_Range;

        Set_ATK_HP();
    }
    public void Set_ATK_HP()
    {
        ATK = Base_Manager.Player.Get_ATK(CH_Data.Rarity);
        HP = Base_Manager.Player.Get_HP(CH_Data.Rarity);
    }
    public override void GetDamage(double dmg)
    {
        base.GetDamage(dmg);

        if (Stage_Manager.isDead)
        {
            return;
        }

        var goOBJ = Base_Manager.Pool.Pooling_OBJ("HIT_TEXT").Get((value) =>
        {
            value.GetComponent<Hit_Text>().Init(transform.position, dmg, true);

        });

        HP -= dmg;

        if(HP <= 0)
        {
            isDead = true;
            DeadEvent();
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
            transform.position += force * Time.deltaTime;
            yield return null;
        }
    }
}
