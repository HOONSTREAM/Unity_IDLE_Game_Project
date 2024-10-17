using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    private Vector3 startPos;
    private Quaternion rotation;
    public Character_Scriptable CH_Data;
    public string CH_Name;

    protected override void Start()
    {
        base.Start();

        Data_Set(Resources.Load<Character_Scriptable>("Scriptable/" + CH_Name));

        Spawner.m_players.Add(this);
            
        startPos = transform.position;
        rotation = transform.rotation;

    }

    private void Data_Set(Character_Scriptable datas)
    {
        CH_Data = datas;
        Attack_Range = datas.M_Attack_Range;
        
    }

    private void Update()
    {
        FindClosetTarget(Spawner.m_monsters.ToArray()); // 리스트를 배열로 형변환

        if (m_target == null)
        {
            
            float targetPos = Vector3.Distance(transform.position, startPos);

            if(targetPos > 0.1f)
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

            return;
        }

        if (m_target.GetComponent<Character>().isDead)
        {
            FindClosetTarget(Spawner.m_monsters.ToArray());
        }

        float targetDistance = Vector3.Distance(transform.position, m_target.position);

        if(targetDistance <= target_Range && targetDistance > Attack_Range && isATTACK == false) // 현재 타겟이 추적 범위 안에는 있지만, 공격범위 안에는 없을 때
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

    public override void GetDamage(double dmg)
    {
        base.GetDamage(dmg);

        var goOBJ = Base_Manager.Pool.Pooling_OBJ("HIT_TEXT").Get((value) =>
        {
            value.GetComponent<Hit_Text>().Init(transform.position, dmg);

        });

        HP -= dmg;
    }

}
