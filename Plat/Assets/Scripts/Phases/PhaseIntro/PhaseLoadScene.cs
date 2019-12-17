using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PhaseLoadScene : Phase
{
    public GameObject loadingScreen;
    public Image loadingImg;

    public override string BuildGameObjectName()
    {
        return "LoadScene";
    }

    public override void LaunchPhase()
    {
        StartCoroutine(LoadLevel());
        Peon.ResetStatic();
    }

    // Start is called before the first frame update
    void Start()
    {
        controlDuration = false;
    }

    IEnumerator LoadLevel()
    {
        AsyncOperation loader = SceneManager.LoadSceneAsync(1);
        loadingScreen.SetActive(true);
        while(!loader.isDone)
        {
            float progress = Mathf.Clamp01(loader.progress / .9f);
            loadingImg.fillAmount = progress;
            yield return null;
        }
    }
}
