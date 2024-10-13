using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Item_OBJ : MonoBehaviour
{
    //Mathf.Deg2Rad : 각도를 호도법(Radian)으로 표현

    [SerializeField]
    private Transform ItemTextRect;
    [SerializeField]
    private TextMeshProUGUI m_Text;
    [SerializeField]
    private float firingAngle = 45.0f;
    [SerializeField]
    private float gravity = 9.8f;
    [SerializeField]
    private ParticleSystem m_Loot;
    [SerializeField]
    private GameObject[] Raritys;

    private Rarity rarity;

    private bool isCheck = false;

    private void RarityCheck()
    {
        isCheck = true;

        transform.rotation = Quaternion.identity; // (0,0,0);

        Raritys[(int)rarity].gameObject.SetActive(true);
        ItemTextRect.gameObject.SetActive(true);
        ItemTextRect.parent = Base_Canvas.instance.Holder_Layer(2);

        m_Text.text = "TEST ITEM";

    }

    private void Update()
    {
        if(isCheck == false) { return; }

        ItemTextRect.position = Camera.main.WorldToScreenPoint(transform.position);
    }
    public void Init(Vector3 pos)
    {
        rarity = (Rarity)Random.Range(0, 5);
        isCheck = false;

        transform.position = pos;

        Vector3 Target_Pos = new Vector3(pos.x + (Random.insideUnitSphere.x * 2.0f), 0.5f , pos.z + (Random.insideUnitSphere.z * 2.0f));
        StartCoroutine(SimulateProjectile(Target_Pos));
    }

    IEnumerator SimulateProjectile(Vector3 pos)
    {
        float target_Distance = Vector3.Distance(transform.position, pos);

        float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);

        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);


        float flight_Duration = target_Distance / Vx;

        transform.rotation = Quaternion.LookRotation(pos - transform.position);

        float time = 0.0f;

        while(time < flight_Duration)
        {
            transform.Translate(0, (Vy - (gravity* time)) * Time.deltaTime, Vx * Time.deltaTime);
            time += Time.deltaTime;

            yield return null;
        }
       
        RarityCheck();
    }
   
}
