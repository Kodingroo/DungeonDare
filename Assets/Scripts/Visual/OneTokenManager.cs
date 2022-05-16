using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// holds the refs to all the Text, Images on the card
public class OneTokenManager : MonoBehaviour {

    public TokenAsset tokenAsset;
    public OneTokenManager PreviewManager;
    public Text TitleText;
    public Text TokenHealthText;
    [Header("Image References")]
    public Image TokenGraphicImage;
    public Image TokenBodyImage;
    public Image TokenFaceFrameImage;    
    public Image TokenStrengthImage;
    public Image TokenLowRibbonImage;
    public Image TokenHealthImage;
    [Header("Other")]
    public bool MainToken;



    void Awake()
    {
        if (tokenAsset != null)
            ReadTokenFromAsset();
    }

    // private bool canBePlayedNow = false;
    // public bool CanBePlayedNow
    // {
    //     get
    //     {
    //         return canBePlayedNow;
    //     }

    //     set
    //     {
    //         canBePlayedNow = value;
    //     }
    // }

    public void ReadTokenFromAsset()
    {
        TitleText.text = tokenAsset.name;
        TokenHealthText.text = tokenAsset.TokenHealth.ToString();
        TokenGraphicImage.sprite = tokenAsset.GraphicImage;
        TokenStrengthImage.sprite = tokenAsset.StrengthImage;
        MainToken = tokenAsset.MainToken;

        if (TokenHealthText.text == "0")
        {
            TokenHealthImage.enabled = false;
            TokenHealthText.enabled = false;
        }
        if (PreviewManager != null)
        {
            PreviewManager.tokenAsset = tokenAsset;
            PreviewManager.ReadTokenFromAsset();
        }        
    }
}
