// namespace Scritps
// {
//     public class oldcut
//     {
//         private void CutPoop()
//     {
//         var cubeNext = GameManager.Instance.CreatedNextCube;
//         var cubeMain = GameManager.Instance.CreatedMainCube;
//         GameObject cubeFall;
//
//         if (IsInsideCube(pointNextCubeX.Plus.x, pointMainCubeX.Minus.x, pointMainCubeX.Plus.x))
//         {
//             float nextCubeScale = Mathf.Abs(pointNextCubeX.Plus.x - pointMainCubeX.Minus.x);
//             Vector3 nextCubePosition = (pointNextCubeX.Plus + pointMainCubeX.Minus) / 2f;
//             nextCubePosition = new Vector3(nextCubePosition.x, GameManager.Instance.spawnPointX.transform.position.y,
//                 nextCubePosition.z);
//             cubeNext = Instantiate(GameManager.Instance.OriginalNextCubePrefab, nextCubePosition, Quaternion.identity);
//
//             Vector3 newScale = cubeNext.transform.localScale;
//             newScale.x = nextCubeScale;
//             cubeNext.transform.localScale = newScale;
//
//
//             AddHeightToY();
//             GameManager.Instance.CreatedMainCube = cubeNext;
//             Destroy(GameManager.Instance.CreatedNextCube);
//             cubeNext = GameManager.Instance.CreateCube(cubeNext, GameManager.Instance.spawnPointX.transform.position);
//             GameManager.Instance.CreatedNextCube = cubeNext;
//
//             // ------------------------------
//
//
//             float fallCubeScale = Mathf.Abs(pointMainCubeX.Minus.x - pointNextCubeX.Minus.x);
//             Vector3 fallCubePosition = (pointMainCubeX.Minus + pointNextCubeX.Minus) / 2f;
//
//             cubeFall = Instantiate(GameManager.Instance.OriginalNextCubePrefab, fallCubePosition, Quaternion.identity);
//
//             Vector3 newScaleFall = cubeFall.transform.localScale;
//             newScaleFall.x = fallCubeScale;
//             cubeFall.transform.localScale = newScaleFall;
//
//             // -------------------------------
//
//
//             MoveCube moveCubeFall = cubeFall.AddComponent<MoveCube>();
//             moveCubeFall.StartMovingY();
//             MoveCube moveCubeNext = cubeNext.AddComponent<MoveCube>();
//             // moveCubeNext.StartMovingX();
//         }
//         else if (IsInsideCube(pointNextCubeX.Minus.x, pointMainCubeX.Minus.x, pointMainCubeX.Plus.x))
//         {
//             float nextCubeScale = Mathf.Abs(pointNextCubeX.Minus.x - pointMainCubeX.Plus.x);
//             Vector3 nextCubePosition = (pointNextCubeX.Minus + pointMainCubeX.Plus) / 2f;
//             nextCubePosition = new Vector3(nextCubePosition.x, GameManager.Instance.spawnPointX.transform.position.y,
//                 nextCubePosition.z);
//             cubeNext = Instantiate(GameManager.Instance.OriginalNextCubePrefab, nextCubePosition, Quaternion.identity);
//
//             Vector3 newScale = cubeNext.transform.localScale;
//             newScale.x = nextCubeScale;
//             cubeNext.transform.localScale = newScale;
//
//             GameManager.Instance.spawnPointX.transform.position += new Vector3(0, 0.1f, 0);
//             GameManager.Instance.CreatedMainCube = cubeNext;
//             Destroy(GameManager.Instance.CreatedNextCube);
//             cubeNext = GameManager.Instance.CreateCube(cubeNext, GameManager.Instance.spawnPointX.transform.position);
//             GameManager.Instance.CreatedNextCube = cubeNext;
//
//             // ------------------------------
//
//             float fallCubeScale = Mathf.Abs(pointMainCubeX.Plus.x - pointNextCubeX.Plus.x);
//             Vector3 fallCubePosition = (pointMainCubeX.Plus + pointNextCubeX.Plus) / 2f;
//             cubeFall = Instantiate(GameManager.Instance.OriginalNextCubePrefab, fallCubePosition, Quaternion.identity);
//
//             Vector3 newScaleFall = cubeFall.transform.localScale;
//             newScaleFall.x = fallCubeScale;
//             cubeFall.transform.localScale = newScaleFall;
//
//             // -------------------------------
//
//             MoveCube moveCubeFall = cubeFall.AddComponent<MoveCube>();
//             moveCubeFall.StartMovingY();
//             MoveCube moveCubeNext = cubeNext.AddComponent<MoveCube>();
//             // moveCubeNext.StartMovingX();
//         }
//     }
//     }
// }