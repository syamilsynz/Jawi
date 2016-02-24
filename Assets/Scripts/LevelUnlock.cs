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

        this.transform.FindChild("Text").gameObject.GetComponent<Text>().text = levelId.ToString();

        CheckLevelUnlock(levelId);
	}
	
    public void CheckLevelUnlock(string levelId)
    {
        if (PlayerPrefs.HasKey(levelId))
        {
            this.GetComponent<Button>().interactable = true;

            // Change the color of the button
            this.transform.FindChild("ImageUnlock").gameObject.SetActive(true);
            this.transform.FindChild("ImageLock").gameObject.SetActive(false);
        }
        else
        {
            this.GetComponent<Button>().interactable = false;

            // Change the color of the button
            this.transform.FindChild("ImageUnlock").gameObject.SetActive(false);
            this.transform.FindChild("ImageLock").gameObject.SetActive(true);
        }
    }
}
