using UnityEngine;
using System.Collections;

namespace SonusViz {
  public class ScreenshotHelper : MonoBehaviour {

    int screenshot_count_ = 0;

    // Update is called once per frame
    void Update() {
#if UNITY_EDITOR
      if (Input.GetKeyDown(KeyCode.S)) {
        ScreenCapture.CaptureScreenshot("Screenshot" + screenshot_count_ + ".png", 2);
        screenshot_count_++;
      }
#endif
    }
  }
}