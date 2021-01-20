using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopWatch : MonoBehaviour
{
    float sumTime;
    bool stop;

    // Start is called before the first frame update
    void Start()
    {
        this.sumTime = 0;
        this.stop = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!this.stop) {
            this.sumTime += Time.deltaTime;
        }
    }

    /**
     * * Stopのtrue/false反転
    */
    public void Alternate() {
        this.stop = !this.stop;
    }

    /**
     * * Stopであるか否か
    */
    public bool IsStop() {
        return this.stop;
    }

    /**
     * * 通常リセット
    */
    public void ResetStopWatch() {
        this.stop = true;
        this.sumTime = 0;
    }

    /**
     * * 初期値指定リセット
    */
    public void ResetStopWatch(float time) {
        this.stop = true;
        this.sumTime = time;
    }

    public float GetTime() {
        return this.sumTime;
    }
}
