using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MessageManager : MonoBehaviour 
{
    public Text MessageText;
    public GameObject MessagePanel;

    public Text GameOverText;
    public Text PlayerOneScoreText;
    public Text PlayerTwoScoreText;
    public GameObject GameOverPanel;

    public static MessageManager Instance;

    void Awake()
    {
        Instance = this;
        MessagePanel.SetActive(false);
    }

    public void ShowMessage(string Message, float Duration)
    {
        StartCoroutine(ShowMessageCoroutine(Message, Duration));
    }

    IEnumerator ShowMessageCoroutine(string Message, float Duration)
    {
        MessageText.text = Message;
        MessagePanel.SetActive(true);

        yield return new WaitForSeconds(Duration);

        MessagePanel.SetActive(false);
        Command.CommandExecutionComplete();
    }

    public void GameOverMessage(Player player, string message)
    {
        TurnManager.Instance.passedText.SetActive(false);
        int PlayerOneWins = GlobalSettings.PlayerOneWins;
        int PlayerOneLoses = GlobalSettings.PlayerOneLoses;
        int PlayerTwoWins = GlobalSettings.PlayerTwoWins;
        int PlayerTwoLoses = GlobalSettings.PlayerTwoLoses;
        GameOverText.text = message;
        PlayerOneScoreText.text = $"You\nWins: {PlayerOneWins}\nLoses: {PlayerOneLoses}";
        PlayerTwoScoreText.text = $"AI\nWins: {PlayerTwoWins}\nLoses: {PlayerTwoLoses}";
        GameOverPanel.SetActive(true);        
        
    }

    // TEST PURPOSES ONLY
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Y))
        //     ShowMessage("Your Turn", 3f);
        
        // if (Input.GetKeyDown(KeyCode.E))
        //     ShowMessage("Enemy Turn", 3f);
    }
}
