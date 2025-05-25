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

    public void Init(Vector3 pos, double dmg, Color color, bool isMonster = false, bool Critical = false)
    {

        Saving_Mode.onSaving += OnSave;

        pos.x += Random.Range(-0.1f, 0.1f);
        pos.z += Random.Range(-0.1f ,0.1f);

        Target = pos;
        m_Text.text = StringMethod.ToCurrencyString(dmg);
        transform.parent = Base_Canvas.instance.Holder_Layer(1);

        m_Text.color = color;

        m_critical.gameObject.SetActive(Critical);

       
        Base_Manager.instance.Return_Pool(2.0f, this.gameObject, "HIT_TEXT");
    }

    private void OnDisable()
    {
        Saving_Mode.onSaving -= OnSave;
    }
    public void OnSave()
    {
        ReturnText();
    }

    private void Update()
    {

        if (Base_Canvas.isSavingMode) { return;}

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
