using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler {

    public void OnPointerEnter(PointerEventData ped) {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SOUNDFX/sfx_confirmClick");

    }

    public void OnPointerDown(PointerEventData ped) {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SOUNDFX/sfx_confirmClick");
    }
}