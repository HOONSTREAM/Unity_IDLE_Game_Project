using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private Animator animator;

    public double HP;
    public double ATK;
    public float ATK_Speed;
    protected float Attack_Range = 3.0f; // 공격하는 공격 범위
    protected float target_Range = 5.0f; // 추격하는 범위
    protected bool isATTACK = false;

    protected Transform m_target; // 타겟 객체

    [SerializeField]
    private Transform m_BulletTransform;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
    }

    protected void InitAttack() => isATTACK = false;
    //AnyState는 어떤 상태여도 트리거가 작동되면, 해당 애니메이션으로 갈수 있게끔 한다.
    protected void AnimatorChange(string temp)
    {
        if(temp == "isATTACK")
        {
            animator.SetTrigger("isATTACK");
            return;
        }

        animator.SetBool("isIDLE", false);
        animator.SetBool("isMOVE", false);

        animator.SetBool(temp, true);
    }

    protected virtual void Bullet()
    {
        Base_Manager.Pool.Pooling_OBJ("Bullet").Get((value) =>
        {
            value.transform.position = m_BulletTransform.position;
            value.GetComponent<Bullet>().init(m_target, 10, "CH_01");
        });
    }

    /// <summary>
    /// 가장 가까운 객체를 추적한다. (몬스터,플레이어)
    /// </summary>
    protected void FindClosetTarget<T> (T[] targets) where T : Component
    {
        var monsters = targets;
        Transform closetTarget = null; // 가장 가까운 객체를 추적하기 위한 변수
        float maxDistance = target_Range;

        foreach(var monster in monsters)
        {
            float targetDistance = Vector3.Distance(transform.position, monster.transform.position);

            if(targetDistance < maxDistance)
            {
                closetTarget = monster.transform;
                maxDistance = targetDistance;
            }
        }

        m_target = closetTarget;

        if(m_target != null)
        {
            transform.LookAt(m_target.position);
        }
    }

}
