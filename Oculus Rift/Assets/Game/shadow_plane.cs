//Last Modified January 6,2016
//このclassはleapmotionでの値の収集メソッド、平面描画メソッドの本体が記述されています

using UnityEngine;
using System.Collections;
using Leap;
using operation;

namespace make2
{
	public class shadow_plane : MonoBehaviour
	{
		Vector_operation V = new Vector_operation();//vector_operationクラスを実体化

		public Vector3 normal_unit_vector;//leapmotionから取得した手のひらの法線ベクトルをunity座標系に変換したものを代入するベクトル変数
		public Vector3 hand_position;//leapmotionから入手した手のひらの中心の座標をunity座標系に変換したものを代入するベクトル変数
		private float angle;//z軸方向のベクトルと平面の法線ベクトルの成す角を計算して代入する変数
		private Vector3 starting_position;

		public GameObject g;
		public GameObject normal;
		public GameObject top_plane;

		void Start () 
		{
			top_plane = GameObject.Find ("top_plane");
			g = GameObject.Find ("Finger11");
			normal = GameObject.Find ("normal_axis");
		}

		void Update ()//この関数の中は値更新を行いたいものを記述する
		{
			starting_position = top_plane.transform.position;
			this.transform.position = starting_position;
			hand_position = g.transform.position;
			normal_unit_vector = normal.transform.position;
			PlaneWrite ();//平面の描画methodを呼び出す
		}
		
		Vector3 ToVector3( Vector v )//leapmotionで定義されているvector型をunityで定義されているvector3型に変換するためのmethod
		{
			return new UnityEngine.Vector3( v.x, v.y, v.z );//vector3型を返す
		}
		
		void PlaneWrite()//平面を描画するためのメソッド(今のところ面倒なんでコメントは控えます）ここはいじる必要なし
		{
			Mesh mesh;
			mesh = new Mesh();
			Vector3 n = new Vector3(0.0f, 0.0f, 1.0f);
			Vector3[] increased_point = new Vector3[4];
			int[] Triangles = new int[6];
			
			increased_point = V.onepoint_and_normalVector (hand_position, normal_unit_vector);
			
			Vector3 b = increased_point [1] - increased_point [0];
			Vector3 a = increased_point [2] - increased_point [0];
			
			angle = V.twoVector_angle( Vector3.Cross (a, b), n);
			
			if (angle <= (Mathf.PI / 2) && 0 <= angle)
			{
				Triangles [0] = 0;
				Triangles [1] = 2;
				Triangles [2] = 1;
				Triangles [3] = 2;
				Triangles [4] = 3;
				Triangles [5] = 1;
			} 
			else if(angle > (Mathf.PI / 2))
			{
				Triangles [0] = 0;
				Triangles [1] = 1;
				Triangles [2] = 2;
				Triangles [3] = 1;
				Triangles [4] = 3;
				Triangles [5] = 2;
			}
			mesh.vertices = increased_point;
			mesh.triangles = Triangles;
			
			mesh.RecalculateNormals();
			mesh.RecalculateBounds();
			
			GetComponent<MeshFilter>().sharedMesh = mesh;
			GetComponent<MeshFilter>().sharedMesh.name = "myMesh";
		}
	}
}