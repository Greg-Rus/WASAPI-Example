using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BufferedFrameModel : MonoBehaviour {

    private Queue<int> maxBarQueue;

    public IBarData BarData;
    // Use this for initialization
    void Start () {
        maxBarQueue = new Queue<int>(120);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private int? FindLargesBar()
    {
        var max = BarData.BarValues.Max();
        if (max >= 0.1f)
        {
            return BarData.BarValues.ToList().IndexOf(max);
        }
        return null;
    }

    private void UpdateBars()
    {
        var maxIndex = FindLargesBar();
        if (maxIndex.HasValue)
        {
            UpdateMaxBarQueue((int)maxIndex);
            var groups = GetBeatGroups();
            var candidateGroup = GetBeatCandidateBeatGroup(groups);
            var beatCandidateIndex = GetBeatCandidate(candidateGroup);
            //if (maxIndex == beatCandidateIndex)
            //{
            //    ColorBar((int)maxIndex, BeatColor);
            //}
            //else
            //{
            //    ColorBar((int)maxIndex, MaxColor);
            //}

        }
    }

    private void UpdateMaxBarQueue(int newMaxIndex)
    {
        //Debug.Log("Count: " + maxBarQueue.Count +" time: " + Time.time +" index: " + newMaxIndex);
        if (maxBarQueue.Count == 128) maxBarQueue.Dequeue();
        maxBarQueue.Enqueue(newMaxIndex);
    }

    private IOrderedEnumerable<IGrouping<int, int>> GetBeatGroups()
    {
        return maxBarQueue.GroupBy(bar => bar).OrderByDescending(group => group.Count());
    }

    private IGrouping<int, int> GetBeatCandidateBeatGroup(IOrderedEnumerable<IGrouping<int, int>> beatGroups)
    {
        return beatGroups.Take(1).First();
    }

    private int GetBeatCandidate(IGrouping<int, int> beatCandidateGroup)
    {
        return beatCandidateGroup.Key;
    }


}
