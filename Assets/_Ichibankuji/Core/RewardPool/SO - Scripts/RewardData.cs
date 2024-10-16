using Sirenix.OdinInspector;
using UnityEngine;

using Ichibankuji.Core.RewardPools;
using System;
using Ichibankuji.SO;
using Ichibankuji.Core;

namespace Ichibankuji.TransferStructure
{
    [Serializable]
    public class RewardData
    {
        [VerticalGroup("Vertical")]
        [InlineEditor]
        [HideLabel]
        public ItemData ItemData;
        [HorizontalGroup("Vertical/Horizontal")]
        public int Amount;
        [HorizontalGroup("Vertical/Horizontal")]
        public Level Level;

        public Reward GenerateReward() => new Reward{
            Item = ItemData.GenerateItem(),
            Amount = Amount,
            Level = Level
        };
    }
}