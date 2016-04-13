//Last Modified February 17,2016
//このクラスはgameを制御するためのメソッドが記述されています

using UnityEngine;
using System.Collections;
using System;
using operation;
using Leap;

namespace judge
{
	public class Game_judge : MonoBehaviour
	{
		public GameObject g;//手のひらの中心座標を示す球型オブジェクト
		public GameObject normal;//平面の法線の座標を示す球型オブジェクト
		public GameObject correct;//文字'good'
		public GameObject next;//文字'Go to the next question'
		public GameObject finish;//文字'All Clear!!Congratulations!!'
        public GameObject stop;//文字'pause'

		private int question_quantity;//question_normal_vecetorの配列数を代入する変数
		private int question_n = 0;//現在の問題番号を示す変数
		public Vector3[] question_normal_vector;//問題を代入する配列

		public float good_range = 0;//正解幅を指定する変数

		private Vector3 normal_vector;//Game coordinatesでの平面の法線ベクトルを代入する変数
		private Vector3 one_point;//Game coordinatesでの平面上の１点を代入する変数

		private int OK_count = 0;//問題を正解判定に使用するカウンター
		public int correct_second;//正解とする秒数を指定する変数

		Vector_operation V_c = new Vector_operation ();//ベクトル演算を使用可能する

		// Use this for initialization
		void Start ()
		{
            g = GameObject.Find ("Finger11");//手のひらの中心の座標を示す球型オブジェクトを取得
			normal = GameObject.Find ("normal_axis");//平面の法線ベクトル位置を示す球型オブジェクトを取得
			correct = GameObject.Find ("letter'good'");
			next = GameObject.Find ("letter'Go to next Question'");
			finish = GameObject.Find("letter'All Clear!!Congratulations!!'");
			stop = GameObject.Find ("letter'pause'");
			stop.transform.position = new Vector3 (-10f, +10f, 0f);

			question_quantity = question_normal_vector.Length;//問題を代入する配列の長さを取得
		}

		// Update is called once per frame
		void Update () 
		{
			correct.gameObject.SetActive(false);//文字を非表示に設定
			next.gameObject.SetActive(false);//文字を非表示に設定
			finish.gameObject.SetActive (false);//文字を非表示に設定
			stop.gameObject.SetActive (false);//文字を非表示に設定

			
            normal_vector = normal.transform.position;//平面の法線ベクトルの位置ベクトルを取得
            one_point = g.transform.position;//手のひらの中心の座標を示す球型オブジェクトの位置を取得

            normal_vector = V_c.Trans_gu_ug(normal_vector);//normal_vectorをGame coordinatesに変換
            one_point = V_c.Trans_gu_ug(one_point);//one_pointをGame coordinatesに変換

            if (question_n % 2 != 0)//現在の問題番号によってpauseをはさむ
                stop.gameObject.SetActive(true);//文字の表示を許可

			if (question_quantity > question_n)//問題番号が全体配列数より小さかったら問題のコントロール関数と方程式表示関数に飛ぶ
			{
				equation_print ();//代数方程式を表示する関数
                Controll_question();//問題をコントロールするための関数
            } 
			else//問題が終了したらこっち
				finish.gameObject.SetActive (true);//終わりを示す文字の表示を許可
		}

		void equation_print ()//代数方程式を表示する関数
        {
			char[] e = new char[6];//代数方程式の文字部分を代入する文字型配列
			float[] judge = new float[3];//代数方程式の数字部分を代入する配列

			TextMesh e_P = (TextMesh)gameObject.GetComponent (typeof(TextMesh));
			int c;

			for (int k = 3; k < 6; k++)
				e [k] = '+';

			e_P.fontSize = 120;

			judge = V_c.inner_product (normal_vector);
			for (int i = 0; i < 3; i++)
			{
				if(judge[i] == 0.0f)
				{
					c = 5 - i;
					e[c] = (char)32;
					e[i] = (char)32;
				}
				else
					e[i] = (char)(88 + i);
			}

			string P = "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n"+"                     法線ベクトル" + question_normal_vector[question_n] + "を持つ平面" + "\n" + "                     平面の代数方程式 :" + judge[0] + e[0] + e[4] + judge[1] + e[1] + e[3] + judge[2] + e[2] + "=" + Vector3.Dot(normal_vector, one_point).ToString("f1");
			e_P.text = P;
		}

		void Controll_question()//問題をコントロールするための関数
        {
			float temp = V_c.twoVector_angle (normal_vector, question_normal_vector [question_n]);//操作平面の法線ベクトルと問題の法線ベクトルの成す角
			if (normal_vector == Vector3.zero && question_n % 2 != 0)//pause解除の条件
			{
				question_n = question_n + 1;
				next.gameObject.SetActive (true);
			}

			else if (temp <= (good_range / 180.0) * Math.PI && question_n % 2 == 0)//平面の表側の法線ベクトルを確認 
			{
				correct.gameObject.SetActive (true);
				OK_count = OK_count + 1;//正解で1カウント
			} 
			else if (temp >= (Math.PI - (good_range / 180.0) * Math.PI) && question_n % 2 == 0)//平面の裏側の法線ベクトルを確認
			{
				correct.gameObject.SetActive (true);
				OK_count = OK_count + 1;//正解で1カウント
			} 
			else
				OK_count = 0;//不正解で0

			if (OK_count >= correct_second)//カウンターが正解秒数指定変数と等しいか確認
			{
				correct.gameObject.SetActive (false);//goodの文字を消す
				question_n = question_n + 1;//次の問題へ
			}
		}
	}
}
