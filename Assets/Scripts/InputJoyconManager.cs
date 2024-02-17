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

    // ポインターの位置取得
    public PointerManager pointerManager; 

    // 斬撃が発生したかを監視する変数
    private bool _isSlashing = false; // 斬撃中か
    Vector2 _slashPosStart;           // 斬撃の開始時のポインターの位置
    Vector2 _slashPosEnd;             // 斬撃の終了時のポインターの位置

    // どの方向に斬ったか
    private SlashConstant.Direction _slashDirect;

    // このフレームで斬撃が行われたかどうか
    private bool _isSlashedThisFrame = false;
    
    // 斬撃発生の為の加速度の閾値
    private const float ACCEL_THRESHOLD_TO_SLASH = 5.0f;

    // 入力の角度の範囲
    // 斜め入力は人間の動作としてはやりにくいので、やや範囲を広めにとっておく
    // 合わせて90度になるように
    private const float CROSS_DEGREE_RANGE = 35.0f;    // 例：真横に対して上下17.5度ずつが有効範囲
    private const float DIAGONAL_DEGREE_RANGE = 55.0f; // 例：斜めに対して上下27.5度ずつが有効範囲


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
        if(Mathf.Abs(accel.z) > ACCEL_THRESHOLD_TO_SLASH && _isSlashing == false)
        {
            _isSlashing = true;
            _slashPosStart = pointerManager.pos;
        }
        else if(Mathf.Abs(accel.z) < ACCEL_THRESHOLD_TO_SLASH && _isSlashing == true)
        {
            _isSlashing = false;
            _slashPosEnd = pointerManager.pos;

            // 斬撃フラグのON
            _isSlashedThisFrame = true;

            // 斬撃の発生
            Vector2 subPos2 = _slashPosStart - _slashPosEnd;
            float degree = Mathf.Atan2(subPos2.y, subPos2.x) * Mathf.Rad2Deg;

            // 角度を8方向に補正する
            // 得られる角度が反時計回りに 0～180度、-180度～-0度なので、準じて変換する
            // 第一第二象限 0～180度
            if(degree >= 0)
            {
                if(degree <= CROSS_DEGREE_RANGE/2)
                {
                    _slashDirect = SlashConstant.Direction.Left;
                }
                else if(degree <= CROSS_DEGREE_RANGE/2 + DIAGONAL_DEGREE_RANGE)
                {
                    _slashDirect = SlashConstant.Direction.DownLeft;
                }
                else if(degree <= CROSS_DEGREE_RANGE/2 + DIAGONAL_DEGREE_RANGE + CROSS_DEGREE_RANGE)
                {
                    _slashDirect = SlashConstant.Direction.Down;
                }
                else if(degree <= CROSS_DEGREE_RANGE/2 + DIAGONAL_DEGREE_RANGE * 2 + CROSS_DEGREE_RANGE)
                {
                    _slashDirect = SlashConstant.Direction.DownRight;
                }
                else
                {
                    _slashDirect = SlashConstant.Direction.Right;
                }
            }
            // 第三第四象限 -0度～-180度
            else
            {
                degree = Mathf.Abs(degree);

                if(degree <= CROSS_DEGREE_RANGE/2)
                {
                    _slashDirect = SlashConstant.Direction.Left;
                }
                else if(degree <= CROSS_DEGREE_RANGE/2 + DIAGONAL_DEGREE_RANGE)
                {
                    _slashDirect = SlashConstant.Direction.UpLeft;
                }
                else if(degree <= CROSS_DEGREE_RANGE/2 + DIAGONAL_DEGREE_RANGE + CROSS_DEGREE_RANGE)
                {
                    _slashDirect = SlashConstant.Direction.Up;
                }
                else if(degree <= CROSS_DEGREE_RANGE/2 + DIAGONAL_DEGREE_RANGE * 2 + CROSS_DEGREE_RANGE)
                {
                    _slashDirect = SlashConstant.Direction.UpRight;
                }
                else
                {
                    _slashDirect = SlashConstant.Direction.Right;
                }
            }
        }
    }

    // 振動
    public void SetRumble(float low_freq, float high_freq, float amp, int time)
    {
        // 接続できなかった場合は何もしない
        if(!_canUseJoyCon) return;
        
        if(m_joycons[0] != null) 
        {
            m_joycons[0].SetRumble(low_freq, high_freq, amp, time);
        }
    }
}
