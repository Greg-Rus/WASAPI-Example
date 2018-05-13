using UnityEngine;

public class BarFactory : MonoBehaviour
{
    public CubeView cubePrefab;

    public CubeView MakeBarAtPosition(Vector3 position)
    {
        var visiBar = Instantiate(cubePrefab);
        visiBar.transform.position = position;
        visiBar.transform.parent = transform;
        return visiBar;
    }
}