using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuCSS : MonoBehaviour {

    public GameManager gm;
    int CID;

    void Awake()
    {
        gm = (GameManager)FindObjectOfType(typeof(GameManager));
        CID = 5;
    }

    public void Select(int x) //should be universal code here to grab any caster, not on a per-button case.
    {
        CID = x;
    }

    public void LockIn()
    {
        if (CID < 5)
            gm.SetCID(CID);
        else
            Debug.Log("Choose a Caster");
    }
}
