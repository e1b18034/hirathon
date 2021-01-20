using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabInfo : MonoBehaviour
{
    [SerializeField] int modelCourseNumber = 0;
    [SerializeField] int pointNumber = 0;
    [SerializeField] string pointName = "";

    public int GetModelCourseNumber() {
        return this.modelCourseNumber;
    }

    public int GetPointNumber() {
        return this.pointNumber;
    }

    public string GetPointName() {
        return this.pointName;
    }
}
