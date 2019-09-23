/// <summary>
/// PlayerPrefsの中身をすぐに見れるエディター
/// kito ryo
/// 2019/09/23
/// </summary>
using UnityEngine;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;

using UnityEngine.UIElements;
#if UNITY_EDITOR 
using UnityEditor;
using UnityEditor.Experimental.UIElements;//Editor拡張で使うためのusing
#endif

public class PlayerPrefsTool : EditorWindow
{
    private List<PlayerPrefsData> _playerPrefData = null;
    private static readonly string[] NON_TARGET_KEY = { "unity.cloud_userid", "unity.player_sessionid", "unity.player_session_count", "UnityGraphicsQuality" };
    private string[] createData = new string[2];


    /// <summary>
    //// ダミーデータを保存する関数
    /// </summary>
    [MenuItem("Tools/PlayerPrefs/DummyDataAdd")]
    private static void DummyDataAdd()
    {
        PlayerPrefsTool window = new PlayerPrefsTool();
        window.SavePalyerPrefs("DummyData", "DummyData");
    }

    /// <summary>
    /// PlayerPrefsのデータウィンドウを出す処理
    /// </summary>
    [MenuItem("Tools/PlayerPrefs/PlayerPrefsWindow")]
    private static void Create()
    {
        var window = GetWindow<PlayerPrefsTool>("PlayerPrefs");
        window.minSize = new Vector2(500, 180);
        window.LoadPlayerPrefs();
    }

    private void OnGUI()
    {
        RenderToolbar();

        if (_playerPrefData == null || _playerPrefData.Count == 0)
        {
            EditorGUILayout.LabelField("データが存在しません");
            return;
        }

        RenderPrefsDataList();
        NewCreateBar();
    }

    /// <summary>
    /// PlayerPrefsの読み込み
    /// </summary>
    private void LoadPlayerPrefs()
    {
#if UNITY_EDITOR
        //リストの初期化
        if (_playerPrefData != null)
        {
            _playerPrefData.Clear();
        }
        else
        {
            _playerPrefData = new List<PlayerPrefsData>();
        }

        //PlayerPrefsのすべてのキーを検索する
        var keys = new List<string>();
        GetAllPlayerPrefKeys(ref keys);

        //リストの作成
        foreach (var key in keys)
        {
            _playerPrefData.Add(new PlayerPrefsData(key));
        }
#else
        _playerPrefData = null;
#endif
    }

    /// <summary>
    /// PlayerPrefsを保存する処理
    /// </summary>
    private void SavePalyerPrefs(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// 配列でPlayerPrefsを保存する処理
    /// </summary>
    private void ArraySavePalyerPrefs(List<PlayerPrefsData> data)
    {
        foreach (var list in data)
        {
            SavePalyerPrefs(list.Key, list.Value);
        }
    }

    /// <summary>
    /// PlayerPrefsを全て削除する
    /// </summary>
    private void AllDeletePlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }

