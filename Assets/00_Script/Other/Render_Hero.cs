using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Render_Hero : MonoBehaviour
{
    public GameObject[] Plus_Particles;
    public Transform[] Circles;
    public bool[] Get_Character = new bool[6];
    public Transform Pivot;

    public void Get_Particle(bool m_B)
    {
        for(int i = 0; i<Plus_Particles.Length; i++)
        {
            Plus_Particles[i].gameObject.SetActive(m_B);
        }
    }

    public void Init_Hero()
    {
        for(int i = 0; i< Base_Manager.Character.Set_Character.Length; i++)
        {
            if (Base_Manager.Character.Set_Character[i] != null && Get_Character[i] == false)
            {
                Get_Character[i] = true;
                string temp = Base_Manager.Character.Set_Character[i].Data.M_Character_Name;

                var go = Instantiate(Resources.Load<GameObject>("Character/" + temp));
                go.transform.rotation = Quaternion.identity;
                
                for(int j = 0; j < go.transform.childCount; j++)
                {
                    go.transform.GetChild(j).gameObject.layer = LayerMask.NameToLayer("Render_Layer");
                }

                go.transform.parent = transform;
                go.transform.position = Circles[i].transform.position;
                go.transform.LookAt(Pivot.position);
                go.GetComponent<Player>().enabled = false;
                
            }
        }
    }
}
