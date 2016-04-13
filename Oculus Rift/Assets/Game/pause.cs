//Last Modified January 7,2016
//このクラスは"pause"の文字を表示させるメソッドが記述されています

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class pause : MonoBehaviour {
	// Use this for initialization
	void Start () {
		TextMesh tm = (TextMesh)gameObject.GetComponent (typeof(TextMesh));
		string sc_t = "pause";
		tm.text = sc_t;
	}
	
	// Update is called once per frame
	void Update () {
	}
}
