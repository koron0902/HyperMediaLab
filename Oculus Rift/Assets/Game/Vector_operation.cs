//Last Modified February 17,2016
//このクラスはベクトル演算系のメソッドが記述されています

using UnityEngine;
using System.Collections;

namespace operation
{
	public class Vector_operation
	{
		public float twoVector_angle(Vector3 a, Vector3 b)//二つのベクトル間の角度を求めるメソッド
		{
			float angle;//二つのベクトルのなす角を代入する変数
			angle = Mathf.Acos (Vector3.Dot (a, b) / (a.magnitude * b.magnitude));//成す角を計算
			return angle;//角度を返す
		}

		private Vector3[] Schmitt(Vector3[] v)//シュミットの正規直交化を行うメソッド
		{
			int n = 0, i, j;//n:独立なベクトルの本数,i,j:forループで使う変数
			/*while (v[n] != Vector3.zero) 
			{
				n++;
			}*/
			n = 2;//直接独立なベクトルの本数を指定する
			Vector3[] w = new Vector3[n];//シュミットの正規直交化の結果を代入する配列

			for (i = 0; i < n; i++)//シュミットの正規直交化スタート(原理,アルゴリズムは自分で確認してください)
				w [i] = new Vector3 (0.0f, 0.0f, 0.0f);

			Vector3 tmp = new Vector3(0.0f, 0.0f, 0.0f);
			for (i = 0; i < n; i++)
			{
				for(j = 0; j < i; j++)
				{
					tmp = tmp + Vector3.Dot(w[j], v[i]) * w[j];
				}
				w[i] = (v[i] - tmp).normalized;
			}//シュミットの正規直交化終了
			return w;//正規直交化されたベクトル配列を返す
		}

		private Vector3 Gravity_cal(Vector3 a, Vector3 b, Vector3 c)//三角形の重心を求めるメソッド(たぶんもう使うことはありませんので説明を省きます)
		{
			Vector3 Gravity;
			Gravity.x = (a.x + b.x + c.x) / 3;
			Gravity.y = (a.y + b.y + c.y) / 3;
			Gravity.z = (a.z + b.z + c.z) / 3;
			return Gravity;
		}

		public Vector3[] increase_point(Vector3[] p)//平面上の三点の座標が既知のときポリゴンの頂点座標を決定することができるメソッド
		{											//おそらく使うことはないと思うので説明を省きます.知りたかったら自分で解読してください
			Vector3 Gravity;
			Vector3[] increased_p = new Vector3[4];
			Vector3[] tmp = new Vector3[2];

			Gravity = Gravity_cal(p[0], p[1], p[2]);

			tmp [0] = p [0] - Gravity;
			tmp [1] = p [2] - Gravity;
			
			tmp = Schmitt(tmp);
			
			int k = 7;
			increased_p [0] = Gravity + k * tmp[0];
			increased_p [1] = Gravity - k * tmp[1];
			increased_p [2] = Gravity + k * tmp[1];
			increased_p [3] = Gravity - k * tmp[0];

			return increased_p;
		}

		public float[] inner_product(Vector3 v)//三次元ベクトルの成分を取り出し,小数点第二位以下を切り捨てるメソッド
		{
			float[] judge = new float[3];//三次元ベクトルの成分を代入する配列

			for (int k = 0; k < 3; k++)//初期化するためのループ
				judge [k] = 0;

			judge [0] = Vector3.Dot (v, new Vector3(1f, 0f, 0f));//送られてきたベクトルのx成分を抽出
			judge [1] = Vector3.Dot (v, new Vector3(0f, 1f, 0f));//送られてきたベクトルのy成分を抽出
			judge [2] = Vector3.Dot (v, new Vector3(0f, 0f, 1f));//送られてきたベクトルのz成分を抽出

			for (int i = 0; i < 3; i++)//小数点第二位以下を切り捨て?四捨五入?するためのループ
			{
				judge [i] = Mathf.Round (judge [i] * 10);//小数点第二位以下を切り捨て?四捨五入?する
				judge [i] = judge [i] / 10;//結果を代入
			}
			return judge;//三次元ベクトルの各成分を取り出して小数点第一位までにした値を返す
		}

