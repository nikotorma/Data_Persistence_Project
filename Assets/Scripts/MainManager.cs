
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    [Header("UI")]
    [SerializeField] private Text ScoreText;        // Canvas -> ScoreText
    [SerializeField] private Text HighScoreText;    // Canvas -> ScoreText(1) (näytetään parhaat)
    [SerializeField] private Text NameText;         // Canvas -> NameText

    [Header("Game Over UI")]
    [SerializeField] private GameObject GameOverText;

    private string playerName = "Player";
    private int m_Points = 0;
    private bool m_Started = false;
    private bool m_GameOver = false;

    // High score data
    private int highScore = 0;
    private string bestPlayerName = "Player";

    void Start()
    {
        // Lataa high score tiedostosta
        LoadHighScore();

        // Hae nykyinen pelaajan nimi PlayerPrefsistä
        playerName = PlayerPrefs.GetString("PlayerName", "Player");

        // Päivitä UI heti
        if (NameText != null) NameText.text = playerName;
        if (ScoreText != null) ScoreText.text = $"Score: {m_Points}";
        UpdateHighScoreUI();

        // --- Pelin alustus (tiilet jne.) ---
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
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        if (ScoreText != null) ScoreText.text = $"Score: {m_Points}";

        // Päivitä high score lennossa
        if (m_Points > highScore)
        {
            highScore = m_Points;
            bestPlayerName = playerName;
            UpdateHighScoreUI();
            // Voit halutessasi tallentaa heti:
            // SaveHighScore();
        }
    }

    public void GameOver()
    {
        m_GameOver = true;
        if (GameOverText != null) GameOverText.SetActive(true);

        // Tallennetaan high score (aina ok, ylikirjoittaa samalla parhaimman)
        SaveHighScore();
    }

    private void UpdateHighScoreUI()
    {
        if (HighScoreText != null)
        {
            HighScoreText.text = $"Best: {bestPlayerName} score: {highScore}";
        }
    }

    // --- Tallennus ja lataus JSON:iin ---
    [System.Serializable]
    class SaveData
    {
        public int highScore;
        public string bestPlayerName;
    }

    public void SaveHighScore()
    {
        try
        {
            SaveData data = new SaveData
            {
                highScore = highScore,
                bestPlayerName = bestPlayerName
            };

            string json = JsonUtility.ToJson(data);
            string path = Path.Combine(Application.persistentDataPath, "savefile.json");
            File.WriteAllText(path, json);
            // Debug.Log($"HighScore saved to: {path}");
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"SaveHighScore failed: {e.Message}");
        }
    }

    public void LoadHighScore()
    {
        try
        {
            string path = Path.Combine(Application.persistentDataPath, "savefile.json");
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                SaveData data = JsonUtility.FromJson<SaveData>(json);
                if (data != null)
                {
                    highScore = data.highScore;
                    bestPlayerName = data.bestPlayerName;
                }
            }
            else
            {
                highScore = 0;
                bestPlayerName = "Player";
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"LoadHighScore failed: {e.Message}");
            highScore = 0;
            bestPlayerName = "Player";
        }
    }

    // Valinnainen: testityökalu
    public void ClearHighScore()
    {
        highScore = 0;
        bestPlayerName = "Player";
        SaveHighScore();
        UpdateHighScoreUI();
    }
}

