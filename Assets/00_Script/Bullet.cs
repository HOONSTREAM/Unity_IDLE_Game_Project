using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float m_Speed;

    private Transform m_Target;
    private Vector3 m_TargetPos;
    private double m_DMG;
    private string m_Character_Name;
    private bool GetHit = false;

    // 캐싱
    private Character _targetCharacter;
    private GameObject activeProjectile;
    private ParticleSystem activeMuzzle;

    private Dictionary<string, GameObject> m_projectiles = new Dictionary<string, GameObject>();
    private Dictionary<string, ParticleSystem> m_Muzzles = new Dictionary<string, ParticleSystem>();

    public ParticleSystem Attack_Particle;
    private void Awake()
    {
        Transform projectiles = transform.GetChild(0);
        Transform muzzles = transform.GetChild(1);

        for(int i = 0; i< projectiles.childCount; i++)
        {
            m_projectiles.Add(projectiles.GetChild(i).name, projectiles.GetChild(i).gameObject);
        }

        for(int i = 0; i< muzzles.childCount; i++)
        {
            m_Muzzles.Add(muzzles.GetChild(i).name, muzzles.GetChild(i).GetComponent<ParticleSystem>());
        }
    }

    public void Attack_Init(Transform target, double dmg)
    {
        m_Target = target;
        _targetCharacter = m_Target?.GetComponent<Character>();

        _targetCharacter?.GetDamage(dmg);

        GetHit = true;
        Attack_Particle.Play();
        StartCoroutine(ReturnObject(Attack_Particle.main.duration));
    }

    public void init(Transform target, double dmg, string character_Name)
    {
        m_Target = target;
        _targetCharacter = m_Target?.GetComponent<Character>();
        m_TargetPos = m_Target != null ? m_Target.position : Vector3.zero;

        m_DMG = dmg;
        m_Character_Name = character_Name;
        GetHit = false;

        // 캐싱
        if (m_projectiles.TryGetValue(m_Character_Name, out var proj))
            activeProjectile = proj;
        else
            Debug.LogWarning($"Projectile not found: {m_Character_Name}");

        if (m_Muzzles.TryGetValue(m_Character_Name, out var muzzle))
            activeMuzzle = muzzle;
        else
            Debug.LogWarning($"Muzzle not found: {m_Character_Name}");

        if (!Utils.is_Skill_Effect_Save_Mode && activeProjectile != null)
            activeProjectile.SetActive(true);

        transform.LookAt(m_Target);

    }


    private void Update()
    {
        if (GetHit || m_Target == null)
        {
            // 절전모드 후 타겟이 없어진 경우 처리
            if (!GetHit)
            {
                Debug.Log("Bullet auto-returned due to missing target.");
                StartCoroutine(ReturnObject(0f));
            }
            return;
        }


        m_TargetPos.y = 0.5f; // 투사체의 y축을 올려줌.

        transform.position = Vector3.MoveTowards(transform.position, m_TargetPos, Time.deltaTime * m_Speed);

        if (Vector3.Distance(transform.position, m_TargetPos) <= 0.1f)
        {
            GetHit = true;

            if (_targetCharacter != null)
                _targetCharacter.GetDamage(m_DMG);

            if (activeProjectile != null)
                activeProjectile.SetActive(false);

            if (activeMuzzle != null)
                activeMuzzle.Play();

            StartCoroutine(ReturnObject(activeMuzzle != null ? activeMuzzle.main.duration : 0.2f));
        }

    }

    IEnumerator ReturnObject(float timer)
    {
        yield return new WaitForSeconds(timer);

        if (!Base_Manager.Pool.m_pool_Dictionary.TryGetValue("Attack_Helper", out var pool))
        {
            Debug.LogWarning("Attack_Helper 풀에 접근 실패 - Bullet 강제 Destroy");
            Destroy(this.gameObject); // 안전망
            yield break;
        }

        pool.Return(this.gameObject);
    }
}
