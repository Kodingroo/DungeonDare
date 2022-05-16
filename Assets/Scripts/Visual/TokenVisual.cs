using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class TokenVisual : MonoBehaviour
{
    // PUBLIC FIELDS
    public bool TaketokensOpenly = true;
    public SameDistanceChildren slots;
    public TokenPoolLogic tokenPoolLogic;


    // PRIVATE : a list of all token visual representations as GameObjects
    public List<GameObject> TokensInGame = new List<GameObject>();

    private void Start() {
        
    }

    public void PopulateTokensInGame() 
    {
        int iterate = 0;
        foreach (TokenLogic tl in tokenPoolLogic.TokensInPool)
        {        
            GameObject token;
            token = GameObject.Instantiate(GlobalSettings.Instance.TokenPrefab, slots.Children[iterate].transform.position, Quaternion.identity) as GameObject;
            OneTokenManager manager = token.GetComponent<OneTokenManager>();
            manager.tokenAsset = tl.ta;
            manager.ReadTokenFromAsset();
                 
            TokensInGame.Insert(iterate, token);

            WhereIsTheCardOrEnemy w = token.GetComponent<WhereIsTheCardOrEnemy>();
            // w.BringToFront();
            w.Slot = iterate;   

            IDHolder id = token.AddComponent<IDHolder>();
            id.UniqueID = tl.UniqueTokenID;                      

            token.transform.SetParent(slots.transform);

            if (tl.TokenHealth > 0)
            {
                foreach (Player p in Player.Players)
                {
                    int healthAfter = p.Health += tl.TokenHealth;
                    p.Health = healthAfter;
                    p.playerPortraitVisual.GetComponent<PlayerPortraitVisual>().HealthText.text = healthAfter.ToString();                
                }
            }
            iterate++;
        }             
    }
    // add a new token GameObject to Table
    public void AddToken(GameObject token)
    {
        TokensInGame.Insert(0, token);

        token.transform.SetParent(slots.transform);

        PlaceTokensOnNewSlots();
        UpdatePlacementOfSlots();
    }

    public void RemoveToken(GameObject token)
    {
        TokensInGame.Remove(token);

        PlaceTokensOnNewSlots();
        // UpdatePlacementOfSlots();
    }

    public void RemoveTokenAtIndex(int index)
    {
        TokensInGame.RemoveAt(index);
        // re-calculate the position of the hand
        PlaceTokensOnNewSlots();
        UpdatePlacementOfSlots();
    }

    // get a token GameObject with a given index in hand
    public GameObject GettokenAtIndex(int index)
    {
        return TokensInGame[index];
    }
        
    // MANAGING TOKENS AND SLOTS

    // move Slots GameObject according to the number of tokens in hand
    void UpdatePlacementOfSlots()
    {
        float posX;
        if (TokensInGame.Count > 0)
            posX = (slots.Children[0].transform.localPosition.x - slots.Children[TokensInGame.Count - 1].transform.localPosition.x) / 2f;
        else
            posX = 0f;

        // tween Slots GameObject to new position in 0.3 seconds
        slots.gameObject.transform.DOLocalMoveX(posX, 0.3f);  
    }

    // shift all tokens to their new slots
    void PlaceTokensOnNewSlots()
    {
        foreach (GameObject g in TokensInGame)
        {
            // tween this token to a new Slot
            g.transform.DOLocalMoveX(slots.Children[TokensInGame.IndexOf(g)].transform.localPosition.x, 0.3f);

            // apply correct sorting order and HandSlot value for later 
            WhereIsTheCardOrEnemy w = g.GetComponent<WhereIsTheCardOrEnemy>();
            w.Slot = TokensInGame.IndexOf(g);
            w.SetHandSortingOrder();
        }
    }

    // token DRAW METHODS

    // // creates a token and returns a new token as a GameObject
    // GameObject CreateAtokenAtPosition(TokenAsset ta, Vector3 position, Vector3 eulerAngles)
    // {
    //     // Instantiate a token depending on its type
    //     GameObject token;
    //     // this token is a Enemy token
    //     token = GameObject.Instantiate(GlobalSettings.Instance.TokenPrefab, position, Quaternion.Euler(eulerAngles)) as GameObject;
    //     // apply the look of the token based on the info from EnemytokenAsset
    //     OneTokenManager manager = token.GetComponent<OneTokenManager>();
    //     manager.tokenAsset = ta;
    //     manager.ReadTokenFromAsset();

    //     return token;
    // }

    // // gives player a new token from a given position
    // public void GivePlayerAToken(TokenAsset ta, int UniqueID, bool fast = false)
    // {
    //     GameObject token;
    //     token = CreateAtokenAtPosition(ta, DeckTransform.position, new Vector3(0f, -179f, 0f));
    //     // pass this token to HandVisual class
    //     AddToken(token);

    //     // Bring token to front while it travels from draw spot to hand
    //     WhereIsTheCardOrEnemy w = token.GetComponent<WhereIsTheCardOrEnemy>();
    //     w.BringToFront();
    //     w.Slot = 0; 
    //     w.VisualState = VisualStates.Transition;

    //     // pass a unique ID to this token.
    //     IDHolder id = token.AddComponent<IDHolder>();
    //     id.UniqueID = UniqueID;

    //     // move token to the hand;
    //     Sequence s = DOTween.Sequence();
    //     if (!fast)
    //     {
    //         // Debug.Log ("Not fast!!!");
    //         s.Append(token.transform.DOMove(DrawPreviewSpot.position, GlobalSettings.Instance.tokenTransitionTime));
    //         if (TaketokensOpenly)
    //             s.Insert(0f, token.transform.DORotate(Vector3.zero, GlobalSettings.Instance.tokenTransitionTime)); 
    //         //else 
    //             //s.Insert(0f, token.transform.DORotate(new Vector3(0f, -179f, 0f), GlobalSettings.Instance.tokenTransitionTime)); 
    //         s.AppendInterval(GlobalSettings.Instance.tokenPreviewTime);
    //         // displace the token so that we can select it in the scene easier.
    //         s.Append(token.transform.DOLocalMove(slots.Children[0].transform.localPosition, GlobalSettings.Instance.tokenTransitionTime));
    //     }
    //     else
    //     {
    //         // displace the token so that we can select it in the scene easier.
    //         s.Append(token.transform.DOLocalMove(slots.Children[0].transform.localPosition, GlobalSettings.Instance.tokenTransitionTimeFast));
    //         if (TaketokensOpenly)    
    //             s.Insert(0f,token.transform.DORotate(Vector3.zero, GlobalSettings.Instance.tokenTransitionTimeFast)); 
    //     }

    //     s.OnComplete(()=>ChangeLasttokenStatusToInHand(token, w));
    // }

    // // this method will be called when the token arrived to hand 
    // void ChangeLasttokenStatusToInHand(GameObject token, WhereIsTheCardOrEnemy w)
    // {
    //     //Debug.Log("Changing state to Hand for token: " + token.gameObject.name);
    //     // set correct sorting order
    //     w.SetHandSortingOrder();
    //     // end command execution for DrawAtokenCommand
    //     Command.CommandExecutionComplete();
    // } 
}