		public Vector3[] onepoint_and_normalVector(Vector3 tip_position, Vector3 normal_unit_vector)//法線ベクトルと平面上の1点から平面上の4点を生成するためのメソッド
		{
			Vector3[] newVertex = new Vector3[4];
			Vector3 Vertex_Vector1;
			Vector3 Vertex_Vector2;
			Vector3 distance_v;
			Vector3 for_zero = new Vector3 (-9.19f, -7.17f, -3.8463f);

			float distance;
			int constant1 = 7;

			if (twoVector_angle (normal_unit_vector, tip_position) > (Mathf.PI / 2))
				normal_unit_vector = - normal_unit_vector;


			distance = Mathf.Abs(Vector3.Dot (normal_unit_vector, tip_position));
			distance_v = distance * normal_unit_vector;
			
			if (normal_unit_vector != Vector3.zero)
			{
				Vertex_Vector1 = (tip_position - distance_v).normalized;

                Vertex_Vector2 = Vector3.Cross(normal_unit_vector, Vertex_Vector1).normalized;
				
				newVertex [0] = distance_v + constant1 * Vertex_Vector1 - constant1 * Vertex_Vector2 - for_zero;
				newVertex [1] = distance_v + constant1 * Vertex_Vector1 + constant1 * Vertex_Vector2 - for_zero;
				newVertex [2] = distance_v - constant1 * Vertex_Vector1 - constant1 * Vertex_Vector2 - for_zero;
				newVertex [3] = distance_v - constant1 * Vertex_Vector1 + constant1 * Vertex_Vector2 - for_zero;
			}
			return newVertex;
		}

		public Vector3 Trans_lg(Vector3 leap)//Leap CoordinatesをGame Coordinatesに変換するためのメソッド
		{
			Vector3 game;//結果を代入るための変数
			Vector3[] trans_g = new Vector3[3];  //下の行列が変換行列    
			trans_g [0] = new Vector3 (-1, 0, 0);// -1  0  0
			trans_g [1] = new Vector3 (0, 0, -1);//  0  0 -1
			trans_g [2] = new Vector3 (0, -1, 0);//  0 -1  0

			game.x = Vector3.Dot(leap, trans_g[0]);//行列を作用させる
			game.y = Vector3.Dot(leap, trans_g[1]);
			game.z = Vector3.Dot(leap, trans_g[2]);

			return game;
		}

		public Vector3 Trans_lu(Vector3 leap)//Leap CoordinatesをUnity Coordinatesに変換するためのメソッド（現段階では使っていない）
		{
			Vector3 unity;//結果を代入するための変数
			Vector3[] trans_u = new Vector3[3];  //下の行列が変換行列
			trans_u [0] = new Vector3 (-1, 0, 0);// -1  0  0
			trans_u [1] = new Vector3 (0, 0, -1);//  0  0 -1 
			trans_u [2] = new Vector3 (0, 1, 0); //  0  1  0

			unity.x = Vector3.Dot(leap, trans_u [0]);//行列を作用させる
			unity.y = Vector3.Dot(leap, trans_u [1]);
			unity.z = Vector3.Dot(leap, trans_u [2]);

			return unity;
		}

		public Vector3 Trans_gu_ug(Vector3 game_unity)//Game CoordinatesとUnity Coordinatesを相互に変換するためのメソッド
		{
			Vector3 gu;//結果を代入するための変数
			Vector3[] trans_gu = new Vector3[3];  //下の行列が変換行列
			trans_gu [0] = new Vector3 (1, 0, 0); //  1  0  0
			trans_gu [1] = new Vector3 (0, 1, 0); //  0  1  0
			trans_gu [2] = new Vector3 (0, 0, -1);//  0  0 -1
			
			gu.x = Vector3.Dot (game_unity, trans_gu [0]);//行列を作用させる
			gu.y = Vector3.Dot (game_unity, trans_gu [1]);
			gu.z = Vector3.Dot (game_unity, trans_gu [2]);
			
			return gu;
		}

		public Vector3 Trans_lu_normalver(Vector3 leap)//Leap Motionを机に置いたときLeap CoordinatesをGame Coordinatesに変換するためのメソッド
        {
			Vector3 unity;//結果を代入する変数,下に変換行列を示す
			unity = leap;           //  1  0  0
			unity.z = - unity.z;    //  0  1  0
                                    //  0  0 -1
			return unity;
		}

		public Vector3 Trans_SCS(Vector3 CCS)//直交座標を球座標に変換するためのメソッド
        {
			Vector3 SCS;
			if (CCS.x < 0)
				CCS = CCS * (-1);

            SCS.x = CCS.magnitude;
			SCS.y = -Mathf.Atan (CCS.z / CCS.x) * (180 / Mathf.PI);
			SCS.z = Mathf.Atan (CCS.y / Mathf.Sqrt((CCS.x * CCS.x) + (CCS.z * CCS.z))) * (180 / Mathf.PI);

            if (CCS.x == 0 && CCS.z == 0)
                SCS.y = 0;

                return SCS;
		}
	}
}
