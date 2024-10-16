using System.Collections.Generic;
using Ichibankuji.Core;
using Ichibankuji.Core.UI;
using TMPro;
using UnityEngine;

public class MainPanelGenerator
{
    public static MainPanelPresenter GenerateMainPanelPresenterForGachaSystem(GameObject mainPanel, List<TMP_Dropdown.OptionData> options){
        var presenter = mainPanel.AddComponent<MainPanelPresenter>();
        presenter.Initialize(options);
        return presenter;
    }
    public static MainPanelPresenter GenerateMainPanelPresenterForGachaSystem(GachaSystem system, GameObject mainPanel, List<TMP_Dropdown.OptionData> options){
        var presenter = mainPanel.AddComponent<MainPanelPresenter>();
        presenter.Initialize(options);
        system.OnPoolDone += presenter.ActivatePoolButton;
        system.OnPoolDone += presenter.DeactivateComputerPickButton;
        system.SystemReset += presenter.ActivateComputerPickButton;
        system.SystemReset += presenter.TicketOrBoardSpriteReset;
        system.stateMachine.FocusState.OnEnter += presenter.DeactivateComputerPickButton;
        system.stateMachine.FocusState.OnExit += presenter.ActivateComputerPickButton;
        presenter.RegisterComputerPickClickedEvent(system.stateMachine.NormalState.DisableInputs);
        presenter.RegisterPickByRandomClickedEvent(system.stateMachine.NormalState.EnableInputs);
        presenter.RegisterPositionConfirmClickedEvent(system.stateMachine.NormalState.EnableInputs);
        presenter.RegisterPositionCancelClickedEvent(system.stateMachine.NormalState.EnableInputs);
        return presenter;
    }
}
