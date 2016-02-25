using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class backButton : MonoBehaviour {
	public int sceneToStart = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void BackButton ()
	{
		SceneManager.LoadScene (sceneToStart);
	}
}
