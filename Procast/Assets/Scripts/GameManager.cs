using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
    
    public enum GameScenes
    {
        MainMenu = 0,
        CSS = 1,
        HexagonMap = 2
    }

    private static GameManager manager = null;
    public static int CID;
    public static Vector3 startPosition;
    public static GameScenes scene;
    public static GameManager Manager
    {
        get { return manager; }
    }

    void Awake()
    {
        GetThisGameManager();
        scene = GameScenes.MainMenu;
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
        scene = GameScenes.CSS;
        LoadScene();
    }

    public void LoadScene()
    {
        switch (scene)
        {
            case GameScenes.MainMenu:
                SceneManager.LoadScene(0);
                break;
            case GameScenes.CSS:
                SceneManager.LoadScene(1);
                break;
            case GameScenes.HexagonMap:
                SceneManager.LoadScene(2);
                break;
        }
    }

    #region Caster Select and Initialization

    public void SetCID(int chosenID)
    {
        CID = chosenID;
        Debug.Log("You Locked In: " + CID); //Sets the Caster ID?
    }

    public void SetStartPosition(Vector3 chosenStartPosition)
    {
        startPosition = chosenStartPosition;
    }

    public void SetTeam()
    {

    }
    #endregion
}
