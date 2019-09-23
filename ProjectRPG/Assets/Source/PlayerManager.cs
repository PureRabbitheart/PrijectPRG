using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{ 

    //プレイヤーのステータス管理
    public struct PLAYER_STATUS
    {
        public bool isGround;
        public Rigidbody2D _rigidbody;
    }

    public PLAYER_STATUS PlayerStatus;

    PlayerControl _PlayerControl = new PlayerControl();

    
    private void Awake() 
    {
        PlayerStatus.isGround = false;
        PlayerStatus._rigidbody = GetComponent<Rigidbody2D>();
    }


    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update()
    {
        _PlayerControl.PlayerController(ref PlayerStatus);
    }
}
