using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

public class CutCube : MonoBehaviour
{
    
    
    private struct CubePoints
    {
        public Vector3 Minus;
        public Vector3 Plus;
    }

    private CubePoints pointMainCubeX, pointMainCubeZ, pointNextCubeX, pointNextCubeZ;

    private GameObject cubeNextCheck, cubeMainCheck;

    private string _queuing = null;
    // int q = 0;

    private MoveCube _checkBoolMovingCube;
    
    //----------------------

    private void Start()
    {
        AddHeightToY();
        GameManager.Instance.CreatedMainCube = GameManager.Instance.CreateCube(GameManager.Instance.OriginalMainCubePrefab, Vector3.zero);
        Random random = new Random();
        Vector3[] spawns = {GameManager.Instance.spawnPointX.transform.position, GameManager.Instance.spawnPointZ.transform.position};
        Vector3 randomSpawn = spawns[random.Next(spawns.Length)];
        GameManager.Instance.CreatedNextCube = GameManager.Instance.CreateCube(GameManager.Instance.OriginalNextCubePrefab, randomSpawn);
        
        _checkBoolMovingCube = GameManager.Instance.CreatedNextCube.AddComponent<MoveCube>();
        if (randomSpawn == GameManager.Instance.spawnPointZ.transform.position)
        {
            _checkBoolMovingCube.StartMovingZ();
            _queuing = "z";
        }
        else
        {
            _checkBoolMovingCube.StartMovingX();
            _queuing = "x";
        }
  
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // DirectionCheck();
            CheckPosition();
        }
    }

    private void LateUpdate()
    {
        // DirectionCheck();
    }

    private void CheckPosition()
    {
        cubeNextCheck = GameManager.Instance?.CreatedNextCube;
        cubeMainCheck = GameManager.Instance?.CreatedMainCube;

        if (cubeMainCheck == null || cubeNextCheck == null)
        {
            Debug.LogError("Один из кубов (cubeMainCheck или cubeNextCheck) не инициализирован!");
            return;
        }

        pointMainCubeX.Plus = cubeMainCheck.transform.position;
        pointMainCubeX.Plus.x += (cubeNextCheck.transform.localScale.x / 2f);

        pointMainCubeX.Minus = cubeMainCheck.transform.position;
        pointMainCubeX.Minus.x -= (cubeNextCheck.transform.localScale.x / 2f);

        pointNextCubeX.Plus = cubeNextCheck.transform.position;
        pointNextCubeX.Plus.x += (cubeNextCheck.transform.localScale.x / 2f);

        pointNextCubeX.Minus = cubeNextCheck.transform.position;
        pointNextCubeX.Minus.x -= (cubeNextCheck.transform.localScale.x / 2f);

        // ---------------------

        pointMainCubeZ.Plus = cubeMainCheck.transform.position;
        pointMainCubeZ.Plus.z += (cubeNextCheck.transform.localScale.z / 2f);

        pointMainCubeZ.Minus = cubeMainCheck.transform.position;
        pointMainCubeZ.Minus.z -= (cubeNextCheck.transform.localScale.z / 2f);

        pointNextCubeZ.Plus = cubeNextCheck.transform.position;
        pointNextCubeZ.Plus.z += (cubeNextCheck.transform.localScale.z / 2f);

        pointNextCubeZ.Minus = cubeNextCheck.transform.position;
        pointNextCubeZ.Minus.z -= (cubeNextCheck.transform.localScale.z / 2f);
        
        
        if (_checkBoolMovingCube.IsMovingZ)
        {
            if (IsInsideCube(pointNextCubeZ.Plus.z, pointMainCubeZ.Minus.z, pointMainCubeZ.Plus.z) ||
                IsInsideCube(pointNextCubeZ.Minus.z, pointMainCubeZ.Minus.z, pointMainCubeZ.Plus.z))
            {
                // if (_queuing == "z")
                // {
                    Cut("z");
                    // _queuing = "x";
                // }
                    

            }
        }
        else if(_checkBoolMovingCube.IsMovingX)
        {
            if (IsInsideCube(pointNextCubeX.Plus.x, pointMainCubeX.Minus.x, pointMainCubeX.Plus.x) ||
                IsInsideCube(pointNextCubeX.Minus.x, pointMainCubeX.Minus.x, pointMainCubeX.Plus.x))
            {
                // if (_queuing == "x")
                // {
                    Cut("x");
                    // _queuing = "z";
                // }
            }
        }
        else
        {
            Debug.Log("Что то пошло не так");
        }
        
        
        
        Debug.Log($"pointMainCubePlusX - {pointMainCubeX.Plus}");
        Debug.Log($"pointMainCubeMinusX - {pointMainCubeX.Minus}");
        Debug.Log("----------------------------");
        Debug.Log($"pointNextCubePlusX - {pointNextCubeX.Plus}");
        Debug.Log($"pointNextCubeMinusX - {pointNextCubeX.Minus}");
    }

    private void DirectionCheck()
    {
        if (_checkBoolMovingCube.IsMovingZ)
        {
            
        }
        else
        {
            Debug.Log("Is MovingZX FALSE");
        }
    }

    private bool IsInsideCube(float positionTarget, float pointMinus, float pointPlus)
    {
        return (positionTarget) >= pointMinus && (positionTarget) <= pointPlus;
    }


    private static void AddHeightToY()
    {
        GameManager.Instance.spawnPointX.transform.position += new Vector3(0, 0.1f, 0);
        GameManager.Instance.spawnPointZ.transform.position += new Vector3(0, 0.1f, 0);
    }
    

    private void Cut(string axis)
    {
        CreateNextCube(axis);
        CreateFallCube(axis);
    }


    private void CreateNextCube(string AxisForPoints = null)
    {
        var cubeNext = GameManager.Instance.CreatedNextCube;
        if (AxisForPoints == "x")
        {
            if (IsInsideCube(pointNextCubeX.Plus.x, pointMainCubeX.Minus.x, pointMainCubeX.Plus.x))
            {
                cubeNext = CreateCube("x");
                SetNewScaleCube(cubeNext, "x");
                AddHeightToY();
                cubeNext = SetChangesCreateNextCube(cubeNext, "x");
                MoveCube moveCubeNext = cubeNext.AddComponent<MoveCube>();
                moveCubeNext.StartMovingX();
            }
            else if (IsInsideCube(pointNextCubeX.Minus.x, pointMainCubeX.Minus.x, pointMainCubeX.Plus.x))
            {
                cubeNext = CreateCube("x");
                SetNewScaleCube(cubeNext, "x");
                AddHeightToY();
                cubeNext = SetChangesCreateNextCube(cubeNext, "x");
                MoveCube moveCubeNext = cubeNext.AddComponent<MoveCube>();
                moveCubeNext.StartMovingX();
                // Debug.Log("sssssssssssssssssssss");
            }
        }
        else if (AxisForPoints == "z".ToLower())
        {
            if (IsInsideCube(pointNextCubeZ.Plus.z, pointMainCubeZ.Minus.z, pointMainCubeZ.Plus.z))
            {
                cubeNext = CreateCube("z");
                SetNewScaleCube(cubeNext, "z");
                AddHeightToY();
                cubeNext = SetChangesCreateNextCube(cubeNext, "z");
                MoveCube moveCubeNext = cubeNext.AddComponent<MoveCube>();
                moveCubeNext.StartMovingZ();
                Debug.Log("CreateNextCube в первом слуаче");
            }
            else if (IsInsideCube(pointNextCubeZ.Minus.z, pointMainCubeZ.Minus.z, pointMainCubeZ.Plus.z))
            {
                cubeNext = CreateCube("z");
                SetNewScaleCube(cubeNext, "z");
                AddHeightToY();
                cubeNext = SetChangesCreateNextCube(cubeNext, "z");
                MoveCube moveCubeNext = cubeNext.AddComponent<MoveCube>();
                moveCubeNext.StartMovingZ();
                Debug.Log("CreateNextCube в втором слуаче");
            }
        }
        else
        {
            Debug.Log("Только по осям Z and X");
        }
    }

    private void CreateFallCube(string AxisForPoints = null)
    {
        
        if (AxisForPoints == "x")
        {
            var fallCubeScale = 0f;
            Vector3 fallCubePosition;
            if (IsInsideCube(pointNextCubeX.Plus.x, pointMainCubeX.Minus.x, pointMainCubeX.Plus.x))
            {
                fallCubeScale = Mathf.Abs(pointMainCubeX.Minus.x - pointNextCubeX.Minus.x);
                fallCubePosition = (pointMainCubeX.Minus + pointNextCubeX.Minus) / 2f;
            }
            else
            {
                fallCubeScale = Mathf.Abs(pointMainCubeX.Plus.x - pointNextCubeX.Plus.x);
                fallCubePosition = (pointMainCubeX.Plus + pointNextCubeX.Plus) / 2f;
            }

            if (fallCubeScale > 0)
            {
                var cubeFall = Instantiate(GameManager.Instance.OriginalNextCubePrefab, fallCubePosition, Quaternion.identity);

                Vector3 newScaleFall = cubeFall.transform.localScale;
                newScaleFall.x = fallCubeScale;
                cubeFall.transform.localScale = newScaleFall;
                MoveCube moveCubeFall = cubeFall.AddComponent<MoveCube>();
                moveCubeFall.StartMovingY();
            }
            
            
        }// ЗДЕСЬЯЯЯЯЯЯЯЯЯЯЯЯЯЯЯЯЯЯ
        else
        {
            var fallCubeScale = 0f;
            Vector3 fallCubePosition;
            if (IsInsideCube(pointNextCubeZ.Plus.z, pointMainCubeZ.Minus.z, pointMainCubeZ.Plus.z))
            {
                fallCubeScale = Mathf.Abs(pointMainCubeZ.Minus.z - pointNextCubeZ.Minus.z);
                fallCubePosition = (pointMainCubeZ.Minus + pointNextCubeZ.Minus) / 2f;
            }
            else
            {
                fallCubeScale = Mathf.Abs(pointMainCubeZ.Plus.z - pointNextCubeZ.Plus.z);
                fallCubePosition = (pointMainCubeZ.Plus + pointNextCubeZ.Plus) / 2f;
            }
            var cubeFall = Instantiate(GameManager.Instance.OriginalNextCubePrefab, fallCubePosition, Quaternion.identity);

            Vector3 newScaleFall = cubeFall.transform.localScale;
            newScaleFall.z = fallCubeScale;
            cubeFall.transform.localScale = newScaleFall;
            MoveCube moveCubeFall = cubeFall.AddComponent<MoveCube>();
            moveCubeFall.StartMovingY();
        }
        
    }


    private static GameObject SetChangesCreateNextCube(GameObject cubeNext, string axis = null)
    {
        if (axis == "x")
        {
            GameManager.Instance.CreatedMainCube = cubeNext;
            Destroy(GameManager.Instance.CreatedNextCube);

            cubeNext = GameManager.Instance.CreateCube(cubeNext,
                GameManager.Instance.spawnPointX.transform.position);

            GameManager.Instance.CreatedNextCube = cubeNext;
            return cubeNext;
        }
        else if (axis == "z")
        {
            GameManager.Instance.CreatedMainCube = cubeNext;
            Destroy(GameManager.Instance.CreatedNextCube);

            cubeNext = GameManager.Instance.CreateCube(cubeNext,
                GameManager.Instance.spawnPointZ.transform.position);

            GameManager.Instance.CreatedNextCube = cubeNext;
            return cubeNext;
        }

        return null;
    }


    private GameObject CreateCube(string axis)
    {
        switch (axis)
        {
            case "x" when IsInsideCube(pointNextCubeX.Plus.x, pointMainCubeX.Minus.x, pointMainCubeX.Plus.x):
            {
                Vector3 nextCubePositionX = new Vector3((pointNextCubeX.Plus.x + pointMainCubeX.Minus.x) / 2f, 0f, 0f);
                var position = new Vector3(nextCubePositionX.x, GameManager.Instance.spawnPointX.transform.position.y,
                    nextCubePositionX.z);
                Debug.Log("Создание куба по оси X в первом случае CreateCube()");
                return Instantiate(GameManager.Instance.OriginalNextCubePrefab, position, Quaternion.identity);
            }
            case "x":
            {
                Vector3 nextCubePositionX = new Vector3((pointNextCubeX.Minus.x + pointMainCubeX.Plus.x) / 2f, 0f, 0f);
                var position = new Vector3(nextCubePositionX.x, GameManager.Instance.spawnPointX.transform.position.y,
                    nextCubePositionX.z);
                Debug.Log("Создание куба по оси X во втором случае CreateCube()");
                return Instantiate(GameManager.Instance.OriginalNextCubePrefab, position, Quaternion.identity);
            }
            case "z" when IsInsideCube(pointNextCubeZ.Minus.z, pointMainCubeZ.Minus.z, pointMainCubeZ.Plus.z):
            {
                Vector3 nextCubePositionZ = new Vector3(0f, 0f, (pointNextCubeZ.Plus.z + pointMainCubeZ.Minus.z) / 2f);
                var position = new Vector3(nextCubePositionZ.x, GameManager.Instance.spawnPointZ.transform.position.y,
                    nextCubePositionZ.z);
                return Instantiate(GameManager.Instance.OriginalNextCubePrefab, position, Quaternion.identity);
            }
            case "z":
            {
                Vector3 nextCubePositionZ = new Vector3(0f, 0f, (pointNextCubeZ.Minus.z + pointMainCubeZ.Plus.z) / 2f);
                var position = new Vector3(nextCubePositionZ.x, GameManager.Instance.spawnPointZ.transform.position.y,
                    nextCubePositionZ.z);
                return Instantiate(GameManager.Instance.OriginalNextCubePrefab, position, Quaternion.identity);
            }
            default:
                Debug.LogError($"В {axis} было передано неверное значение, оно может принимать только X или Z");
                return null;
        }
    }

    private void SetNewScaleCube(GameObject cubeNext, string axis)
    {
        Vector3 newScale;
        float nextCubeScale;
        if (axis == "x")
        {
            if (IsInsideCube(pointNextCubeX.Plus.x, pointMainCubeX.Minus.x, pointMainCubeX.Plus.x))
            {
                nextCubeScale = Mathf.Abs(pointNextCubeX.Plus.x - pointMainCubeX.Minus.x);
                newScale = cubeNext.transform.localScale;
                newScale.x = nextCubeScale;
                cubeNext.transform.localScale = newScale;
                Debug.Log("SetNewScale в первом случае");
            }
            else
                // else if (IsInsideCubeX(pointNextCubeX.Minus.x, pointMainCubeX.Minus.x, pointMainCubeX.Plus.x))
            {
                nextCubeScale = Mathf.Abs(pointNextCubeX.Minus.x - pointMainCubeX.Plus.x);
                newScale = cubeNext.transform.localScale;
                newScale.x = nextCubeScale;
                cubeNext.transform.localScale = newScale;
                Debug.Log("SetNewScale во втором случае");
            }
        }
        else
        {
            if (IsInsideCube(pointNextCubeZ.Plus.z, pointMainCubeZ.Minus.z, pointMainCubeZ.Plus.z))
            {
                nextCubeScale = Mathf.Abs(pointNextCubeZ.Plus.z - pointMainCubeZ.Minus.z);
                newScale = cubeNext.transform.localScale;
                newScale.z = nextCubeScale;
                cubeNext.transform.localScale = newScale;
            }
            else
            {
                nextCubeScale = Mathf.Abs(pointNextCubeZ.Minus.z - pointMainCubeZ.Plus.z);
                newScale = cubeNext.transform.localScale;
                newScale.z = nextCubeScale;
                cubeNext.transform.localScale = newScale;
            }
        }
        
        
    }
}