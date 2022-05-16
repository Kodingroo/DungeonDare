using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class TokenLogic
{
    // PUBLIC FIELDS
    public TokenAsset ta;
    public int UniqueTokenID;
    public TokenEffect effect;
    public static Dictionary<int, TokenLogic> TokensCreatedThisGame = new Dictionary<int, TokenLogic>();    

    // PROPERTIES
    public int ID
    {
        get{ return UniqueTokenID; }
    }

    private int _tokenHealth;
    public int TokenHealth
    {
        get{ return _tokenHealth;}
    }

    // returns true if we can attack with this Enemy now
    public bool CanPlay
    {
        get
        {
            if (this.ta.MainToken)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    // CONSTRUCTOR
    public TokenLogic(TokenAsset ta)
    {
        this.ta = ta;
        _tokenHealth = ta.TokenHealth;
        UniqueTokenID = IDFactory.GetUniqueID();
        if (ta.TokenEffectName!= null && ta.TokenEffectName!= "")
        {
            effect = System.Activator.CreateInstance(System.Type.GetType(ta.TokenEffectName)) as TokenEffect;
        }
        TokensCreatedThisGame.Add(UniqueTokenID, this);
        if (ta.TokenHealth > 0)
        {
            foreach (Player p in Player.Players)
            {
                int healthAfter = p.Health + ta.TokenHealth;
                p.playerPortraitVisual.GetComponent<PlayerPortraitVisual>().HealthText.text = healthAfter.ToString();
            }
        }
    }

    // METHODS
    public static void RemoveHPIfTokenRemoved(int uniqueTokenID)
    {   
        TokenLogic tl = TokensCreatedThisGame[uniqueTokenID];
        if (tl.TokenHealth > 0)
        {
            foreach (Player p in Player.Players)
            {
                int healthAfter = p.Health -= tl.TokenHealth;
                p.playerPortraitVisual.GetComponent<PlayerPortraitVisual>().HealthText.text = healthAfter.ToString();
            }
        } 
    }

    public void TokenPowersInPlay(EnemyLogic target) 
    {
        new TokenNegateCommand(target.ID, ID).AddToQueue();
        new EnemyDieCommand(target.ID).AddToQueue();
    }
}