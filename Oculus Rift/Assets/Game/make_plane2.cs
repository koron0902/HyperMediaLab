//Last Modified February 17,2016
//このclassはleapmotionでの値の収集メソッド、平面描画メソッドの本体が記述されています

using UnityEngine;
using System.Collections;
using Leap;
using operation;

namespace make2
{
	public class make_plane2 : MonoBehaviour
	{
		Vector_operation V = new Vector_operation();//vector_operationクラスを実体化
		
		Controller controller = new Controller();//leapmotionのcontrollerクラスを実体化
		
		public GameObject FingerObjects;//手のひらの中心の座標を追跡するための球状オブジェクト
		public GameObject Normal_Position;//手のひらの法線ベクトルを位置を追跡するための球状オブジェクト
		public GameObject Normal_Axis;
		public Vector3 normal_vector_rotation;
		private int left_or_right;//右手か左手かをしつこく確認するための変数
		public int n = 1;//右手か左手かを確認する回数を指定する変数0<=n<=60の範囲で設定できる.でもあまりしつこく確認しても意味ないかもしれない
		private float angle;//z軸方向のベクトルと平面の法線ベクトルの成す角を計算して代入する変数

        public Vector3 normal_unit_vector;
        public Vector3 one_point_on_the_plane;

        public bool update;
        public bool Leap_Motion;

        public static bool flg = false;

		void Start () 
		{
			Normal_Position.transform.position = new Vector3 (0f, -0.2f, 0f);
			Normal_Axis.transform.position = new Vector3 (0f, 0f, 0f);

			controller.EnableGesture (Gesture.GestureType.TYPE_SWIPE);
			controller.Config.SetFloat ("Gesture.Swipe.MinLength", 150.0f);
			controller.Config.SetFloat ("Gesture.Swipe.MinVelocity", 80f);
			controller.Config.Save ();
		}

		void Update ()//この関数の中は値更新を行いたいものを記述する
		{
			
            switch(Leap_Motion)
            {
                case true:
                    Leap_Motion_getdata();
                    break;
                case false:
                    Input_planedata();
                    break;
            }
		}

        void Leap_Motion_getdata()
        {
            Frame[] frame = new Frame[n];//leapmotionのフレームをn個確保する.ただし最新のフレームは0です.
            Hand[] hand = new Hand[n];//Handオブジェクトをn個確保する.ただし最新情報は0です.
           

            for (int i = 0; i < n; i++) //n個分のフレーム情報を取得するためのループ
            {
                frame[i] = controller.Frame(i);//frame情報を取得
                hand[i] = frame[i].Hands.Frontmost;//Hand情報を取得
            }
            GestureList gestures = frame[0].Gestures();

            if (Hand_judge(hand, frame) == 1)//Hand_judgeメソッドで右手か左手か何も認識できないかの判定
            {
                Debug.Log("確実に右手ですよ");
                Vector palm = hand[0].PalmNormal;//leapmotionで右手のひらの法線ベクトルを取得
                Vector normalizedPosition = hand[0].PalmPosition;

                normal_unit_vector = V.Trans_lu (ToVector3 (palm));
                //normal_unit_vector = V.Trans_lu_normalver(ToVector3(palm));
                normal_vector_rotation = V.Trans_SCS(normal_unit_vector);
                Normal_Position.transform.rotation = Quaternion.AngleAxis(normal_vector_rotation.y, new Vector3(0f, 1f, 0f)) * Quaternion.AngleAxis(normal_vector_rotation.z, new Vector3(0f, 0f, 1f));
                one_point_on_the_plane = (V.Trans_lu (ToVector3 (normalizedPosition)) / 50 - new Vector3(0.5f, -0.5f, 3.0f));
                //one_point_on_the_plane = (V.Trans_lu_normalver(ToVector3(normalizedPosition)) / 50 + new Vector3(-0.5f, -3.0f, -3.0f));
                FingerObjects.transform.position = one_point_on_the_plane;//FingerObjectsオブジェクトを取得した手のひらの中心の座標に移動
                Normal_Axis.transform.position = normal_unit_vector;

                PlaneWrite(one_point_on_the_plane, normal_unit_vector);//平面の描画methodを呼び出す
            }
            else if (Hand_judge(hand, frame) == 0)//Hand_judgeメソッドで左手または何も認識できなかったときはこっち
            {
                Debug.Log("左手です！または手が認識できません！");
            }

           for (int i = 0; i < gestures.Count; i++)
            {
                Gesture gesture = gestures[i];
                if (gesture.Type == Gesture.GestureType.TYPESWIPE)
                {
                    Debug.Log("swipe");
                    Normal_Axis.transform.position = new Vector3(0f, 0f, 0f);
                }

            }
        }

