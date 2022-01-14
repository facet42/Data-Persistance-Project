using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public string PlayerName = string.Empty;

    public string HighScoreName;
    public int HighScore = -1;

    private string saveFile;

    public int Score { get; private set; }

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);

            saveFile = Path.Combine(Application.persistentDataPath, "config.json");
            this.LoadHighScore();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void SaveHighScore()
    {
        Debug.Log("Saving to " + saveFile);

        var saveData = new SaveData() { HighScore = this.HighScore, HighScorePlayer = this.HighScoreName };

        var json = JsonUtility.ToJson(saveData);
        File.WriteAllText(saveFile, json);
    }

    public void LoadHighScore()
    {
        Debug.Log("Loading from " + saveFile);

        if (File.Exists(saveFile))
        {
            var json = File.ReadAllText(saveFile);
            var saveData = JsonUtility.FromJson<SaveData>(json);

            this.HighScore = saveData.HighScore;
            this.HighScoreName = saveData.HighScorePlayer;
        }
        else
        {
            this.HighScoreName = "Mr Magoo";
            this.HighScore = 0;
        }
    }

    public void AddScore(int points)
    {
        this.Score += points;
    }

    public void ResetScore()
    {
        this.Score = 0;
    }

    struct SaveData
    {
        public int HighScore;
        public string HighScorePlayer;
    }
}
