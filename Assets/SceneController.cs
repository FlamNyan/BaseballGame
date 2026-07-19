using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public GameObject eventSystem;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        DontDestroyOnLoad(eventSystem);
    }

    public void unLoadScene(string sceneName)
    {
        SceneManager.UnloadSceneAsync(sceneName);   
    }
}
