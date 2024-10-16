using System;
using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace Ichibankuji.Core.StateMachines
{
    [Serializable]
    public class FocusState : IState
    {
        private const int ANIMATION_FINISHED_TIME = 1;

        [NonSerialized]
        private GachaStateMachine stateMachine;
        [NonSerialized]
        private NormalState normalState;

        private Inputs1.GameplayActions gameplayInputs;
        private GameObject focusMask;
        private GameObject selectedTicket;
        private GameObject selectedStub;
        private float prevX;
        [SerializeField]
        private float tearOutMultiplier;
        private bool isTearingOut;

        public Action<GameObject> RedeemReward;
        public Action OnEnter, OnExit;

        public FocusState(GachaStateMachine stateMachine, GachaSystem system){
            this.stateMachine = stateMachine;
            focusMask = system.FocusMask;
            tearOutMultiplier = system.GetFocusContent();
            gameplayInputs = new Inputs1().Gameplay;
        }

        public void Enter(){
            gameplayInputs.Click.performed += Click;
            gameplayInputs.Release.performed += Release;
            gameplayInputs.Enable();

            focusMask.SetActive(true);
            OnEnter.Invoke();
        }

        public void Exit(){
            gameplayInputs.Disable();
            gameplayInputs.Click.performed -= Click;
            gameplayInputs.Release.performed -= Release;

            focusMask.SetActive(false);
            OnExit.Invoke();
        }

        public void FrameUpdate(){}
        public void LateUpdate(){}
        public void PhysicsUpdate(){
            if(isTearingOut){
                HandleTearOut();
            }
        }

        private void Click(InputAction.CallbackContext context){
            if(!isTearingOut){
                HandleFocusMode();
            }
        }

        private void Release(InputAction.CallbackContext context){
            if(isTearingOut){ isTearingOut = false; }
        }

        private void HandleTearOut(){
            var stubAnimator = selectedStub.GetComponent<Animator>();
            var currentTearOut = stubAnimator.GetFloat("TearOut");
            if(currentTearOut == ANIMATION_FINISHED_TIME){
                isTearingOut = false;
                RedeemReward?.Invoke(selectedTicket);
                ToNormalState();
                return ;
            }

            Vector2 pointerPosition = gameplayInputs.Pointer.ReadValue<Vector2>();
            var deltaX = pointerPosition.x - prevX;
            var tearOutFloat = deltaX * tearOutMultiplier;
            stubAnimator.SetFloat("TearOut", Mathf.Clamp(currentTearOut+tearOutFloat, 0, 1f));
            prevX = pointerPosition.x;
        }

        private void ToNormalState(){
            stateMachine.ChangeState(normalState);
        }

        private void HandleFocusMode(){
            Vector2 pointerPosition = gameplayInputs.Pointer.ReadValue<Vector2>();
            Ray ray = Camera.main.ScreenPointToRay(pointerPosition);
            bool isTicketStub = Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, LayerMask.GetMask("MainView"))
                             && hit.collider.CompareTag("TicketStub");
            if(isTicketStub){
                selectedStub = hit.collider.gameObject;
                selectedTicket = selectedStub.transform.parent.gameObject;
                prevX = pointerPosition.x;
                isTearingOut = true;
            }else{ ToNormalState(); }
        }

        public void SetNormalState(NormalState normalState) => this.normalState = normalState;

        public void AutomaticallyTearOut(MonoBehaviour mono, GameObject ticketObject){
            selectedTicket = ticketObject;
            var stubObject = selectedTicket.transform.Find("Stub").gameObject;
            selectedTicket.transform.Find("TicketObject").gameObject.layer = 0;
            stubObject.transform.Find("StubObject").gameObject.layer = 0;
            mono.StartCoroutine(AutomaticallyTearOutTask(stubObject));
        }
        private IEnumerator AutomaticallyTearOutTask(GameObject stubObject){
            DisableInputs();
            var duration = 2f;
            var startTime = Time.time;
            var timer = 0.0f;
            var stubAnimator = stubObject.GetComponent<Animator>();
            var currentTearOut = stubAnimator.GetFloat("TearOut");
            while(currentTearOut < ANIMATION_FINISHED_TIME){
                yield return null;
                timer = Time.time - startTime;
                currentTearOut = Mathf.Min(timer / duration, ANIMATION_FINISHED_TIME);
                stubAnimator.SetFloat("TearOut", currentTearOut);
            }
            RedeemReward?.Invoke(selectedTicket);
            Reset();
            ToNormalState();
        }

        public void Reset(){
            selectedTicket = null;
            selectedStub = null;
        }
        public void EnableInputs(){
            gameplayInputs.Click.performed += Click;
            gameplayInputs.Release.performed += Release;
            gameplayInputs.Enable();
        }

        public void DisableInputs(){
            gameplayInputs.Disable();
            gameplayInputs.Click.performed -= Click;
            gameplayInputs.Release.performed -= Release;
        }
    }
}