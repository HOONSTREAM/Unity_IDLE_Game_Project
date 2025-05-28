using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

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

    private void Start()
    {
        InitEXP();
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
        if (Stage_Manager.M_State == Stage_State.Dead)
        {
            Base_Canvas.instance.Get_Toast_Popup().Initialize("��� ���¿�����, �������� �Ұ��մϴ�.");
            return;
        }

        if (Data_Manager.Main_Players_Data.Player_Money < Utils.Data.levelData.Get_LEVELUP_MONEY())
        {
            Base_Canvas.instance.Get_Toast_Popup().Initialize("��尡 �����մϴ�.");
            return;
        }

        ApplyLevelUp();
    }

    private void ApplyLevelUp()
    {
        double levelUpCost = Utils.Data.levelData.Get_LEVELUP_MONEY();
        Data_Manager.Main_Players_Data.Player_Money -= levelUpCost;
        Data_Manager.Main_Players_Data.Levelup++;
        Base_Manager.Player.EXP_UP();

        InitEXP();
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
        int countPerFrame = 20;

        while (true)
        {
            for (int i = 0; i < countPerFrame; i++)
            {
                double currentCost = Utils.Data.levelData.Get_LEVELUP_MONEY();

                if (Data_Manager.Main_Players_Data.Player_Money < currentCost)
                {
                    Auto_Levelup_Stop_Button.gameObject.SetActive(false);
                    break;
                }
                    
                Data_Manager.Main_Players_Data.Player_Money -= currentCost;
                Data_Manager.Main_Players_Data.Levelup++;
                Base_Manager.Player.EXP_UP();
                levelsGained++;
            }

            double nextCost = Utils.Data.levelData.Get_LEVELUP_MONEY();
            if (Data_Manager.Main_Players_Data.Player_Money < nextCost)              
                break;

            yield return null; // ������ �л�
        }

        

        if (levelsGained > 0)
        {
            InitEXP();
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

    private void InitEXP()
    {
        Main_Exp_Silder.fillAmount = Base_Manager.Player.EXP_Percentage();
        Exp_Text.text = string.Format("{0:0.0}", Base_Manager.Player.EXP_Percentage() * 100.0f) + "%";
        ATK_Text.text = "+" + StringMethod.ToCurrencyString(Utils.Data.levelData.Get_Levelup_Next_ATK());
        HP_Text.text = "+" + StringMethod.ToCurrencyString(Utils.Data.levelData.Get_Levelup_Next_ATK());
        Get_Exp_Text.text = "<color=#00FF00>EXP</color> +" + string.Format("{0:0.0}", Base_Manager.Player.Next_EXP()) + "%";
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
