using UnityEngine;

namespace SonusViz {
  public class BackgroundFlasher : MonoBehaviour {

    AudioInputSource m_audioSource;

    [SerializeField]
    private Color m_flashColor = Color.white;

    void Start() {
      m_audioSource = AudioInputSource.Instance;
    }

    void Update() {
      GetComponent<Camera>().backgroundColor = m_flashColor * m_audioSource.AverageVolume() * 70.0f;
    }
  }
}