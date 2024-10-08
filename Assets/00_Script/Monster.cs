using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public float M_Speed;
    private Animator animator;

    bool isSpawn = false;

    private void Start()
    {
        animator = GetComponent<Animator>();      
    }

    public void Init()
    {
        StartCoroutine(Spawn_Start());
    }

    /// <summary>
    /// 몬스터가 스폰이 될 때, 스케일의 크기변화를 줍니다.
    /// </summary>
    /// <returns></returns>
    IEnumerator Spawn_Start()
    {
        float current = 0.0f;
        float percent = 0.0f;
        float start = 0.0f;
        float end = transform.localScale.x; // 몬스터의 로컬스케일 

        while(percent < 1)
        {
            current += Time.deltaTime;
            percent = current / 0.3f;
            float LerpPos = Mathf.Lerp(start,end, percent); // 선형보간 (시작값,끝값,시간)
            transform.localScale = new Vector3(LerpPos, LerpPos, LerpPos);
            yield return null;
        }

        yield return new WaitForSeconds(0.3f);
        isSpawn = true;
    }

    private void Update()
    {

        //한 지점에서 다른 지점으로 일정 속도로 이동할 때 유용하게 사용됩니다.
        //MoveToWards 메서드는 목적지에 도달할 때까지 선형적으로 이동하며, 속도를 조절할 수 있습니다.
        
        transform.LookAt(Vector3.zero);

        if (isSpawn == false) return;


        //Vector3.Distance는 Unity에서 두 지점 간의 거리를 계산하는 데 사용되는 메서드입니다.
        //이 메서드는 3D 공간에서 두 개의 Vector3 간의 유클리드 거리(Euclidean Distance)를 반환합니다.
        float targetDistance = Vector3.Distance(transform.position, Vector3.zero);

        if(targetDistance <= 0.5f)
        {
            AnimatorChange("isIDLE");
        }

        else
        {
            transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, Time.deltaTime * M_Speed);
            AnimatorChange("isMOVE");
        }

        
    }

    private void AnimatorChange(string temp)
    {
        animator.SetBool("isIDLE", false);
        animator.SetBool("isMOVE", false);

        animator.SetBool(temp, true);
    }

}
