using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Meteor : MonoBehaviour
{

    [Range(0.0f, 100.0f)]
    public float speed;

    public ParticleSystem Explosion_Particle;
    public Transform Meteor_OBJ;
    //public Transform Circle;

    Transform parentTransform;

    public void Init(double dmg)
    {
        if (parentTransform == null)
        {
            parentTransform = transform.parent;
        }
        transform.parent = null;
        StartCoroutine(Meteor_Coroutine(dmg));
        Base_Manager.Stage.M_ReadyEvent += DisableOBJ;
    }

    void DisableOBJ()
    {
        StopAllCoroutines();
        transform.parent = parentTransform;
        Meteor_OBJ.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
        Base_Manager.Stage.M_ReadyEvent -= DisableOBJ;
    }

    IEnumerator Meteor_Coroutine(double dmg)
    {
        if (!Utils.is_Skill_Effect_Save_Mode)
        {
            Meteor_OBJ.localPosition = new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(10.0f, 15.0f), Random.Range(5.0f, 10.0f));
            Meteor_OBJ.gameObject.SetActive(true);
            Meteor_OBJ.LookAt(transform.parent);
        }
       

      
        while (true)
        {
            if (!Utils.is_Skill_Effect_Save_Mode)
            {
                float distance = Vector3.Distance(Meteor_OBJ.localPosition, Vector3.zero);

                if (distance >= 0.1f)
                {
                    Meteor_OBJ.localPosition = Vector3.MoveTowards(Meteor_OBJ.localPosition, Vector3.zero, Time.deltaTime * speed);
                    float ScaleValue = distance / speed;
                    // Mathf.Min(float, float) - 두 값 중에 더욱 작은 값을 반환
                    // renderer.color = new Color(0, 0, 0, Mathf.Min((distance / speed), 0.5f));
                    // Circle.localScale = new Vector3(ScaleValue, ScaleValue, ScaleValue);
                    yield return null;
                }
                else
                {
                    Explosion_Particle.Play();
                    //Camera_Manager.instance.Camera_Shake();
                    for (int i = 0; i < Spawner.m_monsters.Count; i++)
                    {
                        if (Vector3.Distance(transform.position, Spawner.m_monsters[i].transform.position) <= 1.5f)
                        {
                            Spawner.m_monsters[i].GetDamage(dmg);
                        }
                    }
                    break;
                }
            }

            else
            {
                for (int i = 0; i < Spawner.m_monsters.Count; i++)
                {
                    if (Vector3.Distance(transform.position, Spawner.m_monsters[i].transform.position) <= 1.5f)
                    {
                        Spawner.m_monsters[i].GetDamage(dmg);
                    }
                }
                break;
            }
            
        }
        yield return new WaitForSeconds(0.5f);
        transform.parent = parentTransform;
        Meteor_OBJ.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }
}


