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

    public void SetParentActiveTrue(GameObject go)
    {
        go.transform.parent.gameObject.SetActive(true);
    }

    public void SetParentActiveFalse(GameObject go)
    {
        go.transform.parent.gameObject.SetActive(false);
    }

    // This

    public void SetThisActiveTrue()
    {
        this.gameObject.SetActive(true);
    }

    public void SetThisActiveFalse()
    {
        this.gameObject.SetActive(false);
    }

    public void SetThisParentActiveTrue()
    {
        this.transform.parent.gameObject.SetActive(true);
    }

    public void SetThisParentActiveFalse()
    {
        this.transform.parent.gameObject.SetActive(false);
    }
}
