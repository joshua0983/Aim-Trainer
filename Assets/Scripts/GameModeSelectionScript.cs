using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameModeSelectionScript : MonoBehaviour
{
    public button BackButton;
    // Start is called before the first frame update
    void Start()
    {
        BackButton.onClick.AddListener(BackButton);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void BackButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
