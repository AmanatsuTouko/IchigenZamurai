using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pattern_01_hai : MonoBehaviour
{
    private float moveRate = 1.3f;
    private bool moving = true;

    private bool onRotate = false;
    private float rotateRate = 30.0f;

    // Start is called before the first frame update
    void Start()
    {
        moveRate += Random.Range(0, 1.0f);

        int random = Random.Range(0,3);
        if (random == 0)
        {
            onRotate = true;
            transform.Rotate(0, 180, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            transform.Translate(0, Time.deltaTime * moveRate, 0);
            if (onRotate)
            {
                transform.Rotate(0, Time.deltaTime * rotateRate, 0); ;
            }
        }
        if (transform.localPosition.y > 16 && moving)
        {
            Destroy(gameObject);
        }
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

        //update�����y���̂��߃X�N���v�g�𖳌��ɂ���
        enabled = false;
    }
}
