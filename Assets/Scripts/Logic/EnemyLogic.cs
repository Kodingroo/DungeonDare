using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

[System.Serializable]
public class EnemyLogic: IIdentifiable 
{
    // PUBLIC FIELDS
    public Player owner;
    public EnemyCardAsset ca;
    public int UniqueEnemyID;
    public static Dictionary<int, EnemyLogic> EnemiesCreatedThisGame = new Dictionary<int, EnemyLogic>();    

    // PROPERTIES
    // property from ICharacter interface
    public int ID
    {
        get{ return UniqueEnemyID; }
    }

    // the basic health that we have in CardAsset
    private int _baseHealth;
    // health with all the current buffs taken into account
    public int BaseHealth
    {
        get{ return _baseHealth;}
    }    
    // current health of this creature
    private int _health;
    public int Health
    {
        get{ return _health; }

        set
        {
            if (value > BaseHealth)
                _health = BaseHealth;
            else if (value <= 0)
                Die();
            else
                _health = value;
        }
    }    

    // returns true if we can attack with this Enemy now
    public bool CanAttack
    {
        get
        {
            bool ownersTurn = (TurnManager.Instance.whoseTurn == owner);
            return (ownersTurn && (AttacksLeftThisTurn > 0));
        }
    }

    // property for Attack
    private int _baseAttack;
    public int Attack
    {
        get{ return _baseAttack; }
    }
    public int AttacksLeftThisTurn
    {
        get;
        set;
    }

    // CONSTRUCTOR
    public EnemyLogic(Player owner, EnemyCardAsset ca)
    {
        this.ca = ca;
        _baseHealth = owner.Health;
        _baseAttack = ca.Attack;
        this.owner = owner;     
        UniqueEnemyID = IDFactory.GetUniqueID();
        EnemiesCreatedThisGame.Add(UniqueEnemyID, this);
    }

    // METHODS

    public void Die()
    {   
        // Tabletop.Instance.dungeonVisual.EnemiesOnTable.Remove(this);

        new EnemyDieCommand(UniqueEnemyID).AddToQueue();
    }

    public void GoFace()
    {
        int targetHealthAfter = TurnManager.Instance.whoseTurn.otherPlayer.Health -= Attack;

        new EnemyAttackCommand(TurnManager.Instance.whoseTurn.otherPlayer.PlayerID, UniqueEnemyID, Attack, targetHealthAfter).AddToQueue();
        new EnemyDieCommand(UniqueEnemyID).AddToQueue();
    }

    public static void FlipIconBeforeResolve(GameObject gameObject)
    {
        bool FlipIcon = false;
        FlipIcon = (gameObject.transform.rotation.eulerAngles.y == 180);
        if (FlipIcon)
        {
            gameObject.transform.DORotate(Vector3.zero, GlobalSettings.Instance.CardTransitionTime);
        }        
    }

    public void AttackPlayer (EnemyLogic target)
    {
        this.GoFace();
    }

    public void AttackEnemyWithID(int uniqueEnemyID)
    {
        EnemyLogic target = EnemyLogic.EnemiesCreatedThisGame[uniqueEnemyID];
        AttackPlayer(target);
    }
}