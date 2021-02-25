using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPoint {

    public Vector3 position;
    public Color color;
    public Vector2 uv;
    public float brightness;
    public int oscIndex;
    public int vertIndex;

    public LightPoint(Vector3 _position, Color _color, Vector2 _uv, float _brightness, int _oscIndex, int _vertIndex) {
        position = _position;
        color = _color;
        uv = _uv;
        brightness = _brightness;
        oscIndex = _oscIndex;
        vertIndex = _vertIndex;
    }

}
