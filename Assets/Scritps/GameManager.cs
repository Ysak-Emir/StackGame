using System;
using System.Collections.Generic;
using System.ComponentModel;
using Scritps;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool start;
    [SerializeField] public GameObject triggerDetectCube;
    [SerializeField] public GameObject spawnPointX;
    [SerializeField] public GameObject spawnPointZ;
    [SerializeField] public GameObject upPointY;
    
    [SerializeField] [Range(0, 10)] public float speedX = 1f;
    [SerializeField] [Range(0, 10)] public float speedY = 1f;
    [SerializeField] [Range(0, 10)] public float speedZ = 1f;
    [SerializeField] private GameObject mainCubePrefab;
    [SerializeField] private GameObject nextCubePrefab;

    public MoveDirection currentMoving;
    public Queue<MoveDirection> moveSequence;

    [SerializeField] private CutBlock _cutBlock;
    [SerializeField] private MoveCube moveCube;
    [SerializeField] private CubeFactory _cubeFactory;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] public TextMeshProUGUI scoreText;
    private int score = 0;
    // private void test()
    // {
    //     _cutBlock.moveCube.StopFullMoving();
    // }
    //

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        Application.targetFrameRate = 120;
        QualitySettings.vSyncCount = 0;
    }

    private void Update()
    {
        if (_cubeFactory.currentCube == null) return; // нет куба — ничего не делаем

        Transform cubeTransform = _cubeFactory.currentCube.transform;
        if (cubeTransform == null) return; // доп-проверка на всякий случай

        Vector3 pos = cubeTransform.position;
        if (pos.x > 4f || pos.z > 4f)
        {
            GameOver();
        }
    }

    private void Start()
    {
        if (mainCubePrefab == null && nextCubePrefab == null)
        {
            Debug.LogError("Префаб или префабы не привязаны в инспекторе.");
            return;
        }

        Debug.Log($"{mainCubePrefab.name} префаб найден!");
        if (!triggerDetectCube)
        {
            Debug.LogError("Триггер куба нет, привяжи");
        }
        else
        {
            triggerDetectCube = Instantiate(triggerDetectCube, Vector3.zero, quaternion.identity);
        }
        
        // _cubeFactory.CreateFirstCube(GameManager.Instance.OriginalNextCubePrefab);
        triggerDetectCube.transform.position = new Vector3(0, -15, 0);
        triggerDetectCube.transform.localScale = new Vector3(20, 5, 20);
        if (triggerDetectCube.GetComponent<BoxCollider>())
            triggerDetectCube.GetComponent<BoxCollider>().enabled = true;
        
        triggerDetectCube.GetComponent<BoxCollider>().isTrigger = true;

        if (triggerDetectCube.GetComponent<MeshRenderer>())
            triggerDetectCube.GetComponent<MeshRenderer>().enabled = false;

        triggerDetectCube.AddComponent<TriggerDetect>();
        
        _cubeFactory.InitFactory();
        
        moveSequence = new Queue<MoveDirection>();
        moveSequence.Enqueue(MoveDirection.Z);
        moveSequence.Enqueue(MoveDirection.X);
        

        if (_cubeFactory.randomSpawn == _cubeFactory.randomSpawnX)
        {
            // Debug.Log("START MOVING X");
            // _cubeFactory.currentCube.GetComponent<MoveCube>().StartMovingX();
            currentMoving = MoveDirection.X;
        }
        else if (_cubeFactory.randomSpawn == _cubeFactory.randomSpawnZ)
        {
            // Debug.Log("START MOVING Z");
            // _cubeFactory.currentCube.GetComponent<MoveCube>().StartMovingZ();
            currentMoving = MoveDirection.Z;
            
        }
        else
        {
            Debug.Log("AZAZAZA");
        }
    }
    
    public void RaiseY(float amount = 0.1f)
    {
        Vector3 delta = new Vector3(0, amount, 0);

        // if (triggerDetectCube != null) 
        //     triggerDetectCube.transform.position += delta;
        // if (spawnPointX != null) 
            spawnPointX.transform.position += delta;
        // if (spawnPointZ != null) 
            spawnPointZ.transform.position += delta;
        // if (upPointY != null) 
            upPointY.transform.position += delta;
        _cubeFactory.randomSpawnX += delta;
        _cubeFactory.randomSpawnZ += delta;
        
    }

    public void AddPoints(int point)
    {
        score += point;
        scoreText.text = score.ToString();
    }

    public MoveDirection GetNextDirection()
    {
        MoveDirection direction = moveSequence.Dequeue();
        moveSequence.Enqueue(direction);
        return direction;
    }
    
    public GameObject OriginalNextCubePrefab
    {
        get => nextCubePrefab;
        set => nextCubePrefab = value;
    }

    public GameObject OriginalMainCubePrefab
    {
        get => mainCubePrefab;
        set => mainCubePrefab = value;
    }

    public void GameOver()
    {
        Destroy(_cubeFactory.currentCube.GetComponent<MoveCube>());
        FindAnyObjectByType<ButtonController>().enabled = false;
        _cubeFactory.currentCube.AddComponent<Rigidbody>().useGravity = true;
        _cubeFactory.currentCube = null; // очень важно обнулить!
        gameOverMenu.SetActive(true);

        enabled = false; // ОТКЛЮЧАЕМ Update
    }
  
}