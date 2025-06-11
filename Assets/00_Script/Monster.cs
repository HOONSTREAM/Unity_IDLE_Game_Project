using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Monster : Character
{
    public float M_Speed;
    public float  R_ATTACK_RANGE;
    private bool isSpawn = false;
    public bool isBoss = false;
    public bool isDPSBoss = false;
    public bool isDungeon = false;
    private Vector3 Original_Scale;
    private double MaxHP;  


    private void Awake()
    {
        Original_Scale = transform.localScale;
    }
    protected override void Start()
    {
        base.Start();

        Base_Manager.Stage.M_DeadEvent += OnDead;
    }

    /// <summary>
    /// ���ϴ� ������ ��� Init�� ��ų�� �ִ�.
    /// </summary>
    public void Init(int Value = 0)
    {
        isDead = false;
        ATK = isBoss ? Utils.Data.stageData.Get_ATK(Value)  * 10.0f : Utils.Data.stageData.Get_ATK(Value);
        HP = isBoss ? Utils.Data.stageData.Get_HP(Value) * 60.0f : Utils.Data.stageData.Get_HP(Value);
        ATK_Speed = 1.0f;
        MaxHP = HP;

        Attack_Range = R_ATTACK_RANGE;
        target_Range = Mathf.Infinity;
        transform.localScale = Original_Scale;

        if (isBoss && isDungeon == false)
        {
            StartCoroutine(Skill_Coroutine());
        }
        
        StartCoroutine(Spawn_Start());

    }

    IEnumerator Skill_Coroutine()
    {
        yield return new WaitForSeconds(3.0f);
        GetComponent<Skill_Base>().Set_Skill();

        StartCoroutine(Skill_Coroutine());
    }

    private void Update()
    {
        if(isSpawn == false)
        {
            return;
        }

        if (isDungeon) // �������� �����ߴٸ�, ���������ʰ�, �������� �ްԲ� ó����.
        {
            return;
        }

        if (Stage_Manager.M_State == Stage_State.Play || Stage_Manager.M_State == Stage_State.BossPlay)
        {
            if (m_target == null || !m_target.gameObject.activeInHierarchy)
            {
                FindClosetTarget(Spawner.m_players.ToArray()); // ��ȿ�� Ÿ�� ����
                return; // ��ȿ���� ������ ������Ʈ ����
            }

            if (m_target != null)
            {
                if (m_target.GetComponent<Character>().isDead)
                {
                    FindClosetTarget(Spawner.m_players.ToArray());
                }

                float targetDistance = Vector3.Distance(transform.position, m_target.position);

                if (targetDistance > Attack_Range && isATTACK == false) // ���� Ÿ���� ���� ���� �ȿ��� ������, ���ݹ��� �ȿ��� ���� ��
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
               
            //�� �������� �ٸ� �������� ���� �ӵ��� �̵��� �� �����ϰ� ���˴ϴ�.
            //MoveToWards �޼���� �������� ������ ������ ���������� �̵��ϸ�, �ӵ��� ������ �� �ֽ��ϴ�.


            //Vector3.Distance�� Unity���� �� ���� ���� �Ÿ��� ����ϴ� �� ���Ǵ� �޼����Դϴ�.
            //�� �޼���� 3D �������� �� ���� Vector3 ���� ��Ŭ���� �Ÿ�(Euclidean Distance)�� ��ȯ�մϴ�.

        }

    }

    /// <summary>
    /// ���Ͱ� ������ �� ��, �������� ũ�⺯ȭ�� �ݴϴ�.
    /// </summary>
    /// <returns></returns>
    IEnumerator Spawn_Start()
    {
        float current = 0.0f;
        float percent = 0.0f;
        float start = 0.0f;
        float end = transform.localScale.x; // ������ ���ý����� 

        while(percent < 1)
        {
            current += Time.deltaTime;
            percent = current / 0.2f;
            float LerpPos = Mathf.Lerp(start,end, percent); // �������� (���۰�,����,�ð�)
            transform.localScale = new Vector3(LerpPos, LerpPos, LerpPos);
            yield return null;
        }

        yield return new WaitForSeconds(0.3f);
        isSpawn = true;
    }
    public override void GetDamage(double dmg)
    {
        if (isDead || this == null || gameObject == null)
        {
            Debug.LogWarning("[Monster] GetDamage ȣ��Ǿ����� �̹� ��� �Ǵ� �ı���");
            return;
        }

        bool critical = Critical_Damage(ref dmg);


        if (!Utils.is_Skill_Effect_Save_Mode)
        {
            Base_Manager.Pool.Pooling_OBJ("HIT_TEXT").Get((value) =>
            {
                if (value == null) return;

                var hitText = value.GetComponent<Hit_Text>();
                if (hitText != null && this != null && transform != null)
                {
                    hitText.Init(transform.position, dmg, Color.white, false, critical);
                }
            });
        }
        

        HP -= dmg;

        if (isBoss)
        {
            Main_UI.Instance.Boss_Slider_Count(HP, MaxHP); 
        }

        if (isDPSBoss)
        {           
            Main_UI.Instance.Boss_Slider_Count(0,0,dmg);
        }

        if(HP <= 0)
        {
            isDead = true;
            Delegate_Holder.Monster_Dead(this);
            Dead_Event();
        }

    }
    private void OnDead()
    {
        StopAllCoroutines();

        if (isDungeon)
        {
            return;
        }

        AnimatorChange("isIDLE");
    }
    private void Dead_Event()
    {
        if (Main_Quest.GetEnemy)
        {
            Main_Quest.monster_index++;
        }

        if (Stage_Manager.isDungeon)
        {
            Stage_Manager.DungeonCount--;
            Main_UI.Instance.Dungeon_Monster_Slider_Count();

            if (isBoss)
            {
                if(Stage_Manager.Dungeon_Enter_Type == 1)
                {
                    Base_Manager.Pool.m_pool_Dictionary["Gold_Dungeon"].Return(this.gameObject);
                    Base_Manager.Stage.State_Change(Stage_State.Dungeon_Clear, Stage_Manager.Dungeon_Enter_Type);
                    Base_Manager.BACKEND.Log_Clear_Dungeon(Stage_Manager.Dungeon_Enter_Type);
                }

                else if (Stage_Manager.Dungeon_Enter_Type == 2)
                {
                    Base_Manager.Pool.m_pool_Dictionary["Enhancement_Dungeon"].Return(this.gameObject);
                    Base_Manager.Stage.State_Change(Stage_State.Dungeon_Clear, Stage_Manager.Dungeon_Enter_Type);
                    Base_Manager.BACKEND.Log_Clear_Dungeon(Stage_Manager.Dungeon_Enter_Type);
                }

                else if (Stage_Manager.Dungeon_Enter_Type == 3)
                {
                    Base_Manager.Pool.m_pool_Dictionary["Tier_Dungeon"].Return(this.gameObject);
                    Base_Manager.Stage.State_Change(Stage_State.Dungeon_Clear, Stage_Manager.Dungeon_Enter_Type);
                    Base_Manager.BACKEND.Log_Clear_Dungeon(Stage_Manager.Dungeon_Enter_Type);
                }

                else if (Stage_Manager.Dungeon_Enter_Type == 4)
                {
                    Base_Manager.Pool.m_pool_Dictionary["DPS_Dungeon"].Return(this.gameObject);
                    Base_Manager.Stage.State_Change(Stage_State.Dungeon_Clear, Stage_Manager.Dungeon_Enter_Type);
                    Base_Manager.BACKEND.Log_Clear_Dungeon(Stage_Manager.Dungeon_Enter_Type);
                }

            }
        }

        else
        {
            if (!isBoss)
            {
                if (!Stage_Manager.isDead)
                {
                    Stage_Manager.Count++;
                    Main_UI.Instance.Monster_Slider_Count();
                }

            }

            else
            {
                Base_Manager.Stage.State_Change(Stage_State.Clear);
            }
                  
        }
        
        Spawner.m_monsters.Remove(this);

        Base_Manager.Pool.Pooling_OBJ("Smoke").Get((value) =>
        {
            value.transform.position = new Vector3(transform.position.x, 1.0f, transform.position.z);
            Base_Manager.instance.Return_Pool(value.GetComponent<ParticleSystem>().duration, value, "Smoke");

        });

        if (Base_Canvas.isSavingMode)
        {
            var reward = Utils.Data.stageData.Get_DROP_MONEY() * (1 + Base_Manager.Player.Calculate_Gold_Drop_Percentage());
            Data_Manager.Main_Players_Data.Player_Money += reward;
        }
        else
        {           
            Base_Manager.Pool.Pooling_OBJ("COIN_PARENT").Get((value) =>
            {
                value.GetComponent<Coin_Parent>().Init(transform.position);
            });
        }

        var Drop_items = Base_Manager.Item.Get_Drop_Set();

        for (int i = 0; i < Drop_items.Count; i++)
        {
            Base_Manager.Pool.Pooling_OBJ("Item_OBJ").Get((value) =>
            {
                value.GetComponent<Item_OBJ>().Init(transform.position, Drop_items[i]); // ���� ��ġ ����
            });
        }

        if (!isBoss)
        {
            Base_Manager.Pool.m_pool_Dictionary[Utils.GetStage_MonsterPrefab(Data_Manager.Main_Players_Data.Player_Stage)].Return(this.gameObject);           
        }

    }

    /// <summary>
    /// �������� ����Ʈ���� �ڷ�ƾ���� ���Ͻ����ִ� �޼��� �Դϴ�.
    /// </summary>
    /// <param name="timer"></param>
    /// <param name="obj"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    IEnumerator ReturnCoroutine(float timer, GameObject obj, string path)
    {
        yield return new WaitForSeconds(timer);
        Base_Manager.Pool.m_pool_Dictionary[path].Return(obj);
    }

    protected virtual bool Critical_Damage(ref double dmg)
    {
        float critical_percentage = Random.Range(0.0f, 100.0f);

        if (critical_percentage <= Base_Manager.Player.Calculate_Critical_Percentage())
        {
            dmg *= (Base_Manager.Player.Calculate_Cri_Damage_Percentage() / 100); 
            return true;
        }

        else
        {
            return false;
        }
    }


    private void OnDestroy()
    {
        StopAllCoroutines();

        if (Spawner.m_monsters.Contains(this))
        {
            Spawner.m_monsters.Remove(this);
        }
    }
}
