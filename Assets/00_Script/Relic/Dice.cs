using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class Dice : MonoBehaviour
{
    // textmeshpro- -> ui가 아닌곳에서 활용하는 text
    // textmeshprougui -> ui에서 활용하는 텍스트
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
    /// 주사위 값에 따라 보너스 골드 비율을 결정
    /// </summary>
    private void ApplyGoldBonus()
    {
        // Dice 주사위 값 기반 보너스 비율 결정 (1:5%, 6:30%)
        float bonusPercentage = GetGoldBonusPercentage(RandomValue_Bonus_Gold);

        // 현재 플레이어의 골드
        double baseGold = Utils.Data.stageData.Get_DROP_MONEY() *
                          (1 + Base_Manager.Player.Calculate_Gold_Drop_Percentage());

        // 추가 골드 보너스 적용
        double bonusGold = baseGold * (bonusPercentage / 100.0);
        double totalGold = baseGold + bonusGold;

        // 플레이어 골드에 반영
        Data_Manager.Main_Players_Data.Player_Money += totalGold;

        Debug.Log($"주사위 결과: {RandomValue_Bonus_Gold}, 추가 보너스: {bonusPercentage}%, 지급된 골드: {StringMethod.ToCurrencyString(totalGold)}");
        Main_UI.Instance.Main_UI_PlayerInfo_Text_Check();
    }

    /// <summary>
    /// 주사위 값에 따른 보너스 비율 반환
    /// </summary>
    private float GetGoldBonusPercentage(int diceValue)
    {
        return diceValue * 5f; // 1 = 5%, 2 = 10%, ..., 6 = 30%
    }
}
