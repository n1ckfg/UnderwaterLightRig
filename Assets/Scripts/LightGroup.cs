using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightGroup : MonoBehaviour {

    public LightPoint[] points;
    public float lerpSpeed = 0.01f;
    public float updateColorInterval = 1f;

    private float pointCount;
    private Vector3 avgPosition;
    private Color avgColor;
    private Color currentColor;
    private float avgBrightness;
    private float currentBrightness;
    private Vector3 avgColorVec;
    private Vector3 currentColorVec;
    private Light pointLight;

    private void Start() {
        pointLight = GetComponent<Light>();
        pointLight.range = 100f;

        avgPosition = Vector3.zero;
        avgColor = new Color(0f, 0f, 0f);
        avgColorVec = Vector3.zero;
        currentColorVec = Vector3.zero;
        avgBrightness = 1f;
        currentBrightness = 0f;
    }

    private void Update() {
        currentBrightness = Mathf.Lerp(currentBrightness, avgBrightness, lerpSpeed);
        currentColorVec = Vector3.Lerp(colToVec(currentColor), avgColorVec, lerpSpeed);
        currentColor = vecToCol(currentColorVec);

        pointLight.intensity = Mathf.Clamp(currentBrightness * 10f, 1f, 8f);
        pointLight.color = currentColor;
    }

    public void init(LightPoint[] _points) {
        points = _points;
        pointCount = points.Length;
        Debug.Log("Initialized group with " + pointCount + " points.");
        transform.localPosition = getAvgPosition();

        StartCoroutine(getAvgValues());
    }

    private IEnumerator getAvgValues() {
        while (true) {
            getAvgColor();
            getAvgBrightness();

            yield return new WaitForSeconds(updateColorInterval);
        }
    }

    public Vector3 getAvgPosition() {
        /*
        for (int i = 0; i < points.Count; i++) {
            avgPosition += points[i].position;
        }
        avgPosition /= pointCount;
        return avgPosition;
        */
        return points[0].position;
    }

    public Color getAvgColor() {
        avgColorVec = Vector3.zero;
        for (int i = 0; i < points.Length; i++) {
            avgColorVec += colToVec(points[i].color);
        }
        avgColorVec /= pointCount;
        avgColor = vecToCol(avgColorVec);
        return avgColor;
    }

    public float getAvgBrightness() {
        avgBrightness = 0f;
        for (int i = 0; i < points.Length; i++) {
            avgBrightness += points[i].brightness;
        }
        avgBrightness = avgBrightness / pointCount;
        return avgBrightness;
    }

    public Vector3 colToVec(Color col) {
        return new Vector3(col.r, col.g, col.b);
    }

    public Color vecToCol(Vector3 vec) {
        return new Color(vec.x, vec.y, vec.z);
    }

}
