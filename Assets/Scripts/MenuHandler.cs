using System.Collections;
using System.Collections.Generic;

using UnityEditor;

using UnityEngine;
using UnityEngine.SceneManagement;

using TMPro;

public class MenuHandler : MonoBehaviour
{
    [SerializeField] GameObject playerName;
    [SerializeField] TextMeshProUGUI highScore;
    [SerializeField] TextMeshProUGUI profileName;
    TMP_InputField playerNameInput;

    // Start is called before the first frame update
    void Start()
    {
        this.playerNameInput = this.playerName.GetComponent<TMP_InputField>();
        this.highScore.text = "High Score: " + GameManager.Instance.HighScoreName + " - " + GameManager.Instance.HighScore;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewGame()
    {
        SceneManager.LoadScene("main");
    }

    public void Quit()
    {
        GameManager.Instance.SaveHighScore();

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    public void NameEditEnded()
    {
        Debug.Log("Entered: " + this.playerNameInput.text);
        GameManager.Instance.PlayerName = this.playerNameInput.text;
        this.profileName.text = GameManager.Instance.PlayerName;
    }

    public void NameValueChanged()
    {
        Debug.Log("Update: " + this.playerNameInput.text);
        this.profileName.text = this.playerNameInput.text;
    }
}
