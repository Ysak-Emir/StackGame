using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Scritps
{
    // public enum MoveDirection
    // {
    //     X,
    //     Z
    // }
    
    

    public class CubeFactory : MonoBehaviour
    {
        private MoveCube _moveCube;
        public GameObject currentCube;
        private GameObject _nextCube;
        private GameObject _fallCube;
        private GameObject _newCube;
        
        private Vector3 _xSpawnPoint;
        private Vector3 _zSpawnPoint;
        // public MoveDirection currentMoving;
        // private Queue<MoveDirection> moveSequence;
        private Random _random;
        public Vector3 randomSpawn;
        Vector3[] spawns;
        private Vector3 z, x;


        public void InitFactory()
        {
            _random = new Random();
            x = GameManager.Instance.spawnPointX.transform.position;
            z = GameManager.Instance.spawnPointZ.transform.position;
            spawns = new Vector3[] { x, z };
            randomSpawn = spawns[_random.Next(spawns.Length)];
        }

        public void Initialize()
        {
            // moveSequence = new Queue<MoveDirection>();
            // moveSequence.Enqueue(MoveDirection.Z);
            // moveSequence.Enqueue(MoveDirection.X);
        }
        
        public GameObject CreateMainCube()
        {
            return Instantiate(GameManager.Instance.OriginalMainCubePrefab);
        }
        
        // public GameObject CreateInitialMovingCube(Vector3 spawnPosition, out MoveCube moveCube, out MoveDirection direction)
        // {
        //     _currentCube = Instantiate(GameManager.Instance.OriginalNextCubePrefab, spawnPosition, Quaternion.identity);
        //     moveCube = _currentCube.AddComponent<MoveCube>();
        //     
        //     if (spawnPosition == _xSpawnPoint)
        //     {
        //         moveCube.StartMovingX();
        //         direction = MoveDirection.X;
        //     }
        //     else
        //     {
        //         moveCube.StartMovingZ();
        //         direction = MoveDirection.Z;
        //     }
        //     
        //     return _currentCube;
        // }
        
        // public MoveDirection GetNextDirection()
        // {
        //     MoveDirection direction = moveSequence.Dequeue();
        //     moveSequence.Enqueue(direction);
        //     return direction;
        // }
        
        public GameObject CreateNextCube()
        {
            Vector3[] spawns = new Vector3[] {
                GameManager.Instance.spawnPointX.transform.position,
                GameManager.Instance.spawnPointZ.transform.position
            };
            Vector3 spawnPos = spawns[new System.Random().Next(spawns.Length)];
    
            GameObject prefab = GameManager.Instance.OriginalNextCubePrefab;
            GameObject cube = Instantiate(prefab, spawnPos, Quaternion.identity);
            // cube.AddComponent<MoveCube>(); // или .Initialize(direction), если есть

            return cube;
        }

        
        public GameObject CreateStaticCube()
        {
            _newCube = Instantiate(currentCube, currentCube.transform.position, Quaternion.identity);
            return _newCube;
        }
        
        public GameObject CreateFallingCube()
        {
            _fallCube = Instantiate(currentCube, currentCube.transform.position, Quaternion.identity);
            return _fallCube;
        }
        
        public GameObject CreateFirstCube(GameObject sourceCube)
        {
            Debug.Log("Создан самый первый куб");
            currentCube = Instantiate(sourceCube, randomSpawn, Quaternion.identity);
            return currentCube;
        }
        
        // public GameObject CreateFallingCube(GameObject sourceCube, Vector3 position)
        // {
        //     _fallCube = Instantiate(sourceCube, position, Quaternion.identity);
        //     _fallCube.AddComponent<Rigidbody>().useGravity = true;
        //     return _fallCube;
        // }

        public GameObject CreateCube(GameObject obj)
        {
            return null;
        }
    }
}