using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public static class StayDataEditor
{
    class StayData {
        public int visitCount;
        public float averageStayTime;

        public StayData(int visitCount, float averageStayTime) {
            this.visitCount = visitCount;
            this.averageStayTime = averageStayTime;
        }
    }

    class Pair {
        public string pointName;
        public StayData stayData;

        public Pair(string pointName, StayData stayData) {
            this.pointName = pointName;
            this.stayData = stayData;
        }
    }

    static Dictionary<string, StayData> datas = new Dictionary<string, StayData>();
    static bool initialized = false;

    public static void Initialize(string[] prefabNames) {
        // * リーダを生成
        StreamReader reader = new StreamReader(Application.dataPath + "/Resources/Data/StayData.txt");

        // * ディクショナリに登録及びプレハブ名取得
        for(int i = 0; i < prefabNames.Length; i++) {
            string line = reader.ReadLine();

            string[] elements = line.Split(' ');
            int visitCount = int.Parse(elements[0]);
            float averageStayTime = float.Parse(elements[1]);

            datas.Add(prefabNames[i], new StayData(visitCount, averageStayTime));
        }

        // * リーダをクローズ
        reader.Close();

        // * 初期化完了フラグを立てる
        initialized = true;
    }

    public static void SaveAllData() {
        // * 未初期化に対しての例外発生
        if(!initialized) throw new Exception("Uninitialized");

        // * ライターを生成
        StreamWriter writer = new StreamWriter(Application.dataPath + "/Resources/Data/StayData.txt", false);

        // * 一行ずつ書込み
        foreach(StayData data in datas.Values) {
            writer.WriteLine(data.visitCount + " " + data.averageStayTime);
        }

        // * ライターを閉じる
        writer.Close();
    }

    public static void UpdateStayTime(string visitPrefabName, float time) {
        // * 未初期化に対しての例外発生
        if(!initialized) throw new Exception("Uninitialized");

        // * 必要データ取得
        StayData stayData = datas[visitPrefabName];
        int visitCount = stayData.visitCount;
        float averageStayTime = stayData.averageStayTime;

        // * 更新データ計算
        float sumTime = averageStayTime * visitCount + time;
        visitCount++;
        averageStayTime = sumTime / visitCount;

        // * 更新データをインスタンスに格納
        stayData.visitCount = visitCount;
        stayData.averageStayTime = averageStayTime;

        // * リストに戻す
        datas[visitPrefabName] = stayData;
    }

    public static void SaveSortedByTime(Dictionary<string, string> prefabPointNameDictionary) {
        // * 未初期化に対しての例外発生
        if(!initialized) throw new Exception("Uninitialized");

        // * 必要データ抽出
        List<Pair> stayDataList = new List<Pair>();
        foreach(string pointName in prefabPointNameDictionary.Keys) {
            StayData data = datas[prefabPointNameDictionary[pointName]];
            Pair pair = new Pair(pointName, data);
            stayDataList.Add(pair);
        }

        // * ソート
        int maxIndex = 0;
        for(int i = 0; i < stayDataList.Count - 1; i++) {
            maxIndex = i;
            for(int j = i + 1; j < stayDataList.Count; j++) {
                float timeMax = stayDataList[maxIndex].stayData.averageStayTime;
                float timeJ = stayDataList[j].stayData.averageStayTime;
                if(timeJ > timeMax) {
                    maxIndex = j;
                }
            }

            Pair pair = stayDataList[maxIndex];
            stayDataList[maxIndex] = stayDataList[i];
            stayDataList[i] = pair;
        }

        // * 結果出力
        StreamWriter writer = new StreamWriter(Application.dataPath + "/Resources/Data/SortedByTime.txt", false);
        for(int i = 0; i < stayDataList.Count; i++) {
            Pair pair = stayDataList[i];
            StayData data = pair.stayData;
            string line = pair.pointName + ": " + data.visitCount + " " + data.averageStayTime;

            writer.WriteLine(line);
        }
        writer.Close();
    }
}
