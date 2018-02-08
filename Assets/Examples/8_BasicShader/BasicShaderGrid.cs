using UnityEngine;
using System.Collections;

public class BasicShaderGrid : MonoBehaviour {

  private Material m_material;
  private SonusViz.AudioInputSource m_audioSource;
  private bool m_started = false;
  private Texture2D m_audioTexture;
  private Color[] m_audioTextColorArray = new Color[4096];

  void Awake() {
    m_material = new Material(Shader.Find("Hidden/BasicShaderGrid"));
    m_audioTexture = new Texture2D(64, 64, TextureFormat.RGBA32, false);
  }

  void Start() {
    m_audioSource = SonusViz.AudioInputSource.Instance;
    m_started = true;
  }

  void OnRenderImage(RenderTexture source, RenderTexture destination) {
    // only process the spectrum data once we've started
    if (m_started) {
      for (int x = 0; x < 64; ++x) {
        for (int y = 0; y < 64; ++y) {
          if (x + y * 64 >= 2048) {
            break;
          }
          m_audioTextColorArray[x + y * 64] = new Color(m_audioSource.m_spectrum[x + y * 64] * 300.0f, 0.0f, 0.0f, 1.0f);
        }
      }
      m_audioTexture.SetPixels(m_audioTextColorArray);
      m_audioTexture.Apply();
      m_material.SetTexture("_SpectrumTexture", m_audioTexture);
    }

    Graphics.Blit(source, destination, m_material);
  }
}
