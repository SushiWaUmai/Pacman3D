﻿using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ScriptableObjectArchitecture;

public class UIManager : MonoBehaviour
{
    [SerializeField] private IntVariable totalScore;
    [SerializeField] private IntVariable pacmanLives;
    [SerializeField] private GameEvent OnGameClear;
    [SerializeField] private GameEvent OnPauseGame;
    [SerializeField] private BoolVariable canInteract;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI totalScoreDisplay;
    [SerializeField] private Sprite pacmanLifeIcon;
    [SerializeField] private GameObject InGameUI;
    [SerializeField] private Transform pacmanLifeDisplayHolder;

    [Header("Game Over")]
    [SerializeField] private Animator gameOverAnimation;

    [Header("Game Clear")]
    [SerializeField] private Animator gameClearAnimation;
    [SerializeField] private TextMeshProUGUI gameClearScoreDisplay;

    [Header("Pause Menu")]
    [SerializeField] private GameObject pauseMenu;

    private bool IsPaused => Time.timeScale == 0;

    private void Start()
    {
        totalScore.Value = 0;
        SetupLives();
        SetCursor(false);

        totalScore.AddListener(UpdateScore);
        pacmanLives.AddListener(UpdateLives);
        OnGameClear.AddListener(GameClear);
        OnPauseGame.AddListener(TooglePause);
    }

    private void OnDestroy()
    {
        totalScore.RemoveListener(UpdateScore);
        pacmanLives.RemoveListener(UpdateLives);
        OnGameClear.RemoveListener(GameClear);
        OnPauseGame.RemoveListener(TooglePause);
    }

    private void SetupLives()
    {
        float pacmanHolderSize = pacmanLifeDisplayHolder.GetComponent<RectTransform>().sizeDelta.y;
        Vector2 rtSizeDelta = new Vector2(pacmanHolderSize, pacmanHolderSize);

        for (int i = 0; i < pacmanLives.Value; i++)
        {
            GameObject go = new GameObject("Pacman Live");
            go.transform.Rotate(Vector3.forward, -90);
            go.transform.parent = pacmanLifeDisplayHolder;

            Image img = go.AddComponent<Image>();
            img.sprite = pacmanLifeIcon;
            img.color = new Color(1, 1, 0, 1);

            RectTransform rt = go.GetComponent<RectTransform>();
            rt.sizeDelta = rtSizeDelta;
            rt.localScale = Vector3.one;
        }
    }

    // TODO: Can't add lives, add it when needed
    private void UpdateLives()
    {
        for (int i = 0; i < pacmanLifeDisplayHolder.childCount; i++)
        {
            if (i >= pacmanLives.Value)
                pacmanLifeDisplayHolder.GetChild(i).gameObject.SetActive(false);
        }

        if (pacmanLives == 0)
            GameOver();
    }

    private void UpdateScore()
    {
        totalScoreDisplay.text = totalScore.Value.ToString();
    }

    private void GameOver()
    {
        InGameUI.SetActive(false);
        gameOverAnimation.gameObject.SetActive(true);
        gameOverAnimation.SetTrigger("Shutdown");

        SetCursor(true);
    }

    private void GameClear()
    {
        InGameUI.SetActive(false);
        gameClearAnimation.gameObject.SetActive(true);
        gameClearAnimation.SetTrigger("GameClear");

        gameClearScoreDisplay.text = $"Score: {totalScore.Value}";

        SetCursor(true);
    }

    private void SetCursor(bool set)
    {
        Cursor.lockState = set ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = set;
    }

    private void TooglePause()
    {
        Time.timeScale = IsPaused ? 1 : 0;
        SetCursor(IsPaused);
        pauseMenu.SetActive(IsPaused);
        canInteract.Value = !IsPaused;
    }
}
