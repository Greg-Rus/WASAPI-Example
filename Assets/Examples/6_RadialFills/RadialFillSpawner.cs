using UnityEngine;
using System.Collections;

namespace SonusViz.RadialFills {

  public class RadialFillSpawner : MonoBehaviour {

    [SerializeField]
    private CircleFill m_circleFillPrefab;

    [SerializeField]
    private Transform m_canvas;

    private int m_width = 10;
    private int m_height = 10;

    // Use this for initialization
    void Start() {
      for (int i = -m_width / 2; i <= m_width / 2; ++i) {
        for (int j = -m_height / 2; j <= m_height / 2; ++j) {
          CircleFill instance = GameObject.Instantiate(m_circleFillPrefab.gameObject).GetComponent<CircleFill>();
          instance.GetComponent<RectTransform>().position = new Vector3(i * 60.0f, j * 60.0f, 0.0f);
          instance.transform.SetParent(m_canvas.transform, false);
          instance.FrequencyIndex = GetWeightedIndex();
        }
      }
    }

    private int GetWeightedIndex() {
      float x = Random.Range(0.0f, 10.0f);
      return (int)(4.0f * Mathf.Pow(1.4f, x));
    }

  }

}
