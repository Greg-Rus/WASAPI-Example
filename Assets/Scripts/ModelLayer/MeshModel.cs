using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using System;
using UniRx;
using UniRx.InternalUtil;

public class MeshModel {
    private readonly IReactiveBarData _singleFrameModel;
    private readonly MeshConfig _config;

    private readonly Queue<float[]> _history;
    private ReactiveCollection<float[]> _reactiveQueue;
 
    public MeshModel(IReactiveBarData singleFrameModel, MeshConfig config)
    {
        _singleFrameModel = singleFrameModel;
        _config = config;
        _history = new Queue<float[]>();
        _reactiveQueue  = _history.ToReactiveCollection();
        SetupSubscription();
        

    }

    private void SetupSubscription()
    {
        IObserver<float[]> observer = new FrameObserver(UpdateFrameQueue);
        _singleFrameModel.ReactiveFrame.Subscribe(observer);
    }

    public void UpdateFrameQueue(float[] frame)
    {
        _history.Dequeue();
        _history.Enqueue(frame);
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
        
    }

    public void OnError(Exception error)
    {
        
    }

    public void OnNext(float[] value)
    {
        _frameSetter(value);
    }
}
