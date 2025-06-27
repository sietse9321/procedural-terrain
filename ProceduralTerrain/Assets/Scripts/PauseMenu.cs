using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseObject;
    [SerializeField] Button firstPauseButton;

    bool paused;
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7)) && !paused)
        {
            Cursor.visible = true;
            Time.timeScale = 0;
            pauseObject.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            paused = true;
            EventSystem.current.SetSelectedGameObject(firstPauseButton.gameObject);
        }
        else if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7)) && paused)
        {
            ResumeGame();
        }
    }
    public void ResumeGame()
    {
        Cursor.visible = false;
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        pauseObject.SetActive(false);
        paused = false;
    }
    
    public void OnHover(Image img)
    {
        img.color = new Color32(255, 255, 255, 255);
        img.transform.localScale = new Vector3(1.55f, 1.55f, 1.55f);
    }
    public void ExitHover(Image img)
    {
        img.color = new Color32(255, 255, 255, 200);
        img.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
    }
}
