using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaneSample : MonoBehaviour
{
    public GameObject StartPoint;
    public GameObject Plane;
    public GameObject Pointer;

    public GameObject DisplayPointer;

    private int maxX = 320;
    private int maxY = 180;
    private int scale = 50;

    Vector2 prePos = new Vector2(0, 0);
    Vector2 nowPos;
    private float slash_threshold = 2.0f;

    private Vector2 fixedPos;

    // Update is called once per frame
    void Update()
    {
        var n = Plane.transform.up;
        var x = Plane.transform.position;
        var x0 = StartPoint.transform.position;
        var m = StartPoint.transform.forward;
        var h = Vector3.Dot(n, x);

        

        var intersectPoint = x0 + ((h - Vector3.Dot(n, x0)) / (Vector3.Dot(n, m))) * m;

        /*
        if(intersectPoint.x > 0)
        {
            intersectPoint.x = Mathf.Sqrt(intersectPoint.x);
        }else if(intersectPoint.x < 0){
            intersectPoint.x = - Mathf.Sqrt(Mathf.Abs(intersectPoint.x));
        }
        if (intersectPoint.z > 0)
        {
            intersectPoint.z = Mathf.Sqrt(intersectPoint.z);
        }
        else if (intersectPoint.z < 0)
        {
            intersectPoint.z = -Mathf.Sqrt(Mathf.Abs(intersectPoint.z));
        }
        */

        //à íuÇÃèCê≥
        if (intersectPoint.x < -maxX) intersectPoint = new Vector3(-maxX, intersectPoint.y, intersectPoint.z);
        else if (intersectPoint.x > maxX) intersectPoint = new Vector3(maxX, intersectPoint.y, intersectPoint.z);
        if (intersectPoint.z < -maxY) intersectPoint = new Vector3(intersectPoint.x, intersectPoint.y, -maxY);
        else if (intersectPoint.z > maxY) intersectPoint = new Vector3(intersectPoint.x,intersectPoint.y, maxY);


        Pointer.transform.position = intersectPoint;

        DisplayPointer.GetComponent<RectTransform>().anchoredPosition = new Vector3(intersectPoint.x*scale, -intersectPoint.z*scale, 0);

        
        nowPos = new Vector2(intersectPoint.x, -intersectPoint.z);

        Vector2 subPos = prePos - nowPos;
        if (subPos.x > slash_threshold) Debug.Log("ç∂Ç…êUÇ¡ÇΩ");
        if (subPos.x < -slash_threshold) Debug.Log("âEÇ…êUÇ¡ÇΩ");
        if (subPos.y > slash_threshold) Debug.Log("â∫Ç…êUÇ¡ÇΩ");
        if (subPos.y < -slash_threshold) Debug.Log("è„Ç…êUÇ¡ÇΩ");

        prePos = nowPos;
    }
}