using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    public ScoreManager scoreManager;

    public bool isCollision = false;

    // 当たったオブジェクトに一限以外が含まれるかをカウントする
    public int CountDontSlash = 0;

    private void OnTriggerEnter(Collider other)
    {
        //切れていればコンボ継続のために当たった判定を残す
        isCollision = true;

        //スコアの加算とエフェクトの再生
        if(other.gameObject.tag == "gen1")
        {
            AddCount_AvoidTwiceCount(other, ScoreManager.HaiType.gen_1);
            EffectManager.Instance.PlayEffect(EffectManager.EffectType.Slash, other.transform.position);
        }
        else if (other.gameObject.tag == "gen2")
        {
            AddCount_AvoidTwiceCount(other, ScoreManager.HaiType.gen_2);
            EffectManager.Instance.PlayEffect(EffectManager.EffectType.IncorrectSlash, other.transform.position);
            CountDontSlash += 1;
        }
        else if (other.gameObject.tag == "gen3")
        {
            AddCount_AvoidTwiceCount(other, ScoreManager.HaiType.gen_3);
            EffectManager.Instance.PlayEffect(EffectManager.EffectType.IncorrectSlash, other.transform.position);
            CountDontSlash += 1;
        }
    }

    //2重で加算されることを防ぐ
    //ぶっ飛ぶ最初のフレームはまだ当たり判定が残っているときがあるので
    private void AddCount_AvoidTwiceCount(Collider other, ScoreManager.HaiType haiType)
    {
        if (other.GetComponent<IsSlashed>().isSlashed)
        {
            return;
        }
        else
        {
            other.GetComponent<IsSlashed>().isSlashed = true;
            scoreManager.AddCount(haiType);
        }
    }

    // 当たり判定のリセット
    public void ResetCountDontSlash()
    {
        CountDontSlash = 0;
    }
}
