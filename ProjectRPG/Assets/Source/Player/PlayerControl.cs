/// <summary>
/// プレイヤーの基本的な処理
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR                
using UnityEditor;                 
#endif   

public class PlayerControl : MonoBehaviour
{
    private DebugManager _debugManager;
    private GameObject debugObject;

    /// <summary>
    /// 初期化
    /// </summary>
    public void initialize() 
    {
        #if UNITY_EDITOR
            debugObject = GameObject.Find("DebugTool");
            if(debugObject)
            {
                _debugManager = debugObject.GetComponent<DebugManager>();
                if(!_debugManager)
                {
                    Debug.Log("DebugManagerクラスを探すことが出来ませんでした");
                    EditorApplication.isPaused = true;
                }
            }
        #endif
    }

    /// <summary>
    /// プレイヤーの移動処理の制御
    /// </summary>
    /// <param name="PlayerStatus">プレイヤーの情報</param>
    public void PlayerController( ref PlayerManager.PLAYER_STATUS PlayerStatus ) 
    {
        Move( ref PlayerStatus._transform , PlayerStatus.fSpeed , PlayerStatus.LayerObjectList);
        //jump(ref PlayerStatus.isGround , ref PlayerStatus._rigidbody);
    }
    
    /// <summary>
    /// 移動の処理
    /// </summary>
    /// <param name="_rigidbody">物理エンジン</param>
    private void Move( ref Transform _transform　, float Speed , int LayerMask)
    {
        var vx =_transform.position.x;
        Vector3 nextPos = _transform.position;
        Vector3 direction = new Vector3( 0 , 0 , 0 );
        float directionSize = _transform.localScale.x / 2;
        if(InputManager.GetButtonHorizontal() > 0.0f)
        {   
            vx = _transform.position.x + Speed;//現在の位置に対してこれから行く場所
            direction = new Vector3( directionSize , 0 , 0 );//　右
        }
        else if(InputManager.GetButtonHorizontal() < -0.0f)
        {
            vx = _transform.position.x + -Speed;//現在の位置に対してこれから行く場所
            direction = new Vector3( -directionSize , 0 , 0 );//　左
        }
        
        nextPos = new Vector3( vx , _transform.position.y , _transform.position.z );
        RaycastHit2D hit = Physics2D.Raycast( nextPos , direction , directionSize ,LayerMask);
        Debug.DrawRay( nextPos , direction, Color.blue , 0);// 可視化

        if( hit.collider != null && hit.collider.gameObject.tag == "Floor" )
        {
            nextPos.x = _transform.position.x;
            #if UNITY_EDITOR
                if(debugObject)
                {
                    _debugManager.HitObject("Floor");
                }
            #endif
        }
        else
        {
            #if UNITY_EDITOR
                if(debugObject)
                {
                    _debugManager.HitObject("");
                }
            #endif
        }

        _transform.position = nextPos;
    }

    /// <summary>
    /// マ〇オジャンプの処理
    /// </summary>
    /// <param name="isGround">地面に接しているかフラグ</param>
    /// <param name="_rigidbody">物理エンジン</param>
    private void jump(ref bool isGround , ref Rigidbody2D _rigidbody) 
    {
        var speed = 10.0f;
        var xv = speed * Input.GetAxis("Horizontal");

        _rigidbody.velocity = new Vector2(xv, Mathf.Max(-5.0f, _rigidbody.velocity.y));

        var jumpPower = 5.0f;
        if(isGround == true && Input.GetKeyDown(KeyCode.Space))
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, jumpPower);
        }
        if(isGround == false && _rigidbody.velocity.y > 0.0f && Input.GetKey(KeyCode.Space))
        {
            _rigidbody.gravityScale = 0.5f;
        }
        else 
        {
            _rigidbody.gravityScale = 1.0f;
        }
    }
}
