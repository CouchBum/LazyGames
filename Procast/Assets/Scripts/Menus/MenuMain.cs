using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuMain : MonoBehaviour {

    public GameManager gm;

    void Awake()
    {
        gm = (GameManager)FindObjectOfType(typeof(GameManager));
    }

    public void PlayGame()
    {
        gm.OnPlayGame();
    }
}
