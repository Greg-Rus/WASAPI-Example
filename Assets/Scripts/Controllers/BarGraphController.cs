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

public class BarGraphController
{
    private CubeView[] _bars;
    private readonly BasicSingleFrameModel _basicSingleFrameModel;
    private readonly BarFactory _barFactory;

    public BarGraphController(BasicSingleFrameModel basicSingleFrameModel, BarFactory barFactory)
    {
        _basicSingleFrameModel = basicSingleFrameModel;
        _barFactory = barFactory;
        Setup();
    }


    // Use this for initialization
    void Setup()
    {
        Debug.Log("START");
        MakeBars();
        InitializeReactiveBarSubscriptions();
    }

    private void InitializeReactiveBarSubscriptions()
    {
        for (int i = 0; i < _basicSingleFrameModel.NumberOfBars; i++)
        {
            var i1 = i;
            _basicSingleFrameModel.GetReactiveBarAtIndex(i).Subscribe(curData =>
            {
                _bars[i1].transform.position = new Vector3(i1, curData / 2.0f * 10.0f, 0);
                _bars[i1].transform.localScale = new Vector3(1, curData * 10.0f, 1);
            });
        }
    }

    void MakeBars()
    {
        Debug.Log("Making bar objects");
        _bars = new CubeView[_basicSingleFrameModel.NumberOfBars];
        for (int i = 0; i < _bars.Length; i++)
        {
            _bars[i] = _barFactory.MakeBarAtPosition(new Vector3(i,0,0));
        }
    }
}