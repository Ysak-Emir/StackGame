using System;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

public class CutCube : MonoBehaviour
{
    private struct CubePointsX
    {
        public Vector3 MinusX;
        public Vector3 PlusX;

        public CubePointsX(Vector3 minusX, Vector3 plusX)
        {
            MinusX = minusX;
            PlusX = plusX;
        }
    }

    private struct CubePointsZ
    {
        public Vector3 MinusZ;
        public Vector3 PlusZ;

        public CubePointsZ(Vector3 minusZ, Vector3 plusZ)
        {
            MinusZ = minusZ;
            PlusZ = plusZ;
        }
    }

    private CubePointsX pointMainCubeX, pointNextCubeX;
    private CubePointsZ pointMainCubeZ, pointNextCubeZ;


    private GameObject cubeNextCheck, cubeMainCheck;
    public MoveCube _moveCubeDirectionCheck;
    public string _nextSpawnCube = null;

    //CreateNextCube FUNC
    private GameObject cubeNext;
    
    //IsInside FUNC
    private bool checkPlusX;
    private bool checkMinusX;
    private bool checkPlusZ;
    private bool checkMinusZ;
    //----------------------

    private void Start()
    {
        MoveCube moveCube = GetComponent<MoveCube>();
        AddHeightToY();
        GameManager.Instance.CreatedMainCube =
            GameManager.Instance.CreateCube(GameManager.Instance.OriginalMainCubePrefab, Vector3.zero);
        Random random = new Random();
        // Vector3[] spawns =
        // {
        //     GameManager.Instance.spawnPointX.transform.position, GameManager.Instance.spawnPointZ.transform.position
        // };
        // Vector3 randomSpawn = spawns[random.Next(spawns.Length)];
        GameManager.Instance.CreatedNextCube =
            GameManager.Instance.CreateCube(GameManager.Instance.OriginalNextCubePrefab,
                GameManager.Instance.spawnPointX.transform.position);

        _moveCubeDirectionCheck = GameManager.Instance.CreatedNextCube.AddComponent<MoveCube>();

        Debug.Log($"pointMainCubePlusX - {pointMainCubeX.PlusX}");
        Debug.Log($"pointMainCubeMinusX - {pointMainCubeX.MinusX}");
        Debug.Log("----------------------------");
        Debug.Log($"pointNextCubePlusX - {pointNextCubeX.PlusX}");
        Debug.Log($"pointNextCubeMinusX - {pointNextCubeX.MinusX}");

        Debug.Log("=========================================");

        Debug.Log($"pointMainCubePlusZ - {pointMainCubeZ.PlusZ}");
        Debug.Log($"pointMainCubeMinusZ - {pointMainCubeZ.MinusZ}");
        Debug.Log("----------------------------");
        Debug.Log($"pointNextCubePlusZ - {pointNextCubeZ.PlusZ}");
        Debug.Log($"pointNextCubeMinusZ - {pointNextCubeZ.MinusZ}");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckPosition();
        }
    }


    private void CheckPosition()
    {
        cubeNextCheck = GameManager.Instance?.CreatedNextCube;
        cubeMainCheck = GameManager.Instance?.CreatedMainCube;

        if (cubeMainCheck == null || cubeNextCheck == null)
        {
            Debug.LogError("Один из кубов (cubeMainCheck или cubeNextCube) не инициализирован!");
            return;
        }

        SetCubePoints(ref pointMainCubeX, cubeMainCheck, cubeNextCheck.transform.localScale.x);
        SetCubePoints(ref pointNextCubeX, cubeNextCheck, cubeNextCheck.transform.localScale.x);

        SetCubePoints(ref pointMainCubeZ, cubeMainCheck, cubeNextCheck.transform.localScale.z);
        SetCubePoints(ref pointNextCubeZ, cubeNextCheck, cubeNextCheck.transform.localScale.z);

        Debug.Log(_nextSpawnCube);


        Debug.Log($"pointMainCubePlusX - {pointMainCubeX.PlusX}");
        Debug.Log($"pointMainCubeMinusX - {pointMainCubeX.MinusX}");
        Debug.Log("----------------------------");
        Debug.Log($"pointNextCubePlusX - {pointNextCubeX.PlusX}");
        Debug.Log($"pointNextCubeMinusX - {pointNextCubeX.MinusX}");

        Debug.Log("=========================================");

        Debug.Log($"pointMainCubePlusZ - {pointMainCubeZ.PlusZ}");
        Debug.Log($"pointMainCubeMinusZ - {pointMainCubeZ.MinusZ}");
        Debug.Log("----------------------------");
        Debug.Log($"pointNextCubePlusZ - {pointNextCubeZ.PlusZ}");
        Debug.Log($"pointNextCubeMinusZ - {pointNextCubeZ.MinusZ}");
        

        if (IsInsideCube())
        {
            Cut("x");
        }
    }

    private void SetCubePoints(ref CubePointsX points, GameObject cube, float scale)
    {
        points.PlusX = cube.transform.position;
        points.PlusX.x += (scale / 2f);

        points.MinusX = cube.transform.position;
        points.MinusX.x -= (scale / 2f);
    }

    private void SetCubePoints(ref CubePointsZ points, GameObject cube, float scale)
    {
        points.PlusZ = cube.transform.position;
        points.PlusZ.z += (scale / 2f);

        points.MinusZ = cube.transform.position;
        points.MinusZ.z -= (scale / 2f);
    }

    private bool IsInsideCube()
    {
        checkPlusX = (pointNextCubeX.PlusX.x >= pointMainCubeX.MinusX.x &&
                     pointNextCubeX.PlusX.x <= pointMainCubeX.PlusX.x);
        checkMinusX = (pointNextCubeX.MinusX.x >= pointMainCubeX.MinusX.x &&
                      pointNextCubeX.MinusX.x <= pointMainCubeX.PlusX.x);
        
        checkPlusZ = (pointNextCubeZ.PlusZ.z >= pointMainCubeZ.MinusZ.z &&
                      pointNextCubeZ.PlusZ.z <= pointMainCubeZ.PlusZ.z);
        checkMinusZ = (pointNextCubeZ.MinusZ.z >= pointMainCubeZ.MinusZ.z &&
                       pointNextCubeZ.MinusZ.z <= pointMainCubeZ.PlusZ.z);

        bool inside = (checkPlusX || checkMinusX) || (checkPlusZ || checkMinusZ);

        Debug.Log($"IsInsideCube: plus - {checkPlusX}, minus - {checkMinusX}, inside - {inside}");

        return inside;
    }


    private static void AddHeightToY()
    {
        GameManager.Instance.spawnPointX.transform.position += new Vector3(0, 0.1f, 0);
        GameManager.Instance.spawnPointZ.transform.position += new Vector3(0, 0.1f, 0);
        GameManager.Instance.upPointY.transform.position += new Vector3(0, 0.1f, 0);
    }


    private void Cut(string axis)
    {
        CreateNextCube(axis);
        CreateFallCube(axis);
    }


    private void CreateNextCube(string AxisForPoints = null)
    {
        if (string.IsNullOrEmpty(AxisForPoints))
        {
            Debug.LogWarning("CreateNextCube: Параметр AxisForPoints не может быть null или пустым.");
            return;
        }
        
        cubeNext = GameManager.Instance.CreatedNextCube;

        if (!cubeNext)
        {
            Debug.LogWarning("CreateNextCube: GameManager.Instance.CreatedNextCube равен null.");
            return;
        }
        
        if (AxisForPoints.Equals("x", StringComparison.OrdinalIgnoreCase))
        {
            cubeNext = CreateCube("x");
            
            if (!cubeNext)
            {
                Debug.Log("GameOver: Ошибка при создании куба по оси X.");
                return;
            }
            
            SetNewScaleCube(cubeNext, "x");
            AddHeightToY();
            
            
            if (cubeNext == null)
            {
                Debug.Log("GameOver: cubeNext равен null после изменений.");
                return;
            }
            
            
            cubeNext = SetChangesCreateNextCube(cubeNext, "x");
            
            if (cubeNext == null)
            {
                Debug.Log("GameOver: cubeNext равен null после SetChangesCreateNextCube.");
                return;
            }
            
            MoveCube moveCubeNext = cubeNext.AddComponent<MoveCube>();
            if (moveCubeNext == null)
            {
                Debug.LogWarning("CreateNextCube: Не удалось добавить компонент MoveCube.");
                return;
            }
            moveCubeNext.StartMovingX();
            _moveCubeDirectionCheck = moveCubeNext;
        }
        else if (AxisForPoints.Equals("z", StringComparison.OrdinalIgnoreCase))
        {
            cubeNext = CreateCube("z");
            SetNewScaleCube(cubeNext, "z");
            AddHeightToY();
            cubeNext = SetChangesCreateNextCube(cubeNext, "z");
            MoveCube moveCubeNext = cubeNext.AddComponent<MoveCube>();
            // moveCubeNext.StartMovingZ();
            // _moveCubeDirectionCheck = moveCubeNext;
        }
        else
        {
            Debug.Log("Только по осям Z and X");
        }
    }

    private void CreateFallCube(string AxisForPoints = null)
    {
        var fallCubeScale = 0f;
        Vector3 fallCubePosition;
        if (AxisForPoints == "x")
        {
            if (checkPlusX)
            {
                fallCubeScale = Mathf.Abs(pointMainCubeX.MinusX.x - pointNextCubeX.MinusX.x);
                fallCubePosition = (pointMainCubeX.MinusX + pointNextCubeX.MinusX) / 2f;
            }
            else if (checkMinusX)
            {
                fallCubeScale = Mathf.Abs(pointMainCubeX.PlusX.x - pointNextCubeX.PlusX.x);
                fallCubePosition = (pointMainCubeX.PlusX + pointNextCubeX.PlusX) / 2f;
            }
            else
            {
                return;
            }
            if (fallCubeScale > 0)
            {
                var cubeFall = Instantiate(GameManager.Instance.OriginalNextCubePrefab, fallCubePosition,
                    Quaternion.identity);

                Vector3 newScaleFall = cubeFall.transform.localScale;
                newScaleFall.x = fallCubeScale;
                cubeFall.transform.localScale = newScaleFall;

                MoveCube moveCubeFall = cubeFall.AddComponent<MoveCube>();
                moveCubeFall.StartMovingY();
            }

        }
        else
        {
            if (checkPlusZ)
            {
                fallCubeScale = Mathf.Abs(pointMainCubeZ.MinusZ.z - pointNextCubeZ.MinusZ.z);
                fallCubePosition = (pointMainCubeZ.MinusZ + pointNextCubeZ.MinusZ) / 2f;
            }
            else if (checkMinusZ)
            {
                fallCubeScale = Mathf.Abs(pointMainCubeZ.PlusZ.z - pointNextCubeZ.PlusZ.z);
                fallCubePosition = (pointMainCubeZ.PlusZ + pointNextCubeZ.PlusZ) / 2f;
            }
            else
            {
                return;
            }
            if (fallCubeScale > 0)
            {
                var cubeFall = Instantiate(GameManager.Instance.OriginalNextCubePrefab, fallCubePosition, Quaternion.identity);

                Vector3 newScaleFall = cubeFall.transform.localScale;
                newScaleFall.z = fallCubeScale;
                cubeFall.transform.localScale = newScaleFall;
                MoveCube moveCubeFall = cubeFall.AddComponent<MoveCube>();
                moveCubeFall.StartMovingY();
            }
        }
    }

    private  GameObject SetChangesCreateNextCube(GameObject cubeNext, string axis = null)
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

        if (axis == "z")
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

    private void GameOver()
    {
        MoveCube moveCube = cubeNext.AddComponent<MoveCube>();
        moveCube.StartMovingY();
    }


    private GameObject CreateCube(string axis)
    {
        if (axis == "x")

        {
            Vector3 nextCubePositionX;
            if (checkPlusX)
            {
                nextCubePositionX = new Vector3((pointNextCubeX.PlusX.x + pointMainCubeX.MinusX.x) / 2f, 0f, 0f);
            }
            else if (checkMinusX)
            {
                nextCubePositionX = new Vector3((pointNextCubeX.MinusX.x + pointMainCubeX.PlusX.x) / 2f, 0f, 0f);
            }
            else
            {
                // Если ни одно из условий не выполнено, куб не создаем
                //GAME OVER
                GameOver();
                Debug.LogWarning("CreateCube: точка не находится внутри границ, куб не будет создан.");
                return null; 
            }

            Vector3 position = new Vector3(nextCubePositionX.x, GameManager.Instance.upPointY.transform.position.y, 0);
            Debug.Log("Создание куба по оси X в первом случае CreateCube()");
            return Instantiate(GameManager.Instance.OriginalNextCubePrefab, position, Quaternion.identity);
        }
        
        return GameManager.Instance.OriginalNextCubePrefab;
    }

    private void SetNewScaleCube(GameObject cubeNext, string axis)
    {
        if (cubeNext == null)
        {
            Debug.LogWarning("SetNewScaleCube: Переданный объект cubeNext равен null!");
            return;
        }

        Vector3 newScale = cubeNext.transform.localScale;

        if (axis == "x")
        {
            float nextCubeScale = 0f;

            if (checkPlusX)
            {
                nextCubeScale = Mathf.Abs(pointNextCubeX.PlusX.x - pointMainCubeX.MinusX.x);
            }
            else if (checkMinusX)
            {
                nextCubeScale = Mathf.Abs(pointNextCubeX.MinusX.x - pointMainCubeX.PlusX.x);
            }
            else
            {
                Debug.LogWarning("SetNewScaleCube: Точка не находится внутри границ, масштаб по X не изменяется.");
                return;
            }

            newScale.x = nextCubeScale;
            Debug.Log($"SetNewScaleCube: Новый размер по оси X = {nextCubeScale}");
        }
        else if (axis == "z")
        {
            float nextCubeScale = 0f;

            if (checkPlusZ)
            {
                nextCubeScale = Mathf.Abs(pointNextCubeZ.PlusZ.z - pointMainCubeZ.MinusZ.z);
            }
            else if (checkMinusZ)
            {
                nextCubeScale = Mathf.Abs(pointNextCubeZ.MinusZ.z - pointMainCubeZ.PlusZ.z);
            }
            else
            {
                Debug.LogWarning("SetNewScaleCube: Точка не находится внутри границ, масштаб по Z не изменяется.");
                return;
            }

            newScale.z = nextCubeScale;
            Debug.Log($"SetNewScaleCube: Новый размер по оси Z = {nextCubeScale}");
        }
        else
        {
            Debug.LogWarning($"SetNewScaleCube: Неизвестная ось '{axis}', масштабирование не выполнено.");
            return;
        }

        cubeNext.transform.localScale = newScale;
    }


    // else if (axis == "z" && IsInsideCube())

    // {

    //     nextCubeScale = Mathf.Abs(pointNextCubeZ.PlusZ.z - pointMainCubeZ.MinusZ.z);

    //     newScale = cubeNext.transform.localScale;

    //     newScale.z = nextCubeScale;

    //     cubeNext.transform.localScale = newScale;

    //     Debug.Log("SetNewScale в первом случае для оси Z");

    // }

    // else if (axis == "z")

    // {

    //     nextCubeScale = Mathf.Abs(pointNextCubeZ.MinusZ.z - pointMainCubeZ.PlusZ.z);

    //     newScale = cubeNext.transform.localScale;

    //     newScale.z = nextCubeScale;

    //     cubeNext.transform.localScale = newScale;


    //     Debug.Log("SetNewScale во втором случае для оси Z");
}