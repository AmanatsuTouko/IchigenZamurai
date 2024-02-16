using System.Collections.Generic;
using UnityEngine;

public class InputJoyconManager : MonoBehaviour
{
    // 使い方
    // // 斬撃の取得のための更新
    // SlashUpdate();
    // if(_inputJoyConManager.IsSlashed())
    // {
    //     Slash(GetSlashDirection());
    // }

    // JoyCon
    private List<Joycon> m_joycons;
    
    // JoyConが使えるかどうか
    private bool _canUseJoyCon = true;

    // 斬撃が発生したかを監視する変数
    private bool _isSlashing = false; // 斬撃中か
    Vector2 _slashPosStart;           // 斬撃の開始時のポインターの位置
    Vector2 _slashPosEnd;             // 斬撃の終了時のポインターの位置

    public PointerManager pointerManager;
    private float ACCEL_THRESHOLD = 5.0f;

    //斜めと真っすぐの入力角度の範囲
    private float DIAGONAL_DEGREE = 27.5f;
    private float STRAIGHT_DEGREE = 17.5f;


    void Start()
    {
        SetControllers();
    }

    private void SetControllers()
    {
        m_joycons = JoyconManager.Instance.j;
        if (m_joycons == null || m_joycons.Count <= 0) 
        {
            //Joycon接続がない場合には起動しない
            _canUseJoyCon = false;
            return;
        }
    }
    
    // どの方向に斬ったか
    private SlashConstant.Direction _slashDirect;

    // このフレームで斬撃が行われたかどうか
    private bool _isSlashedThisFrame = false;

    public bool IsSlashed()
    {
        return _isSlashedThisFrame;
    }

    public SlashConstant.Direction GetSlashDirection()
    {
        return _slashDirect;
    }

    // 斬撃を取得しているかを更新する
    // SlashManagerから呼び出す
    // SlashManager -> InputJoyConManager と実行順を制御するため、Update()関数にはしない
    public void SlashUpdate()
    {
        // 接続できなかった場合は何もしない
        if(!_canUseJoyCon) return;

        // 斬撃フラグのリセット
        _isSlashedThisFrame = false;

        //加速度の値を取得する
        Vector3 accel = m_joycons[0].GetGyro();

        //加速度が閾値を上回ったタイミングのPosを記録
        //加速度が閾値を下回ったタイミングのPosを記録
        //差分を求めて斬撃の角度とする
        if(Mathf.Abs(accel.z) > ACCEL_THRESHOLD && _isSlashing == false)
        {
            _isSlashing = true;
            _slashPosStart = pointerManager.pos;
        }
        
        else if(Mathf.Abs(accel.z) < ACCEL_THRESHOLD && _isSlashing == true)
        {
            _isSlashing = false;
            _slashPosEnd = pointerManager.pos;

            // 斬撃フラグのON
            _isSlashedThisFrame = true;

            // 斬撃の発生
            Vector2 subPos2 = _slashPosEnd - _slashPosStart;
            float degree = Mathf.Atan2(subPos2.y, subPos2.x) * Mathf.Rad2Deg;
            degree += 180;

            // 角度を補正する
            if (0 < degree && degree < STRAIGHT_DEGREE) _slashDirect = SlashConstant.Direction.Left;
            else if (STRAIGHT_DEGREE < degree && degree < STRAIGHT_DEGREE + DIAGONAL_DEGREE * 2) _slashDirect = SlashConstant.Direction.DownLeft;
            else if (STRAIGHT_DEGREE + DIAGONAL_DEGREE * 2 < degree && degree < STRAIGHT_DEGREE * 3 + DIAGONAL_DEGREE * 2) _slashDirect = SlashConstant.Direction.Down;
            else if (STRAIGHT_DEGREE * 3 + DIAGONAL_DEGREE * 2 < degree && degree < STRAIGHT_DEGREE * 3 + DIAGONAL_DEGREE * 4) _slashDirect = SlashConstant.Direction.DownRight;
            else if (STRAIGHT_DEGREE * 3 + DIAGONAL_DEGREE * 4 < degree && degree < STRAIGHT_DEGREE * 5 + DIAGONAL_DEGREE * 4) _slashDirect = SlashConstant.Direction.Right;
            else if (STRAIGHT_DEGREE * 5 + DIAGONAL_DEGREE * 4 < degree && degree < STRAIGHT_DEGREE * 5 + DIAGONAL_DEGREE * 6) _slashDirect = SlashConstant.Direction.UpRight;
            else if (STRAIGHT_DEGREE * 5 + DIAGONAL_DEGREE * 6 < degree && degree < STRAIGHT_DEGREE * 7 + DIAGONAL_DEGREE * 6) _slashDirect = SlashConstant.Direction.Up;
            else if (STRAIGHT_DEGREE * 7 + DIAGONAL_DEGREE * 6 < degree && degree < STRAIGHT_DEGREE * 7 + DIAGONAL_DEGREE * 8) _slashDirect = SlashConstant.Direction.UpLeft;
            else if (STRAIGHT_DEGREE * 7 + DIAGONAL_DEGREE * 8 < degree && degree < STRAIGHT_DEGREE * 9 + DIAGONAL_DEGREE * 8) _slashDirect = SlashConstant.Direction.Left;
        }
    }

    // 振動
    public void SetRumble(float low_freq, float high_freq, float amp, int time)
    {
        if(m_joycons[0] != null) m_joycons[0].SetRumble(low_freq, high_freq, amp, time);
    }
}
