using UnityEngine;

public class ExitGame : MonoBehaviour
{
    public void QuitGame()
    {
        Destroy(GameManager.Instance.gameObject);
        Application.Quit();
    }
}
