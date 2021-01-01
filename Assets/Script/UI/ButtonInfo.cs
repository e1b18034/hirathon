/**
 * このインターフェース実装したクラスでは，
 * ボタン情報取得メソッド(GetNextPrefabName()及びGetRotateY())の実装が必要
*/
public interface ButtonInfo
{
    /**
     * ボタンクリック時の遷移先プレハブ名
    */
    string GetNextPrefabName();

    /**
     * 遷移後のカメラY方向
    */
    float GetRotateY();
}
