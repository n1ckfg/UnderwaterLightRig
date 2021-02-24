using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightGroup : MonoBehaviour {

    public List<LightPoint> points;
    public Vector3 avgPosition;
    public Color avgColor;
    public Color currentColor;
    public float avgBrightness;
    public float currentBrightness;
    public float lerpSpeed = 0.01f;
    public float updateColorInterval = 1f;

    private Vector3 avgColorVec;
    private Vector3 currentColorVec;

    private void Start() {      
        points = new List<LightPoint>();
        avgPosition = Vector3.zero;
        avgColor = new Color(0f, 0f, 0f);
        avgColorVec = Vector3.zero;
        currentColorVec = Vector3.zero;
        avgBrightness = 0f;
        currentBrightness = 0f;
    }

    private void Update() {
        currentBrightness = Mathf.Lerp(currentBrightness, avgBrightness, lerpSpeed);
        currentColorVec = Vector3.Lerp(colToVec(currentColor), avgColorVec, lerpSpeed);
        currentColor = vecToCol(currentColorVec);
    }

    public void init(List<LightPoint> _points) {
        points = _points;
        transform.position = getAvgPosition();
        StartCoroutine(getNewColor());
    }

    private IEnumerator getNewColor() {
        while (true) {
            getAvgColor();
            getAvgBrightness();
            yield return new WaitForSeconds(updateColorInterval);
        }
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

    public float getAvgBrightness() {
        for (int i = 0; i < points.Count; i++) {
            avgBrightness += points[i].brightness;
        }
        avgBrightness /= (float) points.Count;
        return avgBrightness;
    }

    public Vector3 colToVec(Color col) {
        return new Vector3(col.r, col.g, col.b);
    }

    public Color vecToCol(Vector3 vec) {
        return new Color(vec.x, vec.y, vec.z);
    }

}
