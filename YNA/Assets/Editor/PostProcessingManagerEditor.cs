using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.Events;
using UnityEditorInternal;
using UnityEngine.UIElements;

/*[CustomPropertyDrawer(typeof(UnityEvent<CameraFX>), true)]*/
[CustomEditor(typeof(PostProcessingManager))]
public class PostProcessingManagerEditor : Editor
{
    /*public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        PostProcessingManager myBehavior = target as PostProcessingManager;

        List<CameraFX> fx_list = new List<CameraFX>(myBehavior.gameObject.GetComponents<CameraFX>());

        foreach (var fx in fx_list)
        {
                                //makes sure to add only once
            myBehavior.FX_List.TryAdd(fx.name, fx);
        }


    }*/

    /*public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        return base.CreatePropertyGUI(property);
    }*/
    /*string title = "";

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //if( list == null ) Init( property );
        VisualElement PP_FX_Inspector = new VisualElement();


        title = label?.text ?? "no name";

        property.serializedObject.Update();

        SerializedProperty listenersArray = property.FindPropertyRelative("m_PersistentCalls.m_Calls");

        var line = position;

        for (var i = 0; i < listenersArray.arraySize; ++i)
        {
            line.height = 30f;
            line.y = position.y + 30f * i;

            var prop = listenersArray.GetArrayElementAtIndex(0);
            var target = prop.FindPropertyRelative("m_Target");

            EditorGUI.PropertyField(line, target, GUIContent.none);

            VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/PP_FX_Tool.uxml");
            visualTree.CloneTree(PP_FX_Inspector);
        }

        //list.DoLayoutList();

        property.serializedObject.ApplyModifiedProperties();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty listenersArray = property.FindPropertyRelative("m_PersistentCalls.m_Calls");

        return listenersArray.arraySize * 30f;
    }*/
}