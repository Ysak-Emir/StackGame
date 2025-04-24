using System;
using System.ComponentModel;
using Scritps;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool start;
    [SerializeField] public GameObject triggerDetectCube;
    [SerializeField] public GameObject spawnPointX;
    [SerializeField] public GameObject spawnPointZ;
    [SerializeField] public GameObject upPointY;
    
    [SerializeField] [Range(0, 3)] public float speedX = 1f;
    [SerializeField] [Range(0, 3)] public float speedY = 1f;
    [SerializeField] [Range(0, 3)] public float speedZ = 1f;
    private float score = 0;
    [SerializeField] private GameObject mainCubePrefab;
    [SerializeField] private GameObject nextCubePrefab;


    private GameObject _createdStartNextCube;
    private GameObject _createdStartMainCube;


    [SerializeField] private CutBlock _cutBlock;
    [SerializeField] private MoveCube _moveCube;
    [SerializeField] private CubeFactory _cubeFactory;

    // private void test()
    // {
    //     _cutBlock._moveCube.StopFullMoving();
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

        _cubeFactory.CreateFirstCube(OriginalNextCubePrefab);
        
        _moveCube.InitMoveCube();

    }
    
    public void RaiseY(float amount = 0.1f)
    {
        Vector3 delta = new Vector3(0, amount, 0);

        if (triggerDetectCube != null) 
            triggerDetectCube.transform.position += delta;
        if (spawnPointX != null) 
            spawnPointX.transform.position += delta;
        if (spawnPointZ != null) 
            spawnPointZ.transform.position += delta;
        if (upPointY != null) 
            upPointY.transform.position += delta;
    }


    public GameObject CreateCube(GameObject cubePrefab, Vector3 position)
    {
        var cube = Instantiate(cubePrefab, position, Quaternion.identity);
        Debug.Log($"Куб {cubePrefab} создан!");
        cube.AddComponent<MoveCube>();
        return cube;
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
        _moveCube.StopFullMoving();
        
    }
  
}