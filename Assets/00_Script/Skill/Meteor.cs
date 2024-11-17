using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    [Range(0.0f, 100.0f)]
    public float Speed; // 지정된 위치까지 이동하는 속도

    public ParticleSystem Explosion_Particle; // 폭발 효과
    public Transform Meteor_Object;
    public Transform Circle;


    private Transform ParentTransform;

    public void Init(double DMG)
    {
        if(ParentTransform == null)
        {
            ParentTransform = transform.parent;
        }

        transform.parent = null;

        StartCoroutine(Meteor_Coroutine(DMG));
    }

    IEnumerator Meteor_Coroutine(double dmg)
    {
        Meteor_Object.localPosition = new Vector3(Random.Range(10.0f, 15.0f), Random.Range(5.0f, 10.0f));
        Meteor_Object.gameObject.SetActive(true);
        Meteor_Object.LookAt(transform.parent);

        Circle.localScale = Vector3.one;
        SpriteRenderer renderer = Circle.GetComponent<SpriteRenderer>();

        while (true)
        {
            float distance = Vector3.Distance(Meteor_Object.localPosition, Vector3.zero);

            if(distance >= 0.1f)
            {
                Meteor_Object.localPosition = Vector3.MoveTowards(Meteor_Object.localPosition, Vector3.zero, Time.deltaTime * Speed);
                float ScaleValue = distance / Speed;
                renderer.color = new Color(0, 0, 0, Mathf.Min((distance / Speed), 0.5f));
                Circle.localScale = new Vector3(ScaleValue, ScaleValue, ScaleValue);
                yield return null;
            }

            else
            {
                Explosion_Particle.Play();
                Camera_Manager.instance.Camera_Shake();

                for(int i = 0; i<Spawner.m_monsters.Count; i++)
                {
                    if(Vector3.Distance(transform.position, Spawner.m_monsters[i].transform.position) <= 1.5f)
                    {
                        Spawner.m_monsters[i].GetDamage(dmg);
                    }
                }

                break;
            }
        }

        yield return new WaitForSeconds(0.5f);
        transform.parent = ParentTransform;
        Meteor_Object.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
