using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeRenderer : MonoBehaviour {
	[HideInInspector]
	public bool isRendering = false;

	MeshRenderer[] renders;

    // Start is called before the first frame update
    void Start() {
		isRendering = false;

		renders = GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer render in renders) {
			render.enabled = isRendering;
		}
    }

	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			isRendering = true;
			foreach (MeshRenderer render in renders) {
				render.enabled = isRendering;
			}
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.CompareTag("Player")) {
			isRendering = false;
			foreach (MeshRenderer render in renders) {
				render.enabled = isRendering;
			}
		}
	}
}
