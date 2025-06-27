using TMPro;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;

    public Player player;
    public EnemyBoss bossEnemy;
    public TMP_InputField seedInput;
    public string seed;
    
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
        SceneManagement();
    }

    private void SceneManagement()
    {
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 0:
                seedInput = FindObjectOfType<TMP_InputField>();
                if (seedInput) seed = seedInput.text;
                break;
            case 1:
                if (player) return;
                player = FindObjectOfType<Player>();
                bossEnemy = FindObjectOfType<EnemyBoss>();
                IHealth bHealth = bossEnemy.GetComponent<IHealth>();
                if (bHealth != null)
                {
                    if (bHealth is Health h)
                        h.OnDeath += OnBossDeath;
                    else if (bHealth is ArmouredHealth ah)
                        ah.OnDeath += OnBossDeath;
                }
                IHealth health = player.GetComponent<IHealth>();
                if (health != null)
                {
                    if (health is Health h)
                        h.OnDeath += OnPlayerDeath;
                    else if (health is ArmouredHealth ah)
                        ah.OnDeath += OnPlayerDeath;
                }

                break;
        }
    }

    public static Action PlayerDefeated;

    private void OnPlayerDeath()
    {
        Debug.Log("Game Over: Player is dead!");
        // Handle game over logic here (UI, scene, etc.)
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PlayerDefeated?.Invoke();
    }
    public static Action BossDefeated;
    private void OnBossDeath()
    {
        Debug.Log("Game Over: Boss is dead!");
        // Handle game over logic here (UI, scene, etc.)
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        BossDefeated?.Invoke();
    }

    public void GameStart()
    {
        SceneManager.LoadSceneAsync("SampleScene");
    }

    public void LoadMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync("MenuScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}