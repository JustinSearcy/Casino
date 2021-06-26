using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] GameObject parentCam = null;
    [SerializeField] float cameraBaseSize = 5f;
    [SerializeField] float cameraZoomSize = 2.5f;
    [SerializeField] float cameraZoomTime = 1f;

    public void ZoomCenter()
    {
        LeanTween.value(this.gameObject, cameraZoomSize, cameraBaseSize, cameraZoomTime).setOnUpdate(SetCameraSize);
        LeanTween.move(parentCam, Vector2.zero, cameraZoomTime);
    }

    public void ZoomTarget(Transform target)
    {
        LeanTween.value(this.gameObject, cameraBaseSize, cameraZoomSize, cameraZoomTime).setOnUpdate(SetCameraSize);
        LeanTween.move(parentCam, new Vector2(target.position.x, target.position.y), cameraZoomTime);
    }

    private void SetCameraSize(float value)
    {
        this.gameObject.GetComponent<Camera>().orthographicSize = value;
    }
}
