using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SumBarController : MonoBehaviour {
    public float SumBarScaleFactor = 10;
    public float BeatThreshold;
    public float BeatHold = 0.2f;
    public float BeatDecay = 0.98f;
    private float BeatHoldTimer;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
