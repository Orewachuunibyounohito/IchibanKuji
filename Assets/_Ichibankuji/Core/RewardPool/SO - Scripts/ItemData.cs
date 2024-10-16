using System;
using Ichibankuji.Core.RewardPools;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Ichibankuji.SO
{
    [CreateAssetMenu(menuName = "Ichibankuji/Data/Item", fileName = "New Item Data")]
    public class ItemData : ScriptableObject
    {
        [HorizontalGroup("Horizontal", Width = 120)]
        [VerticalGroup("Horizontal/Vertical")]
        public string Name;
        [PreviewField(Alignment = ObjectFieldAlignment.Center), HideLabel]
        [VerticalGroup("Horizontal/Vertical")]
        public Sprite Image;
        [HorizontalGroup("Horizontal")]
        [TextArea(4, 20)]
        public string Description;

        public Item GenerateItem() => new Item{
            Name = Name,
            Description = Description,
        };
    }
}