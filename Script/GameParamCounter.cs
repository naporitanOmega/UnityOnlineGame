using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NCMB;

/// <summary>
/// 各種記録の保存＆読み込みクラス
/// </summary>
public static class GameParamCounter
{
    /// <summary>全プレイヤーのプレイ回数</summary>
    public static int m_Play;

    /// <summary>全プレイヤーの累計スコア</summary>
    public static int m_Cells;

    /// <summary>このゲームを遊んだプレイヤー数</summary>
    public static int m_Player;


    /// <summary>
    /// データストアにクラスを作る
    /// </summary>
    public static void CreateDataStoreClass(string className, string[] dataName, int initValue)
    {
        NCMBObject playCount = new NCMBObject(className);
        for (int i = 0; i < dataName.Length; i++)
        {
            playCount[dataName[i]] = 0;
        }
        playCount.SaveAsync();
    }

    /// <summary>
    /// サーバーに記録を保存(加算)
    /// </summary>
    public static void Save(string className, string dataName, int saveValue, int objListNum)
    {
        // データストアの「HighScore」クラスから、Nameをキーにして検索
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>(className);
        query.FindAsync((List<NCMBObject> objList, NCMBException e) => {

            //検索成功したら
            if (e == null)
            {
                int value = System.Convert.ToInt32(objList[objListNum][dataName]);
                objList[objListNum][dataName] = value + saveValue;
                objList[objListNum].SaveAsync();
            }
        });
    }

    /// <summary>
    /// サーバーからデータを取得して変数に代入
    /// </summary>
    public static void FetchData(string className, string dataName, int dataVar, int objListNum)
    {
        int value = default;

        // データストアの「HighScore」クラスから検索
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>(className);
        query.FindAsync((List<NCMBObject> objList, NCMBException e) => {

            if (e != null)
            {
                //検索失敗時の処理
                return;
            }
            else
            {
                Debug.Log(className + dataName + objList[objListNum][dataName]);
                value = System.Convert.ToInt32(objList[objListNum][dataName]);
            }
        });       
    }

    /// <summary>
    /// サーバーから全プレイヤーの累計プレイ回数を取得
    /// </summary>
    public static void FetchPlayCount()
    {
        // データストアの「HighScore」クラスから検索
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("PlayCount");
        query.FindAsync((List<NCMBObject> objList, NCMBException e) => {

            if (e != null)
            {
                //検索失敗時の処理
                return;
            }
            else
            {
                m_Play = System.Convert.ToInt32(objList[0]["PlayCount"]);
            }
        });
    }

    /// <summary>
    /// サーバーから全プレイヤーの累計スコアを取得
    /// </summary>
    public static void FetchCellsCount()
    {
        // データストアの「HighScore」クラスから検索
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("CellsCount");
        query.FindAsync((List<NCMBObject> objList, NCMBException e) => {

            if (e != null)
            {
                //検索失敗時の処理
                return;
            }
            else
            {
                m_Cells = System.Convert.ToInt32(objList[0]["CellsCount"]);
            }
        });
    }

    /// <summary>
    /// サーバーからランキング登録者数を取得
    /// </summary>
    public static void FetchPlayer()
    {
        // データストアの「HighScore」クラスから検索
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("HiScore");
        query.FindAsync((List<NCMBObject> objList, NCMBException e) => {

            if (e != null)
            {
                //検索失敗時の処理
            }
            else
            {
                m_Player = objList.Count;
            }
        });
    }
}
