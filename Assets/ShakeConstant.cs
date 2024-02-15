using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// SlashManagerとCameraShakeで共通して、振った方向を扱えるようにする
public static class ShakeConstant
{
    public enum Direction
    {
        Left,
        DownLeft,
        Down,
        DownRight,
        Right,
        UpRight,
        Up,
        UpLeft
    }

    // 斬る角度
    public static float[] SlashDegree = new float[]
    {
        0,   // Left
        30,  // DownLeft
        90,  // Down
        150, // DownRight
        180, // Right
        210, // UpRight
        270, // Up
        330  // UpLeft
    };

    public static Vector2[] Vec = new Vector2[]
    {
        new Vector2(-1,  0), // Left
        new Vector2(-1, -1), // DownLeft
        new Vector2( 0, -1), // Down
        new Vector2( 1, -1), // DownRight
        new Vector2( 1,  0), // Right
        new Vector2( 1,  1), // UpRight
        new Vector2( 0,  1), // Up
        new Vector2(-1,  1), // UpLeft
    };
}
