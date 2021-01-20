using System.Collections.Generic;
using UnityEngine;

public class PrefabManager
{
    private readonly Dictionary<string, GameObject> prefabMap;
    List<string> pointNameList;
    Dictionary<string, string> prefabPointDictionary;

    // コンストラクタ
    public PrefabManager()
    {
        this.prefabMap = new Dictionary<string, GameObject>();
        this.pointNameList = new List<string>();
        this.prefabPointDictionary = new Dictionary<string, string>();

        GameObject[] prefabs = Resources.LoadAll<GameObject>("360photo/Prefab/03/");
        foreach(GameObject prefab in prefabs)
        {
            this.prefabMap.Add(prefab.name, prefab);

            PrefabInfo info = prefab.GetComponent<PrefabInfo>();
            if(info != null) {
                this.pointNameList.Add(info.GetPointName());
                this.prefabPointDictionary.Add(info.GetPointName(), prefab.name);
            }
        }
    }

    // 指定名称のプレハブに移動
    public void ChangePrefab(string prefabName)
    {
        if (!this.prefabMap.ContainsKey(prefabName)) return;

        GameObject objectX = GameObject.Find("MainObject");
        foreach(Transform child in objectX.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        GameObject prefab = this.prefabMap[prefabName];
        GameObject instance = GameObject.Instantiate(prefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        instance.transform.parent = objectX.transform;
    }

    public string[] GetAllPrefabNames() {
        string[] keys = new string[prefabMap.Count];

        prefabMap.Keys.CopyTo(keys, 0);

        return keys;
    }

    public List<string> GetAllPointName() {
        return this.pointNameList;
    }

    public Dictionary<string, string> GetPrefabPointNameDictionary() {
        return this.prefabPointDictionary;
    }
}
