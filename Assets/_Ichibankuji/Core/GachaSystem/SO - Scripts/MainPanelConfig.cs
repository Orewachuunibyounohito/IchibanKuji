using Ichibankuji.Core.UI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Ichibankuji.SO
{
    [CreateAssetMenu(menuName = "Ichibankuji/Config/MainPanel", fileName = "New MainPanel Config")]
    public class MainPanelConfig : ScriptableObject
    {
        [FoldoutGroup("Sprites")]
        [LabelText("Pool Button")]
        public SpriteSwapContent PoolButtonSprites;
        [FoldoutGroup("Sprites")]
        [LabelText("Computer Pick Button")]
        public SpriteSwapContent ComputerPickButtonSprites;
        [FoldoutGroup("Sprites/TicketOrBoard")]
        [LabelText("Ticket")]
        public SpriteSwapContent TicketSprites;
        [FoldoutGroup("Sprites/TicketOrBoard")]
        [LabelText("Board")]
        public SpriteSwapContent BoardSprites;
    }
}
