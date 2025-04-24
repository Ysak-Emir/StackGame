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


        public void InitFactory()
        {
            _random = new Random();
            randomSpawnX = GameManager.Instance.spawnPointX.transform.position;
            randomSpawnZ = GameManager.Instance.spawnPointZ.transform.position;
            spawns = new Vector3[] { randomSpawnX, randomSpawnZ };
            randomSpawn = spawns[_random.Next(spawns.Length)];

            CreateFirstCube(GameManager.Instance.OriginalNextCubePrefab);
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
                
                currentCube.GetComponent<MoveCube>().StartMovingX();
            else if (GameManager.Instance.currentMoving == MoveDirection.Z)
                
                currentCube.GetComponent<MoveCube>().StartMovingZ();

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

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Destroy(currentCube);
            }
        }


        public GameObject CreateCube(GameObject obj, Vector3 pos)
        {
            var cube = Instantiate(obj, pos, quaternion.identity);
            return cube;
        }
    }
}