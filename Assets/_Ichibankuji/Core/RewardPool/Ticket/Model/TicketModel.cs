using System;

namespace Ichibankuji.Core.Tickets
{
    [Serializable]
    public class TicketModel
    {
        public int Id{ get; set; }
        public string Name{ get; set; }
        public Level Level{ get; set; }

        public TicketModel(string name, Level level){
            Name = name;
            Level = level;
        }

        public override string ToString() => $"[{Name}-Rare {Level}]";
        public string ToStringZh() => $"[{Name}-{Level}賞]";
    }
}