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
        newContent.AddComponent<TextMeshPro>().text = incString;
        newContent.transform.parent = this.transform;
        newContent.name = "Content_" + incString;
        newContent.GetComponent<TextMeshPro>().margin = new Vector4(0, 0, -10.0f, 0);


        if (FindObjectOfType<CreditsController>().contentFont != null)
            newContent.GetComponent<TextMeshPro>().font = FindObjectOfType<CreditsController>().contentFont;
    }
}
