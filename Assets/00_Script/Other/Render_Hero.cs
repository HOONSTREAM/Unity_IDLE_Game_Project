using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Render_Hero : MonoBehaviour
{
    public GameObject[] Plus_Particles;
    public Transform[] Circles;


    public void Get_Particle(bool m_B)
    {
        for(int i = 0; i<Plus_Particles.Length; i++)
        {
            Plus_Particles[i].gameObject.SetActive(m_B);
        }
    }
}
