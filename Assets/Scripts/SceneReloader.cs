using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneReloader: MonoBehaviour {

    public void ReloadScene()
    {
        // Command has some static members, so let`s make sure that there are no commands in the Queue
        Debug.Log("Scene reloaded");
        // reset all card and creature IDs
        IDFactory.ResetIDs();
        IDHolder.ClearIDHoldersList();
        TokenLogic.TokensCreatedThisGame.Clear();
        EnemyLogic.EnemiesCreatedThisGame.Clear();
        CardLogic.CardsCreatedThisGame.Clear();
        TurnManager.EnemySpritesOne.Clear();
        TurnManager.TokenSpritesOne.Clear();
        Command.CommandQueue.Clear();
        Command.CommandExecutionComplete();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
