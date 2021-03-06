﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScript : MonoBehaviour
{
    [SerializeField] string initPrefabName = "";                // 最初に読み込まれるプレハブ名

    PrefabManager prefabManager;                                // プレハブ管理用オブジェクト
    new CameraScript camera;                                        // カメラ操作用オブジェクト
    new Animation animation;
    AnimationState fadeoutAnimationState;                       // フェードアウト専用アニメーション
    AnimationState fadeinAnimationState;                        // フェードイン専用アニメーション
    StopWatch stopWatch;                                        // 時間計測用オブジェクト
    string prefabName;
    Dictionary<string, string> prefabPointDictionary = new Dictionary<string, string>();


    /**
     * 起動時に一度だけ実行
     */
    void Awake()
    {
        // プレハブ管理用クラスのインスタンス生成
        this.prefabManager = new PrefabManager();

        // カメラ操作用オブジェクトの取得
        this.camera = Camera.main.GetComponent<CameraScript>();

        // フェードアウトアニメーション取得
        GameObject cupsule = GameObject.Find("CameraCupsule");
        if(cupsule == null) {
            throw new UnityException("CameraCupsule is not found");
        }

        // アニメーション取得
        this.animation = cupsule.GetComponent<Animation>();
        if(this.animation == null) {
            throw new MissingComponentException("failed to get animation component");
        } else if(this.animation.GetClipCount() != 2) {
            throw new MissingComponentException("number of animation is not 2");
        }

        // フェードアウト及びフェードインアニメショーン取得
        this.fadeoutAnimationState = this.animation["FadeoutAnimation"];
        this.fadeinAnimationState = this.animation["FadeinAnimation"];

        // ストップウォッチ
        this.stopWatch = this.GetComponent<StopWatch>();
        if(this.stopWatch == null) {
            throw new MissingComponentException("StopWatch is not found");
        }

        // 滞在データエディタの初期化(引数は取り扱う全プレハブ名)
        StayDataEditor.Initialize(this.prefabManager.GetAllPrefabNames());
    }

    /**
     * 起動後に一度だけ実行
     */
    void Start()
    {
        // カーソルロック(デバッグ時)
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        if (this.initPrefabName.Equals(""))
        // 指定がないとき(空文字列のとき)
        {
            throw new System.Exception("Error: InitPrefabName is empty");
        }
        else
        // 指定されたプレハブを読み込み
        {
            this.prefabManager.ChangePrefab(initPrefabName);
            this.prefabName = initPrefabName;
            this.stopWatch.Alternate();
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

                if(info != null) {
                    this.stopWatch.Alternate();
                    StayDataEditor.UpdateStayTime(this.prefabName, this.stopWatch.GetTime());
                    this.stopWatch.ResetStopWatch();

                    this.prefabName = info.GetNextPrefabName();       // ①から遷移先プレハブ名を取得
                    float y = info.GetRotateY();                      // ①から遷移後の水平カメラ回転角度を取得

                    this.animation.Play(this.fadeoutAnimationState.name);

                    this.camera.CameraRotateY(y);                       // カメラ回転
                    this.prefabManager.ChangePrefab(this.prefabName);   // 指定名称を持つプレハブをロード

                    this.animation.Play(this.fadeinAnimationState.name);

                    this.stopWatch.Alternate();
                }
            }
        }
    }

    void OnApplicationQuit() {
        if(!this.stopWatch.IsStop()) {
            this.stopWatch.Alternate();
            StayDataEditor.UpdateStayTime(this.prefabName, this.stopWatch.GetTime());
        }

        // * 全データを外部ファイルに保存
        StayDataEditor.SaveAllData();

        // * データを平均滞在時間でソートし，外部ファイルに保存
        StayDataEditor.SaveSortedByTime(this.prefabManager.GetPrefabPointNameDictionary());
    }
}
