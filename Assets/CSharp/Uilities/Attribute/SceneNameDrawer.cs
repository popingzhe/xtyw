using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SceneNameAttribute))]

public class SceneNameDrawer : PropertyDrawer
{
    int sceneIndex = -1;
    GUIContent[] sceneNames;
    //分隔符
    readonly string[] scenePathSplit = {"/",".unity"};


    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
       if(EditorBuildSettings.scenes.Length == 0) { return; }
        if (sceneIndex == -1)
            GetSceneNameArray(property);
        int olddex = sceneIndex;
        //获取点击选项
        sceneIndex =  EditorGUI.Popup(position, label, sceneIndex, sceneNames);
        if(sceneIndex != olddex)
        {
            property.stringValue = sceneNames[sceneIndex].text;
        }
    }


    //填充容器，确定索引
    private void GetSceneNameArray(SerializedProperty property)
    {
        var scenes = EditorBuildSettings.scenes;

        sceneNames = new GUIContent[scenes.Length];
        for (int i = 0; i < sceneNames.Length; i++)
        {
            string path = scenes[i].path;
            string[] splitPath = path.Split(scenePathSplit,System.StringSplitOptions.RemoveEmptyEntries);
            
            string sceneName = "";

            if(splitPath.Length > 0)
            {
                sceneName = splitPath[splitPath.Length-1];
            }
            else
            {
                sceneName = "(Delete Scene)";
            }
            sceneNames[i] = new GUIContent(sceneName);
        }

        if(sceneNames.Length == 0)
        {
            sceneNames = new[] { new GUIContent("Check your Build Setting") };
        }

        if (String.IsNullOrEmpty(property.stringValue))
        {
            sceneIndex = 0;
        }
        else
        {
            bool nameFound = false;
            for (int i = 0;i < sceneNames.Length;i++)
            {
                if (sceneNames[i].text == property.stringValue)
                {
                    sceneIndex = i;
                    nameFound = true;
                    break;
                }
            }
            if(nameFound == false)
                sceneIndex = 0;
        }

        property.stringValue = sceneNames[sceneIndex].text;
    }



}
