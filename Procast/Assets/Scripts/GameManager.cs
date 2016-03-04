using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour {

    public enum Scenes
    {
        MainMenu = 0,
        CSS = 1,
        HexagonMap = 2
    }

    private static GameManager manager = null;
    public Scenes scene;
    public static GameManager Manager
    {
        get { return manager; }
    }

    void Awake()
    {
        GetThisGameManager();
        scene = Scenes.MainMenu;
    }

    void GetThisGameManager()
    {
        //we don’t want there to be more than one of these at any time, 
        //so we need to make sure that whenever we search for GameManager.Manager, 
        //we will always get the “right” result, because no others will exist.
        if (manager != null && manager != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            manager = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    //Menu Script UI Buttons
    //Main Menu (Scene 0)
    public void OnPlayGame()
    {
        scene = Scenes.CSS;
        LoadScene();
    }

    void LoadScene()
    {
        switch (scene)
        {
            case Scenes.MainMenu:
                SceneManager.LoadScene(0);
                break;
            case Scenes.CSS:
                SceneManager.LoadScene(1);
                break;
            case Scenes.HexagonMap:
                SceneManager.LoadScene(2);
                break;
        }
    }
}
