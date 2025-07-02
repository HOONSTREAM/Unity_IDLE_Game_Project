using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class UberFXShaderConverter : EditorWindow
{
    [MenuItem("Tools/UberFX/Convert All To UberFXSG")]
    public static void ConvertUberFXMaterials()
    {
        string[] materialGuids = AssetDatabase.FindAssets("t:Material");

        Shader oldUberFX = Shader.Find("Piloto Studio/Uber FX");
        Shader oldOpaqueUberFX = Shader.Find("Piloto Studio/Opaque Uber FX");
        Shader newUberFXSG = Shader.Find("Piloto Studio/Uber FXSG");

        if (newUberFXSG == null)
        {
            Debug.LogError("UberFXSG shader not found in the project.");
            return;
        }

        int convertedCount = 0;

        foreach (string guid in materialGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);

            if (mat == null || (mat.shader != oldUberFX && mat.shader != oldOpaqueUberFX))
                continue;

            Undo.RecordObject(mat, "Convert Material to UberFXSG");

            bool wasOpaque = mat.shader == oldOpaqueUberFX;

            // Assign new shader
            mat.shader = newUberFXSG;

            // Enforce Opaque rules if coming from Opaque UberFX
            if (wasOpaque)
            {
                mat.SetFloat("_Surface", 0f); // 0 = Opaque
                mat.DisableKeyword("_USESOFTALPHA");
            }

            // Copy Shader Feature mappings
            CopyKeyword(mat, "_UseSoftAlpha", "_USESOFTALPHA");
            CopyKeyword(mat, "_UseAlphaOverride", "_USEALPHAOVERRIDE");
            CopyKeyword(mat, "_UseRamp", "_USERAMP");
            CopyKeyword(mat, "_Fresnel", "_FRESNEL");
            CopyKeyword(mat, "_UseUVOffset", "_USEUVOFFSET");
            CopyKeyword(mat, "_DisableErosion", "_DISABLEEROSION");
            CopyKeyword(mat, "_UsePixelation", "_USEPIXELATION");

            // Copy blend mode (_SourceBlendRGB) → _Surface + _BlendMode
            if (mat.HasFloat("_SourceBlendRGB"))
            {
                float sourceBlend = mat.GetFloat("_SourceBlendRGB");
                bool isAlphaBlend = Mathf.Approximately(sourceBlend, 10f);

                mat.SetFloat("_Surface", isAlphaBlend ? 1f : 0f); // 1 = Transparent
                mat.SetFloat("_BlendMode", isAlphaBlend ? 0f : 1f); // 0 = Alpha, 1 = Additive

                // NOTE: Unity handles _BUILTIN_* mapping automatically
            }

            // Preserve texture tiling and offsets
            PreserveTextureTiling(mat);

            EditorUtility.SetDirty(mat);
            convertedCount++;
        }

        AssetDatabase.SaveAssets();
        Debug.Log($"UberFX conversion complete. Converted {convertedCount} materials.");
    }

    private static void CopyKeyword(Material mat, string oldKeyword, string newKeyword)
    {
        if (mat.IsKeywordEnabled(oldKeyword))
            mat.EnableKeyword(newKeyword);
        else
            mat.DisableKeyword(newKeyword);
    }

    private static void PreserveTextureTiling(Material mat)
    {
        foreach (string propName in mat.GetTexturePropertyNames())
        {
            if (!mat.HasProperty(propName)) continue;

            Texture tex = mat.GetTexture(propName);
            if (tex != null)
            {
                Vector2 tiling = mat.GetTextureScale(propName);
                Vector2 offset = mat.GetTextureOffset(propName);
                mat.SetTexture(propName, tex);
                mat.SetTextureScale(propName, tiling);
                mat.SetTextureOffset(propName, offset);
            }
        }
    }
}
