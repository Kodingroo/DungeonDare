using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using DG.Tweening;

public class TurnManager : MonoBehaviour {

    // PUBLIC FIELDS
    public bool GameLoaded = false;
    public static Dictionary<int, Sprite> EnemySpritesOne = new Dictionary<int, Sprite>();
    public static Dictionary<int, Sprite> TokenSpritesOne = new Dictionary<int, Sprite>();
    public int activatedCards = 0;
    public bool Passed = false;
    public GameObject passedText;
    public GameObject rulesScreen;
    public GameObject welcomeScreen;
    private bool rulesScreenState = false;
    public bool gameStarted = false;

    // Singleton
    public static TurnManager Instance;
    public Button PassButton;

    private Player _whoseTurn;
    public Player whoseTurn
    {
        get
        {
            return _whoseTurn;
        }

        set
        {
            _whoseTurn = value;

            // GlobalSettings.Instance.EnableEndTurnButtonOnStart(_whoseTurn);

            TurnMaker tm = whoseTurn.GetComponent<TurnMaker>();
            // player`s method OnTurnStart() will be called in tm.OnTurnStart();
            tm.OnTurnStart();
        }
    }

    private bool _isPlayerAI;
    public bool IsPlayerAI
    {
        get
        {
            return _isPlayerAI;
        }

        set
        {
            _isPlayerAI = value;
        }
    }


    // METHODS
    void Awake()
    {
        PassButton.interactable = true;
        Instance = this;
    }


    void Start()
    {
        if (gameStarted)
        {
            OnGameStart();
        } else 
        {
            gameStarted = true;
            welcomeScreen.SetActive(true);
        }
    }

    public void StartGame()
    {
        welcomeScreen.SetActive(false);
        OnGameStart();
    }

    public void OnGameStart()
    {
        foreach (Player p in Player.Players)
        {
            p.LoadCharacterInfoFromAsset();
            // p.TransmitInfoAboutPlayerToVisual();
            // move both portraits to the center
            // p.PArea.Portrait.transform.position = p.PArea.InitialPortraitPosition.position;
        }
        new PopulateTokensCommand().AddToQueue();
        // determine who starts the game.
        int rnd = Random.Range(0,2);  // 2 is exclusive boundary
        Player whoGoesFirst = Player.Players[rnd];
        new StartATurnCommand(whoGoesFirst).AddToQueue();
        CheckIfPlayerAI();
        GameLoaded = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            EndTurn();
        if (Input.GetKeyDown(KeyCode.D))
            Tabletop.Instance.DrawACard();
    }

    public void CheckIfPlayerAI() 
    {
        bool AIComponentPresent = false;
        if (whoseTurn.GetComponent<AITurnMaker>() != null)
        {
            AIComponentPresent = true;
        }
        Debug.Log("Is Player AI? "+AIComponentPresent);
        IsPlayerAI = AIComponentPresent;
    }
    
    public void EndTurn()
    {
        Tabletop.Instance.ControlsON = false;
        whoseTurn.OnTurnEnd();

        new StartATurnCommand(whoseTurn.otherPlayer).AddToQueue();
    }

