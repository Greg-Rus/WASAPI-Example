using UnityEngine;
using System.Collections.Generic;

namespace SonusViz.Spectrum {
  public class SpectrumSpawner : MonoBehaviour {

    [SerializeField]
    private SpectrumBar m_spectrumBarPrefab;

    private List<SpectrumBar> m_spectrumBars = new List<SpectrumBar>();

    private int m_barCount = 30;

    void Start() {
      // centered around 0
      for (int i = -m_barCount / 2; i < m_barCount / 2; ++i) {
        SpectrumBar newBar = GameObject.Instantiate(m_spectrumBarPrefab.gameObject).GetComponent<SpectrumBar>();
        newBar.transform.position = Vector3.zero + i * Vector3.right * 0.3f;
        m_spectrumBars.Add(newBar);
      }

      // set frequency indicies, left to right.
      for (int i = 0; i < m_spectrumBars.Count; ++i) {
        m_spectrumBars[i].FrequencyIndex = (i * 6);
      }
    }
  }
}
