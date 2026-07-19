using Unity.VectorGraphics;
using UnityEngine;

public class init : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.GetComponent<SceneController>().playScene("MainMenuScene");    
    }

}
