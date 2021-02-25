using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://gamedev.stackexchange.com/questions/98632/drawing-a-pixel-to-rendertexture
// https://forum.unity.com/threads/best-easiest-way-to-change-color-of-certain-pixels-in-a-single-sprite.223030/
// https://docs.unity3d.com/ScriptReference/Texture2D.SetPixels.html

public class LightRig : MonoBehaviour {

    public MeshFilter meshFilter;
    public GameObject groupPrefab;
    public LightPoint[] points;
    public List<LightGroup> groups;
    public float updateColorInterval = 1f;
    public int pointBatch = 10;
    public bool ready = false;

    private Renderer ren;
    private Texture2D tex;
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
        tex = Instantiate(ren.material.mainTexture) as Texture2D;
        ren.material.mainTexture = tex;

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

    public void setPixels() {
        tex.SetPixels(getAllCols(), 0);
    }

    public void updatePixels() {
        tex.Apply(false); // don't recalculate mip levels
    }

    private IEnumerator updateValues() {
        while (true) {
            for (int i = 0; i < groups.Count; i++) {
                setGroupColor(groups[i]);
                setGroupBrightness(groups[i]);
            }

            setPixels();
            updatePixels();

            yield return new WaitForSeconds(updateColorInterval);
        }
    }

    public void setGroupPosition(LightGroup group) {
        int loc = group.indices.Length / 2;
        group.transform.localPosition = points[group.indices[loc]].position;       
    }

    public void setGroupColor(LightGroup group) {
        Color avgColor = Color.black;
        for (int i = 0; i < group.indices.Length; i++) {
            avgColor += points[group.indices[i]].color;
        }
        
        group.avgColor = avgColor /= group.indices.Length;
    }

    public void setGroupBrightness(LightGroup group) {
        float avgBrightness = 0f;
        for (int i = 0; i < group.indices.Length; i++) {
            avgBrightness += points[group.indices[i]].brightness;
        }

        group.avgBrightness = avgBrightness / group.indices.Length;
    }
    
    public Color[] getAllCols() {
        Color[] cols = new Color[tex.width*tex.height];
        for (int i=0; i<points.Length; i++) {
            int x = (int) (points[i].uv.x * tex.width);
            int y = (int) (points[i].uv.y * tex.height);
            int loc = x + y * tex.width;
            cols[loc] = new Color(1f, 0f, 0f);// points[i].color;
        }
        return cols;
    }

}
