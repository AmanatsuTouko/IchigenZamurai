using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pattern_03 : MonoBehaviour
{
    public List<GameObject> gens = new List<GameObject>();

    Vector3[,] Pos = new Vector3[5, 3]; //�o���ʒu�̔z��
    private bool[,] alreadyPos = new bool[5, 3]; //���ɏo�����Ă���ʒu�̔z��

    //�I������
    //�͂��߂Ă�Wii�̓��x��2�S�̂�45�b������
    private float endTime = 45.0f;
    private bool isEnd = false;

    //�o���񐔂̃J�E���g
    public ScoreManager scoreManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator Reset()
    {
        isEnd = false;
        yield return StartCoroutine(SetPosList());

        yield return 0;
    }

    private IEnumerator SetPosList()
    {
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

                //GameObject gameObject = Instantiate(gens[0], transform);
                //gameObject.transform.localPosition = Pos[i, j];
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

    IEnumerator StopGenerate()
    {   
        yield return new WaitForSeconds(endTime);
        isEnd = true;
        yield return 0;
    }

    public IEnumerator Generate()
    {
        //������
        yield return StartCoroutine(Reset());

        int simultaneousCount = 0;

        StartCoroutine(StopGenerate());

        //�����_���Ȉʒu�ɐ�������
        //�����ɏo�Ă����������
        while (true)
        {
            //�I������
            if (isEnd) break;

            if(simultaneousCount == 0)
            {
                //2�b�����ɐ�������
                yield return new WaitForSeconds(0.5f);

                //�����ɂ����������邩�����߂�
                int r = Random.Range(1, 4);
                simultaneousCount = r;
            }
            simultaneousCount--;
            
            int posX = Random.Range(0, 5);
            int posY = Random.Range(0, 2);
            bool generateFrag = true;

            while (true)
            {
                
                //�S�����܂��Ă���ꍇ�͐������Ȃ�
                int count = 0;
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if ((i == 1 || i == 3) && j == 2) continue;
                        if (alreadyPos[i, j] == true) count++;
                    }
                }
                if (count >= 12)
                {
                    generateFrag = false;
                    break;
                }

                //���ɂ����Ă��Ȃ��ꏊ�Ń����_���Ȉʒu�����߂�
                posX = Random.Range(0, 5);
                if (posX == 1 || posX == 3) { posY = Random.Range(0, 2); }
                else { posY = Random.Range(0, 3); }
                if (!alreadyPos[posX, posY]) break;
            }

            if (!generateFrag)
            {
                Debug.Log("already all area can generate is None");
                yield return new WaitForSeconds(0.5f);
                continue;
            }

            alreadyPos[posX, posY] = true;

            int genType = 0;
            if( Random.Range(0, 10) < 2)
            {
                genType = Random.Range(1,3);
            }
            //���S�ɂ�1���̂ݏo������悤�ɂ���
            if (posX == 2 && posY == 1) genType = 0;

            GameObject gameObject = Instantiate(gens[genType], transform);
            gameObject.transform.localPosition = Pos[posX, posY];
            gameObject.transform.localScale = new Vector3(0, 0, 0);

            //�o���񐔂̃J�E���g
            if (genType == 0) scoreManager.AddCount_spawn(ScoreManager.HaiType.gen_1);
            else if (genType == 1) scoreManager.AddCount_spawn(ScoreManager.HaiType.gen_2);
            else if (genType == 2) scoreManager.AddCount_spawn(ScoreManager.HaiType.gen_3);

            //�v���g�ɂǂ��ɂ��邩�̏�����������
            pattern_03_hai hai = gameObject.GetComponent<pattern_03_hai>();
            hai.pos = new Vector2Int(posX, posY);
            hai.pattern_3 = this;
        }
        
        yield return 0;
    }

    
    public void deleteAlreadyPos(Vector2Int pos)
    {
        alreadyPos[pos.x, pos.y] = false;
    }
}