    public void ShowRulesScreen()
    {
        rulesScreen.SetActive(!rulesScreenState);
        rulesScreenState = !rulesScreenState;
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    public void OnPass()
    {
        Debug.Log("Whose Turn On Pass: "+whoseTurn);
        passedText.SetActive(true);
        PassButton.interactable = false;
        Tabletop.Instance.ControlsON = false;
        Passed = true;
        int activatedCards = 0;
        int cardsOnTable = Tabletop.Instance.dungeonVisual.EnemiesOnTable.Count;

        whoseTurn.playerPortraitVisual.PortraitFrame.color = new Color32(255,255,225,255); 
        whoseTurn.otherPlayer.playerPortraitVisual.PortraitFrame.color =  new Color32(251,28,28,255);       
        // foreach (KeyValuePair<int, EnemyLogic> enemyEle in EnemyLogic.EnemiesCreatedThisGame)
        // {
        //     // Debug.Log("GlobalSettings.dungeonVisual.EnemiesOnTable.Count: "+Tabletop.Instance.dungeonVisual.EnemiesOnTable.Count);
        //     // Debug.Log("EnemyLogic.EnemiesCreatedThisGame.Count: "+EnemyLogic.EnemiesCreatedThisGame.Count);
        //     bool tokenActivated = false;

        //     foreach (KeyValuePair<int, TokenLogic> tokenEle in TokenLogic.TokensCreatedThisGame)
        //     {

        //         OneIconManager iconManager = IDHolder.GetGameObjectWithID(enemyEle.Value.UniqueEnemyID).GetComponent<OneIconManager>();
        //         OneTokenManager tokenManager = IDHolder.GetGameObjectWithID(tokenEle.Value.UniqueTokenID).GetComponent<OneTokenManager>();
        //         if (iconManager.IconWeaknessOneImage.sprite == tokenManager.TokenStrengthOneImage.sprite)
        //         {
        //             TokenLogic.TokensCreatedThisGame[tokenEle.Value.UniqueTokenID].TokenPowersInPlay(enemyEle.Value);
        //             tokenActivated = true;
        //             activatedCards += 1;
        //         }
        //     }
        //     if (tokenActivated != true)
        //         {
        //             activatedCards += 1;
        //             enemyEle.Value.GoFace();
        //         }
        //     if (whoseTurn.otherPlayer.Health <= 0)
        //     {
        //         whoseTurn.otherPlayer.Die();
        //         break;
        //     }
        // }

        StoreSpriteValuesInDictionaries();

        foreach (KeyValuePair<int, Sprite> spriteEle in EnemySpritesOne)
        {
            bool tokenActivated = false;
            if (TokenSpritesOne.ContainsValue(spriteEle.Value))
            {
                Sprite sprite = spriteEle.Value;
                var tokenKey = TokenSpritesOne.FirstOrDefault(x => x.Value == sprite).Key;
                TokenLogic.TokensCreatedThisGame[tokenKey].TokenPowersInPlay(EnemyLogic.EnemiesCreatedThisGame[spriteEle.Key]);
                tokenActivated = true;
                activatedCards += 1;
            }
            if (tokenActivated != true)
            {
                activatedCards += 1;
                EnemyLogic.EnemiesCreatedThisGame[spriteEle.Key].GoFace();
            }
            if (whoseTurn.otherPlayer.Health <= 0)
            {
                whoseTurn.otherPlayer.Die();
                break;
            }
        }
        if (activatedCards == cardsOnTable && whoseTurn.otherPlayer.Health > 0)
        {
            Debug.Log("You Won!");
            whoseTurn.otherPlayer.Win();
        }
    }

    private void StoreSpriteValuesInDictionaries()
    {
        foreach (KeyValuePair<int, EnemyLogic> enemyEle in EnemyLogic.EnemiesCreatedThisGame)
        {
            Debug.Log("StoreSpriteValuesInDictionaries: "+enemyEle.Value.UniqueEnemyID);
            OneIconManager iconManager = IDHolder.GetGameObjectWithID(enemyEle.Value.UniqueEnemyID).GetComponent<OneIconManager>();
            EnemySpritesOne.Add(enemyEle.Value.UniqueEnemyID, iconManager.IconWeaknessOneImage.sprite);
        }

        foreach (GameObject tokenEle in Tabletop.Instance.tokenVisual.TokensInGame)
        {
            OneTokenManager tokenManager = tokenEle.GetComponent<OneTokenManager>();
            TokenSpritesOne.Add(tokenEle.GetComponent<IDHolder>().UniqueID, tokenManager.TokenStrengthImage.sprite);                  
        } 
    }   
}

