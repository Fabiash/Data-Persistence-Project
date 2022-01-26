using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;

    [SerializeField] GameObject placeholder;
    [SerializeField] GameObject playerNameField;
    public string playerName;

    private GameManager gameManager;
    public int bestScore;
    public string bestScoreName;
    public bool bestScoreSet;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartGame()
    {
        if (playerNameField.GetComponent<Text>().text == "")
        {
            placeholder.GetComponent<Text>().color = Color.red;
        }
        else
        {
            playerName = playerNameField.GetComponent<Text>().text;
            SceneManager.LoadScene(1);
        }
        
    }

    public void SetBestScore()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        if (!bestScoreSet)
        {
            bestScoreSet = true;
        }
        bestScore = gameManager.m_Points;
        bestScoreName = gameManager.currentPlayerName;
    }
}
