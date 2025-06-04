using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Daily_Quest : UI_Base
{
    public Daily_Quest_Parts QuestPanel;
    public Transform Content;
    List<GameObject> Garbage_Object = new List<GameObject>();
    List<Transform> InitPanels = new List<Transform>();

    public static event Action OnDailyQuestUIOpened;
    public static event Action OnDailyQuestUIClosed;

    public override bool Init()
    {
        OnDailyQuestUIOpened?.Invoke();

        if (Garbage_Object.Count > 0)
        {
            for (int i = 0; i < Garbage_Object.Count; i++) Destroy(Garbage_Object[i]);
            Garbage_Object.Clear();
            InitPanels.Clear();
        }

        foreach (var quest in Base_Manager.DAILY.activeQuests)
        {
            var go = Instantiate(QuestPanel, Content);
            go.gameObject.SetActive(true);

            go.Init(quest, this);
            Garbage_Object.Add(go.gameObject);
            InitPanels.Add(go.transform);
        }

        return base.Init();
    }

    public override void DisableOBJ()
    {
        OnDailyQuestUIClosed?.Invoke();
        base.DisableOBJ();
    }
}
