using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

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
        LoadGame();
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

    [System.Serializable]
    class SaveData
    {
        public int bestScore;
        public string bestScoreName;
        public bool bestScoreSet;
    }

    public void SaveGame()
    {
        SaveData data = new SaveData();
        data.bestScore = bestScore;
        data.bestScoreName = bestScoreName;
        data.bestScoreSet = bestScoreSet;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/datapersistence.json", json);
    }

    public void LoadGame()
    {
        string path = Application.persistentDataPath + "/datapersistence.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            bestScore = data.bestScore;
            bestScoreName = data.bestScoreName;
            bestScoreSet = data.bestScoreSet;
        }
    }
}
