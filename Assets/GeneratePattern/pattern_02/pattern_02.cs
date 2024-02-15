using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pattern_02 : MonoBehaviour
{
    public List<GameObject> gens = new List<GameObject>();

    private int count = 16; //�쐬���閃���v�̌�

    //�Ö��ŏ㉺���B���悤�ɂ���
    public RectTransform image_transform_up;
    public RectTransform image_transform_down;
    float moveAmount = 400.0f; //�Ö��̈ړ����x

    //�����蔻��̒�����ύX����
    public GameObject SlashCollider;

    //�o���񐔂̃J�E���g
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
    //�R���[�`����ҋ@���ď��ԂɎ��s����
    public IEnumerator Generate()
    {
        //�����蔻��̒����̕ύX
        SlashCollider.transform.localScale = new Vector3(1, 5, 1);

        yield return StartCoroutine(pattern02_setBlackout());

        yield return StartCoroutine(Main());

        yield return StartCoroutine(pattern02_deleteBlackout());

        //�����蔻��̒����̕ύX
        SlashCollider.transform.localScale = new Vector3(1, 10, 1);

        yield return 0;
    }

    IEnumerator Main()
    {
        this.transform.localRotation = Quaternion.Euler(0, 180, 0);
        Vector3 vec = new Vector3(0, 0, 0);

        for (int i = 0; i < count; i++)
        {
            //�����Ő�������
            int type = 0;
            int random = Random.Range(0, 2);
            if(random == 1)
            {
                type = Random.Range(1, 3);
            }

            GameObject gameObject = Instantiate(gens[type], transform);
            gameObject.transform.localPosition = vec;

            //�o���񐔂̃J�E���g
            if(type == 0)scoreManager.AddCount_spawn(ScoreManager.HaiType.gen_1);
            else if (type == 1) scoreManager.AddCount_spawn(ScoreManager.HaiType.gen_2);
            else if (type == 2) scoreManager.AddCount_spawn(ScoreManager.HaiType.gen_3);


            yield return new WaitForSeconds(2.0f);
        }

        yield return 0;
    }

    IEnumerator pattern02_setBlackout()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.01f);

            image_transform_up.Translate(0, Time.deltaTime * -moveAmount, 0);
            image_transform_down.Translate(0, Time.deltaTime * moveAmount, 0);

            if (image_transform_up.anchoredPosition.y < 400)
            {
                image_transform_up.anchoredPosition = new Vector3(0, 400, 0);
                image_transform_down.anchoredPosition = new Vector3(0, -400, 0);
                break;
            }
        }
        
        yield return 0;
    }
    IEnumerator pattern02_deleteBlackout()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.01f);

            image_transform_up.Translate(0, Time.deltaTime * moveAmount, 0);
            image_transform_down.Translate(0, Time.deltaTime * -moveAmount, 0);

            if (image_transform_up.anchoredPosition.y > 800)
            {
                image_transform_up.anchoredPosition = new Vector3(0, 800, 0);
                image_transform_down.anchoredPosition = new Vector3(0, -800, 0);
                break;
            }
        }

        yield return 0;
    }
}
