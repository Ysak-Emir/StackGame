using System;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;


namespace Scritps
{
    public class CutBlock : MonoBehaviour
    {
        public static CutBlock Instance { get; private set; }
        [SerializeField] public MoveCube _moveCube;
        [SerializeField] private CubeFactory _cubeFactory;
        private GameManager _gameManager;

        // [SerializeField] private Camera _camera;
        [SerializeField] private CameraFollow cameraFollow;

        private Random _random;
        // private GameObject currentCube;
        [SerializeField] private GameObject mainCube; //главный куб на сцене который
        private GameObject nextCube;

        // private Vector3 randomSpawn;
        // Vector3[] spawns;
        // private Vector3 z, x;

        [SerializeField] [Tooltip("Погрешность для идеального совпадения кубов")]
        private float tolerance = 0.05f;
        
        public GameObject newCube;
        public GameObject fallCube;
        List<GameObject> fallCubes = new List<GameObject>();


        // private CubeFactory _cubeFactory;

        // public enum MoveDirection
        // {
        //     X,
        //     Z
        // }
        //
        // private MoveDirection currentMoving;
        // private Queue<MoveDirection> moveSequence;

        private GameObject cubesContainer;
        private int staticCubeCounter = 0;
        

        private void Start()
        {
            cameraFollow = FindObjectOfType<CameraFollow>();
            _cubeFactory.Initialize();

            cubesContainer = new GameObject("StaticCubes");

            GameManager.Instance.RaiseY();

            _random = new Random();
            // spawns = new Vector3[] { x, z };
            // randomSpawn = spawns[_random.Next(spawns.Length)];
            // nextCube = GameManager.Instance.OriginalNextCubePrefab;
            // currentCube = Instantiate(GameManager.Instance.OriginalNextCubePrefab, randomSpawn, quaternion.identity);
            // _cubeFactory.CreateFirstCube(GameManager.Instance.OriginalNextCubePrefab);

            // _moveCube = currentCube.AddComponent<MoveCube>();
            // if (randomSpawn == x)
            // {
            //     _moveCube.StartMovingX();
            //     _cubeFactory.currentMoving = MoveDirection.X;
            // }
            // else if (randomSpawn == z)
            // {
            //     _moveCube.StartMovingZ();
            //     _cubeFactory.currentMoving = MoveDirection.Z;
            // }
            // else
            // {
            //     Debug.Log("AZAZAZA");
            // }

        }

        
        
        public void GetCut()
        {
            if (PerfectSection())
            {
                cameraFollow.StartCamera();
                GameManager.Instance.RaiseY();
                _cubeFactory.CreateNextCube();
                AlignCurrentCubePosition();
            }
            else
            {
                if (Cut())
                {
                    cameraFollow.StartCamera();
                    GameManager.Instance.RaiseY();
                    _cubeFactory.CreateNextCube();
                    AlignCurrentCubePosition();
                }
            }
        }
        
        private void AlignCurrentCubePosition()
        {
            var position = _cubeFactory.currentCube.transform.position;

            if (_moveCube.currentMoving == MoveDirection.Z)
            {
                position.x = mainCube.transform.position.x;
            }
            else
            {
                position.z = mainCube.transform.position.z;
            }

            _cubeFactory.currentCube.transform.position = position;
        }


        // public MoveDirection GetNextDirection()
        // {
        //     MoveDirection direction = moveSequence.Dequeue();
        //     moveSequence.Enqueue(direction);
        //     return direction;
        // }

        private bool Cut()
        {
            Vector3 pointCurrentCube = _cubeFactory.currentCube.transform.position;
            Vector3 pointMainCube = mainCube.transform.position;
            Vector3 sizeCurrentCube = _cubeFactory.currentCube.transform.localScale / 2;
            Vector3 sizeMainCube = mainCube.transform.localScale / 2;

            // Границы currentCube
            Vector3 minCurrent = pointCurrentCube - sizeCurrentCube;
            Vector3 maxCurrent = pointCurrentCube + sizeCurrentCube;

            // Границы mainCube
            Vector3 minMain = pointMainCube - sizeMainCube;
            Vector3 maxMain = pointMainCube + sizeMainCube;

            if (_moveCube.currentMoving == MoveDirection.X)
            {
                
                if (maxCurrent.x > minMain.x && maxCurrent.x < maxMain.x)
                {
                    Destroy(_moveCube);
                    Debug.Log("Кубы  пересекаются! ПО XXX ++++++");
                    _cubeFactory.CreateStaticCube();
                    _cubeFactory.CreateFallingCube();

                    float newScale = Mathf.Abs(minMain.x) + maxCurrent.x;
                    var scale = newCube.transform.localScale;
                    scale.x = newScale;
                    newCube.transform.localScale = scale;

                    float newPosition = minMain.x + maxCurrent.x;
                    var position = newCube.transform.position;
                    position.x = newPosition / 2;
                    newCube.transform.position = position;

                    float newScaleFall = Mathf.Abs(minMain.x) + minCurrent.x;
                    var scaleFall = fallCube.transform.localScale;
                    scaleFall.x = newScaleFall;
                    fallCube.transform.localScale = scaleFall;

                    float newPositionFall = minMain.x + minCurrent.x;
                    var positionFall = fallCube.transform.position;
                    positionFall.x = newPositionFall / 2;
                    fallCube.transform.position = positionFall;

                    fallCube.AddComponent<Rigidbody>().useGravity = true;
                    fallCubes.Add(fallCube);
                    fallCube.name = "FallingCube_" + staticCubeCounter;
                    fallCube.transform.SetParent(cubesContainer.transform);

                    mainCube = newCube;
                    mainCube.name = "StaticCube_" + staticCubeCounter;
                    staticCubeCounter++;
                    mainCube.transform.SetParent(cubesContainer.transform);
                    nextCube = newCube;

                    Destroy(_cubeFactory.currentCube);

                    return true;
                }
                else if (minCurrent.x > minMain.x && maxMain.x > minCurrent.x)
                {
                    Destroy(_moveCube);
                    Debug.Log("Кубы  пересекаются! ПО XXX -----");
                    _cubeFactory.CreateStaticCube();
                    _cubeFactory.CreateFallingCube();

                    float newScale = maxMain.x + Mathf.Abs(minCurrent.x);
                    var scale = newCube.transform.localScale;
                    scale.x = newScale;
                    newCube.transform.localScale = scale;

                    float newPosition = maxMain.x + minCurrent.x;
                    var position = newCube.transform.position;
                    position.x = newPosition / 2;
                    newCube.transform.position = position;

                    float newScaleFall = Mathf.Abs(-maxMain.x) + -maxCurrent.x;
                    var scaleFall = fallCube.transform.localScale;
                    scaleFall.x = newScaleFall;
                    fallCube.transform.localScale = scaleFall;

                    float newPositionFall = maxCurrent.x + maxMain.x;
                    var positionFall = fallCube.transform.position;
                    positionFall.x = newPositionFall / 2;
                    fallCube.transform.position = positionFall;

                    fallCube.AddComponent<Rigidbody>().useGravity = true;
                    fallCubes.Add(fallCube);

                    mainCube = newCube;
                    mainCube.name = "StaticCube_" + staticCubeCounter;
                    staticCubeCounter++;
                    mainCube.transform.SetParent(cubesContainer.transform);
                    nextCube = newCube;
                    fallCube.name = "FallingCube_" + staticCubeCounter;
                    fallCube.transform.SetParent(cubesContainer.transform);

                    Destroy(_cubeFactory.currentCube);

                    return true;
                }
                else
                {
                    //GAME OVER
                    Debug.Log("Кубы НЕ пересекаются!");
                    GameManager.Instance.GameOver();
                    return false;
                }
            }
            else if (_moveCube.currentMoving == MoveDirection.Z)
            {
                if (maxCurrent.z > minMain.z && maxCurrent.z < maxMain.z)
                {
                    Destroy(_moveCube);
                    Debug.Log("Кубы  пересекаются! ПО ZZZ ++++++");
                    // newCube = Instantiate(currentCube, currentCube.transform.position,
                    //     quaternion.identity);
                    // fallCube = Instantiate(currentCube, currentCube.transform.position,
                    //     quaternion.identity);

                    float newScale = Mathf.Abs(minMain.z) + maxCurrent.z;
                    var scale = newCube.transform.localScale;
                    scale.z = newScale;
                    newCube.transform.localScale = scale;

                    float newPosition = minMain.z + maxCurrent.z;
                    var position = newCube.transform.position;
                    position.z = newPosition / 2;
                    newCube.transform.position = position;

                    float newScaleFall = Mathf.Abs(minMain.z) + minCurrent.z;
                    var scaleFall = fallCube.transform.localScale;
                    scaleFall.z = newScaleFall;
                    fallCube.transform.localScale = scaleFall;

                    float newPositionFall = minMain.z + minCurrent.z;
                    var positionFall = fallCube.transform.position;
                    positionFall.z = newPositionFall / 2;
                    fallCube.transform.position = positionFall;

                    fallCube.AddComponent<Rigidbody>().useGravity = true;
                    fallCubes.Add(fallCube);
                    fallCube.name = "FallingCube_" + staticCubeCounter;
                    fallCube.transform.SetParent(cubesContainer.transform);

                    mainCube = newCube;
                    mainCube.name = "StaticCube_" + staticCubeCounter;
                    staticCubeCounter++;
                    mainCube.transform.SetParent(cubesContainer.transform);
                    nextCube = newCube;

                    // Destroy(currentCube);

                    return true;
                }
                else if (minCurrent.z > minMain.z && maxMain.z > minCurrent.z)
                {
                    Destroy(_moveCube);
                    Debug.Log("Кубы пересекаются! ПО ZZZ -----");
                    // newCube = Instantiate(currentCube, currentCube.transform.position,
                    //     quaternion.identity);
                    // fallCube = Instantiate(currentCube, currentCube.transform.position,
                    //     quaternion.identity);

                    float newScale = maxMain.z + Mathf.Abs(minCurrent.z);
                    var scale = newCube.transform.localScale;
                    scale.z = newScale;
                    newCube.transform.localScale = scale;

                    float newPosition = maxMain.z + minCurrent.z;
                    var position = newCube.transform.position;
                    position.z = newPosition / 2;
                    newCube.transform.position = position;

                    float newScaleFall = Mathf.Abs(-maxMain.z) + -maxCurrent.z;
                    var scaleFall = fallCube.transform.localScale;
                    scaleFall.z = newScaleFall;
                    fallCube.transform.localScale = scaleFall;

                    float newPositionFall = maxCurrent.z + maxMain.z;
                    var positionFall = fallCube.transform.position;
                    positionFall.z = newPositionFall / 2;
                    fallCube.transform.position = positionFall;

                    fallCube.AddComponent<Rigidbody>().useGravity = true;
                    fallCubes.Add(fallCube);
                    fallCube.name = "FallingCube_" + staticCubeCounter;
                    fallCube.transform.SetParent(cubesContainer.transform);

                    mainCube = newCube;
                    mainCube.name = "StaticCube_" + staticCubeCounter;
                    staticCubeCounter++;
                    mainCube.transform.SetParent(cubesContainer.transform);
                    nextCube = newCube;

                    // Destroy(currentCube);
                    return true;
                }
                else
                {
                    //GAME OVER
                    GameManager.Instance.GameOver();
                    Debug.Log("Кубы НЕ пересекаются!");
                    return false;
                }
            }

            return false;
        }


