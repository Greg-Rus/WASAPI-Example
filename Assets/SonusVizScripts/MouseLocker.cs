using UnityEngine;
using System.Collections;

namespace SonusViz {
  public class MouseLocker : MonoBehaviour {
    void Start() {
      Cursor.lockState = CursorLockMode.Locked;
      Cursor.visible = false;
    }
  }
}
