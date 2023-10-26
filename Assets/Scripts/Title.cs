using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    //A�{�^���̉��������m����
    //�f�[�^�̗���� StartPoint -> GameManager -> Title
    public GameManager gameManager;

    public GameObject title_logo;
    
    //���[�h�I��
    //0:��I���@1:�`���[�g���A�� 2:���̂܂܃X�^�[�g
    private int select_mode = 0;

    public GameObject mode_select_obj;
    public RectTransform pointerImage;
    public GameObject selected_frame;
    public RectTransform selected_frame_pos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            select_mode = 1;
            selected_frame_pos.anchoredPosition = new Vector3(-450, -200, 0);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            select_mode = 2;
            selected_frame_pos.anchoredPosition = new Vector3(450, -200, 0);
        }
    }

    public IEnumerator Generate()
    {
        //�^�C�g���̕\��
        title_logo.SetActive(true);

        //A�{�^�����͂�ҋ@����
        while (true)
        {
            yield return new WaitForSeconds(0.03f);
            if (gameManager.ButtonDown_A == true)
            {
                gameManager.ButtonDown_A = false;
                break;
            }
        }

        //�^�C�g���̔�\��
        title_logo.SetActive(false);

        //���ʉ���炷


        //�l���I����\��

        //�`���[�g���A���̗L����I��
        mode_select_obj.SetActive(true);

        //A�{�^�����͂�ҋ@����
        gameManager.ButtonDown_A = false;
        while (true)
        {
            yield return new WaitForSeconds(0.03f);
            if (gameManager.ButtonDown_A == true && select_mode != 0)
            {
                gameManager.ButtonDown_A = false;
                break;
            }
            else if(select_mode == 0)
            {
                gameManager.ButtonDown_A = false;
            }

            //X:50~850  Y:40~-460
            //�|�C���^�[�J�[�\���̈ʒu�ɂ���āA�I����ς���
            int posX = (int)pointerImage.anchoredPosition.x;
            int posY = (int)pointerImage.anchoredPosition.y;
            if (-460 < posY && posY < 40)
            {
                //���̑I��
                if (-850 < posX && posX < -50)
                {
                    select_mode = 1;
                    //Debug.Log("�`���[�g���A����I��");
                    //�Ԙg���ړ�������
                    selected_frame_pos.anchoredPosition = new Vector3(-450, -200, 0);
                }
                //�E�̑I��
                else if (50 < posX && posX < 850)
                {
                    select_mode = 2;
                    //Debug.Log("���̂܂܃X�^�[�g��I��");
                    //�Ԙg���ړ�������
                    selected_frame_pos.anchoredPosition = new Vector3(450, -200, 0);
                }
            }

            if(select_mode == 0)
            {
                selected_frame.SetActive(false);
            }
            else
            {
                selected_frame.SetActive(true);
            }
        }
        //�`���[�g���A���I������GameManager�ɑ���
        yield return StartCoroutine(gameManager.SetTutorial(select_mode));

        //������ԂɃ��Z�b�g
        select_mode = 0;
        selected_frame.SetActive(false);

        //�`���[�g���A���I��UI���\��
        mode_select_obj.SetActive(false);

        yield return 0;
    }
}
