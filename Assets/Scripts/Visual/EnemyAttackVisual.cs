using UnityEngine;
using System.Collections;
using DG.Tweening;

public class EnemyAttackVisual : MonoBehaviour 
{
    private OneIconManager manager;
    private WhereIsTheCardOrEnemy w;

    void Awake()
    {
        manager = GetComponent<OneIconManager>();    
        w = GetComponent<WhereIsTheCardOrEnemy>();
    }

    public void AttackTarget(int targetUniqueID, int damageTakenByTarget, int targetHealthAfter)
    {

        EnemyLogic.FlipIconBeforeResolve(gameObject);

        manager.CanAttackNow = false;
        GameObject target = IDHolder.GetGameObjectWithID(targetUniqueID);

        // bring this Enemy to front sorting-wise.
        w.BringToFront();
        VisualStates tempState = w.VisualState;
        w.VisualState = VisualStates.Transition;

        transform.DOMove(target.transform.position, 0.5f).SetLoops(2, LoopType.Yoyo).SetDelay(1).SetEase(Ease.InCubic).OnComplete(() =>
        {
            // if(damageTakenByTarget>0)
            //     DamageEffect.CreateDamageEffect(target.transform.position, damageTakenByTarget);
            if (targetUniqueID == GlobalSettings.Instance.PlayerOne.ID || targetUniqueID == GlobalSettings.Instance.PlayerTwo.ID)
            {
                target.GetComponent<PlayerPortraitVisual>().HealthText.text = targetHealthAfter.ToString();
            }
            w.SetTableSortingOrder();
            w.VisualState = tempState;

            Sequence s = DOTween.Sequence();
            s.AppendInterval(0.2f);
            s.OnComplete(Command.CommandExecutionComplete);
            //Command.CommandExecutionComplete();
        });
    }
        
}
