using System;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public CharController following;
    public Vector3 orginOffset = Vector3.zero;

    // LateUpdate is called once per frame at the end of everything
    void LateUpdate() {
        if (following.currentRoom == null)
            return;

        transform.position = following.currentRoom.position + orginOffset;
		transform.LookAt(following.currentRoom);
    }
}
