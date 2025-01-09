using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Render_Manager : MonoBehaviour
{
    public static Render_Manager instance;



    public Camera cam; // ������ ī�޶��� ķ
    public Camera status_cam;
    public Render_Hero HERO;
    public Render_Status STATUS;

    private void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        
    }

    public Vector2 ReturnScreenPoint(Transform pos)
    {
        return cam.WorldToScreenPoint(pos.position);
    }

}
