using UnityEngine;
using System.Collections;

namespace SonusViz {
  public class KeyboardSpectrumMultiplierSetter : MonoBehaviour {

    [SerializeField]
    private float m_incrementalAmount = 0.1f;

    void Update() {
      if (Input.GetKeyDown(KeyCode.Minus)) {
        AudioInputSource.Instance.AddToSpectrumMultiplier(-m_incrementalAmount);
      }
      if (Input.GetKeyDown(KeyCode.Equals)) {
        AudioInputSource.Instance.AddToSpectrumMultiplier(m_incrementalAmount);
      }
    }
  }

}
