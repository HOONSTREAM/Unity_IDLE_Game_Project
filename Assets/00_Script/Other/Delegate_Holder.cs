using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region RELIC
public delegate void Monster_Dead(Monster monster);
public delegate void Player_Attack(Player player, Monster monster);
public delegate void Player_Hit(Player player);
#endregion

#region STAGE MANAGER
public delegate void OnReadyEvent();
public delegate void OnPlayEvent();
public delegate void OnBossEvent();
public delegate void OnBossPlayEvent();
public delegate void OnClearEvent();
public delegate void OnDeadEvent();
#endregion

public class Delegate_Holder 
{
    public static event Monster_Dead Monster_Dead_Event;
    public static event Player_Attack Player_attack_Event;
    public static event Player_Hit player_hit_Event;


    public static void Clear_Event()
    {
        Monster_Dead_Event = null;
        Player_attack_Event = null;
        player_hit_Event = null;
    }

    public static void Monster_Dead(Monster monster)
    {
        Monster_Dead_Event?.Invoke(monster);
    }

    public static void Player_Attack(Player player, Monster monster)
    {
        Player_attack_Event?.Invoke(player, monster);
    }

    public static void Player_hit(Player player)
    {
        player_hit_Event?.Invoke(player);
    }

}
