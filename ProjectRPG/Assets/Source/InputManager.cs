/// <summary>
/// キーボードとゲームパッドを統一する処理
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    const float MIN_TILT_VALUE = 0.2f;

    /// <summary>
    /// keyboard…[←] [→] [A] [D]
    /// GamePad …アナログスティック左
    /// </summary>
    public float GetAxisHorizontal()
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
}
