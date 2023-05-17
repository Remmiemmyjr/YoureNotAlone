using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

[CustomEditor(typeof(CameraFX))]
public class PP_FX_Tool : Editor
{

    private bool VignetteDrawerPos = false;
    public override VisualElement CreateInspectorGUI()
    {
        VisualElement PP_FX_Inspector = new VisualElement();



        CameraFX myBehavior = target as CameraFX;


        VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/PP_FX_Tool.uxml");
        visualTree.CloneTree(PP_FX_Inspector);

        /*//checks if list is empty
        if (myBehavior.EffectsList.Count != 0)
        {

            //Show Effect values
            //have each effect section be collapsable


            if (myBehavior.EffectsList.Contains(CameraFX.FX.Vignette))
            {
                VignetteDrawerPos = EditorGUI.BeginFoldoutHeaderGroup(new Rect(10.0f, 100.0f, 200.0f, 100.0f), VignetteDrawerPos, "Vignette Properties");

                if (VignetteDrawerPos)
                {
                    myBehavior.testnubmb = EditorGUILayout.IntField("Damage to Enemy", myBehavior.testnubmb);
                }

                EditorGUILayout.EndFoldoutHeaderGroup();

            }
        }*/



        return PP_FX_Inspector;
    }
}
