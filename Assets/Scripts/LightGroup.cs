using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightGroup : MonoBehaviour {

    public int[] indices;
    public float lerpSpeed = 0.01f;
    public float brightnessScale = 10f;
    public float avgBrightness;
    public Color avgColor;

    private float currentBrightness;
    private Color currentColor;
    private Light pointLight;

    private void Start() {
        pointLight = GetComponent<Light>();
        pointLight.range = 100f;

        avgColor = new Color(1f, 1f, 1f, 1f);
        avgBrightness = 1f;
        currentBrightness = 1f;
    }

    private void Update() {
        currentBrightness = Mathf.Lerp(currentBrightness, avgBrightness, lerpSpeed);
        currentColor = Color.Lerp(currentColor, avgColor, lerpSpeed);
        pointLight.intensity = Mathf.Clamp(currentBrightness * brightnessScale, 1f, 8f);
        pointLight.color = currentColor;
    }

    public void init(int[] _indices) {
        indices = _indices;
    }



}
