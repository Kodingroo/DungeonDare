using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerPortraitVisual : MonoBehaviour {

    public PlayerAsset playerAsset;
    [Header("Text Component References")]
    public Text HealthText;
    public Text TitleText;
    [Header("Image References")]
    public Image PortraitImage;
    public Image TitleBackgroundImage;
    public Image PortraitFrame;

    void Awake()
	{
		if(playerAsset != null)
			ApplyLookFromAsset();
	}
	
	public void ApplyLookFromAsset()
    {
        HealthText.text = playerAsset.BaseHealth.ToString();
        PortraitImage.sprite = playerAsset.AvatarImage;
        TitleText.text = playerAsset.TitleText.ToString();
        TitleBackgroundImage.sprite = playerAsset.TitleBackgroundImage;
    }

    public void TakeDamage(int amount, int healthAfter)
    {
        if (amount > 0)
        {
            HealthText.text = healthAfter.ToString();
        }
    }

    public void Explode()
    {
        Instantiate(GlobalSettings.Instance.ExplosionPrefab, transform.position, Quaternion.identity);
        Sequence s = DOTween.Sequence();
        s.PrependInterval(1.5f);
        s.OnComplete(() => GlobalSettings.Instance.GameOverPanel.SetActive(true));
    }    
}
