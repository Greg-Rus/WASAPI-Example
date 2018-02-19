using System;
using UniRx;
using UnityEngine;
public class CubeView : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public Color32 normalColor = Color.white;
    private IDisposable updateSubsctiption;

    private float t = 0;
    public float speed = 1;

    public void ColorCube(Color32 color)
    {
        t = 0;

        if(updateSubsctiption != null) updateSubsctiption.Dispose();

        updateSubsctiption = Observable.EveryUpdate().Subscribe(_ =>
        {
            t += Time.deltaTime * speed;
            meshRenderer.material.color = Color.Lerp(color, normalColor, t);
            if(t >= 1f) updateSubsctiption.Dispose();
        });
    }


}