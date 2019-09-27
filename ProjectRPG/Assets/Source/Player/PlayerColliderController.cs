using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR                
using UnityEditor;                 
#endif   

public class PlayerColliderController : MonoBehaviour
{
    private PlayerManager _playerManager = null;

    /// <summary>
    /// 初期化
    /// </summary>
    private void Awake() 
    {
        _playerManager = GetComponentInParent<PlayerManager>();
        if( _playerManager == null )
        {
            Debug.Log("PlayerManagerクラスを探すことが出来ませんでした");
            #if UNITY_EDITOR
                EditorApplication.isPaused = true;
            #else
                    Application.Quit();
            #endif
        }
    }

    /// <summary>
    /// コライダーが反応した１フレームのみ
    /// </summary>
    /// <param name="other">Collision2D</param>
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Floor")
        {
            _playerManager.PlayerStatus.isGround = true;
        }
    }

    /// <summary>
    /// コライダーが離れたら１フレームのみ
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Floor"){
            _playerManager.PlayerStatus.isGround = false;   
        }
    }
}
