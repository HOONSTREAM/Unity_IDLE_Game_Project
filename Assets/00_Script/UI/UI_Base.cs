using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Base : MonoBehaviour
{
    protected bool _init = false;
    public virtual bool Init()
    {
        if (_init)
        {
            return false;
        }

        else
        {
            return true;
        }
    }

    private void Start()
    {
        Init();
    }

    public virtual void DisableOBJ()
    {
        Utils.UI_Holder.Pop();
        Destroy(this.gameObject);
    }
}
