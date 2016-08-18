//Last Modified January 7,2016
//このクラスは視点変更に関係するメソッドが記述されています

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class camera : MonoBehaviour
{
    public Vector3 position;
    public Quaternion rotation;

    public int Camera_Radius;

	void start()
	{
        //transform.position = new Vector3(0, 0, -Camera_Radius);
        UnityEngine.VR.InputTracking.Recenter();
    }

	void Update()
	{
        rotation = UnityEngine.VR.InputTracking.GetLocalRotation(UnityEngine.VR.VRNode.LeftEye);
        position = UnityEngine.VR.InputTracking.GetLocalPosition(UnityEngine.VR.VRNode.Head);

        //transform.rotation = rotation;
        /*Quaternion tmp = rotation;
        if (rotation.y > 180)
            tmp.y = rotation.y - 180;

        tmp.y /= 90;

        int Mul = (rotation.y > 0 ? 1 : 0);
        transform.rotation = rotation;
        transform.position = position + new Vector3(
                                            (float)(-Camera_Radius * Math.Sin(rotation.y * Math.PI)) / 2,
                                            (float)(Camera_Radius / 2 * Math.Sin(rotation.x * Math.PI)),
                                            (float)Math.Abs((Camera_Radius * Math.Sin(rotation.y * Math.PI))) / 2 + (float)Math.Abs((Camera_Radius * Math.Sin(rotation.x * Math.PI))) / 2
                                            );*/

        Vector3 angles = rotation.eulerAngles;
        if (angles.x > 180)
            angles.x -= 360;
        var theta = (90 - angles.x) * Mathf.PI / 180;
        var phai = angles.y * Mathf.PI / 180;
        transform.position = new Vector3(-Camera_Radius * Mathf.Sin(theta) * Mathf.Sin(phai), Camera_Radius * Mathf.Cos(theta), -Camera_Radius * Mathf.Sin(theta) * Mathf.Cos(phai));

        Debug.Log("Y:" + rotation.eulerAngles.y);
    }
}