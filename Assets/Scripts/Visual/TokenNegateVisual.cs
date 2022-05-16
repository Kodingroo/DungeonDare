using UnityEngine;
using System.Collections;
using DG.Tweening;

public class TokenNegateVisual : MonoBehaviour 
{
    private OneTokenManager manager;
    private WhereIsTheCardOrEnemy w;
    private bool FlipIcon = false;

    void Awake()
    {
        manager = GetComponent<OneTokenManager>();    
        w = GetComponent<WhereIsTheCardOrEnemy>();
    }

    public void NegateTarget(int targetUniqueID)
    {
        GameObject enemyIcon = IDHolder.GetGameObjectWithID(targetUniqueID);
        FlipIcon = (enemyIcon.transform.rotation.eulerAngles.y == 180);
        // FlipIcon = !TurnManager.Instance.IsPlayerAI;
        if (FlipIcon)
        {
            Debug.Log("Please reach here");
            // GameObject enemyIcon = IDHolder.GetGameObjectWithID(targetUniqueID);
            // Debug.Log("Rotation y: "+(enemyIcon.transform.rotation.eulerAngles.y == 180));
            enemyIcon.transform.DORotate(Vector3.zero, GlobalSettings.Instance.CardTransitionTime);
        }

        GameObject target = IDHolder.GetGameObjectWithID(targetUniqueID);

        w.BringToFront();
        VisualStates tempState = w.VisualState;
        w.VisualState = VisualStates.Transition;

        transform.DOMove(target.transform.position, 0.5f).SetLoops(2, LoopType.Yoyo).SetDelay(1).SetEase(Ease.InCubic).OnComplete(() =>
        {
            // if(damageTakenByTarget>0)
            //     DamageEffect.CreateDamageEffect(target.transform.position, damageTakenByTarget);

            w.SetTableSortingOrder();
            w.VisualState = tempState;

            Sequence s = DOTween.Sequence();
            s.AppendInterval(0.2f);
            s.OnComplete(Command.CommandExecutionComplete);
            //Command.CommandExecutionComplete();
        });
    }
        
}
