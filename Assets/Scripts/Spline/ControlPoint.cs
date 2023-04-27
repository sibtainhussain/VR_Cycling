using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ControlPoint {
    
    public Transform pos;
    public bool curve;
    
    public ControlPoint (Transform pos, bool  curve) {
        this.pos = pos;
        this.curve = curve;
    }

    public ControlPoint (Transform pos) {
        this.pos = pos;
        this.curve = true;
    }
}
