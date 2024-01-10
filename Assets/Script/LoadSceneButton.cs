using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneButton : MonoBehaviour
{
    public int sceneToLoad;

    public void LoadTargetScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
