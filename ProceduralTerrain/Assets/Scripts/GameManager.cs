using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;
    
    public Player player;
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
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            seedInput = FindObjectOfType<TMP_InputField>();
            seed = seedInput.text;
        }

        if (SceneManager.GetActiveScene().buildIndex == 1 && player == null)
        {
            player = FindObjectOfType<Player>();
            IHealth health = player.GetComponent<IHealth>();
            if (health != null)
            {
                if (health is Health h)
                    h.OnDeath += OnPlayerDeath;
                else if (health is ArmouredHealth ah)
                    ah.OnDeath += OnPlayerDeath;
            }

        }
    }
    private void OnPlayerDeath()
    {
        Debug.Log("Game Over: Player is dead!");
        // Handle game over logic here (UI, scene, etc.)
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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