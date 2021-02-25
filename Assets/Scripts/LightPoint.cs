using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPoint {

    public Vector3 position;
    public Color color;
    public float brightness;

    public LightPoint(Vector3 _position, Color _color, float _brightness) {
        position = _position;
        color = _color;
        brightness = _brightness;
    }

}
