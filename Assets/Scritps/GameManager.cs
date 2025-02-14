using System;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private float score = 0;


    [SerializeField] private GameObject mainCubePrefab;

    [SerializeField] private GameObject nextCubePrefab;

    [SerializeField] public GameObject spawnPointX;
    [SerializeField] public GameObject spawnPointZ;
    public Vector3 _spawnHeightY = new Vector3(0,0,0.1f);
    

    private GameObject _createdStartNextCube;
    private GameObject _createdStartMainCube;


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

        // _createdStartMainCube = CreateCube(mainCubePrefab, new Vector3(0, 0, 0));
        //
        // _createdStartNextCube = CreateCube(nextCubePrefab, new Vector3(spawnPointX.transform.position.x, 0.1f, 0));
        // _createdStartNextCube = CreateCube(nextCubePrefab, new Vector3(0, 0.1f, spawnPointZ.transform.position.z));
        // spawnPointZ.transform.position += new Vector3(0, 0.1f, 0);
        //
        // MoveCube moveCubeNext = _createdStartNextCube.AddComponent<MoveCube>();
        // moveCubeNext.StartMovingZ();
    }

    public GameObject CreateCube(GameObject cubePrefab, Vector3 position)
    {
        var cube = Instantiate(cubePrefab, position, Quaternion.identity);
        Debug.Log($"Куб {cubePrefab} создан!");
        cube.AddComponent<MoveCube>();
        return cube;
    }

    public GameObject CreatedNextCube
    {
        get => _createdStartNextCube;
        set => _createdStartNextCube = value;
    }

    public GameObject CreatedMainCube
    {
        get => _createdStartMainCube;
        set => _createdStartMainCube = value;
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

    // public float SpawnHeightY
    // {
    //     get => _spawnHeightY;
    //     set => _spawnHeightY = value;
    // }
}