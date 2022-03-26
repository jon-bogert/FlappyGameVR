using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
 
public class UIButtonSoundEvent : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{

    AudioManager audioManager;

    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void OnPointerEnter( PointerEventData ped ) {
        audioManager.Play("Button Highlight");
    }
 
    public void OnPointerDown( PointerEventData ped ) {
        audioManager.Play("Button Click");
    }    
}
