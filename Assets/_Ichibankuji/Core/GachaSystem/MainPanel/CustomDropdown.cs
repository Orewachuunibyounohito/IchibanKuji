using TMPro;
using UnityEngine.EventSystems;

public class CustomDropdown : TMP_Dropdown
{
    public override void Select()
    {
        base.Select();
        gameObject.SetActive(false);
    }
}
