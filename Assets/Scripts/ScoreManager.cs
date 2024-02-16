using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI ComboText;

    public TextMeshPro ScoreTextPlane;
    public TextMeshPro ComboTextPlane;

    public GameObject ComBoBonusTextObject;
    public TextMeshProUGUI ComboBonusText;

    public int Count_1gen = 0;
    public int Count_2gen = 0;
    public int Count_3gen = 0;

    public int score = 0;
    public int combo = 0;
    public int comboTotal = 0;
    public int comboBonus = 0;
    public int comboMax = 0;

    //���U���g�p�ɏo�����������v���L�^���Ă���
    public int Count_1gen_spawn = 0;
    public int Count_2gen_spawn = 0;
    public int Count_3gen_spawn = 0;

    //�_��
    public Image redFlash;

    // �J�����U��
    [SerializeField] CameraShake _cameraShake;

    // Joy-Con�̐U��
    [SerializeField] InputJoyconManager _inputJoyconManager;

    public void ResetParam()
    {
        Count_1gen = 0;
        Count_2gen = 0;
        Count_3gen = 0;

        score = 0;
        combo = 0;
        comboTotal = 0;
        comboBonus = 0;
        comboMax = 0;

        Count_1gen_spawn = 0;
        Count_2gen_spawn = 0;
        Count_3gen_spawn = 0;
    }

    public IEnumerator ResetParamCoroutine()
    {
        ResetParam();
        ReloadText();
        yield return 0;
    }

    public enum HaiType
    {
        gen_1,
        gen_2,
        gen_3
    }

    private void ReloadText()
    {
        score = Count_1gen + comboTotal + comboBonus - (Count_2gen + Count_3gen)*3;
        ScoreText.text = "SCORE:" + score;
        ComboText.text = "COMBO:" + combo;


        ScoreTextPlane.text = "SCORE:" + score;
        ComboTextPlane.text = "COMBO:" + combo;
    }

    public void AddCount(HaiType haiType)
    {
        // 1�����a�����ꍇ
        if (haiType == HaiType.gen_1)
        {
            Count_1gen++;

            // JoyCon�̐U��
            _inputJoyconManager.SetRumble(160, 320, 0.2f, 100);
        }
        // 1���ȊO���a�����ꍇ
        else
        {
            if (haiType == HaiType.gen_2){ Count_2gen++; }
            else if (haiType == HaiType.gen_3){ Count_3gen++; }
            
            // ��e�{�C�X�̉��o
            int random = Random.Range(0, 2);
            if (random == 0) SoundManager.Instance.Play(SoundManager.SE.VoiceDamageGu);
            else if (random == 1) SoundManager.Instance.Play(SoundManager.SE.VoiceDamageGuaaa);

            // �ԉ�ʂ̓_��
            StartCoroutine(Flash());
            // ���O�ɐ؂��������ɐU��������
            _cameraShake.Shake(SlashManager.PreSlashDirection);
            // JoyCon�̐U��
            _inputJoyconManager.SetRumble(160, 320, 1.0f, 200);
        }
    }

    public void AddCount_spawn(HaiType haiType)
    {
        if (haiType == HaiType.gen_1) Count_1gen_spawn++;
        else if (haiType == HaiType.gen_2) Count_2gen_spawn++;
        else if (haiType == HaiType.gen_3) Count_3gen_spawn++;
    }

    public void AddComboCount()
    {
        combo += 1;
        Debug.Log(combo + "�R���{�I");

        ReloadText();
    }

    //�R���{���r�₦���ۂ̏���
    public void StopComboUp()
    {
        //�R���{���̃X�R�A�ւ̉��Z
        comboTotal += combo;

        //�R���{�ɂ��{�[�i�X��ǉ�
        int currentComboBonus = 0;
        if     (combo >= 100) currentComboBonus = (int)(combo * 1.00f);
        else if (combo >= 50) currentComboBonus = (int)(combo * 0.85f);
        else if (combo >= 25) currentComboBonus = (int)(combo * 0.75f);
        else if (combo >= 10) currentComboBonus = (int)(combo * 0.50f);
        comboBonus += currentComboBonus;

        //�R���{�{�[�i�X�̕\��
        if ((currentComboBonus+combo) != 0)
        {
            StartCoroutine(DisplayComboBonus(currentComboBonus + combo));
        }

        //�ő�R���{���̍X�V
        if(comboMax < combo)
        {
            comboMax = combo;
        }

        //�R���{���̃��Z�b�g
        combo = 0;

        ReloadText();
    }

    IEnumerator DisplayComboBonus(int comboBonus)
    {
        ComboBonusText.text = "Bonus +" + comboBonus;
        ComBoBonusTextObject.SetActive(true);

        yield return new WaitForSeconds(2.0f);

        ComBoBonusTextObject.SetActive(false);

        yield return 0;
    }

    //��ʂ̓_��
    IEnumerator Flash()
    {
        redFlash.color = new Color32((byte)255, (byte)0, (byte)0, (byte)100);
        yield return new WaitForSeconds(0.125f);
        
        redFlash.color = new Color32((byte)255, (byte)0, (byte)0, (byte)0);
        yield return new WaitForSeconds(0.05f);
        
        redFlash.color = new Color32((byte)255, (byte)0, (byte)0, (byte)50);
        yield return new WaitForSeconds(0.1f);
        
        redFlash.color = new Color32((byte)255, (byte)0, (byte)0, (byte)0);
        yield return 0;
    }
}
