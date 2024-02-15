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

        //時間内に切られなかった場合
        if (moving)
        {
            //当たり判定の無効化
            BoxCollider boxCollider = GetComponent<BoxCollider>();
            boxCollider.enabled = false;

            yield return StartCoroutine(Vanish());

            //麻雀牌存在リストから削除する
            yield return StartCoroutine(deleteAlreadyList());

            //ゲームオブジェクトの消去
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

    IEnumerator Vanish()
    {
        float scale = 1.0f;
        float rotateAngle = 0.0f;
        float increaseRate = 4.0f;

        while (true)
        {
            yield return new WaitForSeconds(0.01f);

            //大きさを変えて回転しながら消える
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

    //当たったときと、消えるときに麻雀牌がある位置に存在するかどうかのリストから削除する
    IEnumerator deleteAlreadyList()
    {
        pattern_3.deleteAlreadyPos(this.pos);

        yield return 0;
    }

    //当たった時
    private void OnTriggerEnter(Collider other)
    {
        //上昇をやめる
        moving = false;
        //後ろにぶっ飛んでいく
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.useGravity = true;
        rigidbody.AddForce(-transform.parent.forward * 30, ForceMode.Impulse);

        StartCoroutine(endProcessOfSlashed());
    }
    IEnumerator endProcessOfSlashed()
    {
        //麻雀牌存在リストから削除する
        yield return StartCoroutine(deleteAlreadyList());

        //消したことを確認してからスクリプトを無効にする
        //update処理軽減のためスクリプトを無効にする
        enabled = false;
        yield return 0;
    }
}
