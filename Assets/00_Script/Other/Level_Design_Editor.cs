using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Level_Design))]    
public class Level_Design_Editor : Editor
{
    Level_Design design = null;
    public override void OnInspectorGUI()
    {
        design = (Level_Design)target;

        EditorGUILayout.LabelField("Level Design", EditorStyles.boldLabel);

        LevelData leveldata = design.levelData;
        
        EditorGUILayout.LabelField("캐릭터 레벨 그래프", EditorStyles.boldLabel);
        DrawGraph(leveldata);

        EditorGUILayout.Space();

        DrawDefaultInspector();

     
    }

    private void DrawGraph(LevelData data)
    {
        Rect rect = GUILayoutUtility.GetRect(200, 100);
        Handles.DrawSolidRectangleWithOutline(rect, Color.black, Color.white);

        Vector3[] CurvePoint_ATK = GraphDesign(rect, data.ATK);
        Handles.color = Color.green;
        Handles.DrawAAPolyLine(3, CurvePoint_ATK);

        Vector3[] CurvePoint_HP = GraphDesign(rect, data.HP);
        Handles.color = Color.red;
        Handles.DrawAAPolyLine(3, CurvePoint_HP);

        Vector3[] CurvePoint_EXP = GraphDesign(rect, data.EXP);
        Handles.color = Color.blue;
        Handles.DrawAAPolyLine(3, CurvePoint_EXP);

        Vector3[] CurvePoint_MAXEXP = GraphDesign(rect, data.MAX_EXP);
        Handles.color = Color.white;
        Handles.DrawAAPolyLine(3, CurvePoint_MAXEXP);

        Vector3[] CurvePoint_LEVELUP_MONEY = GraphDesign(rect, data.LEVELUP_MONEY);
        Handles.color = Color.yellow;
        Handles.DrawAAPolyLine(3, CurvePoint_LEVELUP_MONEY);



        EditorGUILayout.Space(20);
        GetColorGUI("ATK", StringMethod.ToCurrencyString(design.CalculateValue(10, data.Current_Level, data.ATK)), Color.green);
        GetColorGUI("HP", StringMethod.ToCurrencyString(design.CalculateValue(50, data.Current_Level, data.HP)), Color.red);
        GetColorGUI("EXP", StringMethod.ToCurrencyString(design.CalculateValue(5, data.Current_Level, data.EXP)), Color.blue);
        GetColorGUI("MAXEXP", StringMethod.ToCurrencyString(design.CalculateValue(15, data.Current_Level, data.MAX_EXP)), Color.white);
        GetColorGUI("LEVELUP_MONEY", StringMethod.ToCurrencyString(design.CalculateValue(10, data.Current_Level, data.LEVELUP_MONEY)), Color.yellow);
        EditorGUILayout.Space(20);
    }

    private void GetColorGUI(string baseTemp, string dataTemp, Color color)
    {
        GUIStyle colorLabel = new GUIStyle(EditorStyles.label);
        colorLabel.normal.textColor = color;

        EditorGUILayout.LabelField(baseTemp + " : " + dataTemp, colorLabel);
    }

    private Vector3[] GraphDesign(Rect rect, float data)
    {
        Vector3[] curvePoint = new Vector3[100];

        for(int i = 0; i < 100; i++)
        {
            float t = i / 99.0f;
            float curveValue = Mathf.Pow(t,data);
            curvePoint[i] = new Vector3(rect.x + t * rect.width, rect.y + rect.height - curveValue * rect.height, 0);
        }

        return curvePoint;
    }

}
