using UnityEditor;
using UnityEngine;

public class MaterialTransparencyFixerWindow : EditorWindow
{
    private Shader targetShader;

    [MenuItem("Tools/Material Transparency Fixer")]
    public static void ShowWindow()
    {
        GetWindow<MaterialTransparencyFixerWindow>("Transparency Fixer");
    }

    private void OnGUI()
    {
        GUILayout.Label("Fix Transparent Materials", EditorStyles.boldLabel);
        targetShader = (Shader)EditorGUILayout.ObjectField("Target Shader", targetShader, typeof(Shader), false);

        if (GUILayout.Button("Process Materials"))
        {
            if (targetShader == null)
            {
                Debug.LogWarning("Please select a shader.");
                return;
            }

            ProcessMaterials(targetShader);
        }
    }

    private void ProcessMaterials(Shader shader)
    {
        string[] materialGuids = AssetDatabase.FindAssets("t:Material");
        int changedCount = 0;

        foreach (string guid in materialGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);

            if (mat == null || mat.shader != shader)
                continue;

            // Check if the surface type is Transparent
            if (mat.IsKeywordEnabled("_SURFACE_TYPE_TRANSPARENT") || mat.GetTag("RenderType", false) == "Transparent")
            {
                if (mat.HasProperty("_AlphaCutoffEnable"))
                {
                    mat.SetFloat("_AlphaCutoffEnable", 0f);
                    EditorUtility.SetDirty(mat);
                    changedCount++;
                }
                else
                {
                    Debug.LogWarning($"Material \"{mat.name}\" does not have the _AlphaCutoffEnable property.");
                }
            }
        }

        AssetDatabase.SaveAssets();
        Debug.Log($"Updated {changedCount} transparent materials using shader \"{shader.name}\".");
    }
}
