using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshView : MonoBehaviour {

    public MeshFilter Filter;
	
	// Update is called once per frame
	public void UpdateMesh(Mesh mesh)
	{
	    Filter.mesh = mesh;
	}

}
