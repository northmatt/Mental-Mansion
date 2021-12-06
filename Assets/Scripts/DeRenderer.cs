using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeRenderer : MonoBehaviour {
	[HideInInspector]
	public bool isRendering = false;

	MeshRenderer[] renders;
	Light[] lights;

    // Start is called before the first frame update
    void Start() {
		isRendering = false;

		renders = GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer render in renders) {
			render.enabled = isRendering;
		}
		lights = GetComponentsInChildren<Light>();
		foreach (Light light in lights) {
			light.enabled = false;
		}
    }

	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			isRendering = true;
			foreach (MeshRenderer render in renders) {
				render.enabled = isRendering;
			}
			foreach (Light light in lights) {
				light.enabled = true;
			}
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.CompareTag("Player")) {
			isRendering = false;
			foreach (MeshRenderer render in renders) {
				render.enabled = isRendering;
			}
			foreach (Light light in lights) {
				light.enabled = false;
			}
		}
	}
}
