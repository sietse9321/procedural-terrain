using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;
    public TMP_InputField seedInput;
    public string seed;

    bool loading;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            seedInput = FindObjectOfType<TMP_InputField>();
            seed = seedInput.text;
        }
        
    }

    public void GameStart()
    {
        SceneManager.LoadSceneAsync("SampleScene");
    }

    public void LoadMenu()
    {
        SceneManager.LoadSceneAsync("MenuScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}