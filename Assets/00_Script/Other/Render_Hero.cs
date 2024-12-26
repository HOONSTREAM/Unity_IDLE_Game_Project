using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Render_Hero : MonoBehaviour
{
    public GameObject[] Plus_Particles;
    public Transform[] Circles;
    public Transform Pivot;
    private List<GameObject> Character_OBJ = new List<GameObject>(); // RenderOBJ에서 생성된 캐릭터에 대한 정보를 갖고 있음.
    public void Get_Particle(bool m_B)
    {
        for(int i = 0; i<Plus_Particles.Length; i++)
        {
            Plus_Particles[i].gameObject.SetActive(m_B);
        }
    }

    public void Init_Hero()
    {
        for(int i = 0; i < Character_OBJ.Count; i++)
        {            
            Destroy(Character_OBJ[i]);
        }

        Character_OBJ.Clear();

        for(int i = 0; i< Base_Manager.Character.Set_Character.Length; i++)
        {
            if (Base_Manager.Character.Set_Character[i] != null)
            {
                
                string temp = Base_Manager.Character.Set_Character[i].Data.M_Character_Name;

                var go = Instantiate(Resources.Load<GameObject>("Character/" + temp));
                go.transform.rotation = Quaternion.identity;
                Character_OBJ .Add(go);

                go.transform.parent = transform;
                go.GetComponent<Player>().enabled = false;
                go.transform.position = Circles[i].transform.position;
                go.transform.LookAt(Pivot.position);
                
                
            }
        }
    }
}
