using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DropdownTry : MonoBehaviour
{
    public Image image;
    public TMP_Dropdown dropdown;
    [ReadOnly]
    public TMP_Dropdown.OptionData myOption;

    private void Awake(){
        myOption = new TMP_Dropdown.OptionData("One Piece");

        dropdown.options.Add(myOption);
    }

    public void OnDropdownValueChanged(int index){
        image.sprite = dropdown.options[index].image;
    }
}
