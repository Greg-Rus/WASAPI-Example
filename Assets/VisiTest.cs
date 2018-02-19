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


    // Use this for initialization
    void Start ()
    {
        Debug.Log("START");
        bars = new CubeView[capture.numBars];
        reactiveBars = new ReactiveProperty<float>[capture.numBars];
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
            //visiBar.name = "VisiBar " + i;
            bars[i] = visiBar;
        }
    }
	
	// Update is called once per frame
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

	    //DrawGraph(); //memory leak!

	}

    private void DrawGraph()
    {
        Bitmap bitmap = capture.GetGraph();
        if (bitmap != null)
        {
            ImageConverter converter = new ImageConverter();
            byte[] ba;
            ba = (byte[])converter.ConvertTo(bitmap, typeof(byte[]));
            Texture2D tex = new Texture2D(800, 400);
            tex.LoadImage(ba);
            image.texture = tex;
        }
        
    }

    private void UpdateBars()
    {
        lock (capture.barData)
        {
            for (int i = 0; i < capture.numBars; i++)
            {
                reactiveBars[i].Value = Mathf.Max(0.01f, capture.barData[i]);
            }

            float max = capture.barData.Max();
            if (max >= 0.1f)
            {
                int maxIndex = capture.barData.ToList().IndexOf(max);
                ColorBar(maxIndex, UnityEngine.Color.red);
            }
            
        }
    }

    private void ColorBar(int inxed, Color32 color)
    {
        bars[inxed].ColorCube(color);
    }
}
