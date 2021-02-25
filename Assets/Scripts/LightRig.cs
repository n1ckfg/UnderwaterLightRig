using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// // https://gamedev.stackexchange.com/questions/98632/drawing-a-pixel-to-rendertexture

public class LightRig : MonoBehaviour {

    public RenderTexture rtex;
    public MeshFilter meshFilter;
    public GameObject groupPrefab;
    public List<LightGroup> groups;
    public int pointBatch = 10;
    public bool ready = false;

    private Renderer ren;
    public Texture2D tex;
    private List<Vector3> vertices;
    private List<Vector2> uvs;

    private IEnumerator Start() {
        //tex = new Texture2D(rtex.width, rtex.height);
        ren = GetComponent<Renderer>();
        ren.sharedMaterial.mainTexture = tex;

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
                newPoints[j] = new LightPoint(vertices[loc], new Color(1f, 1f, 1f, 1f), uvs[loc], 1f, i, j);
            }

            LightGroup newGroup = GameObject.Instantiate(groupPrefab).GetComponent<LightGroup>();
            newGroup.transform.parent = transform;
            newGroup.init(newPoints);
            groups.Add(newGroup);
        }
        
        ready = true;
        yield return null;
    }

    public void loadPixels() {
        RenderTexture.active = rtex;
        //tex.ReadPixels(new Rect(0, 0, rtex.width, rtex.height), 0, 0);
    }
    
    public void setPixel(int index, Color col) {
        Vector2 uv = 
        int x = (int) (uvx * tex.width);
        int y = (int) (uvy * tex.height);
        tex.SetPixel(x, y, col);
    }

    public void updatePixels() {
        tex.Apply();
        RenderTexture.active = null;
    }

}
