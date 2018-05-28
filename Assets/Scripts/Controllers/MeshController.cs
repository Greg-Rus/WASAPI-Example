using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeshController  {
    private readonly MeshView _view;
    private readonly MeshModel _model;
    private readonly MeshConfig _config;

    public MeshController(MeshView view, MeshModel model, MeshConfig config)
    {
        _view = view;
        _model = model;
        _config = config;
        _model.OnHistoryChanged = UpdateView;
    }

    private void UpdateView()
    {
        _view.UpdateMesh(GenerateMesh(_model.History));
    }

    public Mesh GenerateMesh(Queue<float[]> history)
    {
        var xSize = history.Peek().Length -1;
        var ySize = history.Count-1;
        Mesh mesh = new Mesh();

        var vertices = new List<Vector3>();
        var normals = new List<Vector3>();

        for (int y = 0; y <= ySize; y++)
        {
            var frame = history.ElementAt(y);
            for (int x = 0; x <= xSize; x++)
            {
                try {
                    vertices.Add(new Vector3(x, frame[x] * _config.boostAmount, y));
                    normals.Add(Vector3.up);
                }
                catch
                {
                    Debug.LogError("x: " + x + "  y: " + y);
                    //throw new ArgumentOutOfRangeException("x: " + x + "  y: " + y);
                }

            }
        }

        int[] triangles = new int[xSize * ySize * 6];
        for (int ti = 0, vi = 0, y = 0; y < ySize; y++, vi++)
        {
            for (int x = 0; x < xSize; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
                triangles[ti + 5] = vi + xSize + 2;
            }
        }
        mesh.SetVertices(vertices);
        mesh.SetNormals(normals);
        mesh.triangles = triangles;
        return mesh;
    }
}
