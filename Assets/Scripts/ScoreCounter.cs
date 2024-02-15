using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    public ScoreManager scoreManager;

    public bool isCollision = false;

    private void OnTriggerEnter(Collider other)
    {
        //�؂�Ă���΃R���{�p���̂��߂ɓ�������������c��
        isCollision = true;

        //�X�R�A�̉��Z�ƃG�t�F�N�g�̍Đ�
        if(other.gameObject.tag == "gen1")
        {
            AddCount_AvoidTwiceCount(other, ScoreManager.HaiType.gen_1);
            EffectManager.Instance.PlayEffect(EffectManager.EffectType.Slash, other.transform.position);
        }
        else if (other.gameObject.tag == "gen2")
        {
            AddCount_AvoidTwiceCount(other, ScoreManager.HaiType.gen_2);
            EffectManager.Instance.PlayEffect(EffectManager.EffectType.IncorrectSlash, other.transform.position);
        }
        else if (other.gameObject.tag == "gen3")
        {
            AddCount_AvoidTwiceCount(other, ScoreManager.HaiType.gen_3);
            EffectManager.Instance.PlayEffect(EffectManager.EffectType.IncorrectSlash, other.transform.position);
        }
    }

    //2�d�ŉ��Z����邱�Ƃ�h��
    //�Ԃ���ԍŏ��̃t���[���͂܂������蔻�肪�c���Ă���Ƃ�������̂�
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
}
