using UnityEngine;
using System.Collections;

public class IABManager : MonoBehaviour 
{
    // non-consumable item
    public const string ULTIMATE_FLASH_CARD_PRODUCT_ID = "Ultimate_Flash_Card"; // Product Id for IAB Nagagami 
    private string ultimateFlashCardFeatureId;

    // consumable item
    public const string MINI_POUCH_PRODUCT_ID = "Mini_Pouch"; // Product Id for IAB coin 50000

    public int miniPouch = 0;

    static IABManager instance;
    public static IABManager Instance { get { return instance; } }

    void Awake()
    {
        instance = this;

        // ------- IAB ---------
        UM_InAppPurchaseManager.OnPurchaseFlowFinishedAction += OnPurchaseFlowFinishedAction;
        UM_InAppPurchaseManager.OnBillingConnectFinishedAction += OnConnectFinished;
    }

	// Use this for initialization
	void Start () 
    {
     

        //subscribign on intit fisigh action
        UM_InAppPurchaseManager.OnBillingConnectFinishedAction += OnBillingConnectFinishedAction;
        UM_InAppPurchaseManager.instance.Init();

        // IAB - Set text price based on localized price
        if(UM_InAppPurchaseManager.instance.IsInited)
        {
//            buttonBuyCoin500000.transform.FindChild("Text").GetComponent<Text>().text = UM_InAppPurchaseManager.Instance.GetProductById(COIN_500000_PRODUCT_ID).LocalizedPrice.ToString();
//            buttonBuyNagaIAP.transform.FindChild("Text").GetComponent<Text>().text = UM_InAppPurchaseManager.Instance.GetProductById(NAGAGAMI_PRODUCT_ID).LocalizedPrice.ToString();
            Debug.Log("Localize price");
        }
        else
        {
//            buttonBuyCoin500000.transform.FindChild("Text").gameObject.GetComponent<Text>().text = UM_InAppPurchaseManager.Instance.GetProductById(COIN_500000_PRODUCT_ID).ActualPrice.ToString();
//            buttonBuyNagaIAP.transform.FindChild("Text").gameObject.GetComponent<Text>().text = UM_InAppPurchaseManager.Instance.GetProductById(NAGAGAMI_PRODUCT_ID).ActualPrice.ToString();
            Debug.Log("Unlocalize price");

        }
	}

    public void CheckUltimateFlashCardStatus(string featureId)
    {
        ultimateFlashCardFeatureId = featureId;

        // IAP Naga character. If the skin not yet buy from IAP
        if(UM_InAppPurchaseManager.instance.IsProductPurchased(ULTIMATE_FLASH_CARD_PRODUCT_ID)) 
        {
            Debug.Log("Ultimate Flash Card Already purchased");

            // Unlock this feature if payment successfull
            PlayerPrefs.SetString(ultimateFlashCardFeatureId, ultimateFlashCardFeatureId);

            PlayerPrefs.Save();


        } 
        else // If Naga character didn't unlock from IAP
        {   
            Debug.Log("Ultimate Flash Card does not purchased");

        }
    }

    public void BuyMiniPouch()
    {
        // Start Purchasing
        if(UM_InAppPurchaseManager.instance.IsInited) 
        {
            UM_InAppPurchaseManager.OnPurchaseFlowFinishedAction += OnPurchaseFlowFinishedAction;
            UM_InAppPurchaseManager.OnBillingConnectFinishedAction += OnConnectFinished;

            UM_InAppPurchaseManager.instance.Purchase(MINI_POUCH_PRODUCT_ID);
            Debug.Log("Start purchsing " + MINI_POUCH_PRODUCT_ID + " product");

            // If the payment is successfull, enter function OnPurchaseFlowFinishedAction
        } 
        else  
        {
            Debug.Log("Try init IAB...");
            //subscribign on intit fisigh action
            UM_InAppPurchaseManager.OnBillingConnectFinishedAction += OnBillingConnectFinishedAction;
            UM_InAppPurchaseManager.instance.Init();
        }



    }

    // Called, when the player buys the red submarine using In App Purchase
    public void BuyNonConsumableProduct(string featureId)
    {
        if (featureId == ultimateFlashCardFeatureId)
        {
            //If the flash card is not yet owned
            if (!PlayerPrefs.HasKey(ultimateFlashCardFeatureId))
            {
                // Start Purchasing Nagagami
                if(UM_InAppPurchaseManager.instance.IsInited) 
                {
                    UM_InAppPurchaseManager.OnPurchaseFlowFinishedAction += OnPurchaseFlowFinishedAction;
                    UM_InAppPurchaseManager.OnBillingConnectFinishedAction += OnConnectFinished;

                    UM_InAppPurchaseManager.instance.Purchase(ULTIMATE_FLASH_CARD_PRODUCT_ID);
                    Debug.Log("Start purchsing " + ULTIMATE_FLASH_CARD_PRODUCT_ID + " product");

                    // If the payment is successfull, enter function OnPurchaseFlowFinishedAction
                } 
                else
                {
                    Debug.Log("Try init IAB...");
                    //subscribign on intit fisigh action
                    UM_InAppPurchaseManager.OnBillingConnectFinishedAction += OnBillingConnectFinishedAction;
                    UM_InAppPurchaseManager.instance.Init();
                }

            }
        }
    }  

    //--------------------------------------
    //  GET/SET
    //--------------------------------------

    //--------------------------------------
    //  EVENTS
    //--------------------------------------

    private void OnConnectFinished(UM_BillingConnectionResult result) {

        if(result.isSuccess) {
            Debug.Log ("Billing init Success");
        } else  {
            Debug.Log("Billing init Failed");
        }
    }

    private void OnPurchaseFlowFinishedAction (UM_PurchaseResult result) {
        UM_InAppPurchaseManager.OnPurchaseFlowFinishedAction -= OnPurchaseFlowFinishedAction;
        if(result.isSuccess) 
        {
            Debug.Log("Product " + result.product.id + " purchase Success");

            // If payment 50000 coin successful
            if (result.product.id == MINI_POUCH_PRODUCT_ID)
            {
                SaveManager.coinAmount += miniPouch;
                SaveManager.SaveData();

                UM_NotificationController.instance.ShowNotificationPoup("In App Purchase", "250 pouch is added to your account");

                // Achievement First Exclusive Pouch
                // if buy is successfull
                GPGSManager.Instance.Achievement_First_Exclusive_Pouch();
            }
            // If payment Nagagami successful
            if (result.product.id == ULTIMATE_FLASH_CARD_PRODUCT_ID)
            {
                
                PlayerPrefs.SetString(ultimateFlashCardFeatureId, ultimateFlashCardFeatureId);

                Debug.Log("Unlock " + result.product.id + " successful!");
                UM_NotificationController.instance.ShowNotificationPoup("In App Purchase", "Ultimate Flash Card Unlock");

            }

            PlayerPrefs.Save();
            AudioManager.Instance.PlayCashRegister();
        } 
        else  
        {
            Debug.Log("Product " + result.product.id + " purchase Failed");
        }
    }

    private void OnBillingConnectFinishedAction (UM_BillingConnectionResult result) {
        UM_InAppPurchaseManager.OnBillingConnectFinishedAction -= OnBillingConnectFinishedAction;
        if(result.isSuccess) 
        {
            Debug.Log("OnBillingConnectFinishedAction - Connected");
        } 
        else 
        {
            Debug.Log("OnBillingConnectFinishedAction - Failed to connect");
        }
    }
}
