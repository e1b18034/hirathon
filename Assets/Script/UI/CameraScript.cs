using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] float speedX = 2F;           // X軸回転速度
    [SerializeField] float speedY = 2F;           // Y軸回転速度
    [SerializeField] float minLimX = -60F;        // X軸角度の最小値
    [SerializeField] float maxLimX = 60F;         // X軸角度の最大値

    float rotationX = 0F;                         // X軸回転角度
    float rotationY = 0F;                         // Y軸回転角度

    object mutex;                                 // 排他制御用ミューテックス
    bool changedCameraRotationY;                  // CameraRotateYメソッドによるカメラ角度変更フラグ

    /**
     * フィールド等初期化用メソッド
     */
    void Awake()
    {
        this.mutex = new object();
        this.changedCameraRotationY = false;
    }

    /**
     * カメラ操作制御メソッド
     */
    void Update()
    {
        lock (mutex)
        {
            if (changedCameraRotationY)
            // 任意角度でのカメラ回転があった場合
            {
                changedCameraRotationY = false;
            }
            else
            {
                // マウス移動量取得
                float mouseX = Input.GetAxis("Mouse X");
                float mouseY = Input.GetAxis("Mouse Y");

                if (mouseX != 0 || mouseY != 0)
                // マウス移動がある場合
                {
                    // Y軸回転角度に水平移動量を加算
                    this.rotationY += mouseX * speedX;
                    // X軸回転角度に垂直移動量を加算
                    this.rotationX = ChangeRotationX(mouseY * speedY);

                    // Z軸回転角度取得
                    float rotationZ = Camera.main.transform.localEulerAngles.z;

                    // 回転
                    Camera.main.transform.localEulerAngles = new Vector3(rotationX, rotationY, rotationZ);
                }
            }
        }
    }

    /**
     * X軸回転角度のチェック
     */
    float ChangeRotationX(float addRotationX)
    {
        float resultRotationX = rotationX - addRotationX;

        if(resultRotationX < -360F - minLimX) {
            resultRotationX += 360F;
        }
        if(resultRotationX > 360F - maxLimX) {
            resultRotationX -= 360F;
        }

        if(resultRotationX < minLimX) {
            return minLimX;
        }
        if(resultRotationX > maxLimX) {
            return maxLimX;
        }

        return resultRotationX;
    }

    /**
     * カメラのy軸角度を任意の角度に変更する
     */
    public void CameraRotateY(float y)
    {
        lock (mutex)
        {
            // X軸回転角度の取得
            float x = Camera.main.transform.localEulerAngles.x;
            // Z軸回転角度の取得
            float z = Camera.main.transform.localEulerAngles.z;

            // Vector3オブジェクトを生成
            Vector3 cameraAngles = new Vector3(x, y, z);

            // 回転
            Camera.main.gameObject.transform.localEulerAngles = cameraAngles;

            // rotationXをカメラのX軸回転角度に同期
            this.rotationX = Camera.main.transform.localEulerAngles.x;
            // rotationYをカメラのY軸回転角度に同期
            this.rotationY = Camera.main.transform.localEulerAngles.y;

            // フラグをtrueに変更
            this.changedCameraRotationY = true;
        }
    }
}