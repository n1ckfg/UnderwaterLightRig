using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightGroup : MonoBehaviour {

    public List<LightPoint> points;
    public Vector3 avgPosition;
    public Color avgColor;
    public Color currentColor;
    public float lerpSpeed = 0.01f;
    public float updateInterval = 1f;

    private Vector3 avgColorVec;
    private Vector3 currentColorVec;

    private void Start() {      
        points = new List<LightPoint>();
        avgPosition = Vector3.zero;
        avgColor = new Color(0, 0, 0);
        avgColorVec = Vector3.zero;
        currentColorVec = Vector3.zero;
    }

    public void update() {
        currentColorVec = Vector3.Lerp(colToVec(currentColor), avgColorVec, lerpSpeed);
        currentColor = vecToCol(currentColorVec);
    }

    public Vector3 getAvgPosition() {
        for (int i = 0; i < points.Count; i++) {
            avgPosition += points[i].position;
        }
        avgPosition /= (float) points.Count;
        return avgPosition;
    }

    public Color getAvgColor() {
        avgColorVec = Vector3.zero;
        for (int i = 0; i < points.Count; i++) {
            avgColorVec += colToVec(points[i].color);
        }
        avgColorVec /= (float) points.Count;
        avgColor = vecToCol(avgColorVec);
        return avgColor;
    }

    public Vector3 colToVec(Color col) {
        return new Vector3(col.r, col.g, col.b);
    }

    public Color vecToCol(Vector3 vec) {
        return new Color(vec.x, vec.y, vec.z);
    }

}
