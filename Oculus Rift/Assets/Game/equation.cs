using UnityEngine;
using System;
using System.Collections;

public class equation : MonoBehaviour {

    private Quaternion mRotation;
    private float mPosition;
    private Vector3 mAngles;
    public float mEquaitionRadius;
    public GameObject mCamera;

    // Use this for initialization
    void Start () {

        mCamera = GameObject.Find("rotation_camera");

    }
	
	// Update is called once per frame
	void Update () {
	    mRotation = UnityEngine.VR.InputTracking.GetLocalRotation(UnityEngine.VR.VRNode.LeftEye);
        mAngles = mRotation.eulerAngles;
        
        if (mAngles.x > 180)
            mAngles.x -= 360;
        mAngles.x += 20;

        Func<int, float> Deg2Rad = (deg) => { return deg * Mathf.PI / 180; };
        var theta = (90 - mAngles.x) * Mathf.PI / 180;
       // theta -= Deg2Rad(20);
        var phai = mAngles.y * Mathf.PI / 180;
        /*transform.localPosition = new Vector3(mEquaitionRadius * Mathf.Sin(theta) * Mathf.Sin(phai),
                                                  -mEquaitionRadius * Mathf.Cos(theta),
                                                  mEquaitionRadius * Mathf.Sin(theta) * Mathf.Cos(phai));*/
        //transform.localRotation = mRotation;
        Debug.Log("X:" + mAngles.x.ToString("f2") + "Y:" + mAngles.y.ToString("f2") + "Z:" + mAngles.z.ToString("f2"));

    }
}
