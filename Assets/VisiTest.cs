using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using UniRx;
using UnityEngine.UI;

public class VisiTest : MonoBehaviour {

    public SoundCapture capture;
    public RawImage image;
    public CubeView cubePrefab;
    CubeView[] bars;

    private ReactiveProperty<float>[] reactiveBars;
    public UnityEngine.Color MaxColor;
    public float MaxColorFadeTime = 0.3f;
    public UnityEngine.Color BeatColor;
    public float SumBarScaleFactor = 10;
    public float BeatThreshold;
    public float BeatHold = 0.2f;
    public float BeatDecay = 0.98f;
    private float BeatHoldTimer;

    private Queue<int> maxBarQueue;
    public Text bpm;

    // Use this for initialization
    void Start ()
    {
        Debug.Log("START");
        bars = new CubeView[capture.numBars];
        reactiveBars = new ReactiveProperty<float>[capture.numBars];
        maxBarQueue = new Queue<int>(120);
        MakeBars();
        InitializeReactiveBars();
    }

    private void InitializeReactiveBars()
    {
        Debug.Log("Making RX bars");
        for (int i = 0; i < capture.numBars; i++)
        {
            reactiveBars[i] = new ReactiveProperty<float>(0);
            var i1 = i;
            reactiveBars[i].Subscribe(curData =>
            {
                bars[i1].transform.position = new Vector3(i1, curData / 2.0f * 10.0f, 0);
                bars[i1].transform.localScale = new Vector3(1, curData * 10.0f, 1);
            });
        }
    }

    void MakeBars()
    {
        Debug.Log("Making bar objects");
        bars = new CubeView[capture.numBars];
        for (int i = 0; i < bars.Length; i++)
        {
            var visiBar = Instantiate(cubePrefab);
            visiBar.transform.position = new Vector3(i, 0, 0);
            visiBar.transform.parent = transform;
            bars[i] = visiBar;
        }
    }

	void Update () {
	    if (bars.Length != capture.numBars)
	    {
	        foreach (var bar in bars)
	        {
	            Destroy(bar);
	        }
	        MakeBars();
	    }
        UpdateBars();
	}

    private void UpdateBars()
    {
        lock (capture.barData)
        {
            UpdateBarValues(capture);
            var maxIndex = FindLargesBar();
            if (maxIndex.HasValue)
            {
                UpdateMaxBarQueue((int)maxIndex);
                var groups = GetBeatGroups();
                var candidateGroup = GetBeatCandidateBeatGroup(groups);
                var beatCandidateIndex = GetBeatCandidate(candidateGroup);
                if (maxIndex == beatCandidateIndex)
                {
                    ColorBar((int)maxIndex, BeatColor);
                }
                else
                {
                    ColorBar((int)maxIndex, MaxColor);
                }
                bpm.text = (candidateGroup.Count() * 0.5f).ToString();
            }
        }
    }

    private int? FindLargesBar()
    {
        var max = capture.barData.Max();
        if (max >= 0.1f)
        {
            return capture.barData.ToList().IndexOf(max);
        }
        return null;
    }

    private void UpdateBarValues(SoundCapture capturedFrame)
    {
        for (int i = 0; i < capturedFrame.numBars; i++)
        {
            reactiveBars[i].Value = Mathf.Max(0.01f, capturedFrame.barData[i]);
        }
        
    }

    

    private void ColorBar(int index, Color32 color)
    {
        bars[index].ColorCube(color,MaxColorFadeTime);
    }

    private void UpdateMaxBarQueue(int newMaxIndex)
    {
        //Debug.Log("Count: " + maxBarQueue.Count +" time: " + Time.time +" index: " + newMaxIndex);
        if (maxBarQueue.Count == 128) maxBarQueue.Dequeue();
        maxBarQueue.Enqueue(newMaxIndex);
    }

    private IOrderedEnumerable<IGrouping<int, int>> GetBeatGroups()
    {
        return maxBarQueue.GroupBy(bar => bar).OrderByDescending(group => group.Count());
    }

    private IGrouping<int, int> GetBeatCandidateBeatGroup(IOrderedEnumerable<IGrouping<int, int>> beatGroups)
    {
        return beatGroups.Take(1).First();
    }

    private int GetBeatCandidate(IGrouping<int, int> beatCandidateGroup)
    {
        return beatCandidateGroup.Key;
    }
}
