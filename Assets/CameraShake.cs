using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // 揺れ幅
    [SerializeField] float _shakeWidth = 10;
    // 揺れ時間
    [SerializeField] float _shakeSec = 0.5f;

    // カメラの振動を行う
    public void Shake(ShakeConstant.Direction shakeDirection)
    {
        StartCoroutine(ShakeCoroutine(shakeDirection));
    }

    // 方向を指定して、N秒かけて振動させる
    IEnumerator ShakeCoroutine(ShakeConstant.Direction shakeDirection)
    {
        // 減衰振動にするためのイージング
        var Ease = Easing.GetEasingMethod(Easing.Ease.OutExpo);
        // 振動する方向の取得
        var shakeDirVec = ShakeConstant.Vec[(int)shakeDirection];

        // 割合が0->1になるまで繰り返す
        float ratio = 0;
        while(true)
        {
            yield return null;
            ratio += Time.deltaTime / _shakeSec;

            // 0～2PIの値に変換
            float rad = Mathf.Lerp(0, 2*Mathf.PI, ratio);
            // 横の揺れ幅
            Vector2 shakedPos = shakeDirVec * (Mathf.Sin(rad) * _shakeWidth *  (1- Ease(ratio)));

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
