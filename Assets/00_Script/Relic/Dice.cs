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
        particle.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        Destroy(this.gameObject);
       
    }
}
