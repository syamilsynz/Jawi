using UnityEngine;
using System.Collections;

public class GuiMain : MonoBehaviour 
{
    public GameObject panelBuyCoin;
    // Animator
    public Animator animBuyCoin;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public void OpenBuyCoin()
    {
//        panelBuyCoin.SetActive(true);

        animBuyCoin.transform.parent.gameObject.SetActive(true);
        animBuyCoin.SetBool("Open", true);


    }

    public void CloseBuyCoin()
    {
        animBuyCoin.SetBool("Open", false);
    }

    public void BuyCoin(int value)
    {
        SaveManager.coinAmount = SaveManager.coinAmount + value;
        SaveManager.SaveData();
    }

    public void BuyMiniPouch(int value)
    {
        IABManager.Instance.miniPouch = value;
        IABManager.Instance.BuyMiniPouch();
    }
        
    public void TengokAds()
    {
        UnityAdsManager.Instance.ShowAd();
    }

    public void TengokRewardAds()
    {
//        UnityAdsManager.Instance.ShowRewardedAd();
        UnityAdsManager.Instance.ShowCoinRewardedAd(100);
    }


}
