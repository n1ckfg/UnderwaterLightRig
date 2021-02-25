using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://docs.unity3d.com/ScriptReference/Texture2D.SetPixel.html
// https://forum.unity.com/threads/setpixel-crash.8830/

public class Tester : MonoBehaviour {

    public LightRig lightRig;
    public Material mtl;
    public float updateInterval = 2f;

    private Texture2D tex;

    private void Start() {
        tex = (Texture2D) mtl.mainTexture;

        StartCoroutine(updateTexture());
    }

    private IEnumerator updateTexture() {
        while (true) {
            if (lightRig.ready) {
                for (int i = 0; i < lightRig.groups.Count; i++) {
                    for (int j = 0; j < lightRig.groups[i].points.Length; j++) {
                        Vector2 uv = lightRig.groups[i].points[j].uv;
                        int y = (int) (uv.x * tex.width);
                        int x = (int) (uv.y * tex.height);
                        Color color = new Color(1f, 0f, 0f, 1f);
                        tex.SetPixel(x, y, color);
                    }
                }
                tex.Apply();
            }
            yield return new WaitForSeconds(updateInterval);
        }
    }

}
