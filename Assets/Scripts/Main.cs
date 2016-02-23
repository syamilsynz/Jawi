using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
        // Load Playerpref data
        SaveManager.LoadData();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

    public void GoToScene(string name)
	{
//		Application.LoadLevel(1);

        SceneManager.LoadScene(name);
	}
        
}
