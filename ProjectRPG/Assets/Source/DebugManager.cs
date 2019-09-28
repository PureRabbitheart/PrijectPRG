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
    //入力値を表示するTextMeshPro
    [SerializeField]
    private TextMeshProUGUI InputManagerText;

    //objectに当たったかを表示するTextMeshPro
    [SerializeField]
    private TextMeshProUGUI HitObjectText;

    [SerializeField]
    private TextMeshProUGUI FPSText;

    int frameCount;
    float fPrevTime;

    void Start()
    {
        frameCount = 0;
        fPrevTime = 0.0f;
    }

    /// <summary>
    /// 表示更新処理
    /// </summary>
    void Update()
    {
        InputManagerText.text = "Width: " + InputManager.GetButtonHorizontal().ToString() + "\n";
        FPS(); 
    }

    /// <summary>
    /// 表示更新処理
    /// </summary>
    public void HitObject(string txt)
    {
        HitObjectText.text = "Hit: " + txt + "\n";
    }

    private void FPS() 
    {
        frameCount++;
        float time = Time.realtimeSinceStartup - fPrevTime;
    
        if (time >= 0.5f) {
            FPSText.text = "FPS: " + (frameCount / time).ToString();
    
            frameCount = 0;
            fPrevTime = Time.realtimeSinceStartup;
        }
    }

    /// <summary>
    /// inspectorを日本語表示にする
    /// </summary>
#if UNITY_EDITOR
    [CustomEditor(typeof(DebugManager))]
    public class DebugEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DebugManager _debugManager = target as DebugManager;

            _debugManager.InputManagerText = EditorGUILayout.ObjectField("入力状態のデバッグテキスト", _debugManager.InputManagerText, typeof(TextMeshProUGUI), true) as TextMeshProUGUI;//入力値
            _debugManager.HitObjectText = EditorGUILayout.ObjectField("当たり判定のデバッグテキスト", _debugManager.HitObjectText, typeof(TextMeshProUGUI), true) as TextMeshProUGUI;//当たり判定
            _debugManager.FPSText = EditorGUILayout.ObjectField("フレームレートのデバッグテキスト", _debugManager.FPSText, typeof(TextMeshProUGUI), true) as TextMeshProUGUI;//フレームレート判定
        }
    }
#endif
}
