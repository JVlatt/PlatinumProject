using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Script;

public class MainMenu : MonoBehaviour
{
    public void Play(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void TestSound()
    {
        GameManager.GetManager()._soundManager.Play("test");
    }
}
