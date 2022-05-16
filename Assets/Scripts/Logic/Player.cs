using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour, ICharacter
{
    public int PlayerID;
    public PlayerAsset playerAsset;
    public PlayerPortraitVisual playerPortraitVisual;

    // a static array that will store both players, should always have 2 players
    public static Player[] Players;

    // PROPERTIES 
    // this property is a part of interface ICharacter
    public int ID
    {
        get{ return PlayerID; }
    }

    // opponent player
    public Player otherPlayer
    {
        get
        {
            if (Players[0] == this)
                return Players[1];
            else
                return Players[0];
        }
    }

    private int health;
    public int Health
    {
        get { return health;}
        set
        {
            // if (value > playerAsset.BaseHealth)
            //     health = playerAsset.BaseHealth;
            // else
                health = value;
            // if (value <= 0)
            //     Die(); 
        }
    }

    // ALL METHODS
    void Awake()
    {
        // find all scripts of type Player and store them in Players array
        // (we should have only 2 players in the scene)
        Players = GameObject.FindObjectsOfType<Player>();
        PlayerID = IDFactory.GetUniqueID();
        IDHolder id = playerPortraitVisual.gameObject.AddComponent<IDHolder>();
        id.UniqueID = ID;        
    }

    public virtual void OnTurnStart()
    {
        Debug.Log("In ONTURNSTART for "+ gameObject.name);
    }

    public void OnTurnEnd()
    {
        Debug.Log("In ONTURNEND for "+ gameObject.name);
        GetComponent<TurnMaker>().StopAllCoroutines();
    }

    // STUFF THAT OUR PLAYER CAN DO


    // FOR TESTING ONLY
    void Update()
    {
        
    }

    // METHODS TO PLAY CREATURES 
    // 1st overload - by ID
    // public void PlayACreatureFromHand(int UniqueID, int tablePos)
    // {
    //     PlayACreatureFromHand(CardLogic.CardsCreatedThisGame[UniqueID], tablePos);
    // }

    // // 2nd overload - by logic units
    // public void PlayACreatureFromHand(CardLogic playedCard, int tablePos)
    // {

    //     // Debug.Log("Mana Left after played a creature: " + ManaLeft);
    //     // create a new creature object and add it to Table
    //     CreatureLogic newCreature = new CreatureLogic(this, playedCard.ca);
    //     table.CreaturesOnTable.Insert(tablePos, newCreature);
    //     // 
    //     new PlayACreatureCommand(playedCard, this, tablePos, newCreature.UniqueCreatureID).AddToQueue();
    //     // cause battlecry Effect
    //     if (newCreature.effect != null)
    //         newCreature.effect.WhenACreatureIsPlayed();
    //     // remove this card from hand
    //     hand.CardsInHand.Remove(playedCard);
    //     HighlightPlayableCards();
    // }

    public void Die()
    {
        // game over
        // block both players from taking new moves 
        Tabletop.Instance.ControlsON = false;
        // otherPlayer.t.ControlsON = false;
        if (TurnManager.Instance.GameLoaded)
            {
                if (this.ID == 1)
                {
                    GlobalSettings.PlayerOneLoses +=1;
                    new GameOverCommand(this, "You have lost!").AddToQueue();
                } else if (this.ID == 2)
                {
                    GlobalSettings.PlayerTwoLoses +=1;
                    new GameOverCommand(this, "The AI has Died!").AddToQueue();
                }
                // new ShowMessageCommand($"Player {this.ID}\nHas Died", 2.0f).AddToQueue();
                Debug.Log($"Player {this.ID} is Dead");
            }

    }

    public void Win()
    {
        if (this.ID == 1)
        {
            GlobalSettings.PlayerOneWins +=1;
            new GameOverCommand(this, "You have WON!").AddToQueue();
        } else if (this.ID == 2)
        {
            GlobalSettings.PlayerTwoWins +=1;
            new GameOverCommand(this, "The AI has WON!").AddToQueue();
        }
    }

    // // START GAME METHODS
    public void LoadCharacterInfoFromAsset()
    {
        Health = playerAsset.BaseHealth;
        // change the visuals for portrait, hero power, etc...
        // t.Portrait.playerAsset = playerAsset;
        // t.Portrait.ApplyLookFromAsset();
    }

    // public void TransmitInfoAboutPlayerToVisual()
    // {
    //     t.Portrait.gameObject.AddComponent<IDHolder>().UniqueID = PlayerID;
    //     // if (GetComponent<TurnMaker>() is AITurnMaker)
    //     // {
    //     //     // turn off turn making for this character
    //     //     t.AllowedToControlThisPlayer = false;
    //     // }
    //     // else
    //     // {
    //     //     // allow turn making for this character
    //         t.AllowedToControlThisPlayer = true;
    //     // }
    // }
       
        
}
