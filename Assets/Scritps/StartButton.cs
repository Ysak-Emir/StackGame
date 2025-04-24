using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class StartButton : MonoBehaviour
{
    [FormerlySerializedAs("SceneName")] [SerializeField] private string _sceneName;
    
    public void OnButtonClick()
    {
        LoadScene();
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(_sceneName);
        // if(GameManager.Instance.gameObject)
        //     Destroy(GameManager.Instance.gameObject);
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
