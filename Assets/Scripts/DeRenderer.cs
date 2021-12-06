using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeRenderer : MonoBehaviour
{
	MeshRenderer[] renders;

    // Start is called before the first frame update
    void Start()
    {
		renders = GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer render in renders) {
			render.enabled = false;
		}
    }

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player")) {
			foreach (MeshRenderer render in renders) {
				render.enabled = true;
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player")) {
			foreach (MeshRenderer render in renders) {
				render.enabled = false;
			}
		}
	}
}
