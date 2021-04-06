﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ScriptableObjectArchitecture;

public class UIManager : MonoBehaviour
{
    [SerializeField] private IntVariable totalScore;
    [SerializeField] private IntVariable pacmanLives;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI totalScoreDisplay;
    [SerializeField] private Sprite pacmanLifeIcon;
    [SerializeField] private GameObject InGameUI;
    [SerializeField] private Transform pacmanLifeDisplayHolder;
    [SerializeField] private Animator gameOverAnimation;

    private void Start()
    {
        totalScore.Value = 0;
        totalScore.AddListener(UpdateScore);

        SetupLives();
        pacmanLives.AddListener(UpdateLives);

        SetCursor(false);
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
        //Debug.Log("Game Over");
        InGameUI.SetActive(false);
        gameOverAnimation.gameObject.SetActive(true);
        gameOverAnimation.SetTrigger("Shutdown");

        SetCursor(true);

        totalScore.RemoveListener(UpdateScore);
        pacmanLives.RemoveListener(UpdateLives);
    }

    private void SetCursor(bool set)
    {
        Cursor.lockState = set ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = set;
    }
}