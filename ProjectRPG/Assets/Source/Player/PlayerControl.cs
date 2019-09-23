/// <summary>
/// プレイヤーの基本的な処理
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    public void PlayerController(ref PlayerManager.PLAYER_STATUS PlayerStatus) {
        Move(ref PlayerStatus._rigidbody);
        jump(ref PlayerStatus.isGround , ref PlayerStatus._rigidbody);
    }

    private void Move(ref Rigidbody2D _rigidbody)
    {
        var speed = 5.0f;
        var vx = speed * Input.GetAxis("Horizontal");
        _rigidbody.velocity = new Vector2(vx, _rigidbody.velocity.y);
    }


    private void jump(ref bool isGround , ref Rigidbody2D _rigidbody) 
    {
        var jumpPower = 10.0f;
        if(Input.GetKeyDown(KeyCode.Space)){
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, jumpPower);

        }
    }
}
