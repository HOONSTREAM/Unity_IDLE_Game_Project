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
    /// 원하는 시점에 계속 Init을 시킬수 있다.
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

        if (isDungeon) // 골드던전에 진입했다면, 공격하지않고, 데미지만 받게끔 처리함.
        {
            return;
        }

        if (Stage_Manager.M_State == Stage_State.Play || Stage_Manager.M_State == Stage_State.BossPlay)
        {
            if (m_target == null || !m_target.gameObject.activeInHierarchy)
            {
                FindClosetTarget(Spawner.m_players.ToArray()); // 유효한 타겟 갱신
                return; // 유효하지 않으면 업데이트 종료
            }

            if (m_target != null)
            {
                if (m_target.GetComponent<Character>().isDead)
                {
                    FindClosetTarget(Spawner.m_players.ToArray());
                }

                float targetDistance = Vector3.Distance(transform.position, m_target.position);

                if (targetDistance > Attack_Range && isATTACK == false) // 현재 타겟이 추적 범위 안에는 있지만, 공격범위 안에는 없을 때
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
               
            //한 지점에서 다른 지점으로 일정 속도로 이동할 때 유용하게 사용됩니다.
            //MoveToWards 메서드는 목적지에 도달할 때까지 선형적으로 이동하며, 속도를 조절할 수 있습니다.


            //Vector3.Distance는 Unity에서 두 지점 간의 거리를 계산하는 데 사용되는 메서드입니다.
            //이 메서드는 3D 공간에서 두 개의 Vector3 간의 유클리드 거리(Euclidean Distance)를 반환합니다.

        }

    }

    /// <summary>
    /// 몬스터가 스폰이 될 때, 스케일의 크기변화를 줍니다.
    /// </summary>
    /// <returns></returns>
    IEnumerator Spawn_Start()
    {
        float current = 0.0f;
        float percent = 0.0f;
        float start = 0.0f;
        float end = transform.localScale.x; // 몬스터의 로컬스케일 

        while(percent < 1)
        {
            current += Time.deltaTime;
            percent = current / 0.2f;
            float LerpPos = Mathf.Lerp(start,end, percent); // 선형보간 (시작값,끝값,시간)
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
            Debug.LogWarning("[Monster] GetDamage 호출되었지만 이미 사망 또는 파괴됨");
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
                value.GetComponent<Item_OBJ>().Init(transform.position, Drop_items[i]); // 몬스터 위치 삽입
            });
        }

        if (!isBoss)
        {
            Base_Manager.Pool.m_pool_Dictionary[Utils.GetStage_MonsterPrefab(Data_Manager.Main_Players_Data.Player_Stage)].Return(this.gameObject);           
        }

    }

    /// <summary>
    /// 여러가지 이펙트들을 코루틴으로 리턴시켜주는 메서드 입니다.
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
