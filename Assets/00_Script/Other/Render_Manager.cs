using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Render_Manager : MonoBehaviour
{
    public static Render_Manager instance;

    private void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        
    }

    public Render_Hero HERO;
}
