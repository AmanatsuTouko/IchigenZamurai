using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JoyConSample : MonoBehaviour
{
    private static readonly Joycon.Button[] m_buttons =
        Enum.GetValues(typeof(Joycon.Button)) as Joycon.Button[];

    private List<Joycon> m_joycons;
    private Joycon m_joyconL;
    private Joycon m_joyconR;
    private Joycon.Button? m_pressedButtonL;
    private Joycon.Button? m_pressedButtonR;

    //加速度の閾値(超えると斬撃判定が発生する)
    private float SLASH_THRESHOLD = 10.0f;
    private bool OnSlashing = false;

    private void Start()
    {
        SetControllers();
    }

    private void Update()
    {
        //m_pressedButtonL = null;
        //m_pressedButtonR = null;

        if (m_joycons == null || m_joycons.Count <= 0) return;
        //SetControllers();

        /*
        foreach (var button in m_buttons)
        {
            if (m_joyconL.GetButton(button))
            {
                m_pressedButtonL = button;
            }
            if (m_joyconR.GetButton(button))
            {
                m_pressedButtonR = button;
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Z))
        {
            m_joyconL.SetRumble(160, 320, 0.6f, 200);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            m_joyconR.SetRumble(160, 320, 0.6f, 200);
        }
        */

        //加速度の値を取得する
        var accel = m_joycons[0].GetGyro();

        //スラッシュ状態でない時
        if(OnSlashing == false)
        {
            //閾値を越えた場合にスラッシュ状態にする
            //if (accel.z > SLASH_THRESHOLD || accel.y > SLASH_THRESHOLD)
            if (MathF.Abs(accel.z) > SLASH_THRESHOLD)
            {
                OnSlashing = true;
                //斬撃のベクトルの表示
                //Debug.Log(accel.z + " " + accel.y);
                //Debug.Log(accel.z);
                //if (accel.z > SLASH_THRESHOLD) Debug.Log("右に切った！");
                //if (accel.z < SLASH_THRESHOLD) Debug.Log("左に切った！");
            }
        }

        //加速度の値が閾値よりも下回った場合にスラッシュ状態を解除する
        if(accel.z < SLASH_THRESHOLD)
        {
            OnSlashing = false;
        }
    }

    private void OnGUI()
    {
        var style = GUI.skin.GetStyle("label");
        style.fontSize = 24;

        if (m_joycons == null || m_joycons.Count <= 0)
        {
            GUILayout.Label("Joy-Con が接続されていません");
            return;
        }

        /*
        if (!m_joycons.Any(c => c.isLeft))
        {
            GUILayout.Label("Joy-Con (L) が接続されていません");
            return;
        }

        if (!m_joycons.Any(c => !c.isLeft))
        {
            GUILayout.Label("Joy-Con (R) が接続されていません");
            return;
        }
        */

        GUILayout.BeginHorizontal(GUILayout.Width(960));

        foreach (var joycon in m_joycons)
        {
            var isLeft = joycon.isLeft;
            var name = isLeft ? "Joy-Con (L)" : "Joy-Con (R)";
            var key = isLeft ? "Z キー" : "X キー";
            //var button = isLeft ? m_pressedButtonL : m_pressedButtonR;
            var stick = joycon.GetStick();
            var gyro = joycon.GetGyro();
            var accel = joycon.GetAccel();
            var orientation = joycon.GetVector();
            
            GUILayout.BeginVertical(GUILayout.Width(480));
            //GUILayout.Label(name);
            //GUILayout.Label(key + "：振動");
            //GUILayout.Label("押されているボタン：" + button);
            //GUILayout.Label(string.Format("スティック：({0}, {1})", stick[0], stick[1]));
            //gyro *= 10;
            //gyro = new Vector3(Mathf.Floor(gyro.x), Mathf.Floor(gyro.y), Mathf.Floor(gyro.z)) /10;
            //gyro = new Vector3(Mathf.FloorToInt(gyro.x), Mathf.FloorToInt(gyro.y), Mathf.FloorToInt(gyro.z));
            //Vector3Int gyroInt = new Vector3Int(Mathf.FloorToInt(gyro.x), Mathf.FloorToInt(gyro.y), Mathf.FloorToInt(gyro.z));
            //Vector3Int accelInt = new Vector3Int(Mathf.FloorToInt(accel.x), Mathf.FloorToInt(accel.y), Mathf.FloorToInt(accel.z));
            GUILayout.Label("ジャイロX：" + gyro.x);
            GUILayout.Label("ジャイロY：" + gyro.y);
            GUILayout.Label("ジャイロZ：" + gyro.z);
            GUILayout.Label("加速度X：" + accel.x);
            GUILayout.Label("加速度Y：" + accel.y);
            GUILayout.Label("加速度Z：" + accel.z);
            GUILayout.Label("傾きX：" + orientation.x);
            GUILayout.Label("傾きY：" + orientation.y);
            GUILayout.Label("傾きZ：" + orientation.z);
            GUILayout.Label("傾きW：" + orientation.w);
            GUILayout.EndVertical();
        }

        GUILayout.EndHorizontal();
    }

    private void SetControllers()
    {
        m_joycons = JoyconManager.Instance.j;
        if (m_joycons == null || m_joycons.Count <= 0) return;
        m_joyconL = m_joycons.Find(c => c.isLeft);
        m_joyconR = m_joycons.Find(c => !c.isLeft);
    }
}