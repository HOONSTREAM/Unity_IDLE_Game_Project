using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnReadyEvent();
public delegate void OnPlayEvent();
public delegate void OnBossEvent();
public delegate void OnClearEvent();
public delegate void OnDeadEvent();

public class Stage_Manager
{
    public static Stage_State M_State;
    public OnReadyEvent M_ReadyEvent;
    public void State_Change(Stage_State state)
    {
        M_State = state;
        switch(state)
        {
            case Stage_State.Ready:
                break;
            case Stage_State.Play:
                break;
            case Stage_State.Boss:
                break;
            case Stage_State.Clear:
                break;
            case Stage_State.Dead:
                break;
        }
    }
}
