using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class LevelUp_Button : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private Image Main_Exp_Silder;
    [SerializeField]
    private TextMeshProUGUI Exp_Text, ATK_Text, HP_Text, Gold_Text, Get_Exp_Text;
    [SerializeField]
    private GameObject Auto_Levelup_Stop_Button;

    private bool isPush = false;
    private bool isLongPush = false;
    private float holdDuration = 0f;
    private float repeatTimer = 0f;

    private bool notified_3 = false;
    private bool notified_2 = false;
    private bool notified_1 = false;

    private Coroutine levelUpCoroutine;
    public static Action Pressed_Levelup_Button_Tutorial;

    private void Start()
    {
        Auto_Levelup_Stop_Button.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!isPush) return;

        holdDuration += Time.deltaTime;

        // 1�� �̻� ������ ���� �������� Exp_Up()
        if (holdDuration >= 1f && !isLongPush)
        {
            repeatTimer += Time.deltaTime;

            if (repeatTimer >= 0.01f)
            {
                repeatTimer = 0f;
                Exp_Up();
            }
        }

        // �ȳ� �޽���
        if (holdDuration >= 7f && !notified_3)
        {
            Base_Canvas.instance.Get_MainGame_Error_UI().Initialize("��� ������ ��, 3�� �ڿ� ��� ��带 �Һ��� �� ����, �ڵ������� �մϴ�.");
            notified_3 = true;
        }
        else if (holdDuration >= 8f && !notified_2)
        {
            Base_Canvas.instance.Get_MainGame_Error_UI().Initialize("��� ������ ��, 2�� �ڿ� ��� ��带 �Һ��� �� ����, �ڵ������� �մϴ�.");
            notified_2 = true;
        }
        else if (holdDuration >= 9f && !notified_1)
        {
            Base_Canvas.instance.Get_MainGame_Error_UI().Initialize("��� ������ ��, 1�� �ڿ� ��� ��带 �Һ��� �� ����, �ڵ������� �մϴ�.");
            notified_1 = true;
        }

        // 10�� �̻� ���� �� ��� ��� �Ҹ��Ͽ� ��� ������
        if (holdDuration >= 10f && !isLongPush)
        {
            isLongPush = true;
            LevelUpAllAtOnce();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isLongPush)
        {
            isPush = false;
            isLongPush = false;
            return;
        }

        holdDuration = 0f;
        repeatTimer = 0f;
        notified_1 = false;
        notified_2 = false;
        notified_3 = false;
        isPush = true;

        Exp_Up(); // ù Ŭ�� 1ȸ ����
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPush = false;
        isLongPush = false;
        holdDuration = 0f;
        repeatTimer = 0f;
    }

    public void Exp_Up()
    {       
        if (Utils.is_Tutorial)
        {
            Pressed_Levelup_Button_Tutorial?.Invoke();
        }

        if (Stage_Manager.M_State == Stage_State.Dead)
        {
            Base_Canvas.instance.Get_Toast_Popup().Initialize("��� ���¿�����, �������� �Ұ��մϴ�.");
            return;
        }

        
        ApplyLevelUp();
    }

    private void ApplyLevelUp()
    {
        double unitCost = Utils.Data.levelData.Get_LEVELUP_MONEY();  // 1ȸ Ŭ���� ��� ���
        Debug.Log($"{unitCost}�� 1ȸ Ŭ���� ��� ���");
        double unitExp = Utils.Data.levelData.Get_EXP();             // 1ȸ Ŭ���� �����ϴ� EXP
        Debug.Log($"{unitExp}�� 1ȸ Ŭ���� �����ϴ� EXP");
        double maxExp = Utils.Data.levelData.Get_MAXEXP();           // ��ü EXP (100%)
        Debug.Log($"{maxExp}�� ��ü EXP");

        double totalCost = unitCost * (maxExp / unitExp);            // �� �ʿ� ���
        Debug.Log($"{totalCost}�� ������ �ʿ� ���");

        if (Data_Manager.Main_Players_Data.Player_Money < totalCost)
        {
            Base_Canvas.instance.Get_Toast_Popup().Initialize("��尡 �����մϴ�.");
            return;
        }

        Data_Manager.Main_Players_Data.Player_Money -= totalCost;
        Data_Manager.Main_Players_Data.Levelup++;
        Data_Manager.Main_Players_Data.Player_Level++;

        // ATK / HP ����
        Data_Manager.Main_Players_Data.ATK = Utils.Data.levelData.Get_Levelup_Next_ATK();
        Data_Manager.Main_Players_Data.HP = Utils.Data.levelData.Get_Levelup_Next_HP();

        Main_UI.Instance.Main_UI_PlayerInfo_Text_Check();
        transform.DORewind();
        transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.25f);
    }

    private void LevelUpAllAtOnce()
    {
        if (Stage_Manager.M_State == Stage_State.Dead)
        {
            Base_Canvas.instance.Get_Toast_Popup().Initialize("��� ���¿�����, �������� �Ұ��մϴ�.");
            return;
        }

        if (levelUpCoroutine != null)
        {
            StopCoroutine(levelUpCoroutine);
        }

        Base_Canvas.instance.Get_MainGame_Error_UI().Initialize("�ڵ� �������� �����մϴ�. �ߴ��Ϸ��� ������ �ߴ� ��ư�� ��������. ");
        Base_Manager.Player.isAutoLeveling = true;

        levelUpCoroutine = StartCoroutine(LevelUpAllCoroutine());
        Auto_Levelup_Stop_Button.gameObject.SetActive(true);
    }

    private IEnumerator LevelUpAllCoroutine()
    {
        int levelsGained = 0;

        while (true)
        {
            double unitCost = Utils.Data.levelData.Get_LEVELUP_MONEY();
            double maxExp = Utils.Data.levelData.Get_MAXEXP();
            double totalCost = unitCost * maxExp;

            if (Data_Manager.Main_Players_Data.Player_Money < totalCost)
                break;

            Data_Manager.Main_Players_Data.Player_Money -= totalCost;
            Data_Manager.Main_Players_Data.Levelup++;
            Data_Manager.Main_Players_Data.Player_Level++;

            // ATK/HP ����
            Data_Manager.Main_Players_Data.ATK = Utils.Data.levelData.Get_Levelup_Next_ATK();
            Data_Manager.Main_Players_Data.HP = Utils.Data.levelData.Get_Levelup_Next_HP();

            levelsGained++;
            yield return null; // ������ �л�
        }

        if (levelsGained > 0)
        {
            Main_UI.Instance.Main_UI_PlayerInfo_Text_Check();
            transform.DORewind();
            transform.DOPunchScale(new Vector3(0.4f, 0.4f, 0.4f), 0.4f);
        }
        else
        {
            Base_Canvas.instance.Get_Toast_Popup().Initialize("������ ������ ��尡 �����ϴ�.");
        }

        levelUpCoroutine = null;
    }

    public void StopAutoLevelUp()
    {
        if (levelUpCoroutine != null)
        {
            StopCoroutine(levelUpCoroutine);
            levelUpCoroutine = null;
            Base_Manager.Player.isAutoLeveling = false;
            Base_Canvas.instance.Get_MainGame_Error_UI().Initialize("�ڵ� �������� �������� �ߴ��߽��ϴ�.");
            Auto_Levelup_Stop_Button.gameObject.SetActive(false);
        }

        Auto_Levelup_Stop_Button.gameObject.SetActive(false);
    }
}
