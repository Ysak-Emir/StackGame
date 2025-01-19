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

    [SerializeField] private GameObject spawnPointX;
    

    private GameObject _createdNextCube;
    private GameObject _createdMainCube;


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

        _createdMainCube = CreateCube(mainCubePrefab, new Vector3(0, 0, 0));
        _createdNextCube = CreateCube(nextCubePrefab, new Vector3(spawnPointX.transform.position.x, (float)0.1, 0));
        
        MoveCube moveCubeNext = _createdNextCube.AddComponent<MoveCube>();
        moveCubeNext.StartMovingX();
    }

    private GameObject CreateCube(GameObject cubePrefab, Vector3 position)
    {
        var cube = Instantiate(cubePrefab, position, Quaternion.identity);
        Debug.Log($"Куб {cubePrefab} создан!");
        return cube;
    }

    public GameObject CreatedNextCube
    {
        get => _createdNextCube;
        set => _createdNextCube = value;
    }

    public GameObject CreatedMainCube => _createdMainCube;

    public GameObject OriginalNextCubePrefab => nextCubePrefab;
    public GameObject OriginalMainCubePrefab => mainCubePrefab;
}