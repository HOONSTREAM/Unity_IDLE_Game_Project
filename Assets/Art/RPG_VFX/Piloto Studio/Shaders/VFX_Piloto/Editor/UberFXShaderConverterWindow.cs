using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class UberFXShaderConverterWindow : EditorWindow
{
    Shader oldUberFXShader, oldOpaqueUberFXShader, newUberFXSGShader;
    Material testMaterial;

    [MenuItem("Tools/UberFX Shader Converter")]
    static void Init()
    {
        var window = GetWindow<UberFXShaderConverterWindow>();
        window.titleContent = new GUIContent("UberFX Converter");
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("🧩 Shaders", EditorStyles.boldLabel);
        oldUberFXShader = (Shader)EditorGUILayout.ObjectField("Old UberFX Shader", oldUberFXShader, typeof(Shader), false);
        oldOpaqueUberFXShader = (Shader)EditorGUILayout.ObjectField("Old Opaque UberFX Shader", oldOpaqueUberFXShader, typeof(Shader), false);
        newUberFXSGShader = (Shader)EditorGUILayout.ObjectField("New UberFXSG Shader", newUberFXSGShader, typeof(Shader), false);

        GUILayout.Space(10);
        GUILayout.Label("🎯 Single Material Test", EditorStyles.boldLabel);
        testMaterial = (Material)EditorGUILayout.ObjectField("Test Material", testMaterial, typeof(Material), false);

        if (GUILayout.Button("✅ Convert Selected Material"))
        {
            if (testMaterial != null)
                ConvertMaterial(testMaterial);
            else
                Debug.LogWarning("Please assign a test material.");
        }

        GUILayout.Space(10);
        if (GUILayout.Button("✅ Convert All Matching Materials"))
            BatchConvert();
    }

    void BatchConvert()
    {
        var guids = AssetDatabase.FindAssets("t:Material");
        int count = 0;
        foreach (var g in guids)
        {
            var mat = AssetDatabase.LoadAssetAtPath<Material>(AssetDatabase.GUIDToAssetPath(g));
            if (mat.shader == oldUberFXShader || mat.shader == oldOpaqueUberFXShader)
            {
                ConvertMaterial(mat);
                count++;
            }
        }
        Debug.Log($"Converted {count} materials.");
    }

    void ConvertMaterial(Material mat)
    {
        Debug.Log($"➡️ Converting: {mat.name}");
        Undo.RecordObject(mat, "Convert UberFX Material");

        bool isOpaque = mat.shader == oldOpaqueUberFXShader;

        // ✅ STORE _SourceBlendRGB BEFORE shader is changed
        float previousSourceBlend = mat.HasProperty("_SourceBlendRGB") ? mat.GetFloat("_SourceBlendRGB") : -1f;

        // Store keyword states
        var keywordStates = new Dictionary<string, bool>
        {
            { "_USESOFTALPHA", mat.GetFloat("_UseSoftAlpha") != 0f },
            { "_USEALPHAOVERRIDE", mat.GetFloat("_UseAlphaOverride") != 0f },
            { "_USERAMP", mat.GetFloat("_UseRamp") != 0f },
            { "_FRESNEL", mat.GetFloat("_Fresnel") != 0f },
            { "_USEUVOFFSET", mat.GetFloat("_UseUVOffset") != 0f },
            { "_DISABLEEROSION", mat.GetFloat("_DisableErosion") != 0f },
            { "_USEPIXELATION", mat.GetFloat("_UsePixelation") != 0f }
        };

        // ✅ Switch shader
        mat.shader = newUberFXSGShader;

        // Set Surface type
        mat.SetFloat("_Surface", isOpaque ? 0f : 1f);

        // ✅ Correct blend mode logic using PREVIOUS value
        if (!isOpaque)
        {
            if (Mathf.Approximately(previousSourceBlend, 10f))
            {
                mat.SetFloat("_BlendMode", 0f); // Alpha
                Debug.Log($"   ℹ️ _SourceBlendRGB (from old shader) = 10 → Alpha Blend");
            }
            else
            {
                mat.SetFloat("_BlendMode", 1f); // Additive
                Debug.Log($"   ℹ️ _SourceBlendRGB (from old shader) = {previousSourceBlend} → Additive Blend");
            }
        }

        // Restore keyword states
        foreach (var kvp in keywordStates)
        {
            mat.SetFloat(kvp.Key, kvp.Value ? 1f : 0f);
            SetLocalKeyword(mat, kvp.Key, kvp.Value);
        }

        // Preserve textures and panner adjustments
        Dictionary<string, string> textureToPanner = new()
        {
            { "_MainTex", "_MainTexturePanning" },
            { "_AlphaOverride", "_AlphaOverridePanning" },
            { "_DetailNoise", "_DetailNoisePanning" }
        };

        foreach (var kvp in textureToPanner)
        {
            string texProp = kvp.Key;
            string panProp = kvp.Value;

            if (!mat.HasProperty(texProp) || !mat.HasProperty(panProp))
            {
                Debug.Log($"   ✖ Skipping: {texProp} or {panProp} missing");
                continue;
            }

            Texture tex = mat.GetTexture(texProp);
            Vector2 tiling = mat.GetTextureScale(texProp);
            Vector2 offset = mat.GetTextureOffset(texProp);
            Vector2 panner = mat.GetVector(panProp);

            mat.SetTexture(texProp, tex);
            mat.SetTextureScale(texProp, tiling);
            mat.SetTextureOffset(texProp, offset);

            Vector2 newPanner = panner;

            if (!Mathf.Approximately(panner.x, 0f) && !Mathf.Approximately(tiling.x, 1f) && !Mathf.Approximately(tiling.x, 0f))
                newPanner.x = panner.x / Mathf.Abs(tiling.x);

            if (!Mathf.Approximately(panner.y, 0f) && !Mathf.Approximately(tiling.y, 1f) && !Mathf.Approximately(tiling.y, 0f))
                newPanner.y = panner.y / Mathf.Abs(tiling.y);

            mat.SetVector(panProp, newPanner);
            Debug.Log($"   ✅ Adjusted {panProp}: {panner} → {newPanner}");
        }

        // Final render queue
        mat.renderQueue = isOpaque
            ? (int)RenderQueue.Geometry
            : (int)RenderQueue.Transparent;

        EditorUtility.SetDirty(mat);
        Debug.Log($"✔️ Finished: {mat.name}");
    }

    void SetLocalKeyword(Material mat, string key, bool enable)
    {
        if (mat.shader == null) return;
        var lk = new LocalKeyword(mat.shader, key);
        mat.SetKeyword(lk, enable);
    }
}
