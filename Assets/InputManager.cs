using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // 頻用するので、簡易的な記述にしておく
    Func<UnityEngine.KeyCode, bool> _input = Input.GetKeyDown;

    public static InputManager Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this);   
        }
    }

    // Joy-Con入力とキーボード入力を受け取れるようにする

    // 上下左右 WASD
    // 斜め入力の対応
    // 左上 Q
    // 右上 E
    // 左下 Z
    // 右下 C

    public bool IsInputLeft()
    {
        return _input(KeyCode.LeftArrow) || _input(KeyCode.A);
    }

    public bool IsInputRight()
    {
        return _input(KeyCode.RightArrow) || _input(KeyCode.D);
    }

    public bool IsInputUp()
    {
        return _input(KeyCode.UpArrow) || _input(KeyCode.W);
    }

    public bool IsInputDown()
    {
        return _input(KeyCode.DownArrow) || _input(KeyCode.S);
    }

    // 斜め入力

    public bool IsInputUpLeft()
    {
        return (_input(KeyCode.UpArrow) && _input(KeyCode.LeftArrow)) || _input(KeyCode.Q);
    }

    public bool IsInputUpRight()
    {
        return (_input(KeyCode.UpArrow) && _input(KeyCode.RightArrow)) || _input(KeyCode.E);
    }

    public bool IsInputDownLeft()
    {
        return (_input(KeyCode.DownArrow) && _input(KeyCode.LeftArrow)) || _input(KeyCode.Z);
    }

    public bool IsInputDownRight()
    {
        return (_input(KeyCode.DownArrow) && _input(KeyCode.RightArrow)) || _input(KeyCode.C);
    }
}
