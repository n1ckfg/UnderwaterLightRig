using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// // https://gamedev.stackexchange.com/questions/98632/drawing-a-pixel-to-rendertexture

public class LightRig : MonoBehaviour {

    public Texture2D tex;
    public MeshFilter meshFilter;
    public GameObject groupPrefab;
    public LightPoint[] points;
    public List<LightGroup> groups;
    public float updateColorInterval = 1f;
    public int pointBatch = 10;
    public bool ready = false;

    private Renderer ren;
    private List<Vector3> vertices;
    private List<Vector2> uvs;
    private Color defaultColor;

    private IEnumerator Start() {
        groups = new List<LightGroup>();

        vertices = new List<Vector3>();
        uvs = new List<Vector2>();

        meshFilter.mesh.GetVertices(vertices);
        points = new LightPoint[vertices.Count];
        meshFilter.mesh.GetUVs(0, uvs);
        defaultColor = new Color(1f, 0f, 0f, 1f);

        ren = GetComponent<Renderer>();

        for (int i = 0; i < vertices.Count; i += pointBatch) {
            int lastPoint = pointBatch;
            if (vertices.Count - i < pointBatch) lastPoint = vertices.Count - i;
            int[] newIndices = new int[lastPoint];

            for (int j = 0; j < lastPoint; j++) {
                int loc = i + j;
                newIndices[j] = loc;
                points[loc] = new LightPoint(vertices[loc], defaultColor, uvs[loc], 1f);
                setPixel(loc, defaultColor);
            }
            updatePixels();

            LightGroup newGroup = GameObject.Instantiate(groupPrefab).GetComponent<LightGroup>();
            newGroup.transform.parent = transform;
            newGroup.init(newIndices);
            setGroupPosition(newGroup);
            groups.Add(newGroup);
        }
        
        ready = true;

        StartCoroutine(updateValues());

        yield return null;
    }

    public void setPixel(int index, Color col) {
        Vector2 uv = points[index].uv;
        int x = (int) (uv.x * tex.width);
        int y = (int) (uv.y * tex.height);
        tex.SetPixel(x, y, col);
    }

    public void updatePixels() {
        tex.Apply();
    }

    private IEnumerator updateValues() {
        while (true) {
            for (int i = 0; i < groups.Count; i++) {
                setGroupColor(groups[i]);
                setGroupBrightness(groups[i]);
            }

            for (int i = 0; i < points.Length; i++) {
                setPixel(i, points[i].color);
            }
            updatePixels();

            yield return new WaitForSeconds(updateColorInterval);
        }
    }

    public void setGroupPosition(LightGroup group) {
        group.transform.localPosition = points[group.indices[0]].position;       
    }

    public void setGroupColor(LightGroup group) {
        Vector3 avgColorVec = Vector3.zero;
        for (int i = 0; i < group.indices.Length; i++) {
            avgColorVec += LightUtil.colToVec(points[group.indices[i]].color);
        }
        
        group.avgColorVec = avgColorVec /= group.indices.Length;
    }

    public void setGroupBrightness(LightGroup group) {
        float avgBrightness = 0f;
        for (int i = 0; i < group.indices.Length; i++) {
            avgBrightness += points[group.indices[i]].brightness;
        }

        group.avgBrightness = avgBrightness / group.indices.Length;
    }

}
