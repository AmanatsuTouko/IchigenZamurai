using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class SlashManager : MonoBehaviour
{
    // 斬撃エフェクト
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] RectTransform rawImageTransform;
    [SerializeField] GameObject slashColliderCylinder;
    [SerializeField] Transform slashColliderCylinderTransform;

    // スコア処理
    public ScoreCounter scoreCounter;
    public ScoreManager scoreManager;

    // Joy-Conの斬撃方向の取得
    private InputJoyconManager _inputJoyconManager;

    void Awake()
    {
        _inputJoyconManager = GetComponent<InputJoyconManager>();
    }

    void Update()
    {
        // 斬撃アニメーションの再生中は入力できないようにする
        if (videoPlayer.isPlaying) return;

        // キーボード入力できるようにする
        // 上下左右
        if (InputManager.Instance.IsInputLeft())Slash(SlashConstant.Direction.Left);
        if (InputManager.Instance.IsInputDown())Slash(SlashConstant.Direction.Down);
        if (InputManager.Instance.IsInputRight())Slash(SlashConstant.Direction.Right);
        if (InputManager.Instance.IsInputUp())Slash(SlashConstant.Direction.Up);
        // 斜め
        if (InputManager.Instance.IsInputDownLeft())Slash(SlashConstant.Direction.DownLeft);
        if (InputManager.Instance.IsInputDownRight())Slash(SlashConstant.Direction.DownRight);
        if (InputManager.Instance.IsInputUpRight())Slash(SlashConstant.Direction.UpRight);
        if (InputManager.Instance.IsInputUpLeft())Slash(SlashConstant.Direction.UpLeft);

        // Joy-Conの入力がある場合、斬撃の方向を取得する
        _inputJoyconManager.SlashUpdate();
        if(_inputJoyconManager.IsSlashed())
        {
            Slash(_inputJoyconManager.GetSlashDirection());
        }
    }

    private void Slash(SlashConstant.Direction direction)
    {
        StartCoroutine(SlashCoroutine(direction));
    }

    // 間違った牌を斬った際に、斬った方向に振動できるように、方向を保存しておく
    // ScoreManagerから呼び出す
    public static SlashConstant.Direction PreSlashDirection;

    //一部処理を待機する必要があるのでコルーチンで
    IEnumerator SlashCoroutine(SlashConstant.Direction direction)
    {
        // 直前に切った方向の保存
        PreSlashDirection = direction;

        float angle = SlashConstant.SlashDegree[(int)direction];

        angle += 180;
        rawImageTransform.rotation = Quaternion.Euler(0, 0, angle);

        yield return StartCoroutine(ScoreCounterCollisionReset());

        slashColliderCylinder.SetActive(true);
        slashColliderCylinderTransform.localRotation = Quaternion.Euler(0, 0, angle + 90);
        videoPlayer.Play();

        // 効果音を鳴らす
        SoundManager.Instance.Play(SoundManager.SE.Slash);

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
            // 効果音を鳴らす
            SoundManager.Instance.Stop(SoundManager.SE.Slash);
            SoundManager.Instance.Play(SoundManager.SE.SlashNoHit);
        }

        slashColliderCylinder.SetActive(false);
    }

    IEnumerator ScoreCounterCollisionReset()
    {
        //ScoreCounterの衝突発生の有無をオフにする
        scoreCounter.isCollision = false;
        yield return 0;
    }
}
