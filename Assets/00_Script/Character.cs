using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Character : MonoBehaviour
{
    private Animator animator;

    public double HP;
    public double ATK;
    public float ATK_Speed;
    public bool isDead = false;
    public bool Use_Skill = false;
    public bool Skill_none_Attack = false;

    protected float Attack_Range = 3.0f; // 공격하는 공격 범위
    protected float target_Range = 5.0f; // 추격하는 범위
    protected bool isATTACK = false;

    [SerializeField]
    protected Transform m_target; // 타겟 객체

    [SerializeField]
    private Transform m_BulletTransform;

    public string Bullet_Name;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
    }

    protected void InitAttack() => isATTACK = false;
    //AnyState는 어떤 상태여도 트리거가 작동되면, 해당 애니메이션으로 갈수 있게끔 한다.
    public void AnimatorChange(string temp)
    {
        if (animator == null)
        {
            Debug.LogError($"Animator is null on {gameObject.name}. Called from AnimatorChange.");
        }
        else if (!animator.gameObject.activeInHierarchy)
        {
            Debug.LogError($"Animator's GameObject is inactive or destroyed on {animator.gameObject.name}. Called from AnimatorChange.");
        }

        if (Skill_none_Attack && Use_Skill)
        {
            return;
        }

        Debug.Log($"변경하려는 에니메이터 오브젝트 : {animator.gameObject.name}");
        animator.SetBool("isIDLE", false);
        animator.SetBool("isMOVE", false);

        if (temp == "isATTACK" || temp == "isVICTORY" || temp == "isDEAD" || temp == "isSKILL")
        {
            if(temp == "isATTACK")
            {
                animator.speed = ATK_Speed;
            }

            animator.SetTrigger(temp);
            return;
        }

        animator.speed = 1.0f;
        animator.SetBool(temp, true);
    }

    /// <summary>
    /// 캐릭터 공격모션 EVENT에서 실행된다.
    /// </summary>
    protected virtual void Bullet()
    {
        if(m_target == null)
        {
            return;
        }

        

        Base_Manager.Pool.Pooling_OBJ("Attack_Helper").Get((value) =>
        {
            value.transform.position = m_BulletTransform.position;
            value.GetComponent<Bullet>().init(m_target, ATK, Bullet_Name);
        });
    }

    protected virtual void Attack()
    {
        if(m_target == null)
        {
            return;
        }

        Base_Manager.Pool.Pooling_OBJ("Attack_Helper").Get((value) =>
        {
            value.transform.position = m_target.transform.position;
            value.GetComponent<Bullet>().Attack_Init(m_target, ATK);
        });
    }

    public virtual void GetDamage(double dmg)
    {

    }

    public virtual void Heal(double heal)
    {
        HP += heal;

        var goOBJ = Base_Manager.Pool.Pooling_OBJ("HIT_TEXT").Get((value) =>
        {
            value.GetComponent<Hit_Text>().Init(transform.position, heal, Color.green, true);

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

            if (monster == null || !monster.gameObject.activeInHierarchy) // 유효성 검사
            {
                continue;
            }


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

    private void OnDestroy()
    {
        if (animator != null)
        {
            Debug.Log($"Animator on {gameObject.name} is being destroyed.");
            animator = null;
        }

    }

}
