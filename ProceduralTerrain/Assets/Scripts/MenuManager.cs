using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainHolder;
    [SerializeField] private GameObject howToHolder;
    [SerializeField] private Button firstButton;
    [SerializeField] private Button backButton;
    
    /// <summary>
    /// sets the how-to menu active and sets the back button as selected
    /// </summary>
    public void ShowHowTo()
    {
        mainHolder.SetActive(false);
        howToHolder.SetActive(true);
        EventSystem.current.SetSelectedGameObject(backButton.gameObject);
    }

    /// <summary>
    /// sets the main menu as active and sets the first button as selected
    /// </summary>
    public void ShowMain()
    {
        mainHolder.SetActive(true);
        howToHolder.SetActive(false);
        EventSystem.current.SetSelectedGameObject(firstButton.gameObject);
    }
    
    private void OnEnable()
    {
        ShowMain();
    }
}