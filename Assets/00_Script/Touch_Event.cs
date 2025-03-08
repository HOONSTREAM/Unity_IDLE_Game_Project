using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touch_Event : MonoBehaviour
{

    [SerializeField]
    private Camera UI_camera;
    [SerializeField]
    private ParticleSystem Touch_Effect;
    
 
    private void Update()
    {
        Set_Touch_Effect();
    }

    private void Set_Touch_Effect()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                // 터치 위치를 UI 카메라 기준 월드 좌표로 변환
                Vector3 touchPos = UI_camera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, UI_camera.nearClipPlane));

                // 파티클 시스템 위치 이동
                Touch_Effect.transform.position = new Vector3(touchPos.x, touchPos.y, Touch_Effect.transform.position.z);


                Base_Manager.SOUND.Play(Sound.BGS, "Touch_Effect");
                // 파티클 시스템 재생
                Touch_Effect.Play();
            }
        }
    }

}
