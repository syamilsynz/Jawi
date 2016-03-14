using UnityEngine;
using System.Collections;

public class SaveManager : MonoBehaviour 
{
    public static int coinAmount = 100;       // Total coin amount player have
    public static int categoryID = 1;       // default category ID
    public static int score = 0;            // player score
    public static int questionID = 1;       // question id for current category
    public static string levelName;
    public static int levelId;
    public static int timerHighScore;
    public static int audioEnabled = 1;

    public void SetQuestionCategory(int id)
    {
        PlayerPrefs.SetInt("Category", id);
        categoryID = id;
        SaveData();
    }

    public void SetQuestionLevel(string name)
    {
        PlayerPrefs.SetString("Level Name", name);
        levelName = name;
        SaveData();
    }

    public void SetLevel(int level)
    {
        PlayerPrefs.SetInt("Level ID", level);
        levelId = level;
        SaveData();
    }

    //Loads the player data
    public static void LoadData()
    {
        // Delete all data
        //      PlayerPrefs.DeleteAll();

        //If found the coin ammount data, load the data
        if (!PlayerPrefs.HasKey("Coin Amount"))
            SaveData();
        else
        {
            coinAmount = PlayerPrefs.GetInt("Coin Amount");
            categoryID = PlayerPrefs.GetInt("Category");
            score = PlayerPrefs.GetInt("Score");
            questionID = PlayerPrefs.GetInt("Question ID");
            levelName = PlayerPrefs.GetString("Level Name");
            levelId = PlayerPrefs.GetInt("Level ID");
            timerHighScore = PlayerPrefs.GetInt("Timer High Score");
            audioEnabled = PlayerPrefs.GetInt("AudioEnabled");
        }

        PlayerPrefs.Save();
    }

    //Saves the player data
    public static void SaveData()
    {
        PlayerPrefs.SetInt("Coin Amount", coinAmount);
        PlayerPrefs.SetInt("Category", categoryID);
        PlayerPrefs.SetInt("Score", score);
        PlayerPrefs.SetInt("Question ID", questionID);
        PlayerPrefs.SetString("Level Name", levelName);
        PlayerPrefs.SetInt("Level ID", levelId);
        PlayerPrefs.SetInt("Timer High Score", timerHighScore);
        PlayerPrefs.SetInt("AudioEnabled", audioEnabled);

        PlayerPrefs.Save();
    }

    public void SetQuestionId(int id)
    {
        PlayerPrefs.SetInt("Question ID", id);
        questionID = id;
        SaveData();
    }

	
}
