using System;
using System.Collections.Generic;
using ChuuniExtension;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ichibankuji.Core
{
    [Serializable]
    public class ViewportMoveModule
    {
        public enum CodeName
        {
            Start,
            Pointer,
            End,
            ScorllDelta
        }

        [SerializeField]
        private Cinemachine.CinemachineVirtualCamera vCam;
        private InputActionMap actionMap;
        private Dictionary<CodeName, Guid> actionsID;
        private Vector2 prevScreenPosition;
        private bool isMoving;
        [SerializeField]
        private float multiplierForMove = 0.8f;
        [SerializeField]
        private bool horizontalReverse = true;
        [SerializeField]
        private bool verticalReverse = true;
        [SerializeField]
        private float multiplierForZoom = 0.125f;

        public Cinemachine.CinemachineVirtualCamera VCam => vCam;

        public ViewportMoveModule(){}
        public ViewportMoveModule(ViewportMoveModuleContent content){
            multiplierForMove = content.MultiplierForMove;
            horizontalReverse = content.HorizontalReverse;
            verticalReverse = content.VerticalReverse;
            multiplierForZoom = content.MultiplierForZoom;
        }
        public ViewportMoveModule(Cinemachine.CinemachineVirtualCamera vCam, Dictionary<CodeName, Guid> actionsID, InputActionMap actionMap){
            this.vCam = vCam;
            this.actionMap = actionMap;
            this.actionsID = actionsID;
        }

        public void Initialize(Dictionary<CodeName, Guid> actionsID, InputActionMap actionMap){
            this.actionMap = actionMap;
            this.actionsID = actionsID;
        }
        public void Initialize(Dictionary<CodeName, Guid> actionsID, InputActionMap actionMap, float multiplierForMove, float multiplierForZoom = 0.01f){
            this.actionMap = actionMap;
            this.actionsID = actionsID;
            this.multiplierForMove = multiplierForMove;
            this.multiplierForZoom = multiplierForZoom;
        }

        public void Enable(){
            actionMap.FindAction(actionsID[CodeName.Start]).performed += MoveStart;
            actionMap.FindAction(actionsID[CodeName.End]).performed += MoveEnd;
            actionMap.FindAction(actionsID[CodeName.ScorllDelta]).performed += ZoomInOut;
            actionMap.Enable();
        }
        public void Disable(){
            actionMap.Disable();
            actionMap.FindAction(actionsID[CodeName.Start]).performed -= MoveStart;
            actionMap.FindAction(actionsID[CodeName.End]).performed -= MoveEnd;
            actionMap.FindAction(actionsID[CodeName.ScorllDelta]).performed += ZoomInOut;
        }


        public void PositionUpdate(){
            if(!isMoving){ return ; }
            if(vCam == null){ return ; }
            var newScreenPosition = actionMap.FindAction(actionsID[CodeName.Pointer]).ReadValue<Vector2>();
            var delta = (newScreenPosition - prevScreenPosition) * multiplierForMove;
            delta.x = horizontalReverse? delta.x * -1 : delta.x;
            delta.y = verticalReverse? delta.y * -1 : delta.y;
            vCam.transform.position += delta.AsVector3InXZ();

            prevScreenPosition = newScreenPosition;
        }

        public void SetVCam(Cinemachine.CinemachineVirtualCamera vCam){
            this.vCam = vCam;
        }

        private void MoveStart(InputAction.CallbackContext context){
            prevScreenPosition = actionMap.FindAction(actionsID[CodeName.Pointer]).ReadValue<Vector2>();
            isMoving = true;
        }

        private void MoveEnd(InputAction.CallbackContext context) =>
            isMoving = false;
        
        private void ZoomInOut(InputAction.CallbackContext context){
            var scorllDelta = actionMap.FindAction(actionsID[CodeName.ScorllDelta]).ReadValue<Vector2>();
            vCam.transform.position -= new Vector3(0, scorllDelta.y * multiplierForZoom, 0);
        }

        public static Dictionary<CodeName, Guid> GenerateInputMap(Guid moveStartActionID,
                                                                  Guid cameraPointerActionID,
                                                                  Guid moveEndActionID,
                                                                  Guid zoomInOutActionID) =>
            new(){
                { CodeName.Start, moveStartActionID },
                { CodeName.Pointer, cameraPointerActionID },
                { CodeName.End, moveEndActionID },
                { CodeName.ScorllDelta, zoomInOutActionID }
            };
    }
    
    [Serializable]
    public class ViewportMoveModuleContent
    {
        [SerializeField]
        public Cinemachine.CinemachineVirtualCamera TicketVCam;
        [Range(0, 0.1f)]
        public float MultiplierForMove = 0.0001f;
        public bool HorizontalReverse;
        public bool VerticalReverse;
        [Range(0, 0.5f)]
        public float MultiplierForZoom = 0.1f;
    }
}