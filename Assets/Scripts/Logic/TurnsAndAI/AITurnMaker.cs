using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//this class will take all decisions for AI. 

public class AITurnMaker: TurnMaker {

    public int ActionsThisTurn = 0;

    public override void OnTurnStart()
    {
        ActionsThisTurn = 0;
        base.OnTurnStart();
        // dispay a message that it is enemy`s turn
        new ShowMessageCommand("AI`s\n Turn!", 1.0f).AddToQueue();
        // p.DrawACard();
        Tabletop.Instance.DrawACard();
        StartCoroutine(MakeAITurn());
    }

    // THE LOGIC FOR AI
    IEnumerator MakeAITurn()
    {
        while (MakeOneAIMove() && ActionsThisTurn == 0)
        {
            yield return null;
        }

        InsertDelay(0.3f);

        TurnManager.Instance.EndTurn();
    }

    bool MakeOneAIMove()
    {
        if (Command.CardDrawPending())
            return true;
        if (Random.Range(0,2 ) == 0 && Tabletop.Instance.dungeonVisual.EnemiesOnTable.Count > 2 && TurnManager.Instance.Passed == false && ActionsThisTurn == 0) 
        {
            Debug.Log("AI pressed pass");
            ActionsThisTurn +=1;
            TurnManager.Instance.Passed = true;
            return AIPass();
        }
        else if (Random.Range(0,2 ) == 0 && Tabletop.Instance.tokenVisual.TokensInGame.Count > 0 && TurnManager.Instance.Passed == false && ActionsThisTurn == 0) {
            Debug.Log("AI removed a token");
            ActionsThisTurn +=1;
            return PlayATokenFromTabletop();
        }
        else if (TurnManager.Instance.Passed == false && ActionsThisTurn == 0)
        {
            Debug.Log("AI played a card");
            ActionsThisTurn += 1;
            return PlayACardFromHand();
        }
        return false;
    }

    bool AIPass()
    {
        TurnManager.Instance.OnPass();
        return true;
    }

    bool PlayACardFromHand()
    {
        foreach (CardLogic c in Tabletop.Instance.hand.CardsInHandLogic)
        {
            Tabletop.Instance.PlayAnEnemyFromHand(c, 0);
            return true;
        }
        return false;
    }

    bool PlayATokenFromTabletop()
    {
        foreach (CardLogic c in Tabletop.Instance.hand.CardsInHandLogic)
        {
            // random = new Random.Range();
            // HandVisual handVisual = Tabletop.Instance.handVisual;
            HandLogic handLogic = Tabletop.Instance.hand;
            // GameObject card = IDHolder.GetGameObjectWithID(c.ID);
            // handLogic.CardsInHandLogic.Remove(CardLogic.CardsCreatedThisGame[c.ID]);
            // handVisual.RemoveCard(card);
            // GameObject.Destroy(card);

            // int uniqueTokenID = this.GetComponent<IDHolder>().UniqueID;
            // GameObject token = IDHolder.GetGameObjectWithID(uniqueTokenID);
            GameObject card = IDHolder.GetGameObjectWithID(c.ID);

            GameObject randToken = Tabletop.Instance.tokenVisual.TokensInGame[Random.Range (0, Tabletop.Instance.tokenVisual.TokensInGame.Count)];
            int uniqueTokenID = randToken.GetComponent<IDHolder>().UniqueID;

            new PlayATokenCommand(c.ID, card, uniqueTokenID, randToken).AddToQueue(); 
            handLogic.CardsInHandLogic.Remove(c);          
            // Command.CommandExecutionComplete();
            // TurnManager.Instance.EndTurn();
            // InsertDelay(0.5f);
            return true;
        }
        return false;
    }

    // bool UseHeroPower()
    // {
    //     if (p.ManaLeft >= 2 && !p.usedHeroPowerThisTurn)
    //     {
    //         // use HP
    //         p.UseHeroPower();
    //         InsertDelay(1.5f);
    //         //Debug.Log("AI used hero power");
    //         return true;
    //     }
    //     return false;
    // }

    // bool AttackWithACreature()
    // {
    //     foreach (CreatureLogic cl in p.table.CreaturesOnTable)
    //     {
    //         if (cl.AttacksLeftThisTurn > 0)
    //         {
    //             // attack a random target with a creature
    //             if (p.otherPlayer.table.CreaturesOnTable.Count > 0)
    //             {
    //                 int index = Random.Range(0, p.otherPlayer.table.CreaturesOnTable.Count);
    //                 CreatureLogic targetCreature = p.otherPlayer.table.CreaturesOnTable[index];
    //                 cl.AttackCreature(targetCreature);
    //             }                    
    //             else
    //                 cl.GoFace();
                
    //             InsertDelay(1f);
    //             //Debug.Log("AI attacked with creature");
    //             return true;
    //         }
    //     }
    //     return false;
    // }

    void InsertDelay(float delay)
    {
        new DelayCommand(delay).AddToQueue();
    }

}
