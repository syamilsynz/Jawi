using UnityEngine;
using System.Collections;

public class SetActive : MonoBehaviour 
{

    public void SetActiveTrue(GameObject go)
    {
        go.gameObject.SetActive(true);
    }

    public void SetActiveFalse(GameObject go)
    {
        go.gameObject.SetActive(false);
    }
}
