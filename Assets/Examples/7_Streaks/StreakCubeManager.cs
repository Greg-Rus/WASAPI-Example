using UnityEngine;
using System.Collections.Generic;

namespace SonusViz.Streaks {
  public class StreakCubeManager : MonoBehaviour {

    [SerializeField]
    private StreakCube m_streakCubePrefab;

    private int m_streakCubeCountMax = 25;
    private float m_lastSpawnTime = 0.0f;

    private HashSet<StreakCube> m_streakCubes = new HashSet<StreakCube>();
    private List<StreakCube> m_destroyQueue = new List<StreakCube>();

    private void Update() {
      if (Time.time - m_lastSpawnTime > 0.15f && m_streakCubes.Count < m_streakCubeCountMax) {
        m_lastSpawnTime = Time.time;
        CreateCubeStreak();
      }
      CheckDestroyCube();
    }

    private void CheckDestroyCube() {
      foreach (StreakCube cube in m_streakCubes) {
        if (cube.transform.position.y > 15.0f) {
          m_destroyQueue.Add(cube);
        }
      }
      foreach (StreakCube cube in m_destroyQueue) {
        m_streakCubes.Remove(cube);
        GameObject.Destroy(cube.gameObject);
      }
      m_destroyQueue.Clear();
    }

    private void CreateCubeStreak() {
      StreakCube instance = GameObject.Instantiate(m_streakCubePrefab.gameObject).GetComponent<StreakCube>();
      instance.transform.position = new Vector3(Random.Range(-5.0f, 5.0f), -10.0f, 0.0f);
      Rigidbody body = instance.GetComponent<Rigidbody>();
      body.velocity = Vector3.up * Random.Range(5.0f, 10.5f);
      body.angularVelocity = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)) * 10.0f;
      m_streakCubes.Add(instance);
    }
  }
}