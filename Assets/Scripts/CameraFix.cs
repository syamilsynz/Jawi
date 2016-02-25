using UnityEngine;
using System.Collections;

public class CameraFix : MonoBehaviour {

    public float orthographicSize = 5;
    public float aspect = 1.33333f;

    void Start()
    {
        Camera.main.projectionMatrix = Matrix4x4.Ortho(
            -orthographicSize * aspect, orthographicSize * aspect,
            -orthographicSize, orthographicSize,
            GetComponent<Camera>().nearClipPlane, GetComponent<Camera>().farClipPlane);
    }

    void Update()
    {
        Camera.main.projectionMatrix = Matrix4x4.Ortho(
            -orthographicSize * aspect, orthographicSize * aspect,
            -orthographicSize, orthographicSize,
            GetComponent<Camera>().nearClipPlane, GetComponent<Camera>().farClipPlane);
    }
}
