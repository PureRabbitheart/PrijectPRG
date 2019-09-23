/// <summary>
/// プレイヤーの基本的な処理
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Rigidbody2D _rigidbody;

    void Start () {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update () {
        Move();
        jump();
    }

    public void Move()
    {
        var speed = 5.0f;
        var vx = speed * Input.GetAxis("Horizontal");
        _rigidbody.velocity = new Vector2(vx, _rigidbody.velocity.y);
    }


    public void jump() 
    {
        var jumpPower = 10.0f;
        if(Input.GetKeyDown(KeyCode.Space)){
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, jumpPower);

        }
    }
}
