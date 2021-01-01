using UnityEngine;

public class Destination3 : MonoBehaviour, ButtonInfo
{
    [SerializeField] string nextPrefabName = "";
    [SerializeField] float rotateY = 0.0f;

    string ButtonInfo.GetNextPrefabName()
    {
        return this.nextPrefabName;
    }

    float ButtonInfo.GetRotateY()
    {
        return this.rotateY;
    }
}
