using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelUnlock : MonoBehaviour 
{
    public string levelId;
	// Use this for initialization
	void Start () 
    {
        PlayerPrefs.SetString("1","1");
        CheckLevelUnlock(levelId);
	}
	
    public void CheckLevelUnlock(string levelId)
    {
        if (PlayerPrefs.HasKey(levelId))
        {
            this.GetComponent<Button>().interactable = true;
        }
        else
        {
            this.GetComponent<Button>().interactable = false;
        }
    }
}
