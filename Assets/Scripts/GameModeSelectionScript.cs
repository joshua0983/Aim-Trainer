using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class GameModeSelectionScript : MonoBehaviour
{
    public Button Flicking;
    public Button EnemyShooting;
    public Button BackButton; 
    public Toggle toggle;
    public static string GameMode;

    public static bool StrafeEnabled { get; private set; }
    void Start()
    {
        Flicking.onClick.AddListener(FlickingButton);
        BackButton.onClick.AddListener(GameModeSelectionBackButton); 
        EnemyShooting.onClick.AddListener(EnemyShootingButton);
        toggle.onValueChanged.AddListener(onToggleValueChanged);
    }



    void GameModeSelectionBackButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void FlickingButton()
    {
        SceneManager.LoadScene("Difficulty");
        GameMode = "Flicking";
    }
    public void EnemyShootingButton()
    {
        SceneManager.LoadScene("Difficulty");
        GameMode = "EnemyShooting";
    }

    public void onToggleValueChanged(bool value)
    {
        StrafeEnabled = value;
    }
}