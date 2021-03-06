using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{
    private MainManager mainManager;
    private int bestScore;
    public string currentPlayerName;
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;
    public Text BestScoreText;

    public Text ScoreText;
    public GameObject GameOverScreen;

    private bool m_Started = false;
    public int m_Points;

    private bool bestScoreInThisGame = false;

    private bool m_GameOver = false;

    void Start()
    {
        mainManager = GameObject.Find("Main Manager").GetComponent<MainManager>();
        bestScore = mainManager.bestScore;
        currentPlayerName = mainManager.playerName;
        MainManager.Instance.LoadGame();

        if (mainManager.bestScoreSet)
        {
            BestScoreText.text = "Best Score: " + mainManager.bestScoreName + ": " + bestScore;
        }

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = "Score: " + m_Points;
        if (m_Points > bestScore)
        {
            if (!bestScoreInThisGame)
            {
                bestScoreInThisGame = true;
            }
            BestScoreText.text = "<color=red>Best Score: " + currentPlayerName + ": " + m_Points + "</color>";
        }
    }

    public void GameOver()
    {

        if (bestScoreInThisGame)
        {
            mainManager.SetBestScore();
        }
        m_GameOver = true;
        GameOverScreen.SetActive(true);
        MainManager.Instance.SaveGame();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
