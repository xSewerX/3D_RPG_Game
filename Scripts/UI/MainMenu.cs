using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject CreditsScreen, ControlsScreen;

  public void PlayButton()
    {
        SceneManager.LoadScene(1);
    }
    public void QuitButton()
    {
        Application.Quit();
    }

    public void CreditsButton()
    {
        CreditsScreen.SetActive(!CreditsScreen.activeSelf);
        ControlsScreen.SetActive(false);
    }
    public void ControlsButton()
    {
        ControlsScreen.SetActive(!ControlsScreen.activeSelf);
    }
}
