using System;
using ChuuniExtension;
using Ichibankuji.Core.RewardPools;
using Ichibankuji.SO;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Ichibankuji.Core.Tickets
{
    public class Ticket
    {
        private TicketModel model;
        private TicketView view;

        public GameObject gameObject => view.gameObject;

        public Ticket(string name, Level level, GameObject ticketObject){
            model = new TicketModel(name, level);
            view = new TicketView(ticketObject, level);
        }

        public void DisplayView() => view.DisplayView();
        public void HideView() => view.HideView();
        public void ToFocusMode(Vector3 focusedPosition) => view.ToFocusMode(focusedPosition);
        public void ExitFocusMode() => view.ExitFocusMode();
        public Level GetLevel() => model.Level;
        
        public void SetSlotPosition(Vector2Int newPosition) => view.SetSlotPosition(newPosition);
        public void SetPosition(Vector3 newPosition) => view.SetPosition(newPosition);
        public void RandomPosition(Bound3D bound) => view.SetPosition(bound.GenerateInsidePositonByRandom());

        public override string ToString() => $"{model}";

        public string ToStringZh() => $"{model.ToStringZh()}";
    }
}