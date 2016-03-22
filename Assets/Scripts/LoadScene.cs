using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour 
{
    public void LoadLevel(int levelID)
    {
        SceneManager.LoadScene(levelID);
    }

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    //--- Fade Animation Load Scene---

    public Animator fadeImage;
    public AnimationClip fadeColorAnimationClip;        
    private string sceneName;
    public float fadeDelay = 0.5f;

    public void LoadLevelWithFadeAnimation(string strSceneName)
    {
        sceneName = strSceneName;
        //Use invoke to delay calling of LoadDelayed by half the length of fadeColorAnimationClip
        Invoke ("LoadDelayed", fadeColorAnimationClip.length * fadeDelay);

        //Set the trigger of Animator animColorFade to start transition to the FadeToOpaque state.
        fadeImage.SetTrigger ("fade");
    }

    public void LoadDelayed()
    {
        //Load the selected scene, by scene index number in build settings
        SceneManager.LoadScene (sceneName);
        StartCoroutine(WaitBeforeDestroy());
    }

    IEnumerator WaitBeforeDestroy()
    {
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }
}
