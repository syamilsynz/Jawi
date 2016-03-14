using UnityEngine;
using System.Collections;

public class GPGSManager : MonoBehaviour 
{
    private const string achievement_new_adventure = "New Adventure"; // UM - Achievement ID
    private const string achievement_hall_of_fame = "Hall of Fame"; //  UM - Achievement ID
    private const string achievement_ultimate_flash_card = "Ultimate Flash Card"; // UM - Achievement ID
    private const string achievement_unlock_all_flash_card = "Unlock All Flash Card"; // UM - Achievement ID
    private const string achievement_landing_time = "Landing Time"; // UM - Achievement ID
    private const string achievement_knowledgeable = "Knowledgeable"; // UM - Achievement ID
    private const string achievement_harta_qarun = "Harta Qarun"; // UM - Achievement ID
    private const string achievement_awan_awangan = "Awan Awangan"; // UM - Achievement ID
    private const string achievement_first_exclusive_pouch = "Your First Exclusive Pouch"; // UM - Achievement ID
    private const string achievement_open_one_letter = "Open One Letter"; // UM - Achievement ID
    private const string achievement_remove_unwanted_letter = "Remove Unwanted Letter"; // UM - Achievement ID
    private const string achievement_solve_question = "Solve Question"; // UM - Achievement ID


    public int knowledgeableValue = 3;
    public int hartaQarunValue = 10;

    private const string leaderboardHallofFame = "leaderboard_hall_of_fame";

    static GPGSManager instance;
    public static GPGSManager Instance { get { return instance; } }

    void Awake()
    {
        instance = this;
    }

	// Use this for initialization
	void Start () 
    {


        if(UM_GameServiceManager.instance.ConnectionSate == UM_ConnectionState.CONNECTED) 
        {
            // Check Player Property
            if(UM_GameServiceManager.Instance.Player != null) 
            {
                Debug.Log ("Welcome Google Play User \nID: " + UM_GameServiceManager.Instance.Player.PlayerId);
                Debug.Log ("Name: " +  UM_GameServiceManager.Instance.Player.Name);

                //              if(UM_GameServiceManager.Instance.Player.SmallPhoto != null) {
                //                  GUI.DrawTexture(new Rect(10, 10, 75, 75), UM_GameServiceManager.Instance.Player.SmallPhoto);
                //              }
            }
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    //------------- ACHIEVEMENT ---------------//

    public void ShowAchievement()
    {
        UM_GameServiceManager.instance.ShowAchievementsUI();
    }

    public void Achievement_New_Adventure()
    {
        if(UM_GameServiceManager.instance.ConnectionSate == UM_ConnectionState.CONNECTED) 
        {
            UM_GameServiceManager.instance.UnlockAchievement(achievement_new_adventure);
        }
    }

    public void Achievement_Hall_of_Fame()
    {
        if(UM_GameServiceManager.instance.ConnectionSate == UM_ConnectionState.CONNECTED) 
        {
            UM_GameServiceManager.instance.UnlockAchievement(achievement_hall_of_fame);
        }
    }

    public void Achievement_Ultimate_Flash_Card()
    {
        if(UM_GameServiceManager.instance.ConnectionSate == UM_ConnectionState.CONNECTED) 
        {
            UM_GameServiceManager.instance.UnlockAchievement(achievement_ultimate_flash_card);
        }
    }

    public void Achievement_Unlock_All_Flash_Card()
    {
        if(UM_GameServiceManager.instance.ConnectionSate == UM_ConnectionState.CONNECTED) 
        {
            UM_GameServiceManager.instance.UnlockAchievement(achievement_unlock_all_flash_card);
        }
    }

    public void Achievement_Landing_Time()
    {
        if(UM_GameServiceManager.instance.ConnectionSate == UM_ConnectionState.CONNECTED) 
        {
            UM_GameServiceManager.instance.UnlockAchievement(achievement_landing_time);
        }
    }

    public void Achievement_Knowledgeable()
    {
        if(UM_GameServiceManager.instance.ConnectionSate == UM_ConnectionState.CONNECTED) 
        {
            UM_GameServiceManager.instance.UnlockAchievement(achievement_knowledgeable);
        }
    }

    public void Achievement_Harta_Qarun()
    {
        if(UM_GameServiceManager.instance.ConnectionSate == UM_ConnectionState.CONNECTED) 
        {
//            if (coinReceive >= hartaQarunValue)
                UM_GameServiceManager.instance.UnlockAchievement(achievement_harta_qarun);
        }
    }

    public void Achievement_Awan_Awangan()
    {
        if(UM_GameServiceManager.instance.ConnectionSate == UM_ConnectionState.CONNECTED) 
        {
            UM_GameServiceManager.instance.UnlockAchievement(achievement_awan_awangan);
        }
    }

    public void Achievement_First_Exclusive_Pouch()
    {
        if(UM_GameServiceManager.instance.ConnectionSate == UM_ConnectionState.CONNECTED) 
        {
            UM_GameServiceManager.instance.UnlockAchievement(achievement_first_exclusive_pouch);
        }
    }

    public void Achievement_Open_One_Letter()
    {
        if(UM_GameServiceManager.instance.ConnectionSate == UM_ConnectionState.CONNECTED) 
        {
            UM_GameServiceManager.instance.UnlockAchievement(achievement_open_one_letter);
        }
    }

    public void Achievement_Remove_Unwanted_Letter()
    {
        if(UM_GameServiceManager.instance.ConnectionSate == UM_ConnectionState.CONNECTED) 
        {
            UM_GameServiceManager.instance.UnlockAchievement(achievement_remove_unwanted_letter);
        }
    }

    public void Achievement_Solve_Question()
    {
        if(UM_GameServiceManager.instance.ConnectionSate == UM_ConnectionState.CONNECTED) 
        {
            UM_GameServiceManager.instance.UnlockAchievement(achievement_solve_question);
        }
    }

    //------------- LEADERBOARD ---------------//

    public void PostScoreToLeaderboard(int score)
    {
        if(UM_GameServiceManager.instance.ConnectionSate == UM_ConnectionState.CONNECTED) 
        {
            UM_GameServiceManager.ActionScoreSubmitted += HandleActionScoreSubmitted;
            UM_GameServiceManager.instance.SubmitScore(leaderboardHallofFame, score);
        }

    }

    public void ShowLeaderboard()
    {
      if(UM_GameServiceManager.instance.ConnectionSate == UM_ConnectionState.CONNECTED) 
      {
        UM_GameServiceManager.instance.ShowLeaderBoardsUI();
      }
    }

    public void ShowLeaderboardBasedOnID(string leaderboardId)
    {
        if(UM_GameServiceManager.instance.ConnectionSate == UM_ConnectionState.CONNECTED) 
        {
            UM_GameServiceManager.instance.ShowLeaderBoardUI(leaderboardId);
        }
    }

    // [Start] Ultimate Mobile -  Game Service
    void HandleActionScoreSubmitted (UM_LeaderboardResult res) {
        if(res.IsSucceeded) {
            UM_Score playerScore = res.Leaderboard.GetCurrentPlayerScore(UM_TimeSpan.ALL_TIME, UM_CollectionType.GLOBAL);
            Debug.Log("Score submitted, new player high score: " + playerScore.LongScore);
        } else {
            Debug.Log("Score submission failed: " + res.Error.Code + " / " + res.Error.Description);
        }

    }

    private void OnPlayerConnected() {
        Debug.Log("Player Connected to Game Services");
    }


    private void OnPlayerDisconnected() {
        Debug.Log("Player Disconnected to Game Services");
    }
    // [End] Ultimate Mobile -  Game Service
}
