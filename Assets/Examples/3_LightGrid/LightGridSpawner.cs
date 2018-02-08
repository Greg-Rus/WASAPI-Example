using UnityEngine;
using System.Collections;

namespace SonusViz.LightGrid {

  public class LightGridSpawner : MonoBehaviour {

    [SerializeField]
    private Canvas m_mainCanvas;

    [SerializeField]
    private LightCell m_lightCellPrefab;

    private int m_width = 14;
    private int m_height = 14;

    void Start() {
      for (int i = -m_width / 2; i <= m_width / 2; ++i) {
        for (int j = -m_height / 2; j <= m_height / 2; ++j) {
          LightCell newCell = GameObject.Instantiate(m_lightCellPrefab.gameObject).GetComponent<LightCell>();
          newCell.GetComponent<RectTransform>().position = new Vector3(i * 80.0f, j * 80.0f, 0.0f);
          newCell.transform.SetParent(m_mainCanvas.transform, false);
          newCell.FrequencyIndex = GetWeightedIndex();
        }
      }
    }

    private int GetWeightedIndex() {
      float x = Random.Range(0.0f, 10.0f);
      return (int)(4.0f * Mathf.Pow(1.4f, x));
    }
  }

}
