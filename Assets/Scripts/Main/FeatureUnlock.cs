﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FeatureUnlock : MonoBehaviour 
{
    public string levelId = "1";      // Condition level need to unlock
    public string featureId = "1";    // Condition for feature id needed (player pref)
    public int price = 0;
    public string sceneName;    // Name of scene to be loaded
    public GameObject condition;    // Condition display to unlock this feature
    public bool coinDependency;             //  If this feature need coin to unlock?
    public string notification;

    public bool iabDependency;
    private const string ultimateFlashCardId = "keluarga";

    public GameObject fadeGameObject;
    private LoadScene loadLevelFade;

	// Use this for initialization
	void Start () 
    {

        loadLevelFade = GameObject.Find("FadeSceneAnimation").GetComponent<LoadScene>();

        CheckFeatureUnlock(levelId);

        if (iabDependency)
        {
            IABManager.Instance.CheckUltimateFlashCardStatus(ultimateFlashCardId);
        }

	}
	
	// Update is called once per frame
	void Update () 
    {
        CheckFeatureUnlock(levelId);
	}

    public void CheckFeatureUnlock(string levelId)
    {
        if (PlayerPrefs.HasKey(levelId) && PlayerPrefs.HasKey(featureId))
        {
            this.GetComponent<Button>().interactable = true;

            // Change the color of the button
            this.transform.FindChild("ImageUnlock").gameObject.SetActive(true);
            this.transform.FindChild("ImageLock").gameObject.SetActive(false);

            condition.SetActive(false);

            //CheckAchievement();
        }
        else
        {
            this.GetComponent<Button>().interactable = true;

            // Change the color of the button
            this.transform.FindChild("ImageUnlock").gameObject.SetActive(false);
            this.transform.FindChild("ImageLock").gameObject.SetActive(true);

            condition.SetActive(true);
        }
    }

    public void UnlockFeature()
    {
        // If already unlock
        if (PlayerPrefs.HasKey(levelId) && PlayerPrefs.HasKey(featureId))
        {
//            PlayerPrefs.DeleteKey(featureId.ToString());
            // Load scene this feature
//            SceneManager.LoadScene(sceneName);
            loadLevelFade = GameObject.Find("FadeSceneAnimation").GetComponent<LoadScene>();
            loadLevelFade.LoadLevelWithFadeAnimation(sceneName);
        }
        else
        {
            // If need coin/price/iap to unlock
            if (coinDependency == true)
            {
                if (SaveManager.coinAmount >= price)
                {
                    // Unlock this feature
                    PlayerPrefs.SetString(featureId.ToString(), featureId.ToString());    // make this feature available in user data

                    SaveManager.coinAmount = SaveManager.coinAmount - price;
                    SaveManager.SaveData();

                    Debug.Log("feature " + featureId.ToString() + " unlocked!");

                    CheckFeatureUnlock(levelId);

                }
                else
                {
                    // Don't have enough coin
                    // pop up coin panel
                    Debug.Log("Not enough coin " + SaveManager.coinAmount );
                    UM_NotificationController.instance.ShowNotificationPoup("Notification", "Not enough coin " + SaveManager.coinAmount);
                }
            }
            else
            {
                // give notification message

                UM_NotificationController.instance.ShowNotificationPoup("Notification", notification);
                Debug.Log(notification);
            }
        }

    }

    public void UnlockFeatureIAP()
    {
        if (PlayerPrefs.HasKey(levelId) && PlayerPrefs.HasKey(featureId))
        {
            //            PlayerPrefs.DeleteKey(featureId.ToString());
            // Load scene this feature
//            SceneManager.LoadScene(sceneName);
            loadLevelFade = GameObject.Find("FadeSceneAnimation").GetComponent<LoadScene>();
            loadLevelFade.LoadLevelWithFadeAnimation(sceneName);
        }
        else
        {
            // Unlock this feature if payment successfull
//            PlayerPrefs.SetString(featureId.ToString(), featureId.ToString());

            IABManager.Instance.BuyNonConsumableProduct(featureId);

            UM_NotificationController.instance.ShowNotificationPoup("Notification", notification + " - " + featureId);
            Debug.Log(notification);
        }
    }

    public void CheckAchievement()
    {
        // Achievement Unlock Ultimate Flash Card
        if (PlayerPrefs.HasKey("keluarga"))
        {
            GPGSManager.Instance.Achievement_Ultimate_Flash_Card();
        }

        // Achievement All Flash Card
        if (PlayerPrefs.HasKey("keluarga") && PlayerPrefs.HasKey("highestFlashCardUnlockbylevel") && PlayerPrefs.HasKey("allflashcardmustbuyusingcoin"))
        {
            GPGSManager.Instance.Achievement_Unlock_All_Flash_Card();
        }
            
    }

    private void CheckFeatureIdIABPurchasedStatus()
    {
        IABManager.Instance.CheckUltimateFlashCardStatus(featureId);
    }

}
