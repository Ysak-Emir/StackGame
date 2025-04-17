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

        [SerializeField] private Camera _camera;

        private Random _random;
        private GameObject currentCube;
        private GameObject mainCube;
        private GameObject nextCube;

        private Vector3 randomSpawn;
        Vector3[] spawns;
        private Vector3 z, x;
        private float tolerance = 0.05f;
        public MoveCube _moveCube;


        public enum MoveDirection
        {
            X,
            Z
        }

        private MoveDirection currentMoving;
        private Queue<MoveDirection> moveSequence;

        private void Start()
        {
            moveSequence = new Queue<MoveDirection>();
            moveSequence.Enqueue(MoveDirection.Z);
            moveSequence.Enqueue(MoveDirection.X);

            AddY();
            _random = new Random();
            x = GameManager.Instance.spawnPointX.transform.position;
            z = GameManager.Instance.spawnPointZ.transform.position;
            spawns = new Vector3[] { x, z };
            randomSpawn = spawns[_random.Next(spawns.Length)];
            nextCube = GameManager.Instance.OriginalNextCubePrefab;
            mainCube = Instantiate(GameManager.Instance.OriginalMainCubePrefab);
            currentCube = Instantiate(GameManager.Instance.OriginalNextCubePrefab, randomSpawn, quaternion.identity);
            _moveCube = currentCube.AddComponent<MoveCube>();
            if (randomSpawn == x)
            {
                _moveCube.StartMovingX();
                currentMoving = MoveDirection.X;
            }
            else if (randomSpawn == z)
            {
                _moveCube.StartMovingZ();
                currentMoving = MoveDirection.Z;
            }
            else
            {
                Debug.Log("AZAZAZA");
            }
        }

        private void AddY()
        {
            GameManager.Instance.spawnPointX.transform.position += new Vector3(0, 0.1f, 0);
            GameManager.Instance.spawnPointZ.transform.position += new Vector3(0, 0.1f, 0);
            GameManager.Instance.upPointY.transform.position += new Vector3(0, 0.1f, 0);
            x = GameManager.Instance.spawnPointX.transform.position;
            z = GameManager.Instance.spawnPointZ.transform.position;
            var vector3 = _camera.transform.position;
            vector3.y = vector3.y + 0.1f;
            _camera.transform.position = vector3;
        }
        

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log($"currentcube - {currentCube.transform.position}");
                Debug.Log($"maincube -{mainCube.transform.position}");

                if (PerfectSection())
                {
                    AddY();
                    CreateNextCube();
                    if (currentMoving == MoveDirection.Z)

                    {
                        var vector3 = currentCube.transform.position;
                        vector3.x = mainCube.transform.position.x;
                        currentCube.transform.position = vector3;
                    }
                    else
                    {
                        var vector3 = currentCube.transform.position;
                        vector3.z = mainCube.transform.position.z;
                        currentCube.transform.position = vector3;
                    }
                }
                else
                {
                    if (Cut())
                    {
                        AddY();
                        CreateNextCube();
                        if (currentMoving == MoveDirection.Z)
                        {
                            var vector3 = currentCube.transform.position;
                            vector3.x = mainCube.transform.position.x;
                            currentCube.transform.position = vector3;
                        }
                        else
                        {
                            var vector3 = currentCube.transform.position;
                            vector3.z = mainCube.transform.position.z;
                            currentCube.transform.position = vector3;
                        }
                    }
                }
            }
        }

        public MoveDirection GetNextDirection()
        {
            MoveDirection direction = moveSequence.Dequeue();
            moveSequence.Enqueue(direction);
            return direction;
        }

        private bool Cut()
        {
            Vector3 pointCurrentCube = currentCube.transform.position;
            Vector3 pointMainCube = mainCube.transform.position;
            Vector3 sizeCurrentCube = currentCube.transform.localScale / 2;
            Vector3 sizeMainCube = mainCube.transform.localScale / 2;

            // Границы currentCube
            Vector3 minCurrent = pointCurrentCube - sizeCurrentCube;
            Vector3 maxCurrent = pointCurrentCube + sizeCurrentCube;

            // Границы mainCube
            Vector3 minMain = pointMainCube - sizeMainCube;
            Vector3 maxMain = pointMainCube + sizeMainCube;


            if (currentMoving == MoveDirection.X)
            {
                if (maxCurrent.x > minMain.x && maxCurrent.x < maxMain.x)
                {
                    Destroy(_moveCube);
                    Debug.Log("Кубы  пересекаются! ПО XXX ++++++");
                    GameObject newCube;

                    newCube = Instantiate(currentCube, currentCube.transform.position,
                        quaternion.identity);

                    float newScale = Mathf.Abs(minMain.x) + maxCurrent.x;
                    var scale = newCube.transform.localScale;
                    scale.x = newScale;
                    newCube.transform.localScale = scale;

                    float newPosition = minMain.x + maxCurrent.x;
                    var position = newCube.transform.position;
                    position.x = newPosition / 2;
                    newCube.transform.position = position;

                    mainCube = newCube;
                    nextCube = newCube;

                    Destroy(currentCube);

                    return true;
                }
                else if (minCurrent.x > minMain.x && maxMain.x > minCurrent.x)
                {
                    Destroy(_moveCube);
                    Debug.Log("Кубы  пересекаются! ПО XXX -----");
                    GameObject newCube;

                    newCube = Instantiate(currentCube, currentCube.transform.position,
                        quaternion.identity);

                    float newScale = maxMain.x + Mathf.Abs(minCurrent.x);
                    var scale = newCube.transform.localScale;
                    scale.x = newScale;
                    newCube.transform.localScale = scale;

                    float newPosition = maxMain.x + minCurrent.x;
                    var position = newCube.transform.position;
                    position.x = newPosition / 2;
                    newCube.transform.position = position;

                    mainCube = newCube;
                    nextCube = newCube;

                    Destroy(currentCube);

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
            else if (currentMoving == MoveDirection.Z)
            {
                if (maxCurrent.z > minMain.z && maxCurrent.z < maxMain.z)
                {
                    Destroy(_moveCube);
                    Debug.Log("Кубы  пересекаются! ПО ZZZ ++++++");
                    GameObject newCube;

                    newCube = Instantiate(currentCube, currentCube.transform.position,
                        quaternion.identity);

                    float newScale = Mathf.Abs(minMain.z) + maxCurrent.z;
                    var scale = newCube.transform.localScale;
                    scale.z = newScale;
                    newCube.transform.localScale = scale;

                    float newPosition = minMain.z + maxCurrent.z;
                    var position = newCube.transform.position;
                    position.z = newPosition / 2;
                    newCube.transform.position = position;

                    mainCube = newCube;
                    nextCube = newCube;

                    Destroy(currentCube);

                    return true;
                }
                else if (minCurrent.z > minMain.z && maxMain.z > minCurrent.z)
                {
                    Destroy(_moveCube);
                    Debug.Log("Кубы пересекаются! ПО ZZZ -----");
                    GameObject newCube = Instantiate(currentCube, currentCube.transform.position, quaternion.identity);

                    float newScale = maxMain.z + Mathf.Abs(minCurrent.z);
                    var scale = newCube.transform.localScale;
                    scale.z = newScale;
                    newCube.transform.localScale = scale;

                    float newPosition = maxMain.z + minCurrent.z;
                    var position = newCube.transform.position;
                    position.z = newPosition / 2;
                    newCube.transform.position = position;

                    mainCube = newCube;
                    nextCube = newCube;

                    Destroy(currentCube);
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

            if (currentMoving == MoveDirection.X)
            {
                if (Mathf.Abs(currentCube.transform.position.x - mainCube.transform.position.x) <= tolerance)
                {
                    Debug.Log("PERFECT!!! XXX");
                    Destroy(_moveCube);

                    var vector3 = currentCube.transform.position;
                    vector3.x = mainCube.transform.position.x;
                    currentCube.transform.position = vector3;

                    mainCube = currentCube;

                    isPerfect = true;
                }
            }
            else
            {
                if (Mathf.Abs(currentCube.transform.position.z - mainCube.transform.position.z) <= tolerance)
                {
                    Debug.Log("PERFECT!!! ZZZ");
                    Destroy(_moveCube);

                    var vector3 = currentCube.transform.position;
                    vector3.z = mainCube.transform.position.z;
                    currentCube.transform.position = vector3;

                    mainCube = currentCube;

                    isPerfect = true;
                }
            }

            return isPerfect;
        }

        private GameObject CreateNextCube()
        {
            currentMoving = GetNextDirection();

            Vector3 spawnPosition;
            if (currentMoving == MoveDirection.X)
            {
                spawnPosition = x;
            }
            else
            {
                spawnPosition = z;
            }

            currentCube = Instantiate(nextCube, spawnPosition, Quaternion.identity);

            _moveCube = currentCube.AddComponent<MoveCube>();

            if (currentMoving == MoveDirection.X)
            {
                _moveCube.StartMovingX();
            }
            else
            {
                _moveCube.StartMovingZ();
            }

            return null;
        }
    }
}