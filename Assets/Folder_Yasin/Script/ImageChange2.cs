using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class ImageChange2 : MonoBehaviour {

	public GameObject imageObject;
	public Sprite image1st;
	public Sprite image2nd;
	public float duration;
	public bool ignoreTimeScale;
	public bool useAlpha;
	public Animation anim;
	//public int sceneToStart = 1;

	// Use this for initialization
	void Start () {
		//StartCoroutine (FadeToBlack ());
	}


	IEnumerator FadeToBlack ()
	{
		

		yield return new WaitForSeconds(duration);

		imageObject.GetComponent<Image> ().sprite = image1st;
		imageObject.GetComponent<Image> ().CrossFadeColor (Color.black, duration, ignoreTimeScale, useAlpha);

		yield return new WaitForSeconds(duration);

		SceneManager.LoadScene (1);
	}

	public void OnCLick ()
	{
		//GetComponent<Animation> ().Play();
		StartCoroutine (FadeToBlack ());
	}


}