using System;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public Transform following;
    public Vector3 orginOffset = Vector3.zero;

    // LateUpdate is called once per frame at the end of everything
    void LateUpdate() {
        transform.position = following.position + orginOffset;
    }
}
