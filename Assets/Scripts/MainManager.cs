using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public Indestructable IndestructablePrefab;
    public ToughBrick ToughPrefab;

    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text BestScoreText;
    public GameObject GameOverText;

    private bool m_Started = false;
    private int m_Points;

    private bool m_GameOver = false;


    // Start is called before the first frame update
    void Start()
    {
        UpdateBestScoreText();

        SpawnLevel();
    }

    private void SpawnLevel()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 0, 1, 2, 2, 5, 7 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                switch (pointCountArray[i])
                {
                    case 0:
                    {
                        var brick = Instantiate(this.IndestructablePrefab, position, Quaternion.identity);
                        brick.PointValue = pointCountArray[i];
                        break;
                    }

                    case 7:
                    {
                        var brick = Instantiate(this.ToughPrefab, position, Quaternion.identity);
                        brick.PointValue = pointCountArray[i];
                        brick.onDestroyed.AddListener(AddPoint);
                        break;
                    }

                    default:
                    {
                        var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                        brick.PointValue = pointCountArray[i];
                        brick.onDestroyed.AddListener(AddPoint);
                        break;
                    }
                }
            }
        }
    }

    private void UpdateBestScoreText()
    {
        this.BestScoreText.text = "Best Score: " + GameManager.Instance.HighScoreName + " - " + GameManager.Instance.HighScore;
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
        ScoreText.text = $"{GameManager.Instance.PlayerName} - {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);

        if (m_Points > GameManager.Instance.HighScore)
        {
            GameManager.Instance.HighScore = m_Points;
            GameManager.Instance.HighScoreName = GameManager.Instance.PlayerName;
            GameManager.Instance.SaveHighScore();

            UpdateBestScoreText();
        }
    }
}
