using System;
using System.Collections.Generic;
using Ichibankuji.SO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Ichibankuji.Core.UI
{
    public class MainPanelPresenter : MonoBehaviour
    {
        public MainPanelModel Model;
        public MainPanelView View;

        public void Initialize(List<TMP_Dropdown.OptionData> options)
        {
            var config = Resources.Load<MainPanelConfig>(ResourcesPath.MAIN_PANEL_CONFIG);
            Model = new MainPanelModel();
            View = new MainPanelView(transform);
            Model.OnPoolButtonSpritesChanged += View.ChangePoolSpriteSwap;
            Model.OnComputerPickButtonSpritesChanged += View.ChangeComputerPickSpriteSwap;
            Model.OnTicketOrBoardButtonSpritesChanged += View.ChangeTicketOrBoardSpriteSwap;
            Model.OnAddOptions += View.PoolDropdown.AddOptions;
            Model.OnAddOption += View.PoolDropdown.options.Add;
            Model.AddOptions(options);
            Model.AssignSpriteSwap(config);
        }

        public void ActivatePoolButton() => View.PoolButton.interactable = true;
        public void ActivateComputerPickButton() => View.ComputerPickButton.interactable = true;
        public void DeactivateComputerPickButton() => View.ComputerPickButton.interactable = false;
        public void TicketOrBoardSpriteSwitch() => Model.TicketOrBoardSpriteSwitch();
        public void TicketOrBoardSpriteReset() => Model.ChangeTicketOrBoardButtonSprites(MainPanelModel.TicketOrBoard.Board);
        public void RegisterValueChangedEvent(UnityAction<int> call) =>
            View.PoolDropdown.onValueChanged.AddListener(call);
        public void RegisterComputerPickClickedEvent(UnityAction clickedAction) =>
            View.ComputerPickButton.onClick.AddListener(clickedAction);
        public void RegisterPickByRandomClickedEvent(UnityAction clickedAction) =>
            View.PickByRandomButton.onClick.AddListener(clickedAction);
        public void RegisterPositionConfirmClickedEvent(UnityAction clickedAction) =>
            View.PositionConfirmButton.onClick.AddListener(clickedAction);
        public void RegisterPositionCancelClickedEvent(UnityAction clickedAction) => 
            View.PositionCancelButton.onClick.AddListener(clickedAction);
        public void RegisterTicketOrBoardClickedEvent(UnityAction clickedAction) =>
            View.TicketOrBoardButton.onClick.AddListener(clickedAction);

        public Vector2Int GetPosition() => View.GetPosition();

    }
}
