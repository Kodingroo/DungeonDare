using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// an enum to store the info about where this object is
public enum VisualStates
{
    Transition,
    Dragging
}

public class WhereIsTheCardOrEnemy : MonoBehaviour {

    // reference to a HoverPreview Component
    private HoverPreview hover;    

    // reference to a canvas on this object to set sorting order
    private Canvas canvas;

    // a value for canvas sorting order when we want to show this object above everything
    private int TopSortingOrder = 500;

    // PROPERTIES
    private int slot = -1;
    public int Slot
    {
        get{ return slot;}

        set
        {
            slot = value;
            /*if (value != -1)
            {
                canvas.sortingOrder = HandSortingOrder(slot);
            }*/
        }
    }

    private VisualStates state;
    public VisualStates VisualState
    {
        get{ return state; }  

        set
        {
            state = value;
        }
    }

    void Awake()
    {
        hover = GetComponent<HoverPreview>();
        // for characters hover is attached to a child game object
        if (hover == null)
            hover = GetComponentInChildren<HoverPreview>();        
        canvas = GetComponentInChildren<Canvas>();
    }

    public void BringToFront()
    {
        canvas.sortingOrder = TopSortingOrder;
        canvas.sortingLayerName = "AboveEverything";
    }

    // not setting sorting order inside of VisualStaes property because when the card is drawn, 
    // we want to set an index first and set the sorting order only when the card arrives to hand. 
    public void SetHandSortingOrder()
    {
        if (slot != -1)
            canvas.sortingOrder = HandSortingOrder(slot);
        canvas.sortingLayerName = "Cards";
    }

    public void SetTableSortingOrder()
    {
        canvas.sortingOrder = 0;
        canvas.sortingLayerName = "Icons";
    }

    private int HandSortingOrder(int placeInHand)
    {
        return (-(placeInHand + 1) * 10); 
    }


}