        void Input_planedata()
        {
            if (update)
            {
                normal_unit_vector = normal_unit_vector.normalized;
                normal_vector_rotation = V.Trans_SCS(V.Trans_gu_ug(normal_unit_vector));
                Normal_Position.transform.rotation = Quaternion.AngleAxis(normal_vector_rotation.y, new Vector3(0f, 1f, 0f)) * Quaternion.AngleAxis(normal_vector_rotation.z, new Vector3(0f, 0f, 1f));
                FingerObjects.transform.position = V.Trans_gu_ug(one_point_on_the_plane);//FingerObjectsオブジェクトを取得した手のひらの中心の座標に移動
                Normal_Axis.transform.position = V.Trans_gu_ug(normal_unit_vector);

                PlaneWrite(V.Trans_gu_ug(one_point_on_the_plane), V.Trans_gu_ug(normal_unit_vector));//平面の描画methodを呼び出す
            }
        }

        Vector3 ToVector3( Vector v )//leapmotionで定義されているvector型をunityで定義されているvector3型に変換するためのmethod
		{
			return new UnityEngine.Vector3( v.x, v.y, v.z );//vector3型を返す
		}

        int Hand_judge(Hand[] test, Frame[] f)//右手か左手か何も認識できないかの判定を行うメソッド(平面固定用)
        {
            left_or_right = 0;//判定用のフラグ的な？ここでは変数を初期化します
            for (int i = 0; i < n; i++)//nフレーム分をcheckします 
            {
                if (test[i].IsRight && f[i].Hands.Count == 1)//手を１個認識andその手が右手のとき判定フラグを踏んで加算します
                    left_or_right++;//+1加算します
                else
                    break;
            }

            if (left_or_right == n)//検査したフレーム数と踏んだフラグの数が等しいときは１を返して値の収集(leapmotionデータ収集)をはじめます 
                return 1;

            else//違ったら0を返して値を収集しません
                return 0;
        }

        void PlaneWrite(Vector3 one_point_on_the_plane, Vector3 normal_unit_vector)//平面を描画するためのメソッド(今のところ面倒なんでコメントは控えます）ここはいじる必要なし
		{
			Mesh mesh;
			mesh = new Mesh();
			Vector3 n = new Vector3(0.0f, 0.0f, 1.0f);
			Vector3[] increased_point = new Vector3[4];
			int[] Triangles = new int[6];
			
			increased_point = V.onepoint_and_normalVector (one_point_on_the_plane, normal_unit_vector);
			
			Vector3 b = increased_point [1] - increased_point [0];
			Vector3 a = increased_point [2] - increased_point [0];
			
			angle = V.twoVector_angle( Vector3.Cross (a, b), n);
			
			if (angle <= (Mathf.PI / 2) && 0 <= angle)
			{
				Triangles [0] = 0;
				Triangles [1] = 1;
				Triangles [2] = 2;
				Triangles [3] = 1;
				Triangles [4] = 3;
				Triangles [5] = 2;
			} 
			else if(angle > (Mathf.PI / 2))
			{
				Triangles [0] = 0;
				Triangles [1] = 2;
				Triangles [2] = 1;
				Triangles [3] = 2;
				Triangles [4] = 3;
				Triangles [5] = 1;
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