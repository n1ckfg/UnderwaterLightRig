using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightRig : MonoBehaviour {

    public MeshFilter meshFilter;
    public GameObject lightGroupPrefab;
    public List<LightGroup> groups;

    private List<Vector3> vertices;

    private void Start() {
        vertices = new List<Vector3>();
        meshFilter.mesh.GetVertices(vertices);
    }

    private void Update() {
        //
    }

}
