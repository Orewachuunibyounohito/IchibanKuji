using System;

namespace Ichibankuji.Core.RewardPools
{
    [Serializable]
    public class Item : IEquatable<Item>
    {
        public static bool operator ==(Item item1, Item item2){
            if(ReferenceEquals(item1, item2)){ return true; }
            if(ReferenceEquals(item1, null)){ return false; }
            if(ReferenceEquals(item2, null)){ return false; }
            return item1.Equals(item2);
        }
        public static bool operator !=(Item item1, Item item2) => !(item1 == item2);

        public string Name{ get; set; }
        public string Description{ get; set; }

        public Item(){}
        public Item(Item item){
            Name = item.Name;
            Description = item.Description;
        }

        public bool Equals(Item other) => Name == other.Name;

        public override bool Equals(object obj){
            return obj is Item && Equals(obj as Item);
        }
        public override int GetHashCode(){
            return Name.GetHashCode() ^ Description.GetHashCode();
        }
        public override string ToString() => $"{Name}[{Description}]";
    }
}