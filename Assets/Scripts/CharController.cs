using System;
using UnityEngine;
using System.Collections.Generic;

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
    [HideInInspector]
    public Transform currentRoom;

    private GameController gameController;
    private Rigidbody RB3D;
    private Animator anim;
    private Collider col;
    private List<Collider> rooms = new List<Collider>();

    private Inputs curInputs;
    private bool grounded = false;

	// Start is called before the first frame update
	void Start() {
		anim = GetComponent<Animator>();
		RB3D = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

		//transform.GetChild(0).transform.Rotate(0f, 0f, rotateArmature);
    }

	// Update is called once per frame
	void Update() {
		if (!gameController.CursorLocked)
			return;

		//Store input from each update to be considered for fixed updates, dont do needless addition of 0 if unneeded
		curInputs.tempAxis.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (Input.GetButtonDown("Jump") && curInputs.jump == 0 && grounded)
			curInputs.jump = 3;
        if (curInputs.tempAxis.x != 0f) { curInputs.axis.x += curInputs.tempAxis.x; }
		if (curInputs.tempAxis.y != 0f) { curInputs.axis.y += curInputs.tempAxis.y; }
        ++curInputs.framesPassed;
		//anim.SetBool("schmooving", movement);
    }

    private void FixedUpdate() {
		//RB3D.AddTorque(Vector3.up * 100, ForceMode.Impulse);
        grounded = isGrounded();

		if (curInputs.axis.x != 0f || curInputs.axis.y != 0f) {
            //limit magnitude to 1
            curInputs.axis = curInputs.axis / curInputs.framesPassed;
            if (curInputs.axis.magnitude > 1f)
                curInputs.axis = curInputs.axis / curInputs.axis.magnitude;

            RB3D.AddForce(Vector3.right * moveForce * curInputs.axis.x * Time.fixedDeltaTime
				+ Vector3.forward * moveForce * curInputs.axis.y * Time.fixedDeltaTime);
            curInputs.axis = Vector2.zero;
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

        float distance = 999f;
        foreach(Collider room in rooms) {
            float newDist = Vector3.Distance(room.transform.position, transform.position);
            if (newDist < distance) {
                distance = newDist;
                currentRoom = room.transform;
            }
        }

		if (RB3D.velocity != Vector3.zero) {
			RB3D.rotation = Quaternion.AngleAxis(
				Vector3.SignedAngle(Vector3.forward, RB3D.velocity, Vector3.up), Vector3.up);
		}
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Room")) {
            rooms.Add(other);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Room")) {
            rooms.Remove(other);
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
