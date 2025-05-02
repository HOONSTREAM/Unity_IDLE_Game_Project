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

    private void Start()
    {
        StartCoroutine(Gold_blast());
    }
    IEnumerator Gold_blast()
    {
        for(int i = 0; i < 10; i++)
        {
            int RandomValue = Random.Range(1, 7);
            Gold_Text.text = RandomValue.ToString();
            yield return new WaitForSeconds(0.1f);
        }

        ApplyGoldBonus();

        particle.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        Destroy(this.gameObject);  
    }

    /// <summary>
    /// �ֻ��� ���� ���� ���ʽ� ��� ������ ����
    /// </summary>
    private void ApplyGoldBonus()
    {
        // Dice �ֻ��� �� ��� ���ʽ� ���� ���� (1:5%, 6:30%)
        float bonusPercentage = GetGoldBonusPercentage(int.Parse(Gold_Text.text));

        // ���� �÷��̾��� ���
        double baseGold = Utils.Data.stageData.Get_DROP_MONEY() *
                          (1 + Base_Manager.Player.Calculate_Gold_Drop_Percentage());

        // �߰� ��� ���ʽ� ����
        double bonusGold = baseGold * (bonusPercentage / 100.0);
        double totalGold = baseGold + bonusGold;

        // �÷��̾� ��忡 �ݿ�
        Data_Manager.Main_Players_Data.Player_Money += totalGold;

        Debug.Log($"�ֻ��� ���: {Gold_Text.text}, �߰� ���ʽ�: {bonusPercentage}%, ���޵� ���: {StringMethod.ToCurrencyString(totalGold)}");
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
