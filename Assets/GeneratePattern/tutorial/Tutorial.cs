using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public GameObject gen;

    Vector3[,] Pos = new Vector3[5, 3]; //�o���ʒu�̔z��
    private bool[,] alreadyPos = new bool[5, 3]; //���ɏo�����Ă���ʒu�̔z��

    public GameManager gameManager;

    //4�b�������Ń`���[�g���A�����I����悤�ɂ��邽�߂̃J�E���g
    private int buttonADownTimes = 0;

    //�������̃J�E���g�\���p��UI
    public List<Image> timesImages;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    IEnumerator ResetPos()
    {
        buttonADownTimes = 0;

        this.transform.localRotation = Quaternion.Euler(0, 180, 0);

        //�z�u��List�ɕۑ����Ă���
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                float posX = (i - 2) * -3.5f;
                float posY = (j - 1) * -3.5f;
                if (i == 1 || i == 3)
                {
                    if (j == 0) posY = 2.0f;
                    if (j == 1) posY = -2.0f;
                    if (j == 2) continue;
                }
                Pos[i, j] = new Vector3(posX, posY, 0);

                /*
                GameObject gameObject = Instantiate(gens[0], transform);
                gameObject.transform.localPosition = Pos[i, j];
                alreadyPos[i, j] = true;
                */
            }
        }
        Pos[1, 2] = new Vector3(0, 0, -5);
        Pos[3, 2] = new Vector3(0, 0, -5);

        //�����l�̐ݒ�
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                alreadyPos[i, j] = false;
            }
        }

        yield return 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator Generate()
    {
        //������
        yield return StartCoroutine(ResetPos());

        bool generate = true;
        int generateCount = 0;

        //�؂�ꂽ�ʒu�̂ݕ�������
        while (true)
        {
            //�����̂�2�b�����ɂ���B�R���[�`���̉��b���͒Z������
            if (generate)
            {
                //���܂��Ă��Ȃ��ꏊ�����߂Đ�������
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if ((i == 1 || i == 3) && j == 2) continue;

                        if (alreadyPos[i, j] == false)
                        {
                            alreadyPos[i, j] = true;

                            //��������
                            GameObject gameObject = Instantiate(gen, transform);
                            gameObject.transform.localPosition = Pos[i, j];
                            gameObject.transform.localScale = new Vector3(0, 0, 0);

                            //�v���g�ɂǂ��ɂ��邩�̏�����������
                            Tutoria_hai hai = gameObject.GetComponent<Tutoria_hai>();
                            hai.pos = new Vector2Int(i, j);
                            hai.tutorial = this;
                        }
                    }
                }
                generate = false;
            }

            generateCount += 1;
            if(generateCount > 3)
            {
                generateCount = 0;
                generate = true;
            }

            //2�b�����ɕ�������
            //yield return new WaitForSeconds(2.0f);

            yield return new WaitForSeconds(0.5f);


            //�I������
            if (gameManager.ButtonDown_A == true)
            {
                buttonADownTimes += 1;
            }
            else
            {
                buttonADownTimes = 0;
            }

            // Debug.Log(buttonADownTimes);

            //�b���ɉ����Ē������̐F��ς���
            if(buttonADownTimes == 1)
            {
                timesImages[0].color = new Color32((byte)0, (byte)0, (byte)0, (byte)255);
                timesImages[1].color = new Color32((byte)120, (byte)120, (byte)120, (byte)255);
                timesImages[2].color = new Color32((byte)120, (byte)120, (byte)120, (byte)255);
            }
            else if (buttonADownTimes == 2)
            {
                timesImages[0].color = new Color32((byte)0, (byte)0, (byte)0, (byte)255);
                timesImages[1].color = new Color32((byte)0, (byte)0, (byte)0, (byte)255);
                timesImages[2].color = new Color32((byte)120, (byte)120, (byte)120, (byte)255);
            }
            else if(buttonADownTimes == 3)
            {
                timesImages[0].color = new Color32((byte)0, (byte)0, (byte)0, (byte)255);
                timesImages[1].color = new Color32((byte)0, (byte)0, (byte)0, (byte)255);
                timesImages[2].color = new Color32((byte)0, (byte)0, (byte)0, (byte)255);
            }
            else if (buttonADownTimes == 0)
            {
                timesImages[0].color = new Color32((byte)120, (byte)120, (byte)120, (byte)255);
                timesImages[1].color = new Color32((byte)120, (byte)120, (byte)120, (byte)255);
                timesImages[2].color = new Color32((byte)120, (byte)120, (byte)120, (byte)255);
            }
            
            //�I������
            if (buttonADownTimes > 3)
            {
                gameManager.ButtonDown_A = false;
                DestroyAllHai();
                //yield return new WaitForSeconds(0.5f);
                break;
            }

        }
        yield return 0;
    }

    private void DestroyAllHai()
    {
        foreach (Transform n in this.gameObject.transform)
        {
            GameObject.Destroy(n.gameObject);
        }
    }

    public void deleteAlreadyPos(Vector2Int pos)
    {
        alreadyPos[pos.x, pos.y] = false;
    }
}
