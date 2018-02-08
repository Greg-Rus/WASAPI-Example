using UnityEngine;
using System.Collections.Generic;

namespace SonuzViz.SquareHoles {

  public class AudioBlock : MonoBehaviour {

    [SerializeField]
    private bool m_randomizeFreq;

    [SerializeField]
    private int m_frequency;

    private SonusViz.AudioInputSource m_audioSource;
    private int m_frequencyIndex = 0;
    private Vector3 m_initScale;

    private Vector3 m_hueSat;

    void Start() {
      m_audioSource = SonusViz.AudioInputSource.Instance;

      m_hueSat = new Vector3(Random.Range(0.0f, 0.2f), 0.05f, m_frequencyIndex);
      float x = Random.Range(0.0f, 10.0f);
      if (m_randomizeFreq) {
        m_frequencyIndex = (int)(4.0f * Mathf.Pow(1.4f, x));
      } else {
        m_frequencyIndex = m_frequency;
      }

      m_initScale = transform.localScale;
    }

    void Update() {
      float spectrumValue = 0;
      for (int i = 0; i < 10; ++i) {
        spectrumValue += m_audioSource.m_spectrum[m_frequencyIndex + i] * 10.0f;
      }
      transform.localScale = m_initScale * spectrumValue * 2.0f;
      for (int i = 0; i < transform.GetComponentsInChildren<Renderer>().Length; ++i) {
        transform.GetComponentsInChildren<Renderer>()[i].material.color = Color.HSVToRGB(m_hueSat.x, m_hueSat.y, spectrumValue * 1.4f + 0.3f);
      }
    }
  }

}
