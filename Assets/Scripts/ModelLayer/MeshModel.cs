using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using System;
using System.Linq;
using UniRx;
using UniRx.InternalUtil;

public class MeshModel {
    private readonly IReactiveBarData _singleFrameModel;
    private readonly MeshConfig _config;
    public Action OnHistoryChanged;
    private readonly Queue<float[]> _history;
    public Queue<float[]> History
    {
        get { return _history; }
    }

    public MeshModel(IReactiveBarData singleFrameModel, MeshConfig config)
    {
        _singleFrameModel = singleFrameModel;
        _config = config;
        _history = new Queue<float[]>(_config.HistoryFrameCount);

        //OnHistoryChanged = () => { Debug.Log(_history.Count); };

        SetupSubscription();

        

    }

    private void SetupSubscription()
    {
        IObserver<float[]> observer = new FrameObserver(UpdateFrameQueue);
        _singleFrameModel.ReactiveFrame.Subscribe(observer);
    }

    public void UpdateFrameQueue(float[] frame)
    {
        Debug.Log("Frame: " +frame.Sum());
        if (_history.Count == _config.HistoryFrameCount)
        {
            _history.Dequeue();
        }
        _history.Enqueue(frame);
        if(OnHistoryChanged != null) OnHistoryChanged();
    }
}

public class FrameObserver : IObserver<float[]>
{
    private readonly Action<float[]> _frameSetter;

    public FrameObserver(Action<float[]> frameSetter)
    {
        _frameSetter = frameSetter;
    }

    public void OnCompleted()
    {
        Debug.LogError("Completed");
    }

    public void OnError(Exception error)
    {
        Debug.LogError(error.Message);
    }

    public void OnNext(float[] value)
    {
        if(value != null) _frameSetter(value);
    }
}
