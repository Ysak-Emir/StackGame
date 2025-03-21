using UnityEngine;

public class CuttingPoints : MonoBehaviour
{
    private struct CubePoints
    {
        public Vector3 Minus;
        public Vector3 Plus;
    }

    private CubePoints pointMainCubeX, pointMainCubeZ, pointNextCubeX, pointNextCubeZ;

    private GameObject cubeNextCheck, cubeMainCheck;
    
    private void CutPoints()
    {
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
    }
    
}
