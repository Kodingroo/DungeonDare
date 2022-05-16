using UnityEngine;
 
/// <summary>
/// This script should be attached to the card game object to display card`s rotation correctly.
/// </summary>
[ExecuteInEditMode]
public class BetterCardRotation : MonoBehaviour {
 
    // parent game object for all the card face graphics
    public RectTransform CardFront;
 
    // parent game object for all the card back graphics
    public RectTransform CardBack;
 
    // Update is called once per frame
    void Update() 
    {
        if (isCardFrontFacingCamera())
        {
             // show the front side
            CardFront.gameObject.SetActive(true);
            CardBack.gameObject.SetActive(false);
        }
        else
        {
            // show the back side
            CardFront.gameObject.SetActive(false);
            CardBack.gameObject.SetActive(true);
        }
    }
 
    bool isCardFrontFacingCamera()
    {
        return Vector3.Dot(
            CardFront.transform.forward,
            Camera.main.transform.position - CardFront.transform.position) < 0;
    }
}