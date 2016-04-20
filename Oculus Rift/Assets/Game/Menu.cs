using UnityEngine;
using System.Collections;
using Leap;
using operation;

public class Menu : MonoBehaviour {
    int[] m_Fingers = new int[2];
    public int SceneMoveFrame;
    private int SceneMoveCount = 0;

        Controller controller = new Controller();
    // Use this for initialization
    void Start () {
        SceneMoveCount = 0;
	}

    // Update is called once per frame
    void Update()
    {

        Frame[] frame = new Frame[make2.make_plane2.n];//leapmotionのフレームをn個確保する.ただし最新のフレームは0です.
        Hand[] hand = new Hand[make2.make_plane2.n];//Handオブジェクトをn個確保する.ただし最新情報は0です.
        FingerList[] m_Finger = new FingerList[make2.make_plane2.n];


        for (int i = 0; i < make2.make_plane2.n; i++) //n個分のフレーム情報を取得するためのループ
        {
            frame[i] = controller.Frame(i);//frame情報を取得
            hand[i] = frame[i].Hands.Frontmost;//Hand情報を取得
            m_Finger[i] = frame[i].Fingers;
        }

        m_Fingers[0] = m_Fingers[1];
        m_Fingers[1] = make2.make_plane2.GetFingerCount(hand, frame);
        if (m_Fingers[0] == m_Fingers[1])
        {
            SceneMoveCount++;
        }
        else
        {
            SceneMoveCount = 0;
        }

        if (SceneMoveCount >= SceneMoveFrame)
        {
            switch (m_Fingers[0])
            {
                case 1:
                    Application.LoadLevel("for_picture");
                    break;
                default:
                    break;
            }
        }

        Debug.Log(m_Fingers[0]);
	}
}
