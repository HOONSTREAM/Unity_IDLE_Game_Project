using UnityEngine;
using UnityEditor;

public class HDRPMaterialSyncTool : EditorWindow
{
    private Shader targetShader;

    [MenuItem("Piloto Tools/Sync HDRP Materials")]
    public static void ShowWindow()
    {
        var window = GetWindow<HDRPMaterialSyncTool>("HDRP Material Sync");
        window.minSize = new Vector2(350, 260);
    }

    void OnGUI()
    {
        GUILayout.Label("HDRP → URP / Built-in Material Sync", EditorStyles.boldLabel);
        targetShader = (Shader)EditorGUILayout.ObjectField("Target HDRP Shader", targetShader, typeof(Shader), false);

        if (GUILayout.Button("Process All Materials"))
            ProcessAllMaterials();

        if (GUILayout.Button("Process Selected Material"))
            ProcessSelectedMaterial();
    }

    void ProcessAllMaterials()
    {
        string[] matGuids = AssetDatabase.FindAssets("t:Material");
        foreach (string guid in matGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (mat.shader == targetShader)
                SyncMaterial(mat, path);
        }

        AssetDatabase.SaveAssets();
        Debug.Log("✅ All matching materials processed.");
    }

    void ProcessSelectedMaterial()
    {
        if (Selection.activeObject is Material mat)
        {
            string path = AssetDatabase.GetAssetPath(mat);
            SyncMaterial(mat, path);
            AssetDatabase.SaveAssets();
            Debug.Log($"✅ Material '{mat.name}' processed.");
        }
        else
        {
            Debug.LogWarning("⚠️ Please select a material in the Project view.");
        }
    }

    void SyncMaterial(Material mat, string path)
    {
        // ✅ Surface Type
        if (mat.HasProperty("_SurfaceType"))
        {
            int surfaceType = mat.GetInt("_SurfaceType"); // 0 = Opaque, 1 = Transparent
            mat.SetFloat("_OriginalSurfaceType", surfaceType);

            if (mat.HasProperty("_Surface"))
                mat.SetFloat("_Surface", surfaceType);

            if (mat.HasProperty("_BUILTIN_Surface"))
                mat.SetFloat("_BUILTIN_Surface", surfaceType);
        }

        // ✅ Blend Mode
        if (mat.HasProperty("_BlendMode"))
        {
            int hdrpBlend = mat.GetInt("_BlendMode"); // 0 = Alpha, 1 = Additive
            mat.SetFloat("_OriginalBlendMode", hdrpBlend);

            if (mat.HasProperty("_Blend")) mat.SetFloat("_Blend", hdrpBlend == 1 ? 2f : 0f);
            if (mat.HasProperty("_DstBlend")) mat.SetFloat("_DstBlend", hdrpBlend == 1 ? 1f : 10f);

            if (mat.HasProperty("_BUILTIN_Blend")) mat.SetFloat("_BUILTIN_Blend", hdrpBlend == 1 ? 2f : 0f);
            if (mat.HasProperty("_BUILTIN_DstBlend")) mat.SetFloat("_BUILTIN_DstBlend", hdrpBlend == 1 ? 1f : 10f);
        }

        // ✅ Alpha Clipping
        if (mat.HasProperty("_AlphaCutoffEnable"))
        {
            int alphaClip = mat.GetInt("_AlphaCutoffEnable"); // 1 = On, 0 = Off
            mat.SetFloat("_OriginalAlphaClip", alphaClip);

            if (mat.HasProperty("_AlphaClip")) mat.SetFloat("_AlphaClip", alphaClip);
            if (alphaClip == 1)
                mat.EnableKeyword("_ALPHATEST_ON");
            else
                mat.DisableKeyword("_ALPHATEST_ON");

            if (mat.HasProperty("_BUILTIN_AlphaClip"))
                mat.SetFloat("_BUILTIN_AlphaClip", alphaClip);
        }

        // ✅ Corrected Culling Mode (based on HDRP _CullMode and _DoubleSidedEnable)
        int mappedCullValue = 1; // default to back-face

        if (mat.HasProperty("_DoubleSidedEnable") && mat.GetInt("_DoubleSidedEnable") == 1)
        {
            mappedCullValue = 0; // double sided
        }
        else if (mat.HasProperty("_CullMode"))
        {
            int hdrpCull = mat.GetInt("_CullMode");

            if (hdrpCull == 1)
                mappedCullValue = 1; // front-face culling
            else if (hdrpCull == 2)
                mappedCullValue = 2; // back-face culling
        }

  //      mat.SetFloat("_OriginalCullMode", mappedCullValue);

        if (mat.HasProperty("_Cull"))
            mat.SetInt("_Cull", mappedCullValue);

        if (mat.HasProperty("_BUILTIN_CullMode"))
            mat.SetInt("_BUILTIN_CullMode", mappedCullValue);

        EditorUtility.SetDirty(mat);
    }
}
