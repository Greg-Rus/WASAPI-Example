using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SumBars : MonoBehaviour {
    public SoundCapture capture;
    public CubeView cubePrefab;
    public Color thresholdColor;
    public int rangeMin;
    public int rangeMax;
    public Vector3 position;

    private CubeView sumBar;
    private CubeView BeatThresholdObject;

    public float SumBarScaleFactor = 10;
    public float BeatThreshold;
    public float BeatHold = 0.2f;
    public float BeatDecay = 0.98f;
    private float BeatHoldTimer;


    void Start ()
    {
        MakeSumBar();
    }
    void MakeSumBar()
    {
        sumBar = Instantiate(cubePrefab);
        sumBar.transform.position = position;

        BeatThresholdObject = Instantiate(cubePrefab);
        BeatThresholdObject.transform.position = position;
        BeatThresholdObject.transform.localScale = new Vector3(1.5f, 0.5f, 1.5f);
    }

    void Update () {
	    UpdateBars();
    }

    private void UpdateBars()
    {
        lock (capture.barData)
        {
            UpdateSumBarValue(capture);
        }
    }
    private void UpdateSumBarValue(SoundCapture capturedFrame)
    {
        var rangeSum = capturedFrame.barData.Skip(rangeMin).Take(rangeMax - rangeMin).Sum();
        sumBar.transform.position = new Vector3(position.x, rangeSum / 2.0f * SumBarScaleFactor, 0);
        sumBar.transform.localScale = new Vector3(1, rangeSum * SumBarScaleFactor, 1);
        if (rangeSum > BeatThreshold)
        {
            BeatThreshold = rangeSum;
            BeatHoldTimer = BeatHold;
            sumBar.ColorCube(thresholdColor, 0.5f);
            BeatThresholdObject.transform.position = new Vector3(position.x, BeatThreshold * SumBarScaleFactor, 0);
        }
        else
        {
            if (BeatHoldTimer > 0)
            {
                BeatHoldTimer -= Time.smoothDeltaTime;
            }
            else
            {
                BeatThreshold *= BeatDecay;
                BeatThresholdObject.transform.position = new Vector3(position.x, BeatThreshold * SumBarScaleFactor, 0);
            }
        }
    }
}
