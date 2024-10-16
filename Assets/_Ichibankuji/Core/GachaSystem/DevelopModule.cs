using System;
using System.Collections;
using Ichibankuji.Core.RewardPools;
using Ichibankuji.SO;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ichibankuji.Core
{
    [Serializable]
    public class DevelopModule
    {
        private Inputs1.DevelopActions developInputs;
        private GachaSystem gachaSystem;

        public DevelopModule(GachaSystem system){
            developInputs = new Inputs1().Develop;
            gachaSystem = system;
        }

        public void Enable(){
            // developInputs.GenerateTicket_OnePiece.performed += GenerateOnePiece;
            // developInputs.GenerateTicket_咒術迴戰.performed += Generate咒術迴戰;
            // developInputs.StubBoardDisplaySwitcher.performed += ActiveStubBoardSwitcher;
            developInputs.RunOutOfTicket.performed += RunOutOfTicket;
            developInputs.Enable();
        }

        public void Disable(){
            developInputs.Disable();
            // developInputs.GenerateTicket_OnePiece.performed -= GenerateOnePiece;
            // developInputs.GenerateTicket_OnePiece.performed -= Generate咒術迴戰;
            // developInputs.StubBoardDisplaySwitcher.performed -= ActiveStubBoardSwitcher;
            developInputs.RunOutOfTicket.performed -= RunOutOfTicket;
        }

        private void GenerateOnePiece(InputAction.CallbackContext context){
            PoolData onePiece = gachaSystem.PoolConfig.Pools.Find((pool) => pool.name == "OnePiecePool");
            RewardPool onePieceReward = onePiece.CreatePool(gachaSystem);
            onePieceReward.DisplayTicketsWithDelayAndPosition(gachaSystem.StartBound, RewardPool.PositionGeneratedMode.Random);
            gachaSystem.CurrentPool = onePieceReward;
        }

        private void Generate咒術迴戰(InputAction.CallbackContext context){
        }

        private void ActiveStubBoardSwitcher(InputAction.CallbackContext context){
            if(gachaSystem.IsFocusMode){ return ; }
            gachaSystem.CurrentPool?.ActiveStubBoardSwitcher();
        }

        private void RunOutOfTicket(InputAction.CallbackContext context){
            gachaSystem.StartCoroutine(RunOutOfTicketTask());
        }
        private IEnumerator RunOutOfTicketTask(){
            Transform TicketCollection = gachaSystem.GetComponent<CollectionManager>().TicketCollection;
            while(TicketCollection.childCount != 0){
                gachaSystem.ComputerPickByRandom();
                yield return new WaitForSeconds(gachaSystem.AutoDrawInterval);
            }
        }
    }
}