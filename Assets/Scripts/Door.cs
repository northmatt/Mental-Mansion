using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
	public List<GameObject> doors;

    private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			foreach(GameObject door in doors) {
				door.SetActive(false);
			}
			gameObject.SetActive(false);
		}
	}
}
