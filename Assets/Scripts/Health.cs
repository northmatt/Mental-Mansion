using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
	public float maxStress = 100;
	public float currentStress = 0;
	public string deathScreen = "SampleScene";

	public Slider bar;

    // Update is called once per frame
    void Update()
    {
		bar.value = currentStress / maxStress;
        if (currentStress > maxStress) {
			SceneManager.LoadScene(deathScreen);
		}
    }
}
