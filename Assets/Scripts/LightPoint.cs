using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPoint {

    public Vector3 position;
    public Color color;
    public Vector2 uv;
    public float brightness;
    public int vertIndex;
    public int groupIndex;

    public LightPoint(Vector3 _position, Color _color, Vector2 _uv, float _brightness, int _vertIndex, int _groupIndex) {
        position = _position;
        color = _color;
        uv = _uv;
        brightness = _brightness;
        vertIndex = _vertIndex;
        groupIndex = _groupIndex;
    }

}
