using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEditor;

/// <summary>
/// Created on: 01/08/20 by - Tyler J. Sims
/// "Simple" handler for a modular credits system!
/// 
/// Features:
///  * Placeholder
/// </summary>

public class CreditsController : MonoBehaviour
{ 
    public RectTransform header;              // HOLDS ALL THE OBJECTS THAT GET SORTED FOR THE CREDITS
    [Header("Standard Settings")]
    [SerializeField] float scrollSpeed = 85f; // HOW FAST IT SCROLLS ACROSS THE SCREEN

    [SerializeField] private bool doesLoop = true;
    [SerializeField] private float loopPoint;
    [SerializeField] private float startingPoint;
    
    [Range(0.025f,2f)]
    [SerializeField] float sectionGap = 1.0f;
    [Range(0.025f,2f)]
    [SerializeField] float contentGap = 0.75f;
    [Range(16,120)]
    [SerializeField] float sectionFontSize = 50.0f;
    [Range(16,120)]
    [SerializeField] float contentFontSize = 50.0f;
    
    
    [Header("Extra Settings")]
    [Range(24,250)]
    [SerializeField] float sectionHeight = 85.0f;
    [Range(24,250)]
    [SerializeField] float contentHeight = 85.0f;
    public TMP_FontAsset sectionFont;
    public TMP_FontAsset contentFont;


    [Header("Other Settings")] 
    [SerializeField] private bool useDisplayFlags;
    [SerializeField] string returnToScene;

    [Header("Playmode Status")] 
    [SerializeField] private RectTransform endPoint;

    [HideInInspector]
    public string textBox;

    private Transform _startPointDisplay;
    private Transform _loopPointDisplay;
    
    private void Start()
    {
        ResetHead();
    }

    void Update()
    {
        var pos = header.anchoredPosition; // Get header position
        pos.y += scrollSpeed * Time.deltaTime;    // Increase vertical height
        
        if (endPoint.position.y - (header.sizeDelta.y*1.5f) > loopPoint) // bruh, this jank af FIX ASAP
            if(doesLoop)
                pos.y = startingPoint;
            else Destroy(header.gameObject);
        
        header.anchoredPosition = pos;            // Apply movement
        
        
    }

    public void ResetHead()
    {
        SortCredits();
        header.anchoredPosition = new Vector2(header.anchoredPosition.x,startingPoint);
        useDisplayFlags = false;
        if(_startPointDisplay)
            Destroy(_startPointDisplay.gameObject);
        if(_loopPointDisplay)
            Destroy(_loopPointDisplay.gameObject);
    }
    
    // ORGANIZES ALL THE CREDITS IN A MODULAR FASHION
    public void SortCredits()
    {
        float p = 0; // Initial starting y position;
        RectTransform end = null;
        // find each child of header (layer 0) -- THIS is layer 1, section
        for (int i = 0; i < header.transform.childCount; i++)
        {
            RectTransform section = header.GetChild(i).GetComponent<RectTransform>();
            section.anchoredPosition = new Vector3(0,p,0);
                
            // section size
            SetHeight(section,sectionHeight);
            // section font size
            SetFontTypeSize(section, sectionFontSize, sectionFont);
            
            end = section;
            // find each child of section (layer 1) -- THIS is layer 2, content
            for (int j = 0; j < header.transform.GetChild(i).childCount; j++)
            {
                RectTransform content = header.GetChild(i).GetChild(j).GetComponent<RectTransform>();
                
                // content position
                p -= content.sizeDelta.y * contentGap;
                var newContentPosition = p;
                newContentPosition -= section.anchoredPosition.y;
                content.anchoredPosition = new Vector3(0,newContentPosition,0);
                
                // content size
                SetHeight(content,contentHeight);
                // content font size
                SetFontTypeSize(content, contentFontSize, contentFont);
                end = content;
            }
            
            p -= section.sizeDelta.y * sectionGap;
        }

        endPoint = end;
    }

    void SetHeight(RectTransform t, float height)
    {
        var newSize = t.sizeDelta;
        newSize.y = height;
        t.sizeDelta = newSize;
    }

    void SetFontTypeSize(RectTransform t, float fSize, TMP_FontAsset newFont = null)
    {
        var text = t.GetComponent<TextMeshProUGUI>();
        if (text)
        {
            text.fontSize = fSize;
            if (newFont)
                text.font = newFont;
        }
    }

    #region Util Functions
    public void AddSection(string incString)
    {
        GameObject newSection = new GameObject();
        newSection.AddComponent<RectTransform>();
        
        newSection.gameObject.AddComponent<CC_SectionUtil>();
        newSection.gameObject.AddComponent<TextMeshProUGUI>().text = incString;
        newSection.transform.parent = this.transform;
        newSection.transform.localScale = Vector3.one;

        var t = newSection.GetComponent<RectTransform>();
        var text = newSection.GetComponent<TextMeshProUGUI>();
        
        t.anchorMin = new Vector2(0,0.5f);
        t.anchorMax = new Vector2(1.0f,0.5f);
        t.anchoredPosition = Vector2.zero;
        t.offsetMin = new Vector2(0,100);
        t.offsetMax = new Vector2(0,0);
        
        newSection.name = "Section_" + incString;

        text.fontStyle = FontStyles.Underline;
        if (sectionFont != null)
            text.font = sectionFont;
        SortCredits();
    }
    public void ReloadItems()
    {
        for (int i = 0; i < transform.childCount; i++) // Section (I.E. Art, Design, etc...)
        {
            transform.GetChild(i).GetComponent<TextMeshProUGUI>().font = sectionFont;
            transform.GetChild(i).GetComponent<TextMeshProUGUI>().fontSize = sectionFontSize;

            for (int j = 0; j < transform.GetChild(i).childCount; j++) // Section Items (I.E. Tyler, SomeArt by PersonMan, etc...)
            {
                transform.GetChild(i).GetChild(j).GetComponent<TextMeshProUGUI>().fontSize = contentFontSize;

            }
        }
    }

    #if UNITY_EDITOR
    private void OnValidate()
    {
        if (useDisplayFlags)
        {
            if(!_startPointDisplay)
                _startPointDisplay = GameObject.FindWithTag("CC_StartPointFlag").transform;
            if(!_loopPointDisplay)
                _loopPointDisplay = GameObject.FindWithTag("CC_LoopPointFlag").transform;
            SortDisplayGizmos();
        }
        SortCredits();
    }

    void SortDisplayGizmos()
    {
        var loopTransform = _loopPointDisplay.GetComponent<RectTransform>();
        var startTransform = _startPointDisplay.GetComponent<RectTransform>();

        var pos = header.anchoredPosition;
        loopTransform.anchoredPosition = new Vector2(pos.x, loopPoint);
        startTransform.anchoredPosition = new Vector2(pos.x, startingPoint);
    }
    
#endif
    
    #endregion

    public void ReturnToMainMenu()
    {
        // try to get valid scene from 'returnToScene', otherwise default to 0 in the build index
        if (returnToScene == "" || returnToScene == null || !SceneManager.GetSceneByName(returnToScene).IsValid())
            SceneManager.LoadScene(0);
        else SceneManager.LoadScene(returnToScene);
    }
}
