using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UnityAdsManager : MonoBehaviour 
{
    
    static UnityAdsManager instance;
    public static UnityAdsManager Instance { get { return instance; } }

    void Awake()
    {
        instance = this;
    }

    public void ShowAd()
    {
        if (Advertisement.IsReady())
        {
            Advertisement.Show();
        }
    }
        
    public void ShowRewardedAd()
    {
        if (Advertisement.IsReady("rewardedVideoZone"))
        {
            var options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show("rewardedVideoZone", options);
        }
    }

    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");
                //
                // YOUR CODE TO REWARD THE GAMER
                // Give coins etc.
                SaveManager.coinAmount = SaveManager.coinAmount + 100;
                SaveManager.SaveData();

                UM_NotificationController.instance.ShowNotificationPoup("Unity Ads", "Dapat 100 coin bai");
                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                break;
        }
    }

    public void ShowCoinRewardedAd(int amount)
    {
        if (Advertisement.IsReady("rewardedVideo"))
        {
            //          var options = new ShowOptions { resultCallback = HandleShowResultCoinRewardedAd(300) };
            var options = new ShowOptions { resultCallback = result => {
                    switch(result)
                    {
                        case (ShowResult.Finished):
                            Debug.Log("The ad was successfully shown.");
                            //
                            // YOUR CODE TO REWARD THE GAMER
                            // Give coins etc.
                            SaveManager.coinAmount = SaveManager.coinAmount + amount;
                            SaveManager.SaveData();
                            UM_NotificationController.instance.ShowNotificationPoup("Unity Ads", "Dapat" +  amount + " pouch!");
                            break;
                        case (ShowResult.Failed):
                            Debug.Log("The ad was skipped before reaching the end.");
                            break;
                        case(ShowResult.Skipped):
                            Debug.LogError("The ad failed to be shown.");
                            break;
                    } 
                }
            };
            Advertisement.Show("rewardedVideo", options);
        }
    }

    public void ShowFreeCoinRewardedAd(int amount)
    {
        if (Advertisement.IsReady("rewardedVideo"))
        {
            //          var options = new ShowOptions { resultCallback = HandleShowResultCoinRewardedAd(300) };
            var options = new ShowOptions { resultCallback = result => {
                    switch(result)
                    {
                        case (ShowResult.Finished):
                            Debug.Log("The ad was successfully shown.");
                            //
                            // YOUR CODE TO REWARD THE GAMER
                            // Give coins etc.
                            SaveManager.coinAmount = SaveManager.coinAmount + amount;
                            SaveManager.SaveData();
                            UM_NotificationController.instance.ShowNotificationPoup("Unity Ads", "Dapat" +  amount + " pouch!");
                            break;
                        case (ShowResult.Failed):
                            Debug.Log("The ad was skipped before reaching the end.");
                            break;
                        case(ShowResult.Skipped):
                            Debug.LogError("The ad failed to be shown.");
                            break;
                    } 
                }
            };
            Advertisement.Show("rewardedVideo", options);
        }
    }

    public void ShowDoubleCoinRewardedAd(int amount, bool deductAmountThenDoubleIt)
    {
        if (Advertisement.IsReady("rewardedVideo"))
        {
            //          var options = new ShowOptions { resultCallback = HandleShowResultCoinRewardedAd(300) };
            var options = new ShowOptions { resultCallback = result => {
                    switch(result)
                    {
                        case (ShowResult.Finished):
                            Debug.Log("The ad was successfully shown.");
                            //
                            // YOUR CODE TO REWARD THE GAMER
                           
                            // Deduct the normal coin that already receive before double it
                            if (deductAmountThenDoubleIt == true)
                            {
                                SaveManager.coinAmount = SaveManager.coinAmount - amount;
                            }

                            // Double the coins etc.
                            SaveManager.coinAmount = SaveManager.coinAmount + (amount * 2);
                            SaveManager.SaveData();
                            UM_NotificationController.instance.ShowNotificationPoup("Unity Ads", "Dapat" +  (amount * 2) + " pouch!");

                            // Normal Mode
                            if (SceneManager.GetActiveScene().name == "jawi")
                            {
                                GUIManager.Instance.completeLevelParent.transform.FindChild("Pouch Receive").transform.FindChild("Text").GetComponent<Text>().text = (amount * 2).ToString();
                                GUIManager.Instance.completeLevelParent.transform.FindChild("Double Coin").gameObject.SetActive(false);
                            }

                            if (SceneManager.GetActiveScene().name == "timer")
                            {
                                GUIManager.Instance.timerLevelCompleteParent.transform.FindChild("Pouch Receive").transform.FindChild("Text").GetComponent<Text>().text = (amount * 2).ToString();
                                GUIManager.Instance.timerLevelCompleteParent.transform.FindChild("Double Coin").gameObject.SetActive(false);
                            }

                            break;
                        case (ShowResult.Failed):
                            Debug.Log("The ad was skipped before reaching the end.");
                            break;
                        case(ShowResult.Skipped):
                            Debug.LogError("The ad failed to be shown.");
                            break;
                    } 
                }
            };
            Advertisement.Show("rewardedVideo", options);
        }
    }

    public void ShowDoubleCoinRewardedAd(int amount)
    {
        if (Advertisement.IsReady("rewardedVideo"))
        {
            //          var options = new ShowOptions { resultCallback = HandleShowResultCoinRewardedAd(300) };
            var options = new ShowOptions { resultCallback = result => {
                    switch(result)
                    {
                        case (ShowResult.Finished):
                            Debug.Log("The ad was successfully shown.");
                            //
                            // YOUR CODE TO REWARD THE GAMER

                            // Double the coins etc.
                            SaveManager.coinAmount = SaveManager.coinAmount + (amount * 2);
                            SaveManager.SaveData();
                            UM_NotificationController.instance.ShowNotificationPoup("Unity Ads", "Dapat" +  (amount * 2) + " pouch!");



                            break;
                        case (ShowResult.Failed):
                            Debug.Log("The ad was skipped before reaching the end.");
                            break;
                        case(ShowResult.Skipped):
                            Debug.LogError("The ad failed to be shown.");
                            break;
                    } 
                }
            };
            Advertisement.Show("rewardedVideo", options);
        }
    }

    public void ShowFlashCardRewardedAd(string name)
    {
        if (Advertisement.IsReady("rewardedVideo"))
        {
            //          var options = new ShowOptions { resultCallback = HandleShowResultCoinRewardedAd(300) };
            var options = new ShowOptions { resultCallback = result => {
                    switch(result)
                    {
                        case (ShowResult.Finished):
                            Debug.Log("The ad was successfully shown.");
                            //
                            // YOUR CODE TO REWARD THE GAMER
                            // Give coins etc.
                            // Unlock flash card
                            PlayerPrefs.SetString(name, name);
                            FlashCard.Instance.CheckFeatureUnlock();

                            UM_NotificationController.instance.ShowNotificationPoup("Unity Ads", "Dapat unlock " +  name + " flash card!");
                            break;
                        case (ShowResult.Failed):
                            Debug.Log("The ad was skipped before reaching the end.");
                            break;
                        case(ShowResult.Skipped):
                            Debug.LogError("The ad failed to be shown.");
                            break;
                    } 
                }
            };
            Advertisement.Show("rewardedVideo", options);
        }
    }


}
