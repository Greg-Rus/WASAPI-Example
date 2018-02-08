using UnityEngine;

namespace SonusViz {
  public static class ColorHelper {

    public static Color GetRandomHue(float sat, float brightness) {
      return ToColor(Random.Range(0.0f, 1.0f), sat, brightness, 1.0f);
    }

    public static Color ToColor(float hue, float saturation, float brightness, float alpha) {
      float red = brightness;
      float green = brightness;
      float blue = brightness;
      if (saturation != 0) {
        float max = brightness;
        float diff = brightness * saturation;
        float min = brightness - diff;

        float h = hue * 360f;

        if (h < 60f) {
          red = max;
          green = h * diff / 60f + min;
          blue = min;
        } else if (h < 120f) {
          red = -(h - 120f) * diff / 60f + min;
          green = max;
          blue = min;
        } else if (h < 180f) {
          red = min;
          green = max;
          blue = (h - 120f) * diff / 60f + min;
        } else if (h < 240f) {
          red = min;
          green = -(h - 240f) * diff / 60f + min;
          blue = max;
        } else if (h < 300f) {
          red = (h - 240f) * diff / 60f + min;
          green = min;
          blue = max;
        } else if (h <= 360f) {
          red = max;
          green = min;
          blue = -(h - 360f) * diff / 60 + min;
        } else {
          red = 0;
          green = 0;
          blue = 0;
        }
      }

      return new Color(Mathf.Clamp01(red), Mathf.Clamp01(green), Mathf.Clamp01(blue), alpha);
    }

  }
}