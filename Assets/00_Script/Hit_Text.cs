using DamageNumbersPro;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Hit_Text : MonoBehaviour
{
    private Vector3 Target;
    private Camera cam;
    public TextMeshProUGUI m_Text;
    [SerializeField]
    private GameObject m_critical;

    [Range(0.0f, 5.0f)]
    [SerializeField]
    private float UpRange = 0.0f;

    private void Start()
    {
        cam = Camera.main;

    }

    public void Init(Vector3 pos, double dmg, bool isMonster = false, bool Critical = false)
    {
        pos.x += Random.Range(-0.1f, 0.1f);
        pos.z += Random.Range(-0.1f ,0.1f);

        Target = pos;
        m_Text.text = StringMethod.ToCurrencyString(dmg);
        transform.parent = Base_Canvas.instance.Holder_Layer(1);

        if (isMonster)
        {
            m_Text.color = Color.red;
        }
        else
        {
            m_Text.color = Color.white;
        }

        m_critical.gameObject.SetActive(Critical);

        Base_Manager.instance.Return_Pool(2.0f, this.gameObject, "HIT_TEXT");
    }

    private void Update()
    {
        Vector3 targetpos = new Vector3(Target.x, Target.y + UpRange, Target.z);
        transform.position = cam.WorldToScreenPoint(targetpos);

        if(UpRange <= 0.3f)
        {
            UpRange += Time.deltaTime;
        }

    }

    private void ReturnText()
    {
        Base_Manager.Pool.m_pool_Dictionary["HIT_TEXT"].Return(this.gameObject);
    }
   
}
