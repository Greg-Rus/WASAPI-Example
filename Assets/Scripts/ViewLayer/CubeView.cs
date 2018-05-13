using System;
using UniRx;
using UnityEngine;
public class CubeView : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public Color32 normalColor;
    private IDisposable updateSubsctiption;

    //private float t = 0;
    public float speed = 1;

    //public void ColorCube(Color32 color)
    //{
    //    t = 0;

    //    if(updateSubsctiption != null) updateSubsctiption.Dispose();

    //    updateSubsctiption = Observable.EveryUpdate().Subscribe(_ =>
    //    {
    //        t += Time.deltaTime * speed;
    //        meshRenderer.material.color = Color.Lerp(color, normalColor, t);
    //        if(t >= 1f) updateSubsctiption.Dispose();
    //    });
        
    //}

    private TimedUpdateLoop _timedUpdateLoop;

    public void ColorCube(Color32 color, float time = 3)
    {
        if(_timedUpdateLoop != null) _timedUpdateLoop.Dispose();
        _timedUpdateLoop = new TimedUpdateLoop(time, (t) => meshRenderer.material.color = Color.Lerp(normalColor, color, t) );
    }

}

public class TimedUpdateLoop : IDisposable
{
    private readonly Action<float> _excecuteEveryUpdateAction;
    private readonly IDisposable _subscription;

    private float _deadline;

    public TimedUpdateLoop(float timeSpanInSeconds, Action<float> excecuteEveryUpdateAction)
    {
        _excecuteEveryUpdateAction = excecuteEveryUpdateAction;
        _deadline = Time.time + timeSpanInSeconds;

        _subscription = Observable.EveryUpdate().TakeWhile(_ => Time.time < _deadline).Subscribe(_ =>
        {
            var percentElapsed = (_deadline - Time.time)/ timeSpanInSeconds;
            _excecuteEveryUpdateAction(percentElapsed);
        });
    }

    public void Dispose()
    {
        if(_subscription != null)
            _subscription.Dispose();
    }
}