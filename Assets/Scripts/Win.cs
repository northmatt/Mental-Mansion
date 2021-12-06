using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Win : MonoBehaviour
{
	public string sceneName = "Win";
    private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			SceneManager.LoadScene(sceneName);
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
	}
}
