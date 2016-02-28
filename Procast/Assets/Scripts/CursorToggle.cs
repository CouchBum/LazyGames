using UnityEngine;
using System.Collections;

public class CursorToggle : MonoBehaviour {

    bool isLocked;
    void Start () {
	
	}

    void SetCursorLock(bool isLocked)
    {
        this.isLocked = isLocked;
        Cursor.visible = isLocked;
        Cursor.visible = !isLocked;
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if(Input.GetKeyDown(KeyCode.Escape))
        {
            SetCursorLock(!isLocked);
        }
	}
}
