using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

[CustomEditor(typeof(CameraFX))]
public class PP_FX_Tool : Editor
{
    public override VisualElement CreateInspectorGUI()
    {
        VisualElement PP_FX_Inspector = new VisualElement();


        VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/PP_FX_Tool.uxml");
        visualTree.CloneTree(PP_FX_Inspector);

        VisualElement inspectorFoldout = PP_FX_Inspector.Q("Default_Inspector");
        InspectorElement.FillDefaultInspector(inspectorFoldout, serializedObject, this);

        /*
        vignette_groupbox = PP_FX_Inspector.Q("vignette_parameters").Q("unity-content").Q("vignette_groupbox");

        if (myBehavior.GetVignetteToggle() == true)
        {

            vignette_groupbox.visible = true;
        }
        else
        {
            vignette_groupbox.visible = false;
        }
        */

        return PP_FX_Inspector;
    }

    public override void OnInspectorGUI()
    {
        CameraFX myBehaviour = target as CameraFX;

    }
}

