using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightGroup : MonoBehaviour {

    public int[] indices;
    public float lerpSpeed = 0.01f;
    public float brightnessScale = 10f;
    public Vector3 avgColorVec;
    public float avgBrightness;

    private Color avgColor;
    private float currentBrightness;
    private Color currentColor;
    private Vector3 currentColorVec;
    private Light pointLight;

    private void Start() {
        pointLight = GetComponent<Light>();
        pointLight.range = 100f;

        avgColor = new Color(1f, 1f, 1f, 1f);
        avgColorVec = Vector3.zero;
        currentColorVec = Vector3.zero;
        avgBrightness = 1f;
        currentBrightness = 1f;
    }

    private void Update() {
        currentBrightness = Mathf.Lerp(currentBrightness, avgBrightness, lerpSpeed);
        currentColorVec = Vector3.Lerp(LightUtil.colToVec(currentColor), avgColorVec, lerpSpeed);
        currentColor = LightUtil.vecToCol(currentColorVec);

        pointLight.intensity = Mathf.Clamp(currentBrightness * brightnessScale, 1f, 8f);
        pointLight.color = currentColor;
    }

    public void init(int[] _indices) {
        indices = _indices;
    }



}
