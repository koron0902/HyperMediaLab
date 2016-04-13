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

	void start()
	{
        transform.position = Vector3.zero;
        UnityEngine.VR.InputTracking.Recenter();
    }

	void Update()
	{
        rotation = UnityEngine.VR.InputTracking.GetLocalRotation(UnityEngine.VR.VRNode.LeftEye);
        position = UnityEngine.VR.InputTracking.GetLocalPosition(UnityEngine.VR.VRNode.Head);
        
        //transform.rotation = rotation;
        //transform.position = position + new Vector3(0f, +2f, 0f);
    }
}