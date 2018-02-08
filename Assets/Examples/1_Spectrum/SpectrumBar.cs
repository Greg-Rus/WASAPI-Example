using UnityEngine;
using System.Collections;

namespace SonusViz.Spectrum {

  public class SpectrumBar : MonoBehaviour {

    public int FrequencyIndex { get; set; }

    private AudioInputSource m_audioSource;

    // Use this for initialization
    void Start() {
      m_audioSource = AudioInputSource.Instance;
    }

    // Update is called once per frame
    void Update() {
      float spectrumValue = 0.0f;

      // get a range instead of a single index
      for (int i = 0; i < 20; ++i) {
        spectrumValue += m_audioSource.m_spectrum[FrequencyIndex + i] * 100.0f;
      }

      transform.localScale = new Vector3(transform.localScale.x, spectrumValue, transform.localScale.z);
    }
  }

}