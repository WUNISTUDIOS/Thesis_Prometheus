using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Presets;

public class HDRP_MaterialUpdater : EditorWindow
{
    Material mat;
    Color col;

    /// <summary>
    /// render mode:
    /// 0 = Opaque,
    /// 1 = Cutout,
    /// 2 = Fade,
    /// 3 = Transparent,
    /// 4 = Additive,
    /// 5 = Subtractive,
    /// 6 = Modulate
    /// </summary>
    float mode;

    /// <summary>
    /// color mode:
    /// 0 = Multiply,
    /// 1 = Additive,
    /// 2 = Subtractive,
    /// 3 = Overlay,
    /// 4 = Color,
    /// 5 = Difference
    /// </summary>
    float cMode;

    Texture baseMap;
    Texture metalMap;
    Texture specularMap;
    Texture normalMap;
    Texture occlusionMap;
    Texture emissionMap;

    [MenuItem("Tools/HDRP/MaterialUpdater")]
    static void UpdateHDRPMaterials()
    {
        HDRP_MaterialUpdater window = GetWindow<HDRP_MaterialUpdater>();
        window.Show();
    }

    private void OnGUI()
    {
        mat = (Material)EditorGUILayout.ObjectField("Material",
            mat, typeof(Material), false);
        baseMap = (Texture)EditorGUILayout.ObjectField("baseMap",
            baseMap, typeof(Texture), false);
        metalMap = (Texture)EditorGUILayout.ObjectField("metalMap",
            metalMap, typeof(Texture), false);
        specularMap = (Texture)EditorGUILayout.ObjectField("specularMap", specularMap, typeof(Texture), false);
        normalMap = (Texture)EditorGUILayout.ObjectField("normalMap",
            normalMap, typeof(Texture), false);
        occlusionMap = (Texture)EditorGUILayout.ObjectField("occlusionMap",
            occlusionMap, typeof(Texture), false);
        emissionMap = (Texture)EditorGUILayout.ObjectField("emissionMap",
            emissionMap, typeof(Texture), false);

        string shaderName = "No Material Found";

        if (mat)
        {
            shaderName = mat.shader.ToString();
        }

        GUILayout.TextField(shaderName);
        EditorGUILayout.FloatField(mode);
        EditorGUILayout.FloatField(cMode);

        if (GUILayout.Button("Copy Textures"))
        {
            if (!mat)
            {
                if (!Selection.activeObject)
                {
                    return;
                }
                if (!(Selection.activeObject is Material))
                {
                    return;
                }

                mat = Selection.activeObject as Material;
                shaderName = mat.shader.ToString();
            }

            if (shaderName.StartsWith("Universal"))
            {
                col = Color.white;
                mode = -1;
                cMode = -1;
                baseMap = mat.GetTexture("_BaseMap");
                metalMap = mat.GetTexture("_MetallicGlossMap");
                specularMap = mat.GetTexture("_SpecGlossMap");
                normalMap = mat.GetTexture("_BumpMap");
                occlusionMap = mat.GetTexture("_OcclusionMap");
                emissionMap = mat.GetTexture("_EmissionMap");
            }
            else if (shaderName.Contains("Standard Surface"))
            {
                col = mat.GetColor("_Color");
                mode = mat.GetFloat("_Mode");
                cMode = -1;
                baseMap = mat.GetTexture("_MainTex");
                normalMap = mat.GetTexture("_BumpMap");
                emissionMap = mat.GetTexture("_EmissionMap");
            }
            else if (shaderName.Contains("Standard Unlit"))
            {
                col = mat.GetColor("_Color");
                mode = mat.GetFloat("_Mode");
                cMode = mat.GetFloat("_ColorMode");
                baseMap = mat.GetTexture("_MainTex");
                normalMap = mat.GetTexture("_BumpMap");
                emissionMap = mat.GetTexture("_EmissionMap");
            }
            else if (shaderName.Contains("Particles/Additive"))
            {
                mode = -1;
                cMode = -1;
                col = mat.GetColor("_TintColor");
                baseMap = mat.GetTexture("_MainTex");
            }
        }
        if (GUILayout.Button("Switch Shader"))
        {
            Undo.RecordObject(mat, "Update RP");

            mat.shader = Shader.Find("HDRP/Lit");
        }
        if (GUILayout.Button("Paste Textures"))
        {
            Undo.RecordObject(mat, "Update RP");


            //mat.SetFloat("_SurfaceType", ) // 0 = opaque, 1 = transparent
            //mat.SetFloat("_BlendMode", ) // 0 = Alpha, 1 = additive, 2 = premultiply
            //mat.SetFloat("_SupportDecals", 0);
            //mat.SetFloat("_ReceivesSSR", 0);
            //mat.SetFloat("_AlphaCutoffEnable", )
            mat.SetColor("_BaseColor", col);
            mat.SetTexture("_BaseColorMap", baseMap);
            mat.SetTexture("_NormalMap", normalMap);
            mat.SetTexture("_EmissiveColorMap", emissionMap);
        }
    }
}

public class HDRP_Utilities : Editor
{
    [MenuItem("Tools/HDRP/Log Shader")]
    static void LogShader()
    {
        Debug.Log((Selection.activeObject as Material).shader);
    }

    [MenuItem("Tools/HDRP/Switch RP")]
    static void SwitchRP()
    {
        string[] matGUIDs = AssetDatabase.FindAssets("t:Material");

        foreach (string s in matGUIDs)
        {
            string path = AssetDatabase.GUIDToAssetPath(s);
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (mat)
            {
                string matShaderName = mat.shader.ToString();
                if (matShaderName.StartsWith("Universal"))
                {
                    Debug.Log(matShaderName, mat);
                }
                if (mat.shader ==
                    Shader.Find("Universal Render Pipeline/Simple Lit"))
                {
                    SwitchForMatSimpleLit(mat);
                }
                if (mat.shader == Shader.Find("Universal Render Pipeline/Lit"))
                {
                    SwitchForMatLit(mat);
                }
            }
        }
    }



    static void SwitchForMatSimpleLit(Material mat)
    {
        Undo.RecordObject(mat, "Update RP");

        Texture baseMap = mat.GetTexture("_BaseMap");
        Texture specularMap = mat.GetTexture("_SpecGlossMap");
        Texture normalMap = mat.GetTexture("_BumpMap");
        Texture emissionMap = mat.GetTexture("_EmissionMap");

        mat.shader = Shader.Find("HDRP/Lit");

        mat.SetTexture("_BaseColorMap", baseMap);
        mat.SetTexture("_NormalMap", normalMap);
        mat.SetTexture("_EmissiveColorMap", emissionMap);
    }
    static void SwitchForMatLit(Material mat)
    {
        Undo.RecordObject(mat, "Update RP");

        Texture baseMap = mat.GetTexture("_BaseMap");
        Texture metalMap = mat.GetTexture("_MetallicGlossMap");
        Texture specularMap = mat.GetTexture("_SpecGlossMap");
        Texture normalMap = mat.GetTexture("_BumpMap");
        Texture occlusionMap = mat.GetTexture("_OcclusionMap");
        Texture emissionMap = mat.GetTexture("_EmissionMap");

        Texture gloss = metalMap != null ? metalMap : specularMap;

        mat.shader = Shader.Find("HDRP/Lit");

        mat.SetTexture("_BaseColorMap", baseMap);
        mat.SetTexture("_NormalMap", normalMap);
        mat.SetTexture("_EmissiveColorMap", emissionMap);
    }

}
