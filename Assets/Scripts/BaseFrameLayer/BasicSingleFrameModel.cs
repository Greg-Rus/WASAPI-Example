using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using NUnit.Framework.Constraints;
using UniRx;
using UnityEngine;

public interface IReactiveBarData
{
    ReactiveProperty<float> GetReactiveBarAtIndex(int index);
    int NumberOfBars { get; }
}

public class BasicSingleFrameModel : MonoBehaviour, IReactiveBarData
{
    private ReactiveProperty<float>[] _reactiveBars;
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
    }

    void Update()
    {
        for (int i = 0; i < _barDataProvider.NumberOfBars; i++)
        {
            _reactiveBars[i].Value = Mathf.Max(BarUpdateThreshold, _barDataProvider.BarValues[i]);
        }
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
}
