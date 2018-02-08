using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniRx;

public class VisiTest : MonoBehaviour {

    public SoundCapture capture;


    GameObject[] bars;
    private ReactiveProperty<float>[] reactiveBars;


    // Use this for initialization
    void Start ()
    {
        bars = new GameObject[capture.numBars];
        reactiveBars = new ReactiveProperty<float>[capture.numBars];
        MakeBars();
        InitializeReactiveBars();
        


    }

    private void InitializeReactiveBars()
    {
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
        bars = new GameObject[capture.numBars];
        for (int i = 0; i < bars.Length; i++)
        {
            GameObject visiBar = GameObject.CreatePrimitive(PrimitiveType.Cube);
            visiBar.transform.position = new Vector3(i, 0, 0);
            visiBar.transform.parent = transform;
            visiBar.name = "VisiBar " + i;
            bars[i] = visiBar;
        }
    }
	
	// Update is called once per frame
	void Update () {
	    if (bars.Length != capture.numBars)
	    {
	        foreach (GameObject bar in bars)
	        {
	            Destroy(bar);
	        }

	        MakeBars();
	    }

        UpdateBars();
            



        // Since this is being changed on a seperate thread we do this to be safe
        //lock(capture.barData)
        //{
        //    for (int i = 0; i < capture.barData.Length; i++)
        //    {
        //        // Don't make the bars too short
        //        float curData = Mathf.Max(0.01f, capture.barData[i]);

        //        // Set offset so they stretch off the ground instead of expand in the air
        //        bars[i].transform.position = new Vector3(i, curData / 2.0f*10.0f, 0);
        //        bars[i].transform.localScale = new Vector3(1, curData*10.0f, 1);
        //    }
        //}
	}

    private void UpdateBars()
    {
        lock (capture.barData)
        {
            for (int i = 0; i < capture.numBars; i++)
            {
                reactiveBars[i].Value = Mathf.Max(0.01f, capture.barData[i]);
            }
        }
    }
}
