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
    private float diagonalAngle = 27.5f;
    private float straightAngle = 17.5f;

    //JoyCon
    private List<Joycon> m_joycons;
    private Joycon m_joyconL;
    private Joycon m_joyconR;
    
    private bool canUseJoycon = true;


    private bool GetPosState = false;
    Vector2 StartSlashPos = new Vector2(0, 0);
    Vector2 EndSlashPos = new Vector2(0, 0);

    //スコア処理
    public ScoreCounter scoreCounter;
    public ScoreManager scoreManager;

    //効果音
    public AudioSource audioSource_slash;
    public AudioSource audioSource_slash_NonHit;

    // 入力
    private InputManager _input = InputManager.Instance;

    void Start()
    {
        SetControllers();
        videoClipTime = (1 / videoPlayer.frameRate) * (float)videoPlayer.frameCount;

        //Joycon接続がない場合には起動しない
        if (m_joycons == null || m_joycons.Count <= 0) {
            canUseJoycon = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //再生中は入力できないようにする
        if (videoPlayer.isPlaying) return;

        //キーボード入力できるようにする
        if (_input.IsInputLeft())slash(0);
        if (_input.IsInputDown())slash(90);
        if (_input.IsInputRight())slash(180);
        if (_input.IsInputUp())slash(270);

        // 斜め
        if (_input.IsInputDownLeft())slash(30);
        if (_input.IsInputDownRight())slash(150);
        if (_input.IsInputUpRight())slash(210);
        if (_input.IsInputUpLeft())slash(330);


        //加速度の値を取得する
        Vector3 accel = new Vector3(0,0,0);
        if (canUseJoycon)
        {
            accel = m_joycons[0].GetGyro();
        }

        //加速度が閾値を上回ったタイミングのPosを記録
        //加速度が閾値を下回ったタイミングのPosを記録
        //差分を求めて斬撃の角度とする
        if(MathF.Abs(accel.z) > ACCEL_THRESHOLD && GetPosState == false)
        {
            GetPosState = true;
            StartSlashPos = pointerManager.pos;
        }
        if(MathF.Abs(accel.z) < ACCEL_THRESHOLD && GetPosState == true)
        {
            GetPosState = false;
            EndSlashPos = pointerManager.pos;

            //斬撃の発生
            Vector2 subPos2 = EndSlashPos - StartSlashPos;
            float theta = Mathf.Atan2(subPos2.y, subPos2.x) * Mathf.Rad2Deg;
            theta += 180;

            //角度を補正する
            if (0 < theta && theta < straightAngle) theta = 0;
            else if (straightAngle < theta && theta < straightAngle + diagonalAngle * 2) theta = 30;
            else if (straightAngle + diagonalAngle * 2 < theta && theta < straightAngle * 3 + diagonalAngle * 2) theta = 90;
            else if (straightAngle * 3 + diagonalAngle * 2 < theta && theta < straightAngle * 3 + diagonalAngle * 4) theta = 150;
            else if (straightAngle * 3 + diagonalAngle * 4 < theta && theta < straightAngle * 5 + diagonalAngle * 4) theta = 180;
            else if (straightAngle * 5 + diagonalAngle * 4 < theta && theta < straightAngle * 5 + diagonalAngle * 6) theta = 210;
            else if (straightAngle * 5 + diagonalAngle * 6 < theta && theta < straightAngle * 7 + diagonalAngle * 6) theta = 270;
            else if (straightAngle * 7 + diagonalAngle * 6 < theta && theta < straightAngle * 7 + diagonalAngle * 8) theta = 330;
            else if (straightAngle * 7 + diagonalAngle * 8 < theta && theta < straightAngle * 9 + diagonalAngle * 8) theta = 0;

            slash(theta);
        }
    }

    private void slash(float angle)
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
