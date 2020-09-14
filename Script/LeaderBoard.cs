using NCMB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//タイトル画面のランキング
public class LeaderBoard : MonoBehaviour
{
    struct LeaderBoardSet
    {
        public string m_Name;
        public int    m_Score;

        public LeaderBoardSet(string name, int score)
        {
            m_Name  = name;
            m_Score = score;
        }
    }

    [SerializeField] private GameObject m_LeaderBoardPlefab = default;
    [SerializeField] private Transform  m_TargetCanvas      = default;
    [SerializeField] private int        m_HeightSpace       = 30;
    [SerializeField] private Color      m_1stColor          = Color.white;
    [SerializeField] private Color      m_2ndColor          = Color.white;
    [SerializeField] private Color      m_3rdColor          = Color.white;
    [SerializeField] private Color      m_Top10Color        = Color.white;

    private List<LeaderBoardSet> m_TopRankers   = null;

    private Vector3   m_OriginPos = new Vector3(64, 120, 0);

    private void Start()
    {
        FetchTopRankers();
    }

    // サーバーからトップ１０を取得
    public void FetchTopRankers()
    {
        // データストアのHiScoreクラスから検索
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("HiScore");
        query.OrderByDescending("score");
        query.Limit = 10;
        query.FindAsync((List<NCMBObject> objList, NCMBException e) => {

            if (e != null)
            {
                //検索失敗時の処理
            }
            else
            {
                //検索成功時の処理
                List<LeaderBoardSet> list = new List<LeaderBoardSet>();
                // 取得したレコードをリーダーボードクラスとして保存
                foreach (NCMBObject obj in objList)
                {
                    int s = System.Convert.ToInt32(obj["score"]);
                    string n = System.Convert.ToString(obj["name"]);
                    list.Add(new LeaderBoardSet(n, s));
                }
                m_TopRankers = list;
                ShowLeaderBoard();
            }
        });
    }

    //サーバーから取得したデータを表示する
    private void ShowLeaderBoard()
    {
        for (int i = 0; i < m_TopRankers.Count; i++)
        {
            GameObject board = default;

            board = Instantiate(m_LeaderBoardPlefab, m_TargetCanvas);
            board.transform.localPosition = m_OriginPos - new Vector3(0,  i * m_HeightSpace, 0);

            board.transform.Find("Name").GetComponent<TMPro.TextMeshProUGUI>().text  = m_TopRankers[i].m_Name;
            board.transform.Find("Score").GetComponent<TMPro.TextMeshProUGUI>().text = m_TopRankers[i].m_Score.ToString();

            TMPro.TextMeshProUGUI place = board.transform.Find("Place").GetComponent<TMPro.TextMeshProUGUI>();
            place.text = (i + 1).ToString();

            //順位によって色を変える
            if (i + 1 == 1)
                place.color = m_1stColor;
            else if(i + 1 == 2)
                place.color = m_2ndColor;
            else if(i + 1 == 3)
                place.color = m_3rdColor;
            else
                place.color = m_Top10Color;
        }
    }
}