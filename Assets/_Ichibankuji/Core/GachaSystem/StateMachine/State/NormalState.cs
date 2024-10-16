using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace Ichibankuji.Core.StateMachines
{
    [Serializable]
    public class NormalState : IState
    {
        private readonly int mainViewLayerValue = 1 << (LayerMask.NameToLayer("MainView"));
        private readonly int boardViewLayerValue = 1 << (LayerMask.NameToLayer("BoardView"));

        [NonSerialized]
        private GachaStateMachine stateMachine;
        [NonSerialized]
        private FocusState focusState;

        private Inputs1.GameplayActions gameplayInputs;
        private GameObject selectedTicket;
        private GameObject focusMask;
        [SerializeField]
        private ViewportMoveModule viewportMoveModule;
        private (CinemachineVirtualCamera TicketVCam, CinemachineVirtualCamera BoardVCam) vCams;
        private CinemachineVirtualCamera currentVCam => viewportMoveModule.VCam;
        
        public Action<GameObject> TicketSelected;
        public Action<GameObject> TicketDeselected;

        public NormalState(GachaStateMachine stateMachine, GachaSystem system){
            this.stateMachine = stateMachine;
            focusMask = system.FocusMask;
            gameplayInputs = new Inputs1().Gameplay;
            var actionsID = ViewportMoveModule.GenerateInputMap(gameplayInputs.MoveCameraStart.id,
                                                                gameplayInputs.Pointer.id,
                                                                gameplayInputs.MoveCameraEnd.id,
                                                                gameplayInputs.ScorllDelta.id);
            viewportMoveModule = new ViewportMoveModule(system.ViewportMoveModuleContent);
            viewportMoveModule.Initialize(actionsID, gameplayInputs);
            vCams = new(){ TicketVCam = system.ViewportMoveModuleContent.TicketVCam };
        }

        public void Enter(){
            EnableInputs();
            focusMask.SetActive(false);
            OnTicketDeselected();
        }

        public void Exit() => DisableInputs();

        public void FrameUpdate(){}
        public void PhysicsUpdate(){}
        public void LateUpdate() => viewportMoveModule.PositionUpdate();
        
        private void HandlePickUpTicket(){
            Vector2 pointPosition = gameplayInputs.Pointer.ReadValue<Vector2>();
            Ray ray = Camera.main.ScreenPointToRay(pointPosition);
            bool isTicket = Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, LayerMask.GetMask("MainView"))
                         && (hit.collider.CompareTag("Ticket") || hit.collider.CompareTag("TicketStub"));
            if(isTicket){
                var ticketObject = hit.transform.gameObject;
                if (IsSameTicket(ref ticketObject)) { return; }
                OnTicketDeselected();
                OnTicketSelected(ticketObject);
            }else{ OnTicketDeselected(); }
        }

        private void OnTicketSelected(GameObject hit){
            selectedTicket = hit;
            TicketSelected?.Invoke(selectedTicket);
            ToFocusState();
        }

        private void ToFocusState() => stateMachine.ChangeState(focusState);

        private void OnTicketDeselected(){
            if (selectedTicket == null) { return; }
            TicketDeselected?.Invoke(selectedTicket);
            selectedTicket = null;
        }

        private bool IsSameTicket(ref GameObject ticketObject) => 
            selectedTicket == ticketObject;
        
        private void Click(InputAction.CallbackContext context){
            HandlePickUpTicket();
        }

        public void Reset(){
            selectedTicket = null;
        }
        public void SetFocusState(FocusState focusState) => this.focusState = focusState;

        public void SwitchVCamTo(CinemachineVirtualCamera camera) =>
            viewportMoveModule.SetVCam(camera);
        public void SetTicketVCam(CinemachineVirtualCamera camera) =>
            vCams.TicketVCam = camera;
        public void SetBoardVCam(CinemachineVirtualCamera camera) =>
            vCams.BoardVCam = camera;
        
        public void ToTicket(){
            var mask = Camera.main.cullingMask;
            Camera.main.cullingMask += (mask & mainViewLayerValue) == 0 ? mainViewLayerValue : 0;
            Camera.main.cullingMask -= (mask & boardViewLayerValue) == 0 ? 0 : boardViewLayerValue;
            SwitchVCamTo(vCams.TicketVCam);
        }
        public void ToBoard(){
            var mask = Camera.main.cullingMask;
            Camera.main.cullingMask -= (mask & mainViewLayerValue) == 0 ? 0 : mainViewLayerValue;
            Camera.main.cullingMask += (mask & boardViewLayerValue) == 0 ? boardViewLayerValue : 0;
            SwitchVCamTo(vCams.BoardVCam);
        }
        public void SwitchView(){
            if(currentVCam == vCams.TicketVCam){ ToBoard(); }
            else if(currentVCam == vCams.BoardVCam){ ToTicket(); }
        }

        public void EnableInputs(){
            viewportMoveModule.Enable();
            gameplayInputs.Click.performed += Click;
            gameplayInputs.Enable();
        }
        public void DisableInputs(){
            gameplayInputs.Disable();
            gameplayInputs.Click.performed -= Click;
            viewportMoveModule.Disable();
        }

        public void PickByPosition(GachaSystem gachaSystem, Vector2Int position){
            var ticketObject = gachaSystem.CurrentPool.GetTicketObject(position);
            if(!ticketObject){
                Debug.Log($"{position} not exists.");
                return ;
            }
            selectedTicket = ticketObject;
            OnTicketSelected(ticketObject);
            focusState.AutomaticallyTearOut(gachaSystem, selectedTicket);
        }
        public void PickByRandom(GachaSystem gachaSystem){
            Transform TicketCollection = gachaSystem.GetComponent<CollectionManager>().TicketCollection;
            if(TicketCollection.childCount == 0){ return ; }
            int randomIndex = Random.Range(0, TicketCollection.childCount);
            selectedTicket = TicketCollection.GetChild(randomIndex).gameObject;
            OnTicketSelected(selectedTicket);
            focusState.AutomaticallyTearOut(gachaSystem, selectedTicket);
        }
    }
}