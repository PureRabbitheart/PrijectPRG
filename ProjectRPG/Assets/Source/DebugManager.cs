using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
#if UNITY_EDITOR 
using UnityEditor;
using UnityEditor.Experimental.UIElements;//Editor拡張で使うためのusing
#endif

public class DebugManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI InputManagerText;

    void Start()
    {
        
    }

    /// <summary>
    /// 表示更新処理
    /// </summary>
    void Update()
    {
        InputManagerText.text = "Width: " + InputManager.GetAxisHorizontal().ToString() + "\n";
    }

    /// <summary>
    /// inspectorを日本語表示にする
    /// </summary>
#if UNITY_EDITOR
    [CustomEditor(typeof(DebugManager))]
    public class GunEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DebugManager _debugManager = target as DebugManager;

            _debugManager.InputManagerText = EditorGUILayout.ObjectField("入力状態のデバッグテキスト", _debugManager.InputManagerText, typeof(TextMeshProUGUI), true) as TextMeshProUGUI;
        }
    }
#endif
}
