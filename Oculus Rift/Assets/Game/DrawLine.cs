using UnityEngine;
using System.Collections;
using Leap;
using operation;
using System;

namespace make2 {
    public class DrawLine : MonoBehaviour {

        public static int n = 1;
        public bool m_LeapMotion;
        public static bool isOdd = false;

        Controller m_Controller = new Controller();
        Vector_operation V_c = new Vector_operation(); // ベクトル演算を使用可能する

        // 動作状態を示す読み取り専用の変数群。
        // 他のクラスからインスタンスなしに参照したかったのでenumしない
        public static readonly int m_RunningStateNomal = 0; // 通常の動作状態
        public static readonly int m_RunningStatePause = 1; // 一時停止している状態.右手を抜くと次の状態に遷移
        public static readonly int m_RunningStatePreNomal = 2;  // 一時停止後に手が抜かれた状態.右手を入れると通常動作に戻る

        public static int m_RunningState = m_RunningStateNomal; // 動作状態を示す変数

        public static Vector3 m_FingerDirection;

        public GameObject Line;


        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {
            LeapMotionGetData();
        }

        void LeapMotionGetData() {
            var m_Frame = new Frame[n];
            var m_Hand = new Hand[n];
            var m_FingerList = new FingerList[n];

            for (int i = 0; i < n; i++) {
                m_Frame[i] = m_Controller.Frame(i);
                m_Hand[i] = m_Frame[i].Hands.Frontmost;
                m_FingerList[i] = m_Frame[i].Fingers;
            }
            var m_GestureList = m_Frame[0].Gestures();

            if (isOdd) {
                if (Hand_judge(m_Hand, m_Frame) == 0)
                    Debug.Log("スワイプして");
            } else if (m_RunningState != m_RunningStateNomal) {
                if (Hand_judge(m_Hand, m_Frame) == 0)
                    m_RunningState = m_RunningStatePreNomal;
                else if (m_RunningState == m_RunningStatePreNomal)
                    m_RunningState = m_RunningStateNomal;
            } else if (Hand_judge(m_Hand, m_Frame) == 1) {
                Debug.Log("右手屋根");
                GetFingerDirection(m_Hand, m_Frame);

                var tmp = m_FingerDirection;
                tmp.y = 0;
                Vector3 VectorEX = new Vector3(1, 0, 0);
                var ZXangle = (float)(V_c.twoVector_angle(tmp, VectorEX) * 180 / Math.PI);

                var tmp2 = m_FingerDirection;
                tmp2.z = 0;
                var XYangle = (float)(V_c.twoVector_angle(tmp, VectorEX) * 180 / Math.PI);
                
                var tmp3 = m_FingerDirection;
                tmp3.x = 0;
                Vector3 VectorEZ = new Vector3(0, 0, 1);
                var YZangle = (float)(V_c.twoVector_angle(tmp, VectorEZ) * 180 / Math.PI);
                
                Line.transform.rotation = Quaternion.Euler(ZXangle, YZangle, XYangle);
                Debug.Log("<axis:X -> " + ZXangle + "><axis:Y -> " + YZangle + "><axis:Z -> " + XYangle + ">");
            } else if (Hand_judge(m_Hand, m_Frame) == 0){//Hand_judgeメソッドで左手または何も認識できなかったときはこっち
                Debug.Log("左手です！または手が認識できません！");
            }
        }

        static Vector3 ToVector3(Vector v)//leapmotionで定義されているvector型をunityで定義されているvector3型に変換するためのmethod
        {
            return new UnityEngine.Vector3(v.x, v.y, v.z);//vector3型を返す
        }


        public static int Hand_judge(Hand[] test, Frame[] f)//右手か左手か何も認識できないかの判定を行うメソッド(平面固定用)
        {
            var LeftOrRight = 0;//判定用のフラグ的な？ここでは変数を初期化します

            for (int i = 0; i < n; i++)//nフレーム分をcheckします 
            {
                if (test[i].IsRight && f[i].Hands.Count == 1)//手を１個認識andその手が右手のとき判定フラグを踏んで加算します
                    LeftOrRight++;//+1加算します
                else
                    break;
            }

            if (LeftOrRight == n)//検査したフレーム数と踏んだフラグの数が等しいときは１を返して値の収集(leapmotionデータ収集)をはじめます 
                return 1;

            else//違ったら0を返して値を収集しません
                return 0;
        }

        static bool isfirst = true;
        public static bool GetFingerDirection(Hand[] hand, Frame[] frame) {
            Vector[] m_Vector = new Vector[2] { new Vector(), new Vector()};
            

            for (int i = 0; i < n; i++) {
                if (hand[i].Fingers.Extended().Count == 2) {
                    // 一応、伸ばされている指が2本であるという条件をつける
                    foreach (var finger in frame[i].Fingers) {
                        // 指の種類にあわせて分岐
                        switch (finger.Type()) {
                            case Finger.FingerType.TYPE_THUMB:
                                // 親指
                                m_Vector[0] = finger.Direction;
                                break;
                            case Finger.FingerType.TYPE_INDEX:
                                // 人差し指
                                m_Vector[1] = finger.Direction;
                                break;
                            default:
                                break;
                        }
                    }
                }

                
                m_FingerDirection = ToVector3(m_Vector[1]) - ToVector3(m_Vector[0]);        //1
            }
            return true;
        }
    }

}