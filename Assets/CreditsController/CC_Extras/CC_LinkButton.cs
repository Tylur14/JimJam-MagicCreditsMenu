using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

/// <summary>
/// https://stackoverflow.com/questions/7578857/how-to-check-whether-a-string-is-a-valid-http-url
///
/// Features:
///  * Verifies URL is valid
///  * Highlights Text while cursor is hovering over
///  * optional audio clip override
/// </summary>
public class CC_LinkButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [TextArea]
    [SerializeField] string URL;
    
    
    [Header("Button Status")]
    [SerializeField] private bool _ready;

    
    [Header("Button Settings")]
    [SerializeField] Color cNormal =        // the base color to lerp to when the cursor is NOT over the button
        new Color(.75f,.75f,.75f,.75f);
    
    [SerializeField] Color cHighlighted =   // the color to lerp to when the cursor is over the button 
        new Color(1,1,1,1);
    
    [SerializeField] float tintTime = 5.0f; // how quickly the text object changes color
    
    
    [Header("Extra Settings")]
    [SerializeField] AudioClip interactSFX;
    
    // Optional pointer events
    [SerializeField] private UnityEvent pointerEnterEvents; 
    [SerializeField] private UnityEvent pointerExitEvents;
     
    // general references
    private TextMeshProUGUI _text;
    private AudioSource _audio;

    private void Start()
    {
        _audio = FindObjectOfType<AudioSource>();
        _text = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (_ready && Input.GetMouseButtonDown(0))
            Click();

        if (_text)
            SetTextColor();
    }

    void SetTextColor()
    {
        var t = tintTime * Time.deltaTime;
        _text.color = _ready ? Color.Lerp(_text.color, cHighlighted, t) : _text.color = Color.Lerp(_text.color, cNormal, t); 
    }
    
    void Click()
    {
        // Play click SFX
        if (_audio)
            if (interactSFX)
                _audio.PlayOneShot(interactSFX);
            else _audio.PlayOneShot(_audio.clip);
        
        // Verify if URL is valid
        if(Uri.IsWellFormedUriString(URL, UriKind.Absolute))
            Application.OpenURL(URL); // if so, open URL in default internet browser
        else 
            Debug.LogError(this.gameObject.name + " has an invalid URL! Please verify that it is a complete URL!"); // otherwise, throw error
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _ready = true;
        pointerEnterEvents.Invoke();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        _ready = false;
        pointerExitEvents.Invoke();
    }
}
