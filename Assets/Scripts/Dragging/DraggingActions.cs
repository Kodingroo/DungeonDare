using UnityEngine;
using System.Collections;

public abstract class DraggingActions : MonoBehaviour {

    public abstract void OnStartDrag();

    public abstract void OnEndDrag();

    public abstract void OnDraggingInUpdate();

    public virtual bool CanDrag
    {
        get
        {            
            return GlobalSettings.Instance.CanControlAction();
            // return true;
        }
    }

    protected virtual Player playerOwner
    {
        get{
            
            if (tag.Contains("PlayerOne"))
                return GlobalSettings.Instance.PlayerOne;
            else if (tag.Contains("PlayerTwo"))
                return GlobalSettings.Instance.PlayerTwo;
            else
            {
                Debug.LogError("Untagged Card or enemy " + transform.parent.name);
                return null;
            }
        }
    }

    protected abstract bool DragSuccessful();
}
