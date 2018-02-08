using UnityEngine;
using System.Collections;

namespace SonusViz.LightGrid {
  public class LightCell : MonoBehaviour {

    public int FrequencyIndex { get; set; }

    private float m_spectrumValue = 0.0f;

    private AudioInputSource m_audioSource;

    private UnityEngine.UI.Image m_image;

    // Use this for initialization
    void Start() {
      m_audioSource = AudioInputSource.Instance;
      m_image = GetComponent<UnityEngine.UI.Image>();
    }

    // Update is called once per frame
    void Update() {

      // decrease brightness over time
      m_spectrumValue = Mathf.Max(0.0f, m_spectrumValue - Time.deltaTime * 100000.0f);

      // get a range instead of a single index
      for (int i = 0; i < 20; ++i) {
        m_spectrumValue += m_audioSource.m_spectrum[FrequencyIndex + i] * 100.0f;
      }

      m_image.color = Color.HSVToRGB(0.0f, 0.0f, m_spectrumValue);
    }
  }

}
