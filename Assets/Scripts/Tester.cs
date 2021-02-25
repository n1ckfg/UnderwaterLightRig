using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour {

    public LightRig lightRig;
    public float updateInterval = 2f;
    public Color col;

    private void Start() {
        StartCoroutine(updateTexture());
    }

    private IEnumerator updateTexture() {
        while (true) {
            if (lightRig.ready) {
                for (int i = 0; i < lightRig.points.Length; i++) {
                    lightRig.points[i].color = col;
                }
            }
            yield return new WaitForSeconds(updateInterval);
        }
    }

}

/*
1. Raw input signal, positive or negative: -0.25–0.25
2. Cooked input signal, normalized: 0–1
3. Heart rate (bpm): 40 athletic, 60–100 normal 
4. R to r, distance peak to peak--can extrapolate trends: typically 0.6–0.9
5. Respiration, cycle state: 0.1–0.6
6. Respiration rate (rr): 12–18 normal
*/