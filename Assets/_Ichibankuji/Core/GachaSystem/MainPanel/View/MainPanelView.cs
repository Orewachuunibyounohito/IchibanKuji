using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ichibankuji.Core.UI
{
    [Serializable]
    public class MainPanelView
    {
        public Image Bg;
        public Button DisplayButton;
        public Button PoolButton;
        public Button TicketOrBoardButton;
        public Button ComputerPickButton;
        public Button PickByPositionButton;
        public Button PickByRandomButton;
        public Button PositionConfirmButton;
        public Button PositionCancelButton;
        public TMP_InputField PositionXInput;
        public TMP_InputField PositionYInput;
        public TMP_Dropdown PoolDropdown;

        public MainPanelView(Transform presenterTrans)
        {
            Bg = presenterTrans.Find("BG").GetComponent<Image>();
            PoolButton = presenterTrans.Find("PoolButton").GetComponent<Button>();
            DisplayButton = presenterTrans.Find("DisplayButton").GetComponent<Button>();
            TicketOrBoardButton = presenterTrans.Find("TicketOrBoardButton").GetComponent<Button>();
            ComputerPickButton = presenterTrans.Find("ComputerPickButton").GetComponent<Button>();
            PoolDropdown = presenterTrans.Find("PoolDropdown").GetComponent<TMP_Dropdown>();

            Transform selectionPanel = presenterTrans.transform.Find("SelectionPanel");
            PickByPositionButton = selectionPanel.Find("PositionButton").GetComponent<Button>();
            PickByRandomButton = selectionPanel.Find("RandomButton").GetComponent<Button>();

            Transform positionPanel = presenterTrans.transform.Find("PositionPanel");
            PositionXInput = positionPanel.Find("XField/XInput").GetComponent<TMP_InputField>();
            PositionYInput = positionPanel.Find("YField/YInput").GetComponent<TMP_InputField>();
            PositionConfirmButton = positionPanel.Find("Confirm").GetComponent<Button>();
            PositionCancelButton = positionPanel.Find("Cancel").GetComponent<Button>();

            ComputerPickButton.interactable = false;

            PoolButton.onClick.AddListener(OnPoolButtonClicked);
            PoolDropdown.onValueChanged.AddListener(OnPoolDropdownValueChanged());
            ComputerPickButton.onClick.AddListener(ActivePanel(selectionPanel.gameObject));
            PickByPositionButton.onClick.AddListener(ActivePanel(positionPanel.gameObject));
            PickByPositionButton.onClick.AddListener(DeactivePanel(selectionPanel.gameObject));
            PickByRandomButton.onClick.AddListener(DeactivePanel(selectionPanel.gameObject));
            PositionConfirmButton.onClick.AddListener(DeactivePanel(selectionPanel.gameObject));
            PositionConfirmButton.onClick.AddListener(DeactivePanel(positionPanel.gameObject));
            PositionCancelButton.onClick.AddListener(DeactivePanel(selectionPanel.gameObject));
            PositionCancelButton.onClick.AddListener(DeactivePanel(positionPanel.gameObject));
        }

        private void OnPoolButtonClicked() => PoolDropdown.gameObject.SetActive(true);
        private UnityAction<int> OnPoolDropdownValueChanged() =>
            (index) => {
                PoolDropdown.gameObject.SetActive(false);
                PoolButton.interactable = false;
            };
        
        public void ChangePoolSpriteSwap(SpriteSwapContent sprites){
            PoolButton.image.sprite = sprites.Normal;
            PoolButton.spriteState = sprites.CreateSpriteState();
        }
        public void ChangeComputerPickSpriteSwap(SpriteSwapContent sprites){
            ComputerPickButton.image.sprite = sprites.Normal;
            ComputerPickButton.spriteState = sprites.CreateSpriteState();
        }
        public void ChangeTicketOrBoardSpriteSwap(SpriteSwapContent sprites){
            TicketOrBoardButton.image.sprite = sprites.Normal;
            TicketOrBoardButton.spriteState = sprites.CreateSpriteState();
        }

        public Vector2Int GetPosition() => new Vector2Int(int.Parse(PositionXInput.text) - 1, int.Parse(PositionYInput.text) - 1);

        private UnityAction ActivePanel(GameObject targetPanel) =>
            () => targetPanel.SetActive(true);
        private UnityAction DeactivePanel(GameObject targetPanel) =>
            () => targetPanel.SetActive(false);
    }
}