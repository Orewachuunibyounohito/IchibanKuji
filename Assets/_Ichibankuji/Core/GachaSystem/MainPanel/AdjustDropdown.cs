using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TMP_Dropdown))]
public class AdjustDropdown : MonoBehaviour
{
    private const int DEFAULT_DROPDOWN_HEIGHT = 20;
    private const int DEFAULT_DROPDOWN_TEMPLATE_HEIGHT = 28;
    [SerializeField]
    private TMP_Dropdown dropdown;

    private void Awake(){
        dropdown = GetComponent<TMP_Dropdown>();
        var adjustHeightFactor = dropdown.GetComponent<RectTransform>().rect.height / DEFAULT_DROPDOWN_HEIGHT;
        // var adjustHeight = dropdown.GetComponent<RectTransform>().rect.height;
        var content = dropdown.template.GetComponent<ScrollRect>().content;
        content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, adjustHeightFactor * DEFAULT_DROPDOWN_TEMPLATE_HEIGHT);
        content.Find("Item")
               .GetComponent<RectTransform>()
               .SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, adjustHeightFactor * DEFAULT_DROPDOWN_HEIGHT);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
