using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PointerManager : MonoBehaviour
{
    //ポインターの位置
    public Vector2 pos;

    public GameObject StartPoint;
    public GameObject Plane;
    public GameObject Pointer;

    private float scale = 100.0f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //if (StartPoint.transform.position.x + StartPoint.transform.position.y + StartPoint.transform.position.z == 0) return;

        var n = Plane.transform.up;
        var x = Plane.transform.position;
        var x0 = StartPoint.transform.position;
        var m = StartPoint.transform.forward;
        var h = Vector3.Dot(n, x);

        var intersectPoint = x0 + ((h - Vector3.Dot(n, x0)) / (Vector3.Dot(n, m))) * m;

        pos = new Vector2(intersectPoint.x, -intersectPoint.z);
        
        //Pointer.transform.position = new Vector3(intersectPoint.x, -intersectPoint.z, 0);
        
        Pointer.GetComponent<RectTransform>().anchoredPosition = new Vector3(intersectPoint.x*scale, -intersectPoint.z*scale, 0);
    }
}
