using UnityEngine;
using System.Collections;
using Leap;
using operation;

public class Menu : MonoBehaviour {
    public int SceneMoveFrame;
    public int[] m_Fingers = new int[2];
    private static int m_RunningDefficuty = 0;
    private int SceneMoveCount = 0;
    private bool isInitialized = false;
    public static int m_MaxOfMode = 2;
    Controller controller = new Controller();
    private TextMesh txt;

    public GameObject[] m_ModeText = new GameObject[m_MaxOfMode];
    public GameObject m_Enter;

    public int m_Freq;
    public int m_OnFrame;
    private int m_FrameCount = 0;

    // Use this for initialization
    void Start () {

        m_FrameCount = 0;
        SceneMoveCount = 0;

        controller.EnableGesture(Gesture.GestureType.TYPE_SWIPE);
        controller.Config.SetFloat("Gesture.Swipe.MinLength", 150.0f);
        controller.Config.SetFloat("Gesture.Swipe.MinVelocity", 80f);
        controller.Config.Save();

        m_ModeText[0] = GameObject.Find("mode1");
        m_ModeText[1] = GameObject.Find("mode2");

        m_Enter = GameObject.Find("letter'Enter to Swipe'");
        m_Enter.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        Frame[] frame = new Frame[make2.make_plane2.n];//leapmotionのフレームをn個確保する.ただし最新のフレームは0です.
        Hand[] hand = new Hand[make2.make_plane2.n];//Handオブジェクトをn個確保する.ただし最新情報は0です.
        FingerList[] m_Finger = new FingerList[make2.make_plane2.n];

        if (m_FrameCount > m_Freq)
            m_FrameCount = 0;

        for (int i = 0; i < make2.make_plane2.n; i++) //n個分のフレーム情報を取得するためのループ
        {
            frame[i] = controller.Frame(i);//frame情報を取得
            hand[i] = frame[i].Hands.Frontmost;//Hand情報を取得
            m_Finger[i] = frame[i].Fingers;
        }

        GestureList gestures = frame[0].Gestures();
        m_Fingers[0] = m_Fingers[1];
        m_Fingers[1] = make2.make_plane2.GetFingerCount(hand, frame);
        if ((m_Fingers[0] == m_Fingers[1]) && (m_Fingers[0] != 0) && (m_RunningDefficuty == 0) && (m_Fingers[0] <= m_MaxOfMode))
        {
            SceneMoveCount++;
        }
        else
        {
            SceneMoveCount = 0;
        }

        if (SceneMoveCount >= SceneMoveFrame)
        {
            m_RunningDefficuty = m_Fingers[0];
            txt = (TextMesh)m_ModeText[m_Fingers[0] - 1].GetComponent(typeof(TextMesh));
            txt.fontSize = 23;
            m_Enter.SetActive(true);
        }

        if(m_RunningDefficuty != 0)
        {
            if (++m_FrameCount < m_OnFrame)
                m_Enter.SetActive(true);
            else
                m_Enter.SetActive(false);

            for (int i = 0; i < gestures.Count; i++)
            {
                Gesture gesture = gestures[i];
                if (gesture.Type == Gesture.GestureType.TYPESWIPE)
                {
                    isInitialized = true;
                    controller.Dispose();
                    Application.LoadLevel("for_picture");
                }
            }
            if ((make2.make_plane2.Hand_judge(hand, frame) == 0) || (m_Fingers[0] != make2.make_plane2.GetFingerCount(hand,frame)))
            {
                m_RunningDefficuty = 0;
                txt.fontSize = 18;
                m_Enter.SetActive(false);
            }
        }
	}

    public static int GetRunningDificulty()
    {
        return m_RunningDefficuty;
    }


}
