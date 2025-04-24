using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

namespace Scritps
{
    public class CubeFactory : MonoBehaviour
    {
        [SerializeField] public GameObject mainCube;
        [SerializeField] private MoveCube moveCube;
        [SerializeField] private GameObject staticCube;
        [HideInInspector] public GameObject currentCube;
        [HideInInspector] public GameObject nextCube;
        [HideInInspector] public GameObject fallCube;
        [HideInInspector] public GameObject newCube;
        private Vector3 _xSpawnPoint;
        private Vector3 _zSpawnPoint;
        private Random _random;
        public Vector3 randomSpawn;
        Vector3[] spawns;
        public Vector3 randomSpawnZ, randomSpawnX;
        
        [Header("Rainbow Settings")]
        [SerializeField] private bool useRainbowColors = true;
        [SerializeField] [Range(0, 1)] private float hueShiftPerCube = 0.05f;
        [SerializeField] [Range(0, 1)] private float saturation = 1.0f;
        [SerializeField] [Range(0, 1)] private float value = 1.0f;
        private float currentHue = 0f;
        [SerializeField] private bool randomizeStartHue = false;
        [SerializeField] [Range(0, 1)] private float initialHue = 0f;


        public void InitFactory()
        {
            GameManager.Instance.RaiseY();
            _random = new Random();
            randomSpawnX = GameManager.Instance.spawnPointX.transform.position;
            randomSpawnZ = GameManager.Instance.spawnPointZ.transform.position;
            spawns = new Vector3[] { randomSpawnX, randomSpawnZ };
            randomSpawn = spawns[_random.Next(spawns.Length)];

            // GameManager.Instance.RaiseY();
            CreateFirstCube(GameManager.Instance.OriginalNextCubePrefab);

            nextCube = currentCube;
            
            if (randomizeStartHue)
                currentHue = UnityEngine.Random.value; // Случайный стартовый оттенок
            else
                currentHue = initialHue;
        }
        
        public void FinalizeCubesAfterCut(Transform cubesContainer, ref int staticCubeCounter, ref GameObject nextCube)
        {
            fallCube.AddComponent<Rigidbody>().useGravity = true;
            fallCube.name = "FallingCube_" + staticCubeCounter;
            fallCube.transform.SetParent(cubesContainer);

            mainCube = newCube;
            mainCube.name = "StaticCube_" + staticCubeCounter;
            mainCube.transform.SetParent(cubesContainer);

            nextCube = newCube;
            staticCubeCounter++;
            
            Destroy(currentCube);
            // GameManager.Instance.RaiseY();
            CreateNextCube();
        }

        public GameObject CreateNextCube()
        {
            GameManager.Instance.currentMoving = GameManager.Instance.GetNextDirection();

            Vector3 spawnPosition;
            if (GameManager.Instance.currentMoving == MoveDirection.X)
            {
                spawnPosition = randomSpawnX;
            }
            else
            {
                spawnPosition = randomSpawnZ;
            }
            
            currentCube = CreateCube(nextCube, spawnPosition);
            currentCube.name = "current cube";
            
            currentCube.GetComponent<MoveCube>().enabled = true;
            if (GameManager.Instance.currentMoving == MoveDirection.X)
            {
                currentCube.GetComponent<MoveCube>().StartMovingX();
            }
                
            else if (GameManager.Instance.currentMoving == MoveDirection.Z)
            {
                currentCube.GetComponent<MoveCube>().StartMovingZ();
            }
                
                

            return currentCube;
        }
        
        public GameObject CreateStaticCube()
        {
            newCube = CreateCube(currentCube, currentCube.transform.position);
            return newCube;
        }

        public GameObject CreateFallingCube()
        {
            fallCube = CreateCube(currentCube, currentCube.transform.position);
            return fallCube;
        }

        public GameObject CreateFirstCube(GameObject sourceCube)
        {
            
            Debug.Log("Создан самый первый куб");
            currentCube = CreateCube(sourceCube, randomSpawn);
            
            Color newColor = Color.HSVToRGB(currentHue, saturation, value);
            Renderer renderer = mainCube.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = newColor;
            }
            
            currentCube.AddComponent<MoveCube>();
            
            if (randomSpawn == randomSpawnX)
            {
                currentCube.GetComponent<MoveCube>().StartMovingX();
                GameManager.Instance.currentMoving = MoveDirection.X;
            }
            else if (randomSpawn == randomSpawnZ)
            {
                currentCube.GetComponent<MoveCube>().StartMovingZ();
                GameManager.Instance.currentMoving = MoveDirection.Z;
            }
            else
            {
                Debug.Log("AZAZAZA");
            }
            return currentCube;
        }

        // private void Update()
        // {
        //     if (Input.GetKeyDown(KeyCode.Space))
        //     {
        //         Destroy(currentCube);
        //     }
        // }


        public GameObject CreateCube(GameObject obj, Vector3 pos)
        {
            GameObject cube = Instantiate(obj, pos, Quaternion.identity);
            
            if (useRainbowColors)
            {
                ApplyRainbowColor(cube);
            }
            
            return cube;
        }
        
        private void ApplyRainbowColor(GameObject cube)
        {
            // Получаем текущий цвет в HSV и сдвигаем оттенок
            Color newColor = Color.HSVToRGB(currentHue, saturation, value);
            
            // Применяем цвет к материалу куба
            Renderer renderer = cube.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = newColor;
            }
            
            // Сдвигаем оттенок для следующего куба
            currentHue = (currentHue + hueShiftPerCube) % 1.0f;
            
        }
    }
}