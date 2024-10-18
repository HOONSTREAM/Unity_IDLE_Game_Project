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

        if(m_Target != null)
        {
            m_Target.GetComponent<Character>().GetDamage(dmg);
        }

        GetHit = true;
        Attack_Particle.Play();
        StartCoroutine(ReturnObject(Attack_Particle.duration));
    }

    public void init(Transform target, double dmg, string character_Name)
    {
        m_Target = target;
        transform.LookAt(m_Target);
        GetHit = false;
        m_TargetPos = m_Target.position;

        m_DMG = dmg;

        m_Character_Name = character_Name;
        m_projectiles[m_Character_Name].gameObject.SetActive(true);
    }


    private void Update()
    {
        if (GetHit)
        {
            return;
        }
        m_TargetPos.y = 0.5f; // 투사체의 y축을 올려줌.

        transform.position = Vector3.MoveTowards(transform.position, m_TargetPos, Time.deltaTime * m_Speed);

        if(Vector3.Distance(transform.position, m_TargetPos) <= 0.1f)
        {
            if (m_Target != null)
            {
                GetHit = true; 
                m_Target.GetComponent<Character>().GetDamage(m_DMG);
                m_projectiles[m_Character_Name].gameObject.SetActive(false);
                m_Muzzles[m_Character_Name].Play();

                StartCoroutine(ReturnObject(m_Muzzles[m_Character_Name].duration));
            }
        }
    }

    IEnumerator ReturnObject(float timer)
    {
        yield return new WaitForSeconds(timer);
        Base_Manager.Pool.m_pool_Dictionary["Attack_Helper"].Return(this.gameObject);
    }
}
