using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScript : MonoBehaviour
{
    [SerializeField] string initPrefabName = "";                // 最初に読み込まれるプレハブ名

    PrefabManager prefabManager;                                // プレハブ管理用オブジェクト
    CameraScript camera;                                        // カメラ操作用オブジェクト

    /**
     * 起動時に一度だけ実行
     */
    void Awake()
    {
        // プレハブ管理用クラスのインスタンス生成
        this.prefabManager = new PrefabManager();

        // カメラ操作用オブジェクトの取得
        this.camera = Camera.main.GetComponent<CameraScript>();
    }

    /**
     * 起動後に一度だけ実行
     */
    void Start()
    {
        if (this.initPrefabName.Equals(""))
        // 指定がないとき(空文字列のとき)
        {
            throw new System.Exception("Error: InitPrefabName is empty");
        }
        else
        // 指定されたプレハブを読み込み
        {
            this.prefabManager.ChangePrefab(initPrefabName);
        }
    }

    /**
     * RayCast管理用メソッド
     */
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        // 左クリック時
        {
            // メインカメラ上のマウス位置からRayをとばす
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            // Raycastの当たり判定
            {
                GameObject obj = hit.collider.gameObject;           // Raycast衝突オブジェクト取得
                ButtonInfo info = obj.GetComponent<ButtonInfo>();   // ButtonInfoインターフェースを備えたコンポーネント(Script)の実体を取得①

                string prefabName = info.GetNextPrefabName();       // ①から遷移先プレハブ名を取得
                float y = info.GetRotateY();                        // ①から遷移後の水平カメラ回転角度を取得

                this.camera.CameraRotateY(y);                       // カメラ回転
                this.prefabManager.ChangePrefab(prefabName);        // 指定名称を持つプレハブをロード
            }
        }
    }
}