        private bool PerfectSection()
        {
            bool isPerfect = false;

            if (_moveCube.currentMoving == MoveDirection.X)
            {
                if (Mathf.Abs(_cubeFactory.currentCube.transform.position.x - mainCube.transform.position.x) <= tolerance)
                {
                    Debug.Log("PERFECT!!! XXX");
                    Destroy(_moveCube);

                    var vector3 = _cubeFactory.currentCube.transform.position;
                    vector3.x = mainCube.transform.position.x;
                    _cubeFactory.currentCube.transform.position = vector3;

                    mainCube = _cubeFactory.currentCube;
                    mainCube.name = "StaticCube_" + staticCubeCounter;
                    staticCubeCounter++;
                    mainCube.transform.SetParent(cubesContainer.transform);

                    isPerfect = true;
                }
            }
            else
            {
                if (Mathf.Abs(_cubeFactory.currentCube.transform.position.z - mainCube.transform.position.z) <= tolerance)
                {
                    Debug.Log("PERFECT!!! ZZZ");
                    Destroy(_moveCube);
                
                    var vector3 = _cubeFactory.currentCube.transform.position;
                    vector3.z = mainCube.transform.position.z;
                    _cubeFactory.currentCube.transform.position = vector3;
                
                    mainCube = _cubeFactory.currentCube;
                    mainCube.name = "StaticCube_" + staticCubeCounter;
                    staticCubeCounter++;
                    mainCube.transform.SetParent(cubesContainer.transform);
                
                    isPerfect = true;
                }
            }

            return isPerfect;
        }

        

        // private void GameOver()
        // {
        //     _moveCube.StopFullMoving();
        //     currentCube.AddComponent<Rigidbody>();
        // }
    }
}