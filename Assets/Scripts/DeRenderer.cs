using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeRenderer : MonoBehaviour
{
	MeshRenderer[] renders;
	Light[] lights;

    // Start is called before the first frame update
    void Start()
    {
		renders = GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer render in renders) {
			render.enabled = false;
		}
		lights = GetComponentsInChildren<Light>();
		foreach (Light light in lights) {
			light.enabled = false;
		}
    }

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player")) {
			foreach (MeshRenderer render in renders) {
				render.enabled = true;
			}
			foreach (Light light in lights) {
				light.enabled = true;
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player")) {
			foreach (MeshRenderer render in renders) {
				render.enabled = false;
			}
			foreach (Light light in lights) {
				light.enabled = false;
			}
		}
	}
}
