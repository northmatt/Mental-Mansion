using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gaze : MonoBehaviour
{
	public Transform beam;
	public Transform head;
	public Health player;
	public float viewRange = 10;
	public float fov = 30f;
	public float rotRange = 60f;
	public float rotCycleSpeed = 1f;
	public float damage = 1f;
	bool playerInSight = false;
	Quaternion startQuat;
	Quaternion endQuat;
	float counter = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
		GetComponentInChildren<Light>().spotAngle = fov;
        beam.gameObject.SetActive(false);
		startQuat = Quaternion.AngleAxis(rotRange * 0.5f, Vector3.up) * transform.rotation;
		endQuat = Quaternion.AngleAxis(rotRange * -0.5f, Vector3.up) * transform.rotation;
    }

	private void Update() {
		if (playerInSight)	return;
		if (counter < 1f) {
			counter += Time.deltaTime * rotCycleSpeed;
			if (counter > 1f)
				counter = -counter;
		}
		if (counter > 0) {
			transform.rotation = Quaternion.Slerp(startQuat, endQuat, counter);
		}
		else {
			transform.rotation = Quaternion.Slerp(startQuat, endQuat, -counter);
		}
	}

	private void FixedUpdate() {
		LookForPlayer();
		if (playerInSight) {
			player.currentStress += damage * Time.fixedDeltaTime;
		}
	}

	//1.-make sure the distance between the player and enemy is in the sight distance
	//creates five points for linecasts. head, left chest, middle chest, right chest, and feet
	void LookForPlayer()
	{
		playerInSight = false;

		if (Vector3.Distance(player.transform.position, head.position) > viewRange) {
			return;
		}

		Vector3[] offsets = new Vector3[5];

		offsets[0] = Vector3.up;
		offsets[1] = Vector3.zero;
		offsets[2] = Vector3.down;
		offsets[3] = Quaternion.LookRotation(player.transform.position - head.position) * Vector3.right * 0.35f;
		offsets[4] = Quaternion.LookRotation(player.transform.position - head.position) * Vector3.left * 0.35f;

		foreach (Vector3 offsetValue in offsets) {
			checkOffset(offsetValue);
		}
		
		if (playerInSight) {
			if (!beam.gameObject.activeInHierarchy)
				beam.gameObject.SetActive(true);
			DrawBeam(head.position, player.transform.position);
		}
		else if (beam.gameObject.activeInHierarchy)
			beam.gameObject.SetActive(false);
	}

	//check if point is within it's field of view
	//checks if the linecast hit anything
	//check if the object that was hit is the player
	//if so set it's agro time to max
	void checkOffset(Vector3 offset)
	{
		Vector3 lineEnd = player.transform.position + offset;

		float angle = Vector3.SignedAngle(lineEnd - head.position, head.forward, Vector3.up);

		if (Mathf.Abs(angle) > fov / 2)
		{
			return;
		}

		RaycastHit hit;

		if (!Physics.Linecast(head.position, lineEnd, out hit, ~0, QueryTriggerInteraction.Ignore)) {
			return;
		}

		if (!hit.transform.gameObject.CompareTag("Player")) {
			return;
		}

		playerInSight = true;
	}

	void DrawBeam(Vector3 start, Vector3 end) {

		Vector3 averagePos = start + (end - start) * 0.5f;
		beam.position = averagePos;
		beam.LookAt(end);
		beam.localScale = (Vector3.one + Vector3.forward * Vector3.Distance(start, end) + Vector3.back);
	}
}
