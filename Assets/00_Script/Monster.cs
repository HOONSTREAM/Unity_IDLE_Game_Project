using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Character
{
    public float M_Speed;
    

    bool isSpawn = false;

    protected override void Start()
    {
        base.Start();

    }


    /// <summary>
    /// 원하는 시점에 계속 Init을 시킬수 있다.
    /// </summary>
    public void Init()
    {
        isDead = false;
        HP = 5;
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

    public void GetDamage(double dmg)
    {
        if(isDead)
        {
            return;
        }

        Base_Manager.Pool.Pooling_OBJ("HIT_TEXT").Get((value) =>
        {
            value.GetComponent<Hit_Text>().Init(transform.position, dmg, false);
        });

        HP -= dmg;

        if(HP <= 0)
        {
            isDead = true;
            Spawner.m_monsters.Remove(this);

            Base_Manager.Pool.Pooling_OBJ("Smoke").Get((value) =>
            {
                value.transform.position = new Vector3(transform.position.x,0.5f,transform.position.z);
                Base_Manager.instance.Return_Pool(value.GetComponent<ParticleSystem>().duration, value, "Smoke");
                
            });



            Base_Manager.Pool.Pooling_OBJ("COIN_PARENT").Get((value) =>
            {
                value.GetComponent<Coin_Parent>().Init(transform.position);
            });


            Base_Manager.Pool.m_pool_Dictionary["Monster"].Return(this.gameObject);
        }


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

    /// <summary>
    /// 여러가지 이펙트들을 코루틴으로 리턴시켜주는 메서드 입니다.
    /// </summary>
    /// <param name="timer"></param>
    /// <param name="obj"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    IEnumerator ReturnCoroutine(float timer, GameObject obj, string path)
    {
        yield return new WaitForSeconds(timer);
        Base_Manager.Pool.m_pool_Dictionary[path].Return(obj);
    }
   
}
