/// <summary>
/// デバッグツール
/// </summary>
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

using System.Collections;

public static class DebugTool
{
    private static readonly Type            TOOLBAR_TYPE                    = typeof( EditorGUI ).Assembly.GetType( "UnityEditor.Toolbar" );
    private static readonly FieldInfo       TOOLBAR_GET                     = TOOLBAR_TYPE.GetField( "get" );
    private static readonly Type            GUI_VIEW_TYPE                   = typeof( EditorGUI ).Assembly.GetType( "UnityEditor.GUIView" );
    private static readonly BindingFlags    BINDING_ATTR                    = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
    private static readonly PropertyInfo    GUI_VIEW_IMGUI_CONTAINER        = GUI_VIEW_TYPE.GetProperty( "imguiContainer", BINDING_ATTR );
    private static readonly FieldInfo       IMGUI_CONTAINER_ON_GUI_HANDLER  = typeof( IMGUIContainer ).GetField("m_OnGUIHandler", BINDING_ATTR );
    private static List<GameObject> hierarchyObjectList = new List<GameObject>();

    private static bool isDebug = true; //trueならデバッグモード　falseなら本番
    
    [InitializeOnLoadMethod]
    private static void InitializeOnLoad()
    {
        EditorApplication.update += OnUpdate;
    }

    private static void OnUpdate()
    {
        var toolbar = TOOLBAR_GET.GetValue( null );
        if ( toolbar == null ) return;
        EditorApplication.update -= OnUpdate;
        AddHandler( toolbar );
    }

    private static void AddHandler( object toolbar )
    {
        var container   = GUI_VIEW_IMGUI_CONTAINER.GetValue( toolbar, null ) as IMGUIContainer;
        var handler     = IMGUI_CONTAINER_ON_GUI_HANDLER.GetValue( container ) as Action;

        handler += OnGUI;

        IMGUI_CONTAINER_ON_GUI_HANDLER.SetValue( container, handler );
    }

    /// <summary>
    /// 
    /// </summary>
    private static void OnGUI()
    {
        float widthSize = 700.0f * ( (float)Screen.width / 1920); 
        var rect    = new Rect( widthSize, 4, 100, 24 );

        if ( GUI.Button( rect, ChangeText() ) )
        {
            UnityEngine.Object[] allObjectArray = Resources.FindObjectsOfTypeAll(typeof(GameObject));
            isDebug = !isDebug;
            foreach (UnityEngine.Object obj in allObjectArray) 
            {
                if (AssetDatabase.GetAssetOrScenePath(obj).Contains(".unity") && obj.name == "DebugTool")
                {
                    var tmpObj = (GameObject)obj;
                    tmpObj.SetActive(isDebug);
                }
            }
        }
        
    }

    private static string ChangeText()
    {
        if( isDebug )
            return "本番モードへ";

        return "デバッグモードへ";
    }
}