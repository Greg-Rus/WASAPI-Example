using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using NUnit.Framework.Constraints;
using UniRx;
using UnityEngine;

public interface IReactiveBarData
{
    ReactiveProperty<float> GetReactiveBarAtIndex(int index);
    ReactiveProperty<float[]> ReactiveFrame{get;}
    int NumberOfBars { get; }
}

public class BasicSingleFrameModel : MonoBehaviour, IReactiveBarData
{
    private ReactiveProperty<float>[] _reactiveBars;
    private ReactiveProperty<float[]> _reactiveframe;
    private IBarData _barDataProvider;
    public float BarUpdateThreshold = 0.01f;

    public void Inject(IBarData barDataProvider)
    {
        _barDataProvider = barDataProvider;
    }

    public void Setup()
    {
        _reactiveBars = new ReactiveProperty<float>[_barDataProvider.NumberOfBars];
        for (int i = 0; i < _barDataProvider.NumberOfBars; i++)
        {
            _reactiveBars[i] = new ReactiveProperty<float>(0);
        }
        _reactiveframe = new ReactiveProperty<float[]>(new float[_barDataProvider.NumberOfBars]);
    }

    void Update()
    {
        for (int i = 0; i < _barDataProvider.NumberOfBars; i++)
        {
            _reactiveBars[i].Value = Mathf.Max(BarUpdateThreshold, _barDataProvider.BarValues[i]);
        }
        _reactiveframe.Value = _barDataProvider.BarValues.ToArray();
        //Debug.Log("Basic " + _barDataProvider.BarValues.Sum());
    }

    public int NumberOfBars
    {
        get { return _barDataProvider.NumberOfBars; }
    }

    public ReactiveProperty<float> GetReactiveBarAtIndex(int index)
    {
        if(index < _reactiveBars.Length) return _reactiveBars[index];
        else throw new ArgumentOutOfRangeException(string.Format("Requested index {0}, but array of reactive bars has lenght of {1}", index, _reactiveBars.Length));
    }

    public ReactiveProperty<float[]> ReactiveFrame
    {
        get { return _reactiveframe; }
    }
}
