using UnityEngine;
using System.Collections;

public class DontDestroy : MonoBehaviour 
{
    private static bool created = false;

    void Awake()
    {
        
    }

	void Start()
	{
//        if (!created) 
//        {
//            // this is the first instance - make it persist
//            DontDestroyOnLoad(this.gameObject);
//            created = true;
//        } 
//        else 
//        {
//            // this must be a duplicate from a scene reload - DESTROY!
//            Destroy(this.gameObject);
//        }         

		//Causes UI object not to be destroyed when loading a new scene. If you want it to be destroyed, destroy it manually via script.
		DontDestroyOnLoad(this.gameObject);
	}

	

}
