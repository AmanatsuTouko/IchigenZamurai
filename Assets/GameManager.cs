using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Title title;
    public Tutorial tutorial;
    public Tutorial_Explain tutorial_explain;
    public ScreenTransiton screenTransiton;
    public DisplayLevelText displayLevelText;
    public pattern_01 pattern_1;
    public pattern_02 pattern_2;
    public pattern_03 pattern_3;
    public Result result;
    public Ranking ranking;

    public bool ButtonDown_A = false;
    private bool _isPlaytutorial = false;

    public BGMManager bgmManager;
    public ScoreManager scoreManager;
    public GameObject ScoreWindow;
    public GameObject ScoreWindowPlane;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Generate());
    }

    IEnumerator Generate()
    {
        while (true)
        {
            yield return StartCoroutine(bgmManager.PlayTitle());
            yield return StartCoroutine(title.Generate());

            yield return StartCoroutine(bgmManager.StopBGM());

            //�`���[�g���A�����I�������ꍇ
            if (_isPlaytutorial)
            {
                yield return StartCoroutine(bgmManager.PlayGaming(1));
                yield return StartCoroutine(tutorial_explain.Generate());
                yield return StartCoroutine(bgmManager.StopBGM());
                //�X�R�A�̒l�̃��Z�b�g
                yield return StartCoroutine(scoreManager.ResetParamCoroutine());
            }

            //���ʑJ�ڃA�j���[�V�����̕\��
            yield return StartCoroutine(screenTransiton.Generate());

            //�X�R�A�E�B���h�E�̕\��
            ScoreWindow.SetActive(true);
            ScoreWindowPlane.SetActive(true);

            //LEVEL1
            yield return StartCoroutine(displayLevelText.Generate("レベル１"));
            yield return StartCoroutine(bgmManager.PlayGaming(2));
            yield return StartCoroutine(pattern_1.Generate());
            yield return StartCoroutine(bgmManager.StopBGM());

            yield return StartCoroutine(displayLevelText.Generate("レベル２"));
            yield return StartCoroutine(bgmManager.PlayGaming(3));
            yield return StartCoroutine(pattern_2.Generate());
            yield return StartCoroutine(bgmManager.StopBGM());

            yield return StartCoroutine(displayLevelText.Generate("ファイナルレベル"));
            yield return StartCoroutine(bgmManager.PlayGaming(4));
            yield return StartCoroutine(pattern_3.Generate());
            yield return StartCoroutine(bgmManager.StopBGM());

            yield return StartCoroutine(displayLevelText.Generate("終了！"));


            //�X�R�A�E�B���h�E�̔��\��
            ScoreWindow.SetActive(false);
            ScoreWindowPlane.SetActive(false);

            //���ʑJ�ڃA�j���[�V�����̕\��
            yield return StartCoroutine(screenTransiton.Generate());

            //���ʔ��\
            yield return StartCoroutine(result.Generate());

            //�����L���O�̕\��
            yield return StartCoroutine(ranking.Generate());

            //�S�p�����[�^�̃��Z�b�g
            //�X�R�A�̒l�̃��Z�b�g
            yield return StartCoroutine(scoreManager.ResetParamCoroutine());

            //�����v�I�u�W�F�N�g�̍폜
            yield return StartCoroutine(DestroyAllHais());
        }

        yield return 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ButtonDown_A = true;
        }

        //�V�[���̃����[�h
        if (Input.GetKeyDown(KeyCode.R))
        {
            // ���݂�Scene���擾
            Scene loadScene = SceneManager.GetActiveScene();
            // ���݂̃V�[�����ēǂݍ��݂���
            SceneManager.LoadScene(loadScene.name);
        }
    }

    public IEnumerator SetTutorial(int select_mode)
    {
        if (select_mode == 1) _isPlaytutorial = true;
        if (select_mode == 2) _isPlaytutorial = false;
        yield return 0;
    }

    private IEnumerator DestroyAllHais()
    {
        //gen1, gen2, gen3 �̃^�O���t�����I�u�W�F�N�g���폜����
        GameObject[] objects = GameObject.FindGameObjectsWithTag("gen1");
        foreach (GameObject ball in objects)
        {
            Destroy(ball);
        }
        objects = GameObject.FindGameObjectsWithTag("gen2");
        foreach (GameObject ball in objects)
        {
            Destroy(ball);
        }
        objects = GameObject.FindGameObjectsWithTag("gen3");
        foreach (GameObject ball in objects)
        {
            Destroy(ball);
        }

        yield return 0;
    }

}
