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

    // Update is called once per frame
    void Update()
    {
        //再生中は入力できないようにする
        if (videoPlayer.isPlaying) return;

        //キーボード入力できるようにする
        if (InputManager.Instance.IsInputLeft())Slash(ShakeConstant.Direction.Left);
        if (InputManager.Instance.IsInputDown())Slash(ShakeConstant.Direction.Down);
        if (InputManager.Instance.IsInputRight())Slash(ShakeConstant.Direction.Right);
        if (InputManager.Instance.IsInputUp())Slash(ShakeConstant.Direction.Up);

        // 斜め
        if (InputManager.Instance.IsInputDownLeft())Slash(ShakeConstant.Direction.DownLeft);
        if (InputManager.Instance.IsInputDownRight())Slash(ShakeConstant.Direction.DownRight);
        if (InputManager.Instance.IsInputUpRight())Slash(ShakeConstant.Direction.UpRight);
        if (InputManager.Instance.IsInputUpLeft())Slash(ShakeConstant.Direction.UpLeft);

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

            // 斬撃の方向
            ShakeConstant.Direction direction = ShakeConstant.Direction.Left;

            //角度を補正する
            if (0 < degree && degree < STRAIGHT_DEGREE) direction = ShakeConstant.Direction.Left;
            else if (STRAIGHT_DEGREE < degree && degree < STRAIGHT_DEGREE + DIAGONAL_DEGREE * 2) direction = ShakeConstant.Direction.DownLeft;
            else if (STRAIGHT_DEGREE + DIAGONAL_DEGREE * 2 < degree && degree < STRAIGHT_DEGREE * 3 + DIAGONAL_DEGREE * 2) direction = ShakeConstant.Direction.Down;
            else if (STRAIGHT_DEGREE * 3 + DIAGONAL_DEGREE * 2 < degree && degree < STRAIGHT_DEGREE * 3 + DIAGONAL_DEGREE * 4) direction = ShakeConstant.Direction.DownRight;
            else if (STRAIGHT_DEGREE * 3 + DIAGONAL_DEGREE * 4 < degree && degree < STRAIGHT_DEGREE * 5 + DIAGONAL_DEGREE * 4) direction = ShakeConstant.Direction.Right;
            else if (STRAIGHT_DEGREE * 5 + DIAGONAL_DEGREE * 4 < degree && degree < STRAIGHT_DEGREE * 5 + DIAGONAL_DEGREE * 6) direction = ShakeConstant.Direction.UpRight;
            else if (STRAIGHT_DEGREE * 5 + DIAGONAL_DEGREE * 6 < degree && degree < STRAIGHT_DEGREE * 7 + DIAGONAL_DEGREE * 6) direction = ShakeConstant.Direction.Up;
            else if (STRAIGHT_DEGREE * 7 + DIAGONAL_DEGREE * 6 < degree && degree < STRAIGHT_DEGREE * 7 + DIAGONAL_DEGREE * 8) direction = ShakeConstant.Direction.UpLeft;
            else if (STRAIGHT_DEGREE * 7 + DIAGONAL_DEGREE * 8 < degree && degree < STRAIGHT_DEGREE * 9 + DIAGONAL_DEGREE * 8) direction = ShakeConstant.Direction.Left;

            Slash(direction);
        }
    }

    private void Slash(ShakeConstant.Direction direction)
    {
        //効果音を鳴らす
        audioSource_slash.Play();

        StartCoroutine(SlashCoroutine(direction));
    }

    // 間違った牌を斬った際に、斬った方向に振動できるように、方向を保存しておく
    // ScoreManagerから呼び出す
    public static ShakeConstant.Direction PreSlashDirection;

    //一部処理を待機する必要があるのでコルーチンで
    IEnumerator SlashCoroutine(ShakeConstant.Direction direction)
    {
        // 直前に切った方向の保存
        PreSlashDirection = direction;

        float angle = ShakeConstant.SlashDegree[(int)direction];

        angle += 180;
        rawImageTransform.rotation = Quaternion.Euler(0, 0, angle);

        yield return StartCoroutine(ScoreCounterCollisionReset());

        slashColliderCylinder.SetActive(true);
        slashColliderCylinderTransform.localRotation = Quaternion.Euler(0, 0, angle + 90);
        videoPlayer.Play();

        //判定時間を短くして、空振りの際の効果音を鳴らすようにした。
        yield return new WaitForSeconds(0.02f);

        // 何か切れたときはコンボを継続
        if (scoreCounter.isCollision == true)
        {
            scoreManager.AddComboCount();
        }
        // それ以外はコンボ断絶
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
