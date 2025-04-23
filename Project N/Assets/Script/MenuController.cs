using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [Header("Saved To Load")]
    public string _newGame;
    private string savedToLoad;
    [SerializeField] private GameObject noSavedGameDialog = null;

    public void NewGameDialogYes()
    {
        SceneManager.LoadScene(_newGame);
    }

    public void LoadGameDialogYes()
    {
        if (PlayerPrefs.HasKey("SavedGame"))
        {
            savedToLoad = PlayerPrefs.GetString("SavedGame");
            SceneManager.LoadScene(savedToLoad);
        }
        else
        {
            noSavedGameDialog.SetActive(true);
        }
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}