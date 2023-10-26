using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] float m_force = 20;
    [SerializeField] float m_radius = 5;
    [SerializeField] float m_upwards = 0;
    Vector3 m_position;

    [SerializeField] List<GameObject> gameObjects;
    [SerializeField] float destroyTime = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Explosion_AddForce();
        }
        */
    }

    private void OnTriggerEnter(Collider other)
    {
        Explosion_AddForce();
    }

    public void Explosion_AddForce()
    {
        m_position = this.transform.position;

        /*
        // 範囲内のRigidbodyにAddExplosionForce
        Collider[] hitColliders = Physics.OverlapSphere(m_position, m_radius);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            var rb = hitColliders[i].GetComponent<Rigidbody>();
            if (rb)
            {
                rb.isKinematic = false;
                rb.AddExplosionForce(m_force, m_position, m_radius, m_upwards, ForceMode.Impulse);
            }
        }
        */
        
        for (int i = 0; i < gameObjects.Count; i++)
        {
            var rb = gameObjects[i].GetComponent<Rigidbody>();
            if (rb)
            {
                rb.isKinematic = false;
                rb.AddExplosionForce(m_force, m_position, m_radius, m_upwards, ForceMode.Impulse);
            }
        }
        
        enabled = false;

        //一定時間後にオブジェクトの消去
        Invoke("DestroyObjects", destroyTime);
    }

    private void DestroyObjects()
    {
        Destroy(transform.parent.gameObject);
    }
}
