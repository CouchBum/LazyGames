using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuCSS : MonoBehaviour {

    public void ForceCaster()
    {
        SceneManager.LoadScene(2); //Load Map
    }
}
