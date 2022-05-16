using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GlobalSettings: MonoBehaviour 
{
    [Header("Players")]
    public Player PlayerOne;
    public Player PlayerTwo;    
    [Header("Numbers and Values")]
    public float CardPreviewTime = 1f;
    public float CardTransitionTime= 1f;
    public float CardPreviewTimeFast = 0.2f;
    public float CardTransitionTimeFast = 0.5f;
    [Header("Prefabs and Assets")]
    public GameObject EnemyCardPrefab;
    public GameObject EnemyIconPrefab;
    public GameObject TokenPrefab;
    public GameObject ExplosionPrefab;
    [Header("Other")]    
    public GameObject GameOverPanel;

    public static int PlayerOneWins;
    public static int PlayerOneLoses;
    public static int PlayerTwoWins;
    public static int PlayerTwoLoses;

    // public Dictionary<AreaPosition, Player> Players = new Dictionary<AreaPosition, Player>();
    
    // SINGLETON
    public static GlobalSettings Instance;

    void Awake()
    {
        Instance = this;
    }

    public bool CanControlAction()
    {
        bool PlayersTurn = (TurnManager.Instance.whoseTurn);
        bool NotDrawingAnyCards = !Command.CardDrawPending();
        return Tabletop.Instance.ControlsON && PlayersTurn && NotDrawingAnyCards;
    } 
}
