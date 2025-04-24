using System;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;


namespace Scritps
{
    public class CutBlock : MonoBehaviour
    {
        public static CutBlock Instance { get; private set; }
        [SerializeField] private CubeFactory _cubeFactory;
        private GameManager _gameManager;

        [SerializeField] private CameraFollow cameraFollow;

        [SerializeField] [Tooltip("Погрешность для идеального совпадения кубов")]
        private float tolerance = 0.05f;

        List<GameObject> fallCubes = new List<GameObject>();

        private GameObject cubesContainer;
        private int staticCubeCounter = 0;


        private void Start()
        {
            cameraFollow = FindObjectOfType<CameraFollow>();
            cubesContainer = new GameObject("StaticCubes");
            GameManager.Instance.RaiseY();
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
            Debug.Log(" AlignCurrentCubePosition()");
            var position = _cubeFactory.currentCube.transform.position;
            if (GameManager.Instance.currentMoving == MoveDirection.Z)
            {
                position.x = _cubeFactory.mainCube.transform.position.x;
            }
            else
            {
                position.z = _cubeFactory.mainCube.transform.position.z;
            }
            _cubeFactory.currentCube.transform.position = position;
        }

        private bool Cut()
        {
            Vector3 pointCurrentCube = _cubeFactory.currentCube.transform.position;
            Vector3 pointMainCube = _cubeFactory.mainCube.transform.position;
            Vector3 sizeCurrentCube = _cubeFactory.currentCube.transform.localScale / 2;
            Vector3 sizeMainCube = _cubeFactory.mainCube.transform.localScale / 2;
            // Границы currentCube
            Vector3 minCurrent = pointCurrentCube - sizeCurrentCube;
            Vector3 maxCurrent = pointCurrentCube + sizeCurrentCube;
            // Границы mainCube
            Vector3 minMain = pointMainCube - sizeMainCube;
            Vector3 maxMain = pointMainCube + sizeMainCube;

            if (GameManager.Instance.currentMoving == MoveDirection.X)
            {
                if (maxCurrent.x > minMain.x && maxCurrent.x < maxMain.x)
                {
                    _cubeFactory.currentCube.GetComponent<MoveCube>().enabled = false;
                    Debug.Log("Кубы  пересекаются! ПО XXX ++++++");
                    _cubeFactory.CreateStaticCube();
                    _cubeFactory.CreateFallingCube();

                    _cubeFactory.newCube.transform.localScale = new Vector3(Mathf.Abs(minMain.x) + maxCurrent.x,
                        _cubeFactory.newCube.transform.localScale.y, _cubeFactory.newCube.transform.localScale.z);

                    _cubeFactory.newCube.transform.position = new Vector3((minMain.x + maxCurrent.x) / 2f,
                        _cubeFactory.newCube.transform.position.y, _cubeFactory.newCube.transform.position.z);

                    _cubeFactory.fallCube.transform.localScale = new Vector3((Mathf.Abs(minMain.x) + minCurrent.x),
                        _cubeFactory.fallCube.transform.localScale.y, _cubeFactory.fallCube.transform.localScale.z);

                    _cubeFactory.fallCube.transform.position = new Vector3((minMain.x + minCurrent.x) / 2f,
                        _cubeFactory.fallCube.transform.position.y,
                        _cubeFactory.fallCube.transform.position.z);

                    _cubeFactory.FinalizeCubesAfterCut(cubesContainer.transform, ref staticCubeCounter,
                        ref _cubeFactory.nextCube);

                    return true;
                }
                else if (minCurrent.x > minMain.x && maxMain.x > minCurrent.x)
                {
                    _cubeFactory.currentCube.GetComponent<MoveCube>().enabled = false;
                    Debug.Log("Кубы  пересекаются! ПО XXX -----");
                    _cubeFactory.CreateStaticCube();
                    _cubeFactory.CreateFallingCube();

                    _cubeFactory.newCube.transform.localScale = new Vector3(maxMain.x + Mathf.Abs(minCurrent.x),
                        _cubeFactory.newCube.transform.localScale.y, _cubeFactory.newCube.transform.localScale.z);

                    _cubeFactory.newCube.transform.position = new Vector3((maxMain.x + minCurrent.x) / 2f,
                        _cubeFactory.newCube.transform.position.y, _cubeFactory.newCube.transform.position.z);

                    _cubeFactory.fallCube.transform.localScale = new Vector3(Mathf.Abs(-maxMain.x) + -maxCurrent.x,
                        _cubeFactory.fallCube.transform.localScale.y, _cubeFactory.fallCube.transform.localScale.z);

                    _cubeFactory.fallCube.transform.position = new Vector3((maxCurrent.x + maxMain.x) / 2f,
                        _cubeFactory.fallCube.transform.position.y, _cubeFactory.fallCube.transform.position.z);

                    _cubeFactory.FinalizeCubesAfterCut(cubesContainer.transform, ref staticCubeCounter,
                        ref _cubeFactory.nextCube);

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
            else if (GameManager.Instance.currentMoving == MoveDirection.Z)
            {
                if (maxCurrent.z > minMain.z && maxCurrent.z < maxMain.z)
                {
                    _cubeFactory.currentCube.GetComponent<MoveCube>().enabled = false;
                    Debug.Log("Кубы  пересекаются! ПО ZZZ ++++++");
                    _cubeFactory.CreateStaticCube();
                    _cubeFactory.CreateFallingCube();

                    _cubeFactory.newCube.transform.localScale = new Vector3(_cubeFactory.newCube.transform.localScale.x,
                        _cubeFactory.newCube.transform.localScale.y, Mathf.Abs(minMain.z) + maxCurrent.z);

                    _cubeFactory.newCube.transform.position = new Vector3(_cubeFactory.newCube.transform.position.x,
                        _cubeFactory.newCube.transform.position.y, (minMain.z + maxCurrent.z) / 2);

                    _cubeFactory.fallCube.transform.localScale = new Vector3(
                        _cubeFactory.fallCube.transform.localScale.x, _cubeFactory.fallCube.transform.localScale.y,
                        Mathf.Abs(minMain.z) + minCurrent.z);

                    _cubeFactory.fallCube.transform.position = new Vector3(_cubeFactory.fallCube.transform.position.x,
                        _cubeFactory.fallCube.transform.position.y, (minMain.z + minCurrent.z) / 2);

                    _cubeFactory.FinalizeCubesAfterCut(cubesContainer.transform, ref staticCubeCounter,
                        ref _cubeFactory.nextCube);

                    return true;
                }
                else if (minCurrent.z > minMain.z && maxMain.z > minCurrent.z)
                {
                    _cubeFactory.currentCube.GetComponent<MoveCube>().enabled = false;
                    Debug.Log("Кубы пересекаются! ПО ZZZ -----");
                    _cubeFactory.CreateStaticCube();
                    _cubeFactory.CreateFallingCube();

                    _cubeFactory.newCube.transform.localScale = new Vector3(_cubeFactory.newCube.transform.localScale.x,
                        _cubeFactory.newCube.transform.localScale.y, maxMain.z + Mathf.Abs(minCurrent.z));

                    _cubeFactory.newCube.transform.position = new Vector3(_cubeFactory.newCube.transform.position.x,
                        _cubeFactory.newCube.transform.position.y, (maxMain.z + minCurrent.z) / 2f);

                    _cubeFactory.fallCube.transform.localScale = new Vector3(
                        _cubeFactory.fallCube.transform.localScale.x, _cubeFactory.fallCube.transform.localScale.y,
                        Mathf.Abs(-maxMain.z) + -maxCurrent.z);

                    _cubeFactory.fallCube.transform.position = new Vector3(_cubeFactory.fallCube.transform.position.x,
                        _cubeFactory.fallCube.transform.position.y, (maxCurrent.z + maxMain.z) / 2f);

                    _cubeFactory.FinalizeCubesAfterCut(cubesContainer.transform, ref staticCubeCounter,
                        ref _cubeFactory.nextCube);

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
            
            if (_cubeFactory == null || _cubeFactory.currentCube == null || _cubeFactory.mainCube == null)
            {
                Debug.LogWarning("Some required objects are null in PerfectSection()");
                return false;
            }

            if (GameManager.Instance.currentMoving == MoveDirection.X)
            {
                if (Mathf.Abs(
                        _cubeFactory.currentCube.transform.position.x - _cubeFactory.mainCube.transform.position.x) <=
                    tolerance)
                {
                    Debug.Log("PERFECT!!! XXX");
                    
                    if (_cubeFactory.currentCube.GetComponent<MoveCube>() != null)
                        _cubeFactory.currentCube.GetComponent<MoveCube>().enabled = false;
                    
                    var vector3 = _cubeFactory.currentCube.transform.position;
                    vector3.x = _cubeFactory.mainCube.transform.position.x;
                    _cubeFactory.currentCube.transform.position = vector3;
                    
                    _cubeFactory.mainCube = _cubeFactory.currentCube;
                    _cubeFactory.mainCube.name = "StaticCube_" + staticCubeCounter;
                    staticCubeCounter++;
                    _cubeFactory.mainCube.transform.SetParent(cubesContainer.transform);

                    isPerfect = true;
                }
            }
            else
            {
                if (Mathf.Abs(
                        _cubeFactory.currentCube.transform.position.z - _cubeFactory.mainCube.transform.position.z) <=
                    tolerance)
                {
                    Debug.Log("PERFECT!!! ZZZ");
                    
                    if (_cubeFactory.currentCube.GetComponent<MoveCube>() != null)
                        _cubeFactory.currentCube.GetComponent<MoveCube>().enabled = false;
                    
                    var vector3 = _cubeFactory.currentCube.transform.position;
                    vector3.z = _cubeFactory.mainCube.transform.position.z;
                    _cubeFactory.currentCube.transform.position = vector3;
                    
                    _cubeFactory.mainCube = _cubeFactory.currentCube;
                    _cubeFactory.mainCube.name = "StaticCube_" + staticCubeCounter;
                    staticCubeCounter++;
                    _cubeFactory.mainCube.transform.SetParent(cubesContainer.transform);

                    isPerfect = true;
                }
            }

            return isPerfect;
        }
    }
}