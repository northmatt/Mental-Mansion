using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    public bool CursorLocked = false;

    // Start is called before the first frame update
    void Start() {
        CursorHidden(false);
    }

    // Update is called once per frame
    void Update() {
        if (CursorLocked && Input.GetKeyDown(KeyCode.Escape)) {
            CursorHidden(false);
        }

        if (!CursorLocked && Input.GetMouseButton(0)) {
            CursorHidden(true);
        }
    }

    void CursorHidden(bool yesnt) {
        CursorLocked = yesnt;
        Cursor.lockState = CursorLocked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !CursorLocked;
    }
}
