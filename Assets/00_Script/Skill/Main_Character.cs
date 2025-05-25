using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_Character : Skill_Base
{
    private float _skill_cool_time = 5.0f;
    private Coroutine _coroutine;
    private void Start()
    {
        Base_Manager.Stage.M_ReadyEvent += OnReady;
    }

    public override void Set_Skill()
    {
        if (this.gameObject.GetComponent<Player>().isDead)
        {
            return;
        }

        gameObject.GetComponent<Speech_Character>().Init();
        m_Player.Use_Skill = true;
        

        var character = HP_Check();
        m_Player.transform.LookAt(character.transform.position);  
        character.Heal(Skill_Damage(500));      
        Skill_Effect.gameObject.SetActive(true);
        Skill_Effect.transform.position = character.transform.position;

        Invoke("ReturnSkill", 1.0f);
        base.Set_Skill();
    }

    public override void ReturnSkill()
    {
        OnReady();
        Skill_Effect.gameObject.SetActive(false);
        base.ReturnSkill();
    }
    public void OnReady()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        _coroutine = StartCoroutine(Skill_Coroutine(_skill_cool_time));
    }
    IEnumerator Skill_Coroutine(float Value)
    {
        float timer = Value;
       
        while (timer > 0.0f)
        {
            timer -= Time.deltaTime;
            Main_UI.Instance.Main_Character_Skill_CoolTime.fillAmount = timer / Value;
            yield return null;
        }

        Set_Skill();      
    }

}
