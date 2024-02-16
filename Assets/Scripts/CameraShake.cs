using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // 揺れ幅
    [SerializeField] float _shakeWidth = 10;
    // 揺れ時間
    [SerializeField] float _shakeSec = 0.5f;

    // カメラの振動を行う
    public void Shake(SlashConstant.Direction shakeDirection)
    {
        StartCoroutine(ShakeCoroutine(shakeDirection));
    }

    // 方向を指定して、N秒かけて振動させる
    IEnumerator ShakeCoroutine(SlashConstant.Direction shakeDirection)
    {
        // 減衰振動にするためのイージング
        var Ease = Easing.GetEasingMethod(Easing.Ease.OutExpo);
        // 振動する方向の取得
        var shakeDirVec = SlashConstant.Vec[(int)shakeDirection];
        
        // 同じパターンの繰り返しに見えるので
        // 揺れ幅に対して、若干の乱数を設定する
        float random = Random.Range(0.7f, 1.0f);
        // 揺れる方向に対しても、若干ずれるようにする
        Vector2 randomNoizeVec = new Vector2(Random.Range(0.0f, 0.5f), Random.Range(0.0f, 0.5f));
        shakeDirVec += randomNoizeVec;

        // 割合が0->1になるまで繰り返す
        float ratio = 0;
        while(true)
        {
            yield return null;
            ratio += Time.deltaTime / _shakeSec;

            // 0～2PIの値に変換
            float rad = Mathf.Lerp(0, 2*Mathf.PI, ratio);
            // 横の揺れ幅
            Vector2 shakedPos = shakeDirVec * (Mathf.Sin(rad) * _shakeWidth *  (1- Ease(ratio))) * random;

            if(ratio >= 1.0f)
            {
                // 元の位置に戻す
                transform.localPosition = Vector3.zero;
                break;
            }

            // 位置の反映
            transform.localPosition = shakedPos;
        }
    }
}
