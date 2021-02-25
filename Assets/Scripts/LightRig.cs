using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightRig : MonoBehaviour {

    public MeshFilter meshFilter;
    public GameObject groupPrefab;
    public List<LightGroup> groups;
    public int pointBatch = 10;
    public bool ready = false;

    private List<Vector3> vertices;
    private List<Vector2> uvs;

    private IEnumerator Start() {
        groups = new List<LightGroup>();

        vertices = new List<Vector3>();
        uvs = new List<Vector2>();

        meshFilter.mesh.GetVertices(vertices);
        meshFilter.mesh.GetUVs(0, uvs);

        for (int i = 0; i < vertices.Count; i += pointBatch) {
            int lastPoint = pointBatch;
            if (vertices.Count - i < pointBatch) lastPoint = vertices.Count - i;
            LightPoint[] newPoints = new LightPoint[lastPoint];

            for (int j = 0; j < lastPoint; j++) {
                int loc = i + j;
                newPoints[j] = new LightPoint(vertices[loc], new Color(1f, 1f, 0f, 1f), uvs[loc], 1f, 0, 0);
            }

            LightGroup newGroup = GameObject.Instantiate(groupPrefab).GetComponent<LightGroup>();
            newGroup.transform.parent = transform;
            newGroup.init(newPoints);
            groups.Add(newGroup);
        }
        
        ready = true;
        yield return null;
    }

}
