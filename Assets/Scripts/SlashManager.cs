using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class SlashManager : MonoBehaviour
{
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] RectTransform rawImageTransform;
    [SerializeField] GameObject slashColliderCylinder;
    [SerializeField] Transform slashColliderCylinderTransform;

    [SerializeField] float videoClipTime = 0.0f;

    public PointerManager pointerManager;
    private float ACCEL_THRESHOLD = 5.0f;

    //斜めと真っすぐの入力角度の範囲
    private float DIAGONAL_DEGREE = 27.5f;
    private float STRAIGHT_DEGREE = 17.5f;

    //JoyCon
    private List<Joycon> m_joycons;
    private Joycon m_joyconL;
    private Joycon m_joyconR;
    
    private bool _canUseJoyCon = true;


    private bool _isSlashing = false;
    Vector2 _slashPosStart = new Vector2(0, 0);
    Vector2 _slashPosEnd = new Vector2(0, 0);

    //スコア処理
    public ScoreCounter scoreCounter;
    public ScoreManager scoreManager;

    //効果音
    public AudioSource audioSource_slash;
    public AudioSource audioSource_slash_NonHit;

    void Start()
    {
        SetControllers();
        videoClipTime = (1 / videoPlayer.frameRate) * (float)videoPlayer.frameCount;

        //Joycon接続がない場合には起動しない
        if (m_joycons == null || m_joycons.Count <= 0) {
            _canUseJoyCon = false;
        }
    }

    // 斬る方向と入力すべき回転角の対応表
    private class SlashAngle
    {
        public const float Left = 0;
        public const float DownLeft = 30;
        public const float Down = 90;
        public const float DownRight = 150;
        public const float Right = 180;
        public const float UpRight = 210;
        public const float Up = 270;
        public const float UpLeft = 330;
    }

    // Update is called once per frame
    void Update()
    {
        //再生中は入力できないようにする
        if (videoPlayer.isPlaying) return;

        //キーボード入力できるようにする
        if (InputManager.Instance.IsInputLeft())Slash(SlashAngle.Left);
        if (InputManager.Instance.IsInputDown())Slash(SlashAngle.Down);
        if (InputManager.Instance.IsInputRight())Slash(SlashAngle.Right);
        if (InputManager.Instance.IsInputUp())Slash(SlashAngle.Up);

        // 斜め
        if (InputManager.Instance.IsInputDownLeft())Slash(SlashAngle.DownLeft);
        if (InputManager.Instance.IsInputDownRight())Slash(SlashAngle.DownRight);
        if (InputManager.Instance.IsInputUpRight())Slash(SlashAngle.UpRight);
        if (InputManager.Instance.IsInputUpLeft())Slash(SlashAngle.UpLeft);

        //加速度の値を取得する
        Vector3 accel = new Vector3(0,0,0);
        if (_canUseJoyCon)
        {
            accel = m_joycons[0].GetGyro();
        }

        //加速度が閾値を上回ったタイミングのPosを記録
        //加速度が閾値を下回ったタイミングのPosを記録
        //差分を求めて斬撃の角度とする
        if(MathF.Abs(accel.z) > ACCEL_THRESHOLD && _isSlashing == false)
        {
            _isSlashing = true;
            _slashPosStart = pointerManager.pos;
        }
        if(MathF.Abs(accel.z) < ACCEL_THRESHOLD && _isSlashing == true)
        {
            _isSlashing = false;
            _slashPosEnd = pointerManager.pos;

            //斬撃の発生
            Vector2 subPos2 = _slashPosEnd - _slashPosStart;
            float degree = Mathf.Atan2(subPos2.y, subPos2.x) * Mathf.Rad2Deg;
            degree += 180;

            //角度を補正する
            if (0 < degree && degree < STRAIGHT_DEGREE) degree = SlashAngle.Left;
            else if (STRAIGHT_DEGREE < degree && degree < STRAIGHT_DEGREE + DIAGONAL_DEGREE * 2) degree = SlashAngle.DownLeft;
            else if (STRAIGHT_DEGREE + DIAGONAL_DEGREE * 2 < degree && degree < STRAIGHT_DEGREE * 3 + DIAGONAL_DEGREE * 2) degree = SlashAngle.Down;
            else if (STRAIGHT_DEGREE * 3 + DIAGONAL_DEGREE * 2 < degree && degree < STRAIGHT_DEGREE * 3 + DIAGONAL_DEGREE * 4) degree = SlashAngle.DownRight;
            else if (STRAIGHT_DEGREE * 3 + DIAGONAL_DEGREE * 4 < degree && degree < STRAIGHT_DEGREE * 5 + DIAGONAL_DEGREE * 4) degree = SlashAngle.Right;
            else if (STRAIGHT_DEGREE * 5 + DIAGONAL_DEGREE * 4 < degree && degree < STRAIGHT_DEGREE * 5 + DIAGONAL_DEGREE * 6) degree = SlashAngle.UpRight;
            else if (STRAIGHT_DEGREE * 5 + DIAGONAL_DEGREE * 6 < degree && degree < STRAIGHT_DEGREE * 7 + DIAGONAL_DEGREE * 6) degree = SlashAngle.Up;
            else if (STRAIGHT_DEGREE * 7 + DIAGONAL_DEGREE * 6 < degree && degree < STRAIGHT_DEGREE * 7 + DIAGONAL_DEGREE * 8) degree = SlashAngle.UpLeft;
            else if (STRAIGHT_DEGREE * 7 + DIAGONAL_DEGREE * 8 < degree && degree < STRAIGHT_DEGREE * 9 + DIAGONAL_DEGREE * 8) degree = SlashAngle.Left;

            Slash(degree);
        }
    }

    private void Slash(float angle)
    {
        //効果音を鳴らす
        audioSource_slash.Play();

        StartCoroutine(SlashCoroutine(angle));
    }

    //一部処理を待機する必要があるのでコルーチンで
    IEnumerator SlashCoroutine(float angle)
    {
        angle += 180;
        rawImageTransform.rotation = Quaternion.Euler(0, 0, angle);

        yield return StartCoroutine(ScoreCounterCollisionReset());

        slashColliderCylinder.SetActive(true);
        slashColliderCylinderTransform.localRotation = Quaternion.Euler(0, 0, angle + 90);
        videoPlayer.Play();

        //判定時間を短くして、空振りの際の効果音を鳴らすようにした。
        yield return new WaitForSeconds(0.02f);

        if (scoreCounter.isCollision == true)
        {
            scoreManager.AddComboCount();
        }
        else
        {
            scoreManager.StopComboUp();
            //効果音を鳴らす
            audioSource_slash.Stop();
            audioSource_slash_NonHit.Play();
        }

        slashColliderCylinder.SetActive(false);
    }

    IEnumerator ScoreCounterCollisionReset()
    {
        //ScoreCounterの衝突発生の有無をオフにする
        scoreCounter.isCollision = false;
        yield return 0;
    }

    private void slashColliderActiveFalse()
    {
        slashColliderCylinder.SetActive(false);
    }
    private void SetControllers()
    {
        m_joycons = JoyconManager.Instance.j;
        if (m_joycons == null || m_joycons.Count <= 0) return;
        m_joyconL = m_joycons.Find(c => c.isLeft);
        m_joyconR = m_joycons.Find(c => !c.isLeft);
    }
}
