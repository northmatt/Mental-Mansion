﻿using System;
using UnityEngine;

struct Inputs {
    public Vector2 axis;
    public Vector2 tempAxis;
    public uint framesPassed;
    public byte jump;
}

public class CharController : MonoBehaviour {
    public float moveForce = 1400f;
    public float jumpForce = 30f;
    public float gravMult = 9.81f;
    public float rotateArmature = 0f;
    public float jumpCheckYOffset = 0.52f;
    public float jumpCheckRadOffset = 0.975f;

    private GameController gameController;
    private Rigidbody RB3D;
    private Animator anim;
    private Collider col;

    private Inputs curInputs;
    static float sqrt2 = 1f / Mathf.Sqrt(2);         //sqrt is a fairly intensive operation, storing it in memory to avoid using opertaion every fixed update
    private bool grounded = false;
    private bool dash = false;

	// Start is called before the first frame update
	void Start() {
		anim = GetComponent<Animator>();
		RB3D = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        sqrt2 = 1f / Mathf.Sqrt(2);         //sqrt is a fairly intensive operation, storing it in memory to avoid using opertaion every fixed update

        //transform.GetChild(0).transform.Rotate(0f, 0f, rotateArmature);
    }

	// Update is called once per frame
	void Update() {
		if (!gameController.CursorLocked)
			return;

		bool movement = false;

		//Store input from each update to be considered for fixed updates, dont do needless addition of 0 if unneeded
		curInputs.tempAxis.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (Input.GetButtonDown("Jump") && curInputs.jump == 0 && grounded)
			curInputs.jump = 3;
        if (curInputs.tempAxis.x != 0f) { curInputs.axis.x += curInputs.tempAxis.x;	movement = true; }
		if (curInputs.tempAxis.y != 0f) { curInputs.axis.y += curInputs.tempAxis.y;	movement = true; }
        ++curInputs.framesPassed;
		//anim.SetBool("schmooving", movement);
    }

    private void FixedUpdate() {
        grounded = isGrounded();

        //if both inputs were pressed then normalize inputs
        if (curInputs.axis.x != 0f && curInputs.axis.y != 0f)
            curInputs.axis.Set(curInputs.axis.x * sqrt2, curInputs.axis.y * sqrt2);

		if (curInputs.axis.x != 0f) {
            RB3D.AddForce(transform.right * moveForce * curInputs.axis.x / curInputs.framesPassed * Time.fixedDeltaTime);
            curInputs.axis.x = 0f;
		}

        if (curInputs.axis.y != 0f) {
            RB3D.AddForce(transform.forward * moveForce * curInputs.axis.y / curInputs.framesPassed * Time.fixedDeltaTime);
            curInputs.axis.y = 0f;
		}

        //jump on maxed cooldown
        if (curInputs.jump == 3) {
            RB3D.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            --curInputs.jump;
        }
        
        //reduce jump cooldown if grounded
        if (curInputs.jump > 0 && grounded)
            --curInputs.jump;

        //Add downwards force if ungrounded (RB3D has drag)
        if (!grounded)
            RB3D.AddForce(Physics.gravity * gravMult, ForceMode.Acceleration);

        curInputs.framesPassed = 0;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Room")) {
            MeshRenderer[] renders = other.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer render in renders) {
                render.enabled = true;
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Room")) {
            MeshRenderer[] renders = other.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer render in renders) {
                render.enabled = false;
            }
        }
    }

    bool isGrounded() {
        int layer = ~(1 << LayerMask.NameToLayer("Player"));
        /*Debug.DrawRay(transform.position + (Vector3.down * jumpCheckYOffset), Vector3.down * col.bounds.extents.x * jumpCheckRadOffset, Color.red);
        Debug.DrawRay(transform.position + (Vector3.down * jumpCheckYOffset), Vector3.up * col.bounds.extents.x * jumpCheckRadOffset, Color.red);
        Debug.DrawRay(transform.position + (Vector3.down * jumpCheckYOffset), Vector3.forward * col.bounds.extents.x * jumpCheckRadOffset, Color.red);
        Debug.DrawRay(transform.position + (Vector3.down * jumpCheckYOffset), Vector3.back * col.bounds.extents.x * jumpCheckRadOffset, Color.red);
        Debug.DrawRay(transform.position + (Vector3.down * jumpCheckYOffset), Vector3.left * col.bounds.extents.x * jumpCheckRadOffset, Color.red);
        Debug.DrawRay(transform.position + (Vector3.down * jumpCheckYOffset), Vector3.right * col.bounds.extents.x * jumpCheckRadOffset, Color.red);*/
        return Physics.CheckSphere(transform.position + (Vector3.down * jumpCheckYOffset), col.bounds.extents.x * jumpCheckRadOffset, layer, QueryTriggerInteraction.Ignore);
    }
}
