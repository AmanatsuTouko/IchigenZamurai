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

            //大きさを変えて回転しながら出てくる
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

    //当たったときと、消えるときに麻雀牌がある位置に存在するかどうかのリストから削除する
    IEnumerator deleteAlreadyList()
    {
        tutorial.deleteAlreadyPos(this.pos);

        yield return 0;
    }

    //当たった時
    private void OnTriggerEnter(Collider other)
    {
        //後ろにぶっ飛んでいく
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.useGravity = true;
        rigidbody.AddForce(-transform.parent.forward * 30, ForceMode.Impulse);

        // エフェクトの再生
        EffectManager.Instance.PlayEffect(EffectManager.EffectType.Slash, this.transform.position);

        StartCoroutine(endProcessOfSlashed());
    }
    IEnumerator endProcessOfSlashed()
    {
        //麻雀牌存在リストから削除する
        yield return StartCoroutine(deleteAlreadyList());

        //ぶっ飛んでいったあと5秒後にオブジェクトを消す
        yield return new WaitForSeconds(5.0f);
        Destroy(this.gameObject);

        yield return 0;
    }
}
