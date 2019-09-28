/// <summary>
/// キーボードとゲームパッドを統一する処理
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputManager
{
    const float MIN_TILT_VALUE = 0.2f;

    /// <summary>
    /// keyboard…[←] [→] [A] [D]
    /// GamePad …アナログスティック左
    /// float型で返す [-1] ~ [1] で返す　無操作は　[0]
    /// </summary>
    public static float GetAxisHorizontal()
    {

        //　→　と　Dキーを押したら
        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            return 1.0f;
        }

        //　←　と　Aキーを押したら
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            return -1.0f;
        }

        //　アナログステックを右に倒したら
        if(Input.GetAxis("Horizontal") > MIN_TILT_VALUE)
        {
            return Input.GetAxis("Horizontal");
        }

        //　アナログステックを左に倒したら
        if(Input.GetAxis("Horizontal") < -MIN_TILT_VALUE)
        {
            return Input.GetAxis("Horizontal");
        }
        
        return 0.0f;
    }

    /// <summary>
    /// keyboard…[←] [→] [A] [D]
    /// GamePad …アナログスティック左
    /// int型で [-1] [0] [1] で返す
    /// </summary>
    public static int GetButtonHorizontal()
    {

        //　→　と　Dキーを押したら
        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            return 1;
        }

        //　←　と　Aキーを押したら
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            return -1;
        }

        //　アナログステックを右に倒したら
        if(Input.GetAxis("Horizontal") > MIN_TILT_VALUE)
        {
            return 1;
        }

        //　アナログステックを左に倒したら
        if(Input.GetAxis("Horizontal") < -MIN_TILT_VALUE)
        {
            return -1;
        }
        
        return 0;
    }
}