    /// <summary>
    /// ツールバーの表示
    /// </summary>
    private void RenderToolbar()
    {
        using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar, GUILayout.ExpandWidth(true)))
        {
            if (GUILayout.Button("すべて削除", EditorStyles.toolbarButton))
            {
                Debug.Log("すべてのPlayerPrefsデータを削除しました。");
                AllDeletePlayerPrefs();
                _playerPrefData = null;
            }
            if (GUILayout.Button("再読み込み", EditorStyles.toolbarButton))
            {
                Debug.Log("PlayerPrefsの読み込み完了");
                LoadPlayerPrefs();
            }
            if (GUILayout.Button("保存", EditorStyles.toolbarButton))
            {
                AllDeletePlayerPrefs();
                ArraySavePalyerPrefs(_playerPrefData);
                LoadPlayerPrefs();
            }
        }
    }

    /// <summary>
    /// 新規PlayerPrefsのデータを作るバー
    /// </summary>
    private void NewCreateBar()
    {
        EditorGUILayout.BeginHorizontal();
        {
            createData[0] = EditorGUILayout.TextField("Key: ", createData[0]);
            createData[1] = EditorGUILayout.TextField("Value: ", createData[1]);
            if (GUILayout.Button("新規作成", GUILayout.Width(64), GUILayout.Height(16)))
            {
                SavePalyerPrefs(createData[0], createData[1]);
                LoadPlayerPrefs();
                createData[0] = "";
                createData[1] = "";
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    /// <summary>
    /// PlayerPrefsの一覧描画
    /// </summary>
    private void RenderPrefsDataList()
    {
        bool isDeleted = false;

        EditorGUILayout.BeginVertical();
        {
            for (int i = 0; i < _playerPrefData.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                {
                    _playerPrefData[i].Key = EditorGUILayout.TextField("Key: ", _playerPrefData[i].Key);
                    _playerPrefData[i].Value = EditorGUILayout.TextField("Value: ", _playerPrefData[i].Value);

                    if (_playerPrefData[i].Key.Length == 0)
                    {
                        _playerPrefData[i].Key = "MissingData(" + i + ")";
                    }
                    if (_playerPrefData[i].Value.Length == 0)
                    {
                        _playerPrefData[i].Value = "MissingData(" + i + ")";
                    }
                    if (GUILayout.Button("削除", GUILayout.Width(64), GUILayout.Height(16)))
                    {
                        AllDeletePlayerPrefs();
                        ArraySavePalyerPrefs(_playerPrefData);
                        _playerPrefData[i].Delete();
                        isDeleted = true;
                        Debug.Log("『" + _playerPrefData[i].Key + "』削除しました");
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        EditorGUILayout.EndVertical();

        if (isDeleted == true)
        {
            LoadPlayerPrefs();
        }
    }

    /// <summary>
    /// 内部情報の検索
    /// </summary>
    /// <param name="keys"></param>
    void GetAllPlayerPrefKeys(ref List<string> keys)
    {
        if (keys != null)
        {
            keys.Clear();
        }
        else
        {
            keys = new List<string>();
        }

        string regKeyPathPattern =
#if UNITY_EDITOR
                @"Software\Unity\UnityEditor\{0}\{1}";
#else
                @"Software\{0}\{1}";
#endif
        ;

        string regKeyPath = string.Format(regKeyPathPattern, UnityEditor.PlayerSettings.companyName, UnityEditor.PlayerSettings.productName);
        Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(regKeyPath);
        if (regKey == null)
        {
            return;
        }

        string[] playerPrefKeys = regKey.GetValueNames();
        for (int i = 0; i < playerPrefKeys.Length; i++)
        {
            string playerPrefKey = playerPrefKeys[i];
            playerPrefKey = playerPrefKey.Substring(0, playerPrefKey.LastIndexOf("_h"));
            if (playerPrefKey != NON_TARGET_KEY[0] && playerPrefKey != NON_TARGET_KEY[1] && playerPrefKey != NON_TARGET_KEY[2] && playerPrefKey != NON_TARGET_KEY[3])
            {
                keys.Add(playerPrefKey);
            }
        }

    }
}

/// <summary>
/// PlayerPrefsを扱いやすくするデータクラス
/// </summary>
public class PlayerPrefsData
{
    public string Key;
    public string Value;
    public PlayerPrefsData(string key)
    {
        Key = key;
        Value = GetValue(key);
    }
    public void Delete()
    {
        PlayerPrefs.DeleteKey(Key);
    }

    /// <summary>
    /// PlayerPrefsのstring,int,floatのどれかを返す
    /// </summary>
    private string GetValue(string key)
    {
        string value = PlayerPrefs.GetString(key, null);
        if (string.IsNullOrEmpty(value))
        {
            value = PlayerPrefs.GetInt(key, 0).ToString();
            if (value == "0")
            {
                value = PlayerPrefs.GetFloat(key, 0).ToString();
            }
        }
        return value;
    }
}