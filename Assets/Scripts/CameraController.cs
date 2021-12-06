using System;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public CharController following;
    public Vector3 orginOffset = Vector3.zero;
	public float speed = 10f;

	private void Start() {

		transform.position = following.transform.position + orginOffset;
		transform.LookAt(following.transform);
	}

    // LateUpdate is called once per frame at the end of everything
    void LateUpdate() {
        if (following.currentRoom == null)
            return;

        transform.position = Vector3.Lerp(transform.position, following.currentRoom.position + orginOffset, Time.deltaTime * speed);
    }
}
