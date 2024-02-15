using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutoria_hai : MonoBehaviour
{
    public Vector2Int pos;
    public Tutorial tutorial;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Spawn());
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

    // Update is called once per frame
    void Update()
    {

    }

    //���������Ƃ��ƁA������Ƃ��ɖ����v������ʒu�ɑ��݂��邩�ǂ����̃��X�g����폜����
    IEnumerator deleteAlreadyList()
    {
        tutorial.deleteAlreadyPos(this.pos);

        yield return 0;
    }

    //����������
    private void OnTriggerEnter(Collider other)
    {
        //���ɂԂ����ł���
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.useGravity = true;
        rigidbody.AddForce(-transform.parent.forward * 30, ForceMode.Impulse);

        // �G�t�F�N�g�̍Đ�
        EffectManager.Instance.PlayEffect(EffectManager.EffectType.Slash, this.transform.position);

        StartCoroutine(endProcessOfSlashed());
    }
    IEnumerator endProcessOfSlashed()
    {
        //�����v���݃��X�g����폜����
        yield return StartCoroutine(deleteAlreadyList());

        //�Ԃ����ł���������5�b��ɃI�u�W�F�N�g������
        yield return new WaitForSeconds(5.0f);
        Destroy(this.gameObject);

        yield return 0;
    }
}
