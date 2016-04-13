//Last Modified January 7,2016
//このクラスは法線ベクトルを表示させるかさせないかを決めるためのメソッドが記述されています

using UnityEngine;
using System.Collections;

public class normal : MonoBehaviour
{
	public GameObject normal_vector;
	public bool hint;
	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		if(hint)
			normal_vector.gameObject.SetActive (hint);
		else
			normal_vector.gameObject.SetActive (hint);
	}
}
