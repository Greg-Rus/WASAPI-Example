using UnityEngine;
using System.Collections;

namespace SonusViz.SquareHoles {
  public class SquareHoleSpawner : MonoBehaviour {

    [SerializeField]
    private GameObject m_audioBlockPrefab;

    void Start() {
      for (int i = 0; i < 30; ++i) {
        GameObject instance = GameObject.Instantiate(m_audioBlockPrefab.gameObject);
        instance.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0f, Random.Range(0.0f, 360.0f)));
      }
    }
  }
}
