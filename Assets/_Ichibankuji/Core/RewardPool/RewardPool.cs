using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ChuuniExtension;
using Ichibankuji.Core.Tickets;
using Ichibankuji.Generators;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Ichibankuji.Core.RewardPools
{
    public class RewardPool
    {
        public enum PositionGeneratedMode
        {
            Tidy,
            Random
        }

        private GachaSystem system;
        private string name;
        private List<Reward> rewards;
        private List<Item> items;
        private List<Ticket> tickets;
        private Dictionary<GameObject, Ticket> ticketMap;
        private Dictionary<Vector2Int, GameObject> ticketMap_Position;
        private StubBoard stubBoard;

        public string Name => name;
        public string PrintRewards() => string.Join(", ", rewards);
        public string PrintItems() => string.Join(", ", items);
        public bool IsDone => items.Count == 0;

        public Action OnDone;

        public RewardPool(){}

        private RewardPool(GachaSystem system){
            if(system == null){ return ; }
            this.system = system;

            system.stateMachine.NormalState.TicketSelected += SelectTicket;
            system.stateMachine.NormalState.TicketDeselected += DeselectTicket;
            system.stateMachine.FocusState.RedeemReward += Redeem;
        }

        public RewardPool(GachaSystem system, string poolName, List<Reward> rewards) : this(system){
            this.name = poolName;
            this.rewards = rewards;

            StubBoardGenerator generator = system.GetComponent<StubBoardGenerator>();
            stubBoard = generator.GenerateBoard(this);
            system.stateMachine.NormalState.SetBoardVCam(stubBoard.vCam);
        }

        public List<Item> GenerateItems(){
            List<Item> items = new();
            rewards.Aggregate(items, (items, reward) => {
                for(int amount = 0; amount < reward.Amount; amount++){
                    items.Add(new Item(reward.Item));
                }
                return items;
            });
            this.items = items;
            return items;
        }

        public List<Ticket> GenerateTickets(){
            List<Ticket> tickets = new();
            ticketMap = new();
            rewards.Aggregate(tickets, (tickets, reward) => {
                Ticket currentTicket;
                for(int amount = 0; amount < reward.Amount; amount++){
                    var newTicketObject = Object.Instantiate(TicketView.TicketPrefab, GachaSystem.TicketCollection);
                    currentTicket = new Ticket(name, reward.Level, newTicketObject);
                    currentTicket.HideView();
                    tickets.Add(currentTicket);
                    ticketMap.Add(newTicketObject, currentTicket);
                }
                return tickets;
            });
            
            tickets = tickets.RandomSwap();
            this.tickets = tickets;
            return tickets;
        }

        public void Shuffle(){
            items.RandomSwap();
        }
        public void FixedShuffle(int swapCount, int seed){
            items.RandomSwap(swapCount, seed);
        }

        public void SelectTicket(GameObject ticketObject){
            Ticket selectedTicket = ticketMap[ticketObject];
            selectedTicket.ToFocusMode(GachaSystem.FocusedPosition);
        }

        private void DeselectTicket(GameObject ticketObject){
            if(!ticketMap.ContainsKey(ticketObject)){ return ; }
            ticketMap[ticketObject].ExitFocusMode();
        }

        private void Redeem(GameObject ticketObject){
            var selectedTicket = ticketMap[ticketObject];
            Debug.Log($"挑選的票券: {selectedTicket.ToStringZh()}");
            var ticketLevel = selectedTicket.GetLevel();
            var itemSample = rewards.Find((reward) => reward.Level == ticketLevel).Item;
            var found = items.Find((item) => item == itemSample);
            var stubObject = ticketObject.transform.Find("Stub");
            stubBoard.AddStub(stubObject.gameObject, ticketLevel);
            ticketMap.Remove(ticketObject);
            Object.Destroy(ticketObject);
            items.Remove(found);
            Debug.Log($"獎品: {found}");
            if(IsDone){ OnDone.Invoke(); }
        }

        public int TotalAmount => rewards.Sum((reward) => reward.Amount);

        public void DisplayTicketsWithDelayAndPosition(Bound3D bound, PositionGeneratedMode mode, float interval = 0.05f){
            if(interval < 0){ interval = 0.05f; }
            switch(mode){
                case PositionGeneratedMode.Tidy:
                    system.StartCoroutine(DisplayTicketsWithDelayAndTidyPositionTask(bound, interval));
                    break;
                case PositionGeneratedMode.Random:
                    system.StartCoroutine(DisplayTicketsWithDelayAndRandomPositionTask(bound, interval));
                    break;
            }
        }

        private IEnumerator DisplayTicketsWithDelayAndRandomPositionTask(Bound3D bound, float interval = 0.05f){
            foreach(var ticket in tickets){
                ticket.RandomPosition(bound);
                ticket.DisplayView();
                yield return new WaitForSeconds(interval);
            }
        }
        private IEnumerator DisplayTicketsWithDelayAndTidyPositionTask(Bound3D bound, float interval = 0.02f){
            ticketMap_Position = new();
            var spacing = new Vector2(0.95f, 0.75f);
            int column = 0;
            int row = 0;
            foreach(var ticket in tickets){
                var position2D = new Vector2Int(column, row);
                var leftUpPosition = new Vector3(-4.35f, 0, 3.85f);
                var position = leftUpPosition + new Vector3(spacing.x * column, 0, -spacing.y * row);
                ticketMap_Position.Add(position2D, ticket.gameObject);
                ticket.SetSlotPosition(position2D);
                ticket.SetPosition(position);
                ticket.DisplayView();
                yield return new WaitForSeconds(interval);
                column++;
                if(column % 10 == 0){
                    row++;
                    column = 0;
                }
            }
        }

        public List<Reward> GetRareRewards(){
            List<Reward> rareRewards = new();
            rewards.Where((reward) => reward.Amount <= 5)
                   .Aggregate(rareRewards, (rareRewards, reward) => {
                    rareRewards.Add(reward);
                    return rareRewards;
            });
            return rareRewards;
        }
        public List<Reward> GetNormalRewards(){
            List<Reward> normalRewards = new();
            rewards.Where((reward) => reward.Amount > 5)
                   .Aggregate(normalRewards, (normalRewards, reward) => {
                    normalRewards.Add(reward);
                    return normalRewards;
            });
            return normalRewards;
        }

        public GameObject GetTicketObject(Vector2Int position) =>
            ticketMap_Position.ContainsKey(position) ? ticketMap_Position[position]
                                                     : default;

        public void ActiveStubBoardSwitcher(){
            stubBoard.vCam.Priority = stubBoard.vCam.Priority > 10 ? 9 : 11;
        }

        public void Destroy(){
            system.stateMachine.NormalState.TicketSelected -= SelectTicket;
            system.stateMachine.NormalState.TicketDeselected -= DeselectTicket;
            system.stateMachine.FocusState.RedeemReward -= Redeem;

            Object.Destroy(stubBoard.gameObject);
        }

        public static RewardPool CreatePool(string poolName, IEnumerable<Reward> rewards) =>
            new RewardPool{
                name = poolName,
                rewards = new List<Reward>(rewards)
            };

        public static RewardPool CreatePoolAndTicketsAndItems(GachaSystem system, string poolName, IEnumerable<Reward> rewards){
            RewardPool pool = new RewardPool(system, poolName, new List<Reward>(rewards));
            pool.GenerateTickets();
            pool.GenerateItems();
            return pool;
        }
    }
}