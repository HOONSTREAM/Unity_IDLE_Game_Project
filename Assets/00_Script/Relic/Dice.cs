using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class Dice : MonoBehaviour
{
    // textmeshpro- -> ui�� �ƴѰ����� Ȱ���ϴ� text
    // textmeshprougui -> ui���� Ȱ���ϴ� �ؽ�Ʈ
    [SerializeField]
    private TextMeshProUGUI Gold_Text;
    [SerializeField]
    private ParticleSystem particle;
    [SerializeField]
    private GameObject Dice_Object;

    private int RandomValue_Bonus_Gold;

    private void Start()
    {
        StartCoroutine(Gold_blast());
    }
    IEnumerator Gold_blast()
    {
        Dice_Object.gameObject.SetActive(false);

        for (int i = 0; i < 10; i++)
        {
            int RandomValue = Random.Range(1, 7);        
            RandomValue_Bonus_Gold = RandomValue;
            if (!Utils.is_Skill_Effect_Save_Mode)
            {
                Dice_Object.gameObject.SetActive(true);
                Gold_Text.text = RandomValue.ToString();
            }
            yield return new WaitForSeconds(0.1f);
        }

        if (!Utils.is_Skill_Effect_Save_Mode)
        {
            particle.gameObject.SetActive(true);
            yield return new WaitForSeconds(1.0f);
            Destroy(this.gameObject);
        }

        ApplyGoldBonus();
    }

    /// <summary>
    /// �ֻ��� ���� ���� ���ʽ� ��� ������ ����
    /// </summary>
    private void ApplyGoldBonus()
    {
        // Dice �ֻ��� �� ��� ���ʽ� ���� ���� (1:5%, 6:30%)
        float bonusPercentage = GetGoldBonusPercentage(RandomValue_Bonus_Gold);

        // ���� �÷��̾��� ���
        double baseGold = Utils.Data.stageData.Get_DROP_MONEY() *
                          (1 + Base_Manager.Player.Calculate_Gold_Drop_Percentage());

        // �߰� ��� ���ʽ� ����
        double bonusGold = baseGold * (bonusPercentage / 100.0);
        double totalGold = baseGold + bonusGold;

        // �÷��̾� ��忡 �ݿ�
        Data_Manager.Main_Players_Data.Player_Money += totalGold;

        Debug.Log($"�ֻ��� ���: {RandomValue_Bonus_Gold}, �߰� ���ʽ�: {bonusPercentage}%, ���޵� ���: {StringMethod.ToCurrencyString(totalGold)}");
        Main_UI.Instance.Main_UI_PlayerInfo_Text_Check();
    }

    /// <summary>
    /// �ֻ��� ���� ���� ���ʽ� ���� ��ȯ
    /// </summary>
    private float GetGoldBonusPercentage(int diceValue)
    {
        return diceValue * 5f; // 1 = 5%, 2 = 10%, ..., 6 = 30%
    }
}
