using UnityEngine;

namespace SonusViz {
  public class AudioInputSource : MonoBehaviour {

    const int m_sampleRate = 2048;
    public float[] m_spectrum;
    public float m_averageVolume;

    [SerializeField]
    private float m_spectrumMultiplier = 1.0f;

    [SerializeField]
    private AudioSource m_audioSource;

    static AudioInputSource m_instance = null;

    static public AudioInputSource Instance {
      get {
        return m_instance;
      }
    }
    void Awake() {

      m_instance = this;
      m_spectrum = new float[m_sampleRate];

      InitializeAudioSystem();
    }

    public void SetSpectrumMultiplier(float multiplier) {
      m_spectrumMultiplier = multiplier;
    }

    public void AddToSpectrumMultiplier(float delta) {
      m_spectrumMultiplier += delta;
    }

    public float AverageVolume() {
      float avg = 0;
      for (int i = 0; i < m_spectrum.Length; ++i) {
        avg += m_spectrum[i];
      }
      return avg / m_spectrum.Length;
    }

    public float MaxVolume() {
      float maxVolume = 0.0f;
      for (int i = 0; i < m_spectrum.Length; ++i) {
        if (m_spectrum[i] > maxVolume) {
          maxVolume = m_spectrum[i];
        }
      }
      return maxVolume;
    }

    private void InitializeAudioSystem() {

      m_audioSource.clip = Microphone.Start(Microphone.devices[0], true, 10, 44100);
      m_audioSource.loop = true;

      while (!(Microphone.GetPosition(Microphone.devices[0]) > 0)) {
        // wait until the recording has started
      }

      m_audioSource.Play();
      m_audioSource.GetSpectrumData(m_spectrum, 0, FFTWindow.BlackmanHarris);
    }

    private void TearDownAudioSystem() {
      m_audioSource.Stop();
      m_audioSource.clip = null;
      m_audioSource.loop = false;
      Microphone.End(null);
    }

    void Update() {
      m_audioSource.GetSpectrumData(m_spectrum, 0, FFTWindow.BlackmanHarris);
      for (int i = 0; i < m_spectrum.Length; ++i) {
        m_spectrum[i] *= m_spectrumMultiplier;
      }
      m_averageVolume = AverageVolume();
    }

    private void OnApplicationFocus(bool focused) {
      if (focused) {
        InitializeAudioSystem();
      } else {
        TearDownAudioSystem();
      }
    }
  }
}