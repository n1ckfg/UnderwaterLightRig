using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LightUtil {

    public static Vector3 colToVec(Color col) {
        return new Vector3(col.r, col.g, col.b);
    }

    public static Color vecToCol(Vector3 vec) {
        return new Color(vec.x, vec.y, vec.z);
    }

}
