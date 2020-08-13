using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CC_SectionUtil : MonoBehaviour
{
    //[SerializeField] List<GameObject> contentItems = new List<GameObject>(); // may use in the future for something but prolly not
    [HideInInspector]
    public string textBox = "";

    public void AddContent(string incString)
    {
        GameObject newContent = new GameObject();
        newContent.AddComponent<TextMeshProUGUI>().text = incString;
        newContent.transform.parent = this.transform;
        newContent.name = "Content_" + incString;
        newContent.GetComponent<TextMeshProUGUI>().margin = new Vector4(0, 0, -10.0f, 0);
        newContent.transform.localScale = Vector3.one;

        var t = newContent.GetComponent<RectTransform>();
        var text = newContent.GetComponent<TextMeshProUGUI>();
        var controller = FindObjectOfType<CreditsController>();
        
        t.anchorMin = new Vector2(0,0.5f);
        t.anchorMax = new Vector2(1.0f,0.5f);
        t.anchoredPosition = Vector2.zero;
        t.offsetMin = new Vector2(0,100);
        t.offsetMax = new Vector2(0,0);
        
        newContent.name = "Section_" + incString;

        if (controller.contentFont != null)
            text.font = controller.contentFont;
        
        controller.SortCredits();
        controller.SortCredits();
        print("Added section");
    }
}
