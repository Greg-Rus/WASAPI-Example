using UnityEngine;
using System.Collections;

namespace SonusViz.RadialFills {

  public class CircleFill : MonoBehaviour {

    [SerializeField]
    private UnityEngine.UI.Image m_circleImage;

    public int FrequencyIndex { get; set; }

    private AudioInputSource m_audioSource;

    private float m_hue;

    void Start() {
      m_audioSource = AudioInputSource.Instance;
      m_hue = Random.Range(0.0f, 0.05f);
    }

    // Update is called once per frame
    void Update() {
      float spectrumValue = 0.0f;

      // get a range instead of a single index
      for (int i = 0; i < 20; ++i) {
        spectrumValue += m_audioSource.m_spectrum[FrequencyIndex + i] * 100.0f;
      }

      m_circleImage.fillAmount = spectrumValue;
      m_circleImage.color = SonusViz.ColorHelper.ToColor(m_hue, 1.0f, spectrumValue, 1.0f);
    }
  }

}
