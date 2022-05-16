using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class DungeonVisual : MonoBehaviour 
{
    public SameDistanceChildren slots;
    public List<GameObject> EnemiesOnTable = new List<GameObject>();
    private BoxCollider col;
    public bool DealFaceDown = false;

    private bool _cursorOverThisTable = false;    
    public bool CursorOverThisTable
    {
        get{ return _cursorOverThisTable; }
    }

    void Awake()
    {
        col = GetComponent<BoxCollider>();
    }

    // CURSOR/MOUSE DETECTION
    void Update()
    {
        RaycastHit[] hits;
        // raycst to mousePosition and store all the hits in the array
        hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition), 30f);

        bool passedThroughTableCollider = false;
        foreach (RaycastHit h in hits)
        {
            // check if the collider that we hit is the collider on this GameObject
            if (h.collider == col)
            {
                passedThroughTableCollider = true;
            }
        }
        _cursorOverThisTable = passedThroughTableCollider;
    }
   
    // method to create a new creature and add it to the table
    public void AddEnemyAtIndex(EnemyCardAsset eca, int UniqueID ,int index)
    {
        DealFaceDown = TurnManager.Instance.IsPlayerAI;
        GameObject enemy;
        if (DealFaceDown)
        {
            enemy = GameObject.Instantiate(GlobalSettings.Instance.EnemyIconPrefab, slots.Children[index].transform.position, transform.rotation * Quaternion.Euler (0f, 180f, 0f)) as GameObject;
        }
        else
        {
            enemy = GameObject.Instantiate(GlobalSettings.Instance.EnemyIconPrefab, slots.Children[index].transform.position, Quaternion.identity) as GameObject;
        }
        
        OneIconManager manager = enemy.GetComponent<OneIconManager>();
        manager.cardAsset = eca;
        manager.ReadIconFromAsset();

        // add tag according to owner
        // foreach (Transform t in enemy.GetComponentsInChildren<Transform>())
        //     t.tag = owner.ToString()+"Enemy";
        
        // parent a new creature gameObject to table slots
        enemy.transform.SetParent(slots.transform);

        // add a new creature to the list
        EnemiesOnTable.Insert(index, enemy);

        // let this creature know about its position
        WhereIsTheCardOrEnemy w = enemy.GetComponent<WhereIsTheCardOrEnemy>();
        w.Slot = index;

        // add our unique ID to this creature
        IDHolder id = enemy.AddComponent<IDHolder>();
        id.UniqueID = UniqueID;

        // after a new creature is added update placing of all the other creatures
        ShiftSlotsGameObjectAccordingToNumberOfEnemies();
        PlaceEnemiesOnNewSlots();

        // end command execution
        Command.CommandExecutionComplete();
    }


    // returns an index for a new creature based on mousePosition
    // included for placing a new creature to any positon on the table
    public int TablePosForNewEnemy(float MouseX)
    {
        // if there are no creatures or if we are pointing to the right of all creatures with a mouse.
        // right - because the table slots are flipped and 0 is on the right side.
        if (EnemiesOnTable.Count == 0 || MouseX > slots.Children[0].transform.position.x)
            return 0;
        else if (MouseX < slots.Children[EnemiesOnTable.Count - 1].transform.position.x) // cursor on the left relative to all creatures on the table
            return EnemiesOnTable.Count;
        for (int i = 0; i < EnemiesOnTable.Count; i++)
        {
            if (MouseX < slots.Children[i].transform.position.x && MouseX > slots.Children[i + 1].transform.position.x)
                return i + 1;
        }
        Debug.Log("Suspicious behavior. Reached end of TablePosForNewEnemy method. Returning 0");
        return 0;
    }

    // Destroy an ememy
    public void RemoveEnemyWithID(int IDToRemove)
    {
        // TODO: This has to last for some time
        // Adding delay here did not work because it shows one creature die, then another creature die. 
        // 
        //Sequence s = DOTween.Sequence();
        //s.AppendInterval(1f);
        //s.OnComplete(() =>
        //   {
                
        //    });
        Debug.Log("DungeonVisual RemoveEnemyWithID: "+IDToRemove);
        // GameObject enemyToRemove = IDHolder.GetGameObjectWithID(IDToRemove);
        // EnemiesOnTable.Remove(enemyToRemove);
        // Destroy(enemyToRemove);

        ShiftSlotsGameObjectAccordingToNumberOfEnemies();
        // PlaceEnemiesOnNewSlots();
        Command.CommandExecutionComplete();
    }

    /// <summary>
    /// Shifts the slots game object according to number of creatures.
    /// </summary>
    public void ShiftSlotsGameObjectAccordingToNumberOfEnemies()
    {
        float posX;
        if (EnemiesOnTable.Count > 0)
            posX = (slots.Children[0].transform.localPosition.x - slots.Children[EnemiesOnTable.Count - 1].transform.localPosition.x) / 2f;
        else
            posX = 0f;

        slots.gameObject.transform.DOLocalMoveX(posX, 0.3f);  
    }

    /// <summary>
    /// After a new creature is added or an old creature dies, this method
    /// shifts all the creatures and places the creatures on new slots.
    /// </summary>
    public void PlaceEnemiesOnNewSlots()
    {
        foreach (GameObject g in EnemiesOnTable)
        {
            g.transform.DOLocalMoveX(slots.Children[EnemiesOnTable.IndexOf(g)].transform.localPosition.x, 0.3f);
            // apply correct sorting order and HandSlot value for later 
            // TODO: figure out if I need to do something here:
            // g.GetComponent<WhereIsTheCardOrCreature>().SetTableSortingOrder() = EnemiesOnTable.IndexOf(g);
        }
    }

}
