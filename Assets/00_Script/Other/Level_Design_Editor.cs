using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(Level_Design))]    
public class Level_Design_Editor : Editor
{
  
    public override void OnInspectorGUI()
    {
        Level_Design design = (Level_Design)target;

        EditorGUILayout.LabelField("Level Design", EditorStyles.boldLabel);

        LevelData leveldata = design.levelData;
        StageData stagedata = design.stageData;
        
       
        DrawGraph(leveldata, stagedata);

        EditorGUILayout.Space();

        DrawDefaultInspector();

     
    }

    private void DrawGraph(LevelData data,StageData s_data)
    {
        EditorGUILayout.LabelField("캐릭터 레벨 데이터", EditorStyles.boldLabel);
        EditorGUILayout.Space(20);
        GetColorGUI("ATK", StringMethod.ToCurrencyString(Utils.CalculateValue(data.Base_ATK, data.Current_Level, data.ATK)), Color.green);
        GetColorGUI("HP", StringMethod.ToCurrencyString(Utils.CalculateValue(data.Base_HP, data.Current_Level, data.HP)), Color.red);
        GetColorGUI("EXP", StringMethod.ToCurrencyString(Utils.CalculateValue(data.Base_EXP, data.Current_Level, data.EXP)), Color.blue);
        GetColorGUI("MAXEXP", StringMethod.ToCurrencyString(Utils.CalculateValue(data.Base_MAX_EXP, data.Current_Level, data.MAX_EXP)), Color.white);
        GetColorGUI("LEVELUP_MONEY", StringMethod.ToCurrencyString(Utils.CalculateValue(data.Base_LEVELUP_MONEY, data.Current_Level, data.LEVELUP_MONEY)), Color.yellow);
        EditorGUILayout.Space(20);
        EditorGUILayout.LabelField("스테이지 데이터", EditorStyles.boldLabel);
        EditorGUILayout.Space(20);
        GetColorGUI("ATK", StringMethod.ToCurrencyString(Utils.CalculateValue(s_data.Base_MONSTER_ATK, s_data.Current_Stage, s_data.MONSTER_ATK)), Color.green);
        GetColorGUI("HP", StringMethod.ToCurrencyString(Utils.CalculateValue(s_data.Base_MONSTER_HP, s_data.Current_Stage, s_data.MONSTER_HP)), Color.red);
        GetColorGUI("DROP_MONEY", StringMethod.ToCurrencyString(Utils.CalculateValue(s_data.Base_DROP_MONEY, s_data.Current_Stage, s_data.DROP_MONEY)), Color.blue);
    }

    private void GetColorGUI(string baseTemp, string dataTemp, Color color)
    {
        GUIStyle colorLabel = new GUIStyle(EditorStyles.label);
        colorLabel.normal.textColor = color;

        EditorGUILayout.LabelField(baseTemp + " : " + dataTemp, colorLabel);
    }

  

}
