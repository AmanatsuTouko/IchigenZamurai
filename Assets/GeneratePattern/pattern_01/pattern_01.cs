using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pattern_01: MonoBehaviour
{
    public GameObject gen1;
    private int count = 12; //�쐬���閃���v�̌�

    public ScoreManager scoreManager;

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine("Generate");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //�O���N���X����Ăׂ�悤�ɂ���
    public IEnumerator Generate()
    {
        this.transform.localRotation = Quaternion.Euler(0, 180, 0);

        float prePosX = 1.0f;
        float posX = 0.0f;

        for(int i=0; i<count; i++)
        {
            //����X���W�ɖ����v���쐬����Ȃ��悤�ɂ���
            while (true)
            {
                posX = (int)Random.Range(-3, 3) * 2.5f;
                if(posX != prePosX)
                {
                    prePosX = posX;
                    break;
                }
            }
            Vector3 vec = new Vector3(posX, 0, 0);

            GameObject gameObject = Instantiate(gen1, transform);
            gameObject.transform.localPosition = vec;

            //�o�����̃J�E���g
            scoreManager.AddCount_spawn(ScoreManager.HaiType.gen_1);

            yield return new WaitForSeconds(2.0f);
        }

        yield return new WaitForSeconds(10.0f);
        
        yield return 0;
    }
}
