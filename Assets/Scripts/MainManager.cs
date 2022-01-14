using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public GameObject Paddle;

    public Brick BrickPrefab;
    public Indestructable IndestructablePrefab;
    public ToughBrick ToughPrefab;
    public PowerUp PowerUpPrefab;

    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text BestScoreText;
    public GameObject GameOverText;

    private bool m_Started = false;

    private bool m_GameOver = false;

    private static int currentLevel = 0;

    public int RemainingBricks { get; private set; }    // ENCAPSULATION

    private readonly List<int>[] pointCountArray = new List<int>[] { new List<int>(new[] { 1, 1, 2, 2, 5, 5 }), new List<int>(new[] { 0, 1, 2, 2, 5, 7 }) };

    private Vector3 ballPosition;

    // Start is called before the first frame update
    void Start()
    {
        UpdateBestScoreText();
        UpdateScoreText();

        ballPosition = Ball.gameObject.transform.localPosition;

        SpawnLevel(currentLevel);
    }

    public void SpawnRandomPowerup(Vector3 position)
    {
        Instantiate(this.PowerUpPrefab, position, this.PowerUpPrefab.transform.rotation);
    }

    private void SpawnLevel(int level)
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        //m_Started = false;
        //Ball.gameObject.transform.SetParent(Paddle.transform);
        //Ball.gameObject.transform.localPosition = ballPosition;
        //Ball.gameObject.transform.SetParent(null);
        //Ball.velocity = Vector3.zero;
        //Ball.angularVelocity = Vector3.zero;

        this.RemainingBricks = 0;
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                switch (pointCountArray[level][i])
                {
                    case 0:
                    {
                        var brick = Instantiate(this.IndestructablePrefab, position, Quaternion.identity);
                        brick.PointValue = pointCountArray[level][i];
                        break;
                    }

                    case 7:
                    {
                        var brick = Instantiate(this.ToughPrefab, position, Quaternion.identity);
                        brick.PointValue = pointCountArray[level][i];
                        brick.onDestroyed.AddListener(GameManager.Instance.AddScore);
                        break;
                    }

                    default:
                    {
                        var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                        brick.PointValue = pointCountArray[level][i];
                        brick.onDestroyed.AddListener(GameManager.Instance.AddScore);
                        break;
                    }
                }

                RemainingBricks++;
            }
        }
    }

    public void ReduceBrickCount()
    {
        this.RemainingBricks--;

        if (this.RemainingBricks <= 0)
        {
            currentLevel = (currentLevel + 1) % 2;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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

        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        ScoreText.text = $"{GameManager.Instance.PlayerName} - {GameManager.Instance.Score}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);

        if (GameManager.Instance.Score > GameManager.Instance.HighScore)
        {
            GameManager.Instance.HighScore = GameManager.Instance.Score;
            GameManager.Instance.HighScoreName = GameManager.Instance.PlayerName;
            GameManager.Instance.SaveHighScore();

            UpdateBestScoreText();
        }

        GameManager.Instance.ResetScore();

        currentLevel = 0;
    }
}
