using System;
using Sirenix.OdinInspector;

namespace Ichibankuji.Core.RewardPools
{
    [Serializable]
    public class Reward : IEquatable<Reward>
    {
        public Item Item;
        public int Amount; 
        public Level Level; 

        public bool Equals(Reward other) => Item.Name == other.Item.Name;

        public override string ToString() => $"{Item.Name}: {Amount}";
    }
}