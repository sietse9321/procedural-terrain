using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLoseMenu : MonoBehaviour
{
    [SerializeField] GameObject winMenu;
    [SerializeField] GameObject loseMenu;
    private void Start()
    {
        GameManager.BossDefeated += PlayerWin;
        GameManager.PlayerDefeated += PlayerLose;
    }
    private void PlayerWin()
    {
        winMenu.SetActive(true);
    }
    private void PlayerLose()
    {
        loseMenu.SetActive(true);
    }

    private void OnDisable()
    {
        GameManager.BossDefeated -= PlayerWin;
        GameManager.PlayerDefeated -= PlayerLose;
    }
}
