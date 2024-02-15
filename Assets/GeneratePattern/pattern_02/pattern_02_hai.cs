using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pattern_02_hai : MonoBehaviour
{
    private bool moving = true;
    private bool alreadyStop = false;
    private bool stopping = false;

    private int centerY = 8;
    private float moveRate = 15.0f;
    private float stopTime = 0.2f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localPosition.y > centerY && !alreadyStop)
        {
            alreadyStop = true;
            stopping = true;
            Invoke("restart", stopTime);
        }

        if (stopping == false)
        {
            transform.Translate(0, Time.deltaTime * moveRate, 0);
        }
        else
        {
            transform.localPosition = new Vector3(0, centerY, 0);
        }

        if (transform.localPosition.y > 16 && moving)
        {
            Destroy(gameObject);
        }
    }

    private void restart()
    {
        stopping = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        //上昇をやめる
        moving = false;
        //後ろにぶっ飛んでいく
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.useGravity = true;
        rigidbody.AddForce(-transform.forward * 30, ForceMode.Impulse);

        //update処理軽減のためスクリプトを無効にする
        enabled = false;
    }
}
