using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    public ScoreManager scoreManager;

    public bool isCollision = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //�؂�Ă���΃R���{�p���̂��߂ɓ�������������c��
        isCollision = true;

        //�X�R�A�̉��Z
        if(other.gameObject.tag == "gen1")
        {
            AddCount_AvoidTwiceCount(other, ScoreManager.HaiType.gen_1);
        }
        else if (other.gameObject.tag == "gen2")
        {
            AddCount_AvoidTwiceCount(other, ScoreManager.HaiType.gen_2);
        }
        else if (other.gameObject.tag == "gen3")
        {
            AddCount_AvoidTwiceCount(other, ScoreManager.HaiType.gen_3);
        }
    }

    //2�d�ŉ��Z����邱�Ƃ�h��
    //�Ԃ���ԍŏ��̃t���[���͂܂������蔻�肪�c���Ă���Ƃ�������̂�
    private void AddCount_AvoidTwiceCount(Collider other, ScoreManager.HaiType haiType)
    {
        if (other.GetComponent<IsSlashed>().isSlashed) return;
        else
        {
            scoreManager.AddCount(haiType);
            other.GetComponent<IsSlashed>().isSlashed = true;
        }
    }
}
