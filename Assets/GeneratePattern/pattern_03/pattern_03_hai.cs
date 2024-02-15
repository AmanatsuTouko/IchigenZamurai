using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pattern_03_hai : MonoBehaviour
{
    public Vector2Int pos;
    public pattern_03 pattern_3;

    private bool moving = true;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Main());
    }

    IEnumerator Main ()
    {
        yield return StartCoroutine(Spawn());

        yield return new WaitForSeconds(1.0f);

        //���ԓ��ɐ؂��Ȃ������ꍇ
        if (moving)
        {
            //�����蔻��̖�����
            BoxCollider boxCollider = GetComponent<BoxCollider>();
            boxCollider.enabled = false;

            yield return StartCoroutine(Vanish());

            //�����v���݃��X�g����폜����
            yield return StartCoroutine(deleteAlreadyList());

            //�Q�[���I�u�W�F�N�g�̏���
            Destroy(this.gameObject);
        }

        yield return 0;
    }

    IEnumerator Spawn()
    {
        float scale = 0.0f;
        float rotateAngle = -180.0f;
        float increaseRate = 4.0f;
        
        while (true)
        {
            yield return new WaitForSeconds(0.01f);

            //�傫����ς��ĉ�]���Ȃ���o�Ă���
            scale += Time.deltaTime * increaseRate;
            this.transform.localScale = new Vector3(scale, scale, scale);

            rotateAngle += Time.deltaTime * increaseRate * 180;
            this.transform.localRotation = Quaternion.Euler(0, rotateAngle, 0);

            if (scale > 1.0f)
            {
                this.transform.localScale = new Vector3(1, 1, 1);
                this.transform.Rotate(0, 0, 0);
                break;
            }
        }
        yield return 0;
    }

    IEnumerator Vanish()
    {
        float scale = 1.0f;
        float rotateAngle = 0.0f;
        float increaseRate = 4.0f;

        while (true)
        {
            yield return new WaitForSeconds(0.01f);

            //�傫����ς��ĉ�]���Ȃ��������
            scale -= Time.deltaTime * increaseRate;
            this.transform.localScale = new Vector3(scale, scale, scale);

            rotateAngle += Time.deltaTime * increaseRate * 180;
            this.transform.localRotation = Quaternion.Euler(0, rotateAngle, 0);

            if (scale < 0.0f)
            {
                this.transform.localScale = new Vector3(0, 0, 0);
                this.transform.Rotate(0, 180, 0);
                break;
            }
        }
        yield return 0;
    }

    //���������Ƃ��ƁA������Ƃ��ɖ����v������ʒu�ɑ��݂��邩�ǂ����̃��X�g����폜����
    IEnumerator deleteAlreadyList()
    {
        pattern_3.deleteAlreadyPos(this.pos);

        yield return 0;
    }

    //����������
    private void OnTriggerEnter(Collider other)
    {
        //�㏸����߂�
        moving = false;
        //���ɂԂ����ł���
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.useGravity = true;
        rigidbody.AddForce(-transform.parent.forward * 30, ForceMode.Impulse);

        StartCoroutine(endProcessOfSlashed());
    }
    IEnumerator endProcessOfSlashed()
    {
        //�����v���݃��X�g����폜����
        yield return StartCoroutine(deleteAlreadyList());

        //���������Ƃ��m�F���Ă���X�N���v�g�𖳌��ɂ���
        //update�����y���̂��߃X�N���v�g�𖳌��ɂ���
        enabled = false;
        yield return 0;
    }
}
