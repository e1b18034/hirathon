using System.Collections.Generic;
using UnityEngine;

public class PrefabManager
{
    private readonly Dictionary<string, GameObject> prefabMap;

    // コンストラクタ
    public PrefabManager()
    {
        this.prefabMap = new Dictionary<string, GameObject>();

        GameObject[] prefabs = Resources.LoadAll<GameObject>("360photo/Prefab/");
        foreach(GameObject prefab in prefabs)
        {
            this.prefabMap.Add(prefab.name, prefab);
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
}
