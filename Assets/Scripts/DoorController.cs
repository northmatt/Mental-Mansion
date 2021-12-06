using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour {

    public float moveForce = 0f;
    public DeRenderer Room1;
    public DeRenderer Room2;

    private Rigidbody rb;
    private ConstantForce cf;
    private MeshRenderer render;
    private float startOffset = 0f;
    private const float maxRot = 1f / 90f;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        cf = GetComponent<ConstantForce>();
        render = GetComponentInChildren<MeshRenderer>();
        startOffset = transform.localEulerAngles.y;
        rb.centerOfMass = Vector3.zero;
    }

    private void FixedUpdate() {
        float angle = Mathf.DeltaAngle(transform.localEulerAngles.y, startOffset);
        if (Mathf.Abs(angle) > 1f)
            cf.torque = Vector3.up * moveForce * angle * maxRot;
		else
            cf.torque = Vector3.zero;

        render.enabled = Room1.isRendering || Room2.isRendering;
    }
}
