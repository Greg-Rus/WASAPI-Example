using UnityEngine;
using System.Collections;

public class GravityCube : MonoBehaviour {

  [SerializeField]
  private int m_frequencyIndex;

  private SonusViz.AudioInputSource m_audioSource;

  private Renderer m_renderer;
  private Rigidbody m_rigidBody;

  // Use this for initialization
  void Start() {
    m_audioSource = SonusViz.AudioInputSource.Instance;
    m_renderer = GetComponent<Renderer>();
    m_rigidBody = GetComponent<Rigidbody>();
  }

  // Update is called once per frame
  void Update() {

    float spectrumValue = 0;

    // get a range instead of a single index
    for (int i = 0; i < 20; ++i) {
      spectrumValue += m_audioSource.m_spectrum[m_frequencyIndex + i] * 100.0f;
    }

    float distFromZero = 0.3f + transform.position.magnitude;

    // move myself toward center
    m_rigidBody.velocity += -transform.position.normalized * Time.deltaTime * distFromZero * 50.0f;

    // push away from center
    m_rigidBody.velocity += transform.position.normalized * spectrumValue * 1.0f / distFromZero * Time.deltaTime * 50.0f;
    m_renderer.material.color = Color.HSVToRGB(0.0f, 0.0f, spectrumValue);

    // set speed based on spectrum value
    m_rigidBody.velocity = m_rigidBody.velocity.normalized * spectrumValue * 20.0f;


  }
}
