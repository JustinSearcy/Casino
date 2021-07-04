using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerDeath : MonoBehaviour
{
    [SerializeField] float zoomTimeBuffer = 0.5f;
    [SerializeField] float initialVignette = 0.14f;
    [SerializeField] float finalVignette = 0.48f;
    [SerializeField] Volume volume;

    public void Death()
    {
        StartCoroutine(DeathSequence());
    }

    private IEnumerator DeathSequence()
    {
        yield return new WaitForSeconds(zoomTimeBuffer);
        CameraZoom camZoom = FindObjectOfType<CameraZoom>();
        camZoom.ZoomPlayerDeath(FindObjectOfType<ChipSystem>().gameObject.transform);
        LeanTween.value(this.gameObject, initialVignette, finalVignette, camZoom.cameraZoomTimePlayerDeath + zoomTimeBuffer).setOnUpdate(SetVignette);
        yield return new WaitForSeconds(camZoom.cameraZoomTimePlayerDeath + zoomTimeBuffer);
    }

    private void SetVignette(float value)
    {
        if(volume.profile.TryGet<Vignette>(out var vignette))
        {
            vignette.intensity.value = value;
        }
    }
}
