using UnityEngine;
using System.Collections;

namespace SonusViz.SimpleVolume {

  public class VolumeBlock : MonoBehaviour {

    private AudioInputSource m_audioSource;

    [SerializeField]
    private Renderer m_renderer;

    void Start() {
      m_audioSource = AudioInputSource.Instance;
    }

    // Update is called once per frame
    void Update() {
      float brightness = m_audioSource.MaxVolume() * 500.0f;
      m_renderer.material.color = Color.HSVToRGB(0.0f, 0.0f, brightness);
    }
  }

}
