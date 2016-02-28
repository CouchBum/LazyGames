using UnityEngine;
using System.Collections;

public class PlayerTarget : MonoBehaviour
{
    public Texture2D target;
    public Texture2D targetOver;

    public bool overEnemy;
    private bool _overEnemy;

    public LayerMask enemyLayer;
    public LayerMask otherLayer;

    public float enemyDistance = 50.0f;

    public Camera cam;

    public Transform playerTarget;

    public CasterController casterController;
    public CasterCam casterCam;

    void Start()
    {

    }
}
