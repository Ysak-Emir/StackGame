using System;
using UnityEngine;

public class CutCube : MonoBehaviour
{
    // private GameObject pointCut;
    private Vector3 pointMainCubeMinusX;
    private Vector3 pointMainCubePlusX;
    private Vector3 pointNextCubeMinusX;
    private Vector3 pointNextCubePlusX;
    
    private GameObject createdObject1;
    private GameObject createdObject2;
    
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Cut();
            CheckPosition();
            Debug.Log("Cut");
        }
    }

    private void CheckPosition()
    {
        var cubeNext = GameManager.Instance.CreatedNextCube;
        var cubeMain = GameManager.Instance.CreatedMainCube;

        pointMainCubePlusX = cubeMain.transform.position;
        pointMainCubePlusX.x += (cubeNext.transform.localScale.x / 2);
        
        pointMainCubeMinusX = cubeMain.transform.position;
        pointMainCubeMinusX.x -= (cubeNext.transform.localScale.x / 2);
        
        pointNextCubePlusX = cubeNext.transform.position;
        pointNextCubePlusX.x += (cubeNext.transform.localScale.x / 2);
        
        pointNextCubeMinusX = cubeNext.transform.position;
        pointNextCubeMinusX.x -= (cubeNext.transform.localScale.x / 2);
        
        Debug.Log($"pointMainCubePlusX - {pointMainCubePlusX}");
        Debug.Log($"pointMainCubeMinusX - {pointMainCubeMinusX}");
        Debug.Log("----------------------------");
        Debug.Log($"pointNextCubePlusX - {pointNextCubePlusX}");
        Debug.Log($"pointNextCubeMinusX - {pointNextCubeMinusX}");

        if (IsInsideCubeX((cubeNext.transform.position.x / 2), pointMainCubeMinusX.x, pointMainCubePlusX.x))
        {
            Cut();
            Debug.Log("Куб находится внутри диапозона по оси Х");
        }
        else
        {
            Debug.Log("Куб НЕЕ находится внутри диапозона по оси Х");
        }
    }

    private bool IsInsideCubeX(float positionTargetX, float pointMinusX, float pointPlusX)
    {
        return positionTargetX >= pointMinusX && positionTargetX <= pointPlusX;  
    }
    
    private void Cut()
    {
        var cubeNext = GameManager.Instance.CreatedNextCube;
        var cubeMain = GameManager.Instance.CreatedMainCube;
        
        if (IsInsideCubeX(pointNextCubePlusX.x, pointMainCubeMinusX.x, pointMainCubePlusX.x))
        {
            GameObject next;
            GameObject fall;
            
            float nextCubeScale = Math.Abs(pointNextCubePlusX.x - pointMainCubeMinusX.x);
            Vector3 nextCubePosition = (pointNextCubePlusX + pointMainCubeMinusX) / 2;
            nextCubePosition.y += nextCubePosition.y;
            next = Instantiate(GameManager.Instance.OriginalNextCubePrefab, nextCubePosition, Quaternion.identity);

            Vector3 newScale = next.transform.localScale;
            newScale.x = nextCubeScale;
            next.transform.localScale = newScale;

            // ------------------------------
            
            float fallCubeScale = Math.Abs(pointMainCubeMinusX.x - pointNextCubeMinusX.x);
            Vector3 fallCubePosition = (pointMainCubeMinusX + pointNextCubeMinusX) / 2;
            fallCubePosition.y += fallCubePosition.y;
            fall = Instantiate(GameManager.Instance.OriginalNextCubePrefab, fallCubePosition, Quaternion.identity);

            Vector3 newScaleFall = fall.transform.localScale;
            newScaleFall.x = fallCubeScale;
            fall.transform.localScale = newScaleFall;
            
            // -------------------------------
            Destroy(GameManager.Instance.CreatedNextCube);

            GameManager.Instance.CreatedNextCube = next;
            
            MoveCube moveCubeFall = fall.AddComponent<MoveCube>();
            moveCubeFall.StartMovingY();
        }
        else if (IsInsideCubeX(pointNextCubeMinusX.x, pointMainCubeMinusX.x, pointMainCubePlusX.x))
        {
            GameObject next;
            GameObject fall;
            
            float nextCubeScale = Math.Abs(pointNextCubeMinusX.x - pointMainCubePlusX.x);
            Vector3 nextCubePosition = (pointNextCubeMinusX + pointMainCubePlusX) / 2;
            nextCubePosition.y += nextCubePosition.y;
            next = Instantiate(GameManager.Instance.OriginalNextCubePrefab, nextCubePosition, Quaternion.identity);

            Vector3 newScale = next.transform.localScale;
            newScale.x = nextCubeScale;
            next.transform.localScale = newScale;

            // ------------------------------
            
            float fallCubeScale = Math.Abs(pointMainCubePlusX.x - pointNextCubePlusX.x);
            Vector3 fallCubePosition = (pointMainCubePlusX + pointNextCubePlusX) / 2;
            fallCubePosition.y += fallCubePosition.y;
            fall = Instantiate(GameManager.Instance.OriginalNextCubePrefab, fallCubePosition, Quaternion.identity);

            Vector3 newScaleFall = fall.transform.localScale;
            newScaleFall.x = fallCubeScale;
            fall.transform.localScale = newScaleFall;
            
            // -------------------------------
            Destroy(GameManager.Instance.CreatedNextCube);

            GameManager.Instance.CreatedNextCube = next;
            
            MoveCube moveCubeFall = fall.AddComponent<MoveCube>();
            moveCubeFall.StartMovingY();
        }
    }   
}