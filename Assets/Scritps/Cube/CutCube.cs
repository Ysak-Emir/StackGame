// using System;
// using Unity.VisualScripting;
// using UnityEngine;
// using Random = System.Random;
//
// public class CutCube : MonoBehaviour
// {
//     private struct CubePointsX
//     {
//         public Vector3 MinusX;
//         public Vector3 PlusX;
//
//         public CubePointsX(Vector3 minusX, Vector3 plusX)
//         {
//             MinusX = minusX;
//             PlusX = plusX;
//         }
//     }
//
//     private struct CubePointsZ
//     {
//         public Vector3 MinusZ;
//         public Vector3 PlusZ;
//
//         public CubePointsZ(Vector3 minusZ, Vector3 plusZ)
//         {
//             MinusZ = minusZ;
//             PlusZ = plusZ;
//         }
//     }
//
//     private CubePointsX pointMainCubeX, pointNextCubeX;
//     private CubePointsZ pointMainCubeZ, pointNextCubeZ;
//
//
//     private GameObject cubeNextCheck, cubeMainCheck;
//     public string _nextSpawnCube = "x";
//
//     //CreateNextCube FUNC
//     private GameObject cubeNext;
//
//     //IsInside FUNC
//     private bool _checkPlusX;
//     private bool _checkMinusX;
//     private bool _checkPlusZ;
//     private bool _checkMinusZ;
//
//     private MoveCube _moveCubeDirectionCheck;
//     
//     //----------------------
//
//     private void Start()
//     {
//         
//      
//         AddHeightToY();
//         GameManager.Instance.CreatedMainCube =
//             GameManager.Instance.CreateCube(GameManager.Instance.OriginalMainCubePrefab, Vector3.zero);
//
//         Vector3 spawnPosition;
//         if (_nextSpawnCube == "x")
//         {
//             spawnPosition = GameManager.Instance.spawnPointX.transform.position;
//         }
//         else
//         {
//             spawnPosition = GameManager.Instance.spawnPointZ.transform.position;
//         }
//         
//         GameManager.Instance.CreatedNextCube =
//             GameManager.Instance.CreateCube(GameManager.Instance.OriginalNextCubePrefab,
//                 spawnPosition);
//
//         _moveCubeDirectionCheck = GameManager.Instance.CreatedNextCube.AddComponent<MoveCube>();
//
//         if (_nextSpawnCube == "x")
//         {
//             _moveCubeDirectionCheck.StartMovingX();
//         }
//         else
//         {
//             _moveCubeDirectionCheck.StartMovingZ();
//         }
//         
//     }
//
//     private void Update()
//     {
//         if (Input.GetKeyDown(KeyCode.Space))
//         {
//             CheckPosition();
//         }
//     }
//
//
//     private void CheckPosition()
//     {
//         cubeNextCheck = GameManager.Instance?.CreatedNextCube;
//         cubeMainCheck = GameManager.Instance?.CreatedMainCube;
//
//         if (cubeMainCheck == null || cubeNextCheck == null)
//         {
//             Debug.LogError("Один из кубов (cubeMainCheck или cubeNextCube) не инициализирован!");
//             return;
//         }
//     
//         SetCubePoints(ref pointMainCubeX, cubeMainCheck, cubeNextCheck.transform.localScale.x);
//         SetCubePoints(ref pointNextCubeX, cubeNextCheck, cubeNextCheck.transform.localScale.x);
//
//         SetCubePoints(ref pointMainCubeZ, cubeMainCheck, cubeNextCheck.transform.localScale.z);
//         SetCubePoints(ref pointNextCubeZ, cubeNextCheck, cubeNextCheck.transform.localScale.z);
//
//         Debug.Log("Текущая ось: " + _nextSpawnCube);
//         
//         bool insideResult = IsInsideCube();
//         
//         if (insideResult)
//         {
//             Cut(_nextSpawnCube);
//
//             _nextSpawnCube = _nextSpawnCube == "x" ? "z" : "x";
//             Debug.Log("Следующая ось: " + _nextSpawnCube);
//         }
//         else
//         {
//             Debug.LogWarning("GameOver: Кубы не пересекаются!");
//             GameOver();
//         }
//     }
//
//     private void SetCubePoints(ref CubePointsX points, GameObject cube, float scale)
//     {
//         points.PlusX = cube.transform.position;
//         points.PlusX.x += (scale / 2f);
//
//         points.MinusX = cube.transform.position;
//         points.MinusX.x -= (scale / 2f);
//     }
//
//     private void SetCubePoints(ref CubePointsZ points, GameObject cube, float scale)
//     {
//         points.PlusZ = cube.transform.position;
//         points.PlusZ.z += (scale / 2f);
//
//         points.MinusZ = cube.transform.position;
//         points.MinusZ.z -= (scale / 2f);
//     }
//
//     private bool IsInsideCube()
//     {
//         _checkPlusX = (pointNextCubeX.PlusX.x >= pointMainCubeX.MinusX.x &&
//                       pointNextCubeX.PlusX.x <= pointMainCubeX.PlusX.x);
//         _checkMinusX = (pointNextCubeX.MinusX.x >= pointMainCubeX.MinusX.x &&
//                        pointNextCubeX.MinusX.x <= pointMainCubeX.PlusX.x);
//
//         _checkPlusZ = (pointNextCubeZ.PlusZ.z >= pointMainCubeZ.MinusZ.z &&
//                       pointNextCubeZ.PlusZ.z <= pointMainCubeZ.PlusZ.z);
//         _checkMinusZ = (pointNextCubeZ.MinusZ.z >= pointMainCubeZ.MinusZ.z &&
//                        pointNextCubeZ.MinusZ.z <= pointMainCubeZ.PlusZ.z);
//
//         bool inside = (_checkPlusX || _checkMinusX) || (_checkPlusZ || _checkMinusZ);
//
//         Debug.Log($"IsInsideCube: plus - {_checkPlusX}, minus - {_checkMinusX}, inside - {inside}");
//
//         return inside;
//     }
//
//
//     private static void AddHeightToY()
//     {
//         GameManager.Instance.spawnPointX.transform.position += new Vector3(0, 0.1f, 0);
//         GameManager.Instance.spawnPointZ.transform.position += new Vector3(0, 0.1f, 0);
//         GameManager.Instance.upPointY.transform.position += new Vector3(0, 0.1f, 0);
//     }
//
//
//     private void Cut(string axis)
//     {
//         CreateNextCube(axis);
//         CreateFallCube(axis);
//     }
//
//
//     private void CreateNextCube(string AxisForPoints = null)
//     {
//         if (string.IsNullOrEmpty(AxisForPoints))
//         {
//             Debug.LogWarning("CreateNextCube: Параметр AxisForPoints не может быть null или пустым.");
//             return;
//         }
//
//         cubeNext = GameManager.Instance.CreatedNextCube;
//
//         if (!cubeNext)
//         {
//             Debug.LogWarning("CreateNextCube: GameManager.Instance.CreatedNextCube равен null.");
//             return;
//         }
//
//         if (AxisForPoints.Equals("x", StringComparison.OrdinalIgnoreCase))
//         {
//             cubeNext = CreateCube("x");
//             
//             if (cubeNext == null)
//             {
//                 Debug.Log("GameOver: Ошибка при создании куба по оси X.");
//                 GameOver();  // Убедитесь, что GameOver вызывается
//                 return;
//             }
//
//             SetNewScaleCube(cubeNext, "x");
//             AddHeightToY();
//
//
//             if (cubeNext == null)
//             {
//                 Debug.Log("GameOver: cubeNext равен null после изменений.");
//                 return;
//             }
//
//
//             cubeNext = SetChangesCreateNextCube(cubeNext, "x");
//
//             if (cubeNext == null)
//             {
//                 Debug.Log("GameOver: cubeNext равен null после SetChangesCreateNextCube.");
//                 return;
//             }
//
//             MoveCube moveCubeNext = cubeNext.AddComponent<MoveCube>();
//             if (moveCubeNext == null)
//             {
//                 Debug.LogWarning("CreateNextCube: Не удалось добавить компонент MoveCube.");
//                 return;
//             }
//
//             if (_nextSpawnCube == "x")
//             {
//                 moveCubeNext.StartMovingX();
//             }
//             else
//             {
//                 moveCubeNext.StartMovingZ();
//             }
//             _moveCubeDirectionCheck = moveCubeNext;
//         }
//         else if (AxisForPoints.Equals("z", StringComparison.OrdinalIgnoreCase))
//         {
//             cubeNext = CreateCube("z");
//             SetNewScaleCube(cubeNext, "z");
//             AddHeightToY();
//             cubeNext = SetChangesCreateNextCube(cubeNext, "z");
//             MoveCube moveCubeNext = cubeNext.AddComponent<MoveCube>();
//             if (_nextSpawnCube == "x")
//             {
//                 moveCubeNext.StartMovingX();
//             }
//             else
//             {
//                 moveCubeNext.StartMovingZ();
//             }
//             _moveCubeDirectionCheck = moveCubeNext;
//         }
//         else
//         {
//             Debug.LogError("Только по осям Z and X");
//         }
//     }
//
//     private void CreateFallCube(string AxisForPoints = null)
//     {
//         var fallCubeScale = 0f;
//         Vector3 fallCubePosition;
//         if (AxisForPoints == "x")
//         {
//             if (_checkPlusX)
//             {
//                 fallCubeScale = Mathf.Abs(pointMainCubeX.MinusX.x - pointNextCubeX.MinusX.x);
//                 fallCubePosition = (pointMainCubeX.MinusX + pointNextCubeX.MinusX) / 2f;
//             }
//             else if (_checkMinusX)
//             {
//                 fallCubeScale = Mathf.Abs(pointMainCubeX.PlusX.x - pointNextCubeX.PlusX.x);
//                 fallCubePosition = (pointMainCubeX.PlusX + pointNextCubeX.PlusX) / 2f;
//             }
//             else
//             {
//                 return;
//             }
//
//             if (fallCubeScale > 0)
//             {
//                 var cubeFall = Instantiate(GameManager.Instance.OriginalNextCubePrefab, fallCubePosition,
//                     Quaternion.identity);
//
//                 Vector3 newScaleFall = cubeFall.transform.localScale;
//                 newScaleFall.x = fallCubeScale;
//                 cubeFall.transform.localScale = newScaleFall;
//
//                 MoveCube moveCubeFall = cubeFall.AddComponent<MoveCube>();
//                 moveCubeFall.StartMovingY();
//             }
//         }
//         else
//         {
//             if (_checkPlusZ)
//             {
//                 fallCubeScale = Mathf.Abs(pointMainCubeZ.MinusZ.z - pointNextCubeZ.MinusZ.z);
//                 fallCubePosition = (pointMainCubeZ.MinusZ + pointNextCubeZ.MinusZ) / 2f;
//             }
//             else if (_checkMinusZ)
//             {
//                 fallCubeScale = Mathf.Abs(pointMainCubeZ.PlusZ.z - pointNextCubeZ.PlusZ.z);
//                 fallCubePosition = (pointMainCubeZ.PlusZ + pointNextCubeZ.PlusZ) / 2f;
//             }
//             else
//             {
//                 return;
//             }
//
//             if (fallCubeScale > 0)
//             {
//                 var cubeFall = Instantiate(GameManager.Instance.OriginalNextCubePrefab, fallCubePosition,
//                     Quaternion.identity);
//
//                 Vector3 newScaleFall = cubeFall.transform.localScale;
//                 newScaleFall.z = fallCubeScale;
//                 cubeFall.transform.localScale = newScaleFall;
//                 MoveCube moveCubeFall = cubeFall.AddComponent<MoveCube>();
//                 moveCubeFall.StartMovingY();
//             }
//         }
//     }
//
//     private GameObject SetChangesCreateNextCube(GameObject cubeNext, string axis = null)
//     {
//         if (axis != "x" && axis != "z")
//         {
//             Debug.LogError("В axis в SetChangesCreateNextCube было передано неверное значение!");
//             return null;
//         }
//
//         GameManager.Instance.CreatedMainCube = cubeNext;
//         Destroy(GameManager.Instance.CreatedNextCube);
//
//         Vector3 spawnPosition;
//         if (_nextSpawnCube == "x")
//         {
//             spawnPosition = GameManager.Instance.spawnPointX.transform.position;
//         }
//         else
//         {
//             spawnPosition = GameManager.Instance.spawnPointZ.transform.position;
//         }
//
//         cubeNext = GameManager.Instance.CreateCube(cubeNext, spawnPosition);
//         GameManager.Instance.CreatedNextCube = cubeNext;
//
//         return cubeNext;
//     }
//
//     private void GameOver()
//     {
//         Debug.Log("GAME OVER!");
//
//         if (GameManager.Instance.CreatedNextCube != null)
//         {
//             MoveCube moveCube = GameManager.Instance.CreatedNextCube.GetComponent<MoveCube>();
//             if (moveCube == null)
//             {
//                 moveCube = GameManager.Instance.CreatedNextCube.AddComponent<MoveCube>();
//             }
//             moveCube.StartMovingY();
//         }
//     
//         // Дополнительно можно вызвать метод для остановки игры или перезапуска
//         // например: GameManager.Instance.StopGame();
//     }
//
//
//     private GameObject CreateCube(string axis)
//     {
//         if (axis == "x")
//
//         {
//             Vector3 nextCubePositionX;
//             if (_checkPlusX)
//             {
//                 nextCubePositionX = new Vector3((pointNextCubeX.PlusX.x + pointMainCubeX.MinusX.x) / 2f, 0f, 0f);
//             }
//             else if (_checkMinusX)
//             {
//                 nextCubePositionX = new Vector3((pointNextCubeX.MinusX.x + pointMainCubeX.PlusX.x) / 2f, 0f, 0f);
//             }
//             else
//             {
//                 //GAME OVER
//                 GameOver();
//                 Debug.LogWarning("CreateCube: точка не находится внутри границ, куб не будет создан.");
//                 return null;
//             }
//
//             Vector3 position = new Vector3(nextCubePositionX.x, GameManager.Instance.upPointY.transform.position.y, 0);
//             return Instantiate(GameManager.Instance.OriginalNextCubePrefab, position, Quaternion.identity);
//         }
//
//         if (axis == "z")
//         {
//             Vector3 nextCubePositionZ;
//             if (_checkPlusZ)
//             {
//                 nextCubePositionZ = new Vector3(0f, 0f, (pointNextCubeZ.PlusZ.z + pointMainCubeZ.MinusZ.z) / 2f);
//             }
//             else if (_checkMinusZ)
//             {
//                 nextCubePositionZ = new Vector3(0f, 0f, (pointNextCubeZ.MinusZ.z + pointMainCubeZ.PlusZ.z) / 2f);
//             }
//             else
//             {
//                 GameOver();
//                 Debug.LogWarning("CreateCube: точка не находится внутри границ, куб не будет создан.");
//                 return null;
//             }
//             
//             Vector3 position = new Vector3(nextCubePositionZ.z, GameManager.Instance.upPointY.transform.position.y, 0);
//             return Instantiate(GameManager.Instance.OriginalNextCubePrefab, position, Quaternion.identity);
//         }
//
//         Debug.LogError("CreateCube: Неизвестная ось");
//         return null;
//     }
//
//     private void SetNewScaleCube(GameObject cubeNext, string axis)
//     {
//         if (cubeNext == null)
//         {
//             Debug.LogWarning("SetNewScaleCube: Переданный объект cubeNext равен null!");
//             return;
//         }
//
//         Vector3 newScale = cubeNext.transform.localScale;
//
//         if (axis == "x")
//         {
//             float nextCubeScale = 0f;
//
//             if (_checkPlusX)
//             {
//                 nextCubeScale = Mathf.Abs(pointNextCubeX.PlusX.x - pointMainCubeX.MinusX.x);
//             }
//             else if (_checkMinusX)
//             {
//                 nextCubeScale = Mathf.Abs(pointNextCubeX.MinusX.x - pointMainCubeX.PlusX.x);
//             }
//             else
//             {
//                 Debug.LogWarning("SetNewScaleCube: Точка не находится внутри границ, масштаб по X не изменяется.");
//                 return;
//             }
//
//             newScale.x = nextCubeScale;
//             Debug.Log($"SetNewScaleCube: Новый размер по оси X = {nextCubeScale}");
//         }
//         else if (axis == "z")
//         {
//             float nextCubeScale = 0f;
//
//             if (_checkPlusZ)
//             {
//                 nextCubeScale = Mathf.Abs(pointNextCubeZ.PlusZ.z - pointMainCubeZ.MinusZ.z);
//             }
//             else if (_checkMinusZ)
//             {
//                 nextCubeScale = Mathf.Abs(pointNextCubeZ.MinusZ.z - pointMainCubeZ.PlusZ.z);
//             }
//             else
//             {
//                 Debug.LogWarning("SetNewScaleCube: Точка не находится внутри границ, масштаб по Z не изменяется.");
//                 return;
//             }
//
//             newScale.z = nextCubeScale;
//             Debug.Log($"SetNewScaleCube: Новый размер по оси Z = {nextCubeScale}");
//         }
//         else
//         {
//             Debug.LogWarning($"SetNewScaleCube: Неизвестная ось '{axis}', масштабирование не выполнено.");
//             return;
//         }
//
//         cubeNext.transform.localScale = newScale;
//     }
// }