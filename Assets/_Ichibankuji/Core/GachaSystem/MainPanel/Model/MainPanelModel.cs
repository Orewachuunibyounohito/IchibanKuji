using System;
using System.Collections.Generic;
using Ichibankuji.SO;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ichibankuji.Core.UI
{
    [Serializable]
    public class MainPanelModel
    {
        public enum TicketOrBoard
        {
            Ticket,
            Board
        }

        public Sprite BgImage;
        public Sprite DisplayImage;
        public SpriteSwapContent PoolButtonSprites;
        public SpriteSwapContent ComputerPickButtonSprites;
        [ShowInInspector]
        public Dictionary<TicketOrBoard, SpriteSwapContent> TicketOrBoardButtonSprites;
        public List<TMP_Dropdown.OptionData> Options;

        private TicketOrBoard currentSpriteState;

        public UnityAction<TMP_Dropdown.OptionData> OnAddOption;
        public UnityAction<List<TMP_Dropdown.OptionData>> OnAddOptions;
        public UnityAction<SpriteSwapContent> OnPoolButtonSpritesChanged;
        public UnityAction<SpriteSwapContent> OnComputerPickButtonSpritesChanged;
        public UnityAction<SpriteSwapContent> OnTicketOrBoardButtonSpritesChanged;

        public MainPanelModel(){
            Options = new();
        }

        public void AddOption(TMP_Dropdown.OptionData optionData){
            Options.Add(optionData);
            OnAddOption.Invoke(optionData);
        }
        public void AddOptions(List<TMP_Dropdown.OptionData> optionsData){
            Options.AddRange(optionsData);
            OnAddOptions.Invoke(Options);
        }

        public void AssignSpriteSwap(MainPanelConfig config){
            AssignPoolButtonSprites(config.PoolButtonSprites);
            AssignComputerPickButtonSprites(config.ComputerPickButtonSprites);
            AssignTicketOrBoardButtonSprites(new Dictionary<TicketOrBoard, SpriteSwapContent>{
                { TicketOrBoard.Ticket, config.TicketSprites }, { TicketOrBoard.Board, config.BoardSprites }
            });
        }
        private void AssignPoolButtonSprites(SpriteSwapContent content){
            PoolButtonSprites = content;
            OnPoolButtonSpritesChanged?.Invoke(PoolButtonSprites);
        }
        private void AssignComputerPickButtonSprites(SpriteSwapContent content){
            ComputerPickButtonSprites = content;
            OnComputerPickButtonSpritesChanged?.Invoke(ComputerPickButtonSprites);
        }
        private void AssignTicketOrBoardButtonSprites(Dictionary<TicketOrBoard, SpriteSwapContent> content){
            TicketOrBoardButtonSprites = content;
            currentSpriteState = TicketOrBoard.Board;
            OnTicketOrBoardButtonSpritesChanged?.Invoke(TicketOrBoardButtonSprites[currentSpriteState]);
        }

        public void TicketOrBoardSpriteSwitch(){
            if(currentSpriteState == TicketOrBoard.Board){ currentSpriteState--; }
            else{ currentSpriteState++; }
            OnTicketOrBoardButtonSpritesChanged.Invoke(TicketOrBoardButtonSprites[currentSpriteState]);
        }
        public void ChangeTicketOrBoardButtonSprites(TicketOrBoard index){
            currentSpriteState = index;
            OnTicketOrBoardButtonSpritesChanged?.Invoke(TicketOrBoardButtonSprites[currentSpriteState]);
        }
    }

    [Serializable]
    public struct SpriteSwapContent
    {
        [LabelWidth(80)]
        public Sprite Normal;
        [LabelWidth(80)]
        public Sprite Highlighted;
        [LabelWidth(80)]
        public Sprite Pressed;
        [LabelWidth(80)]
        public Sprite Selected;
        [LabelWidth(80)]
        public Sprite Disabled;

        public SpriteState CreateSpriteState() =>
            new SpriteState{
                highlightedSprite = Highlighted,
                selectedSprite = Selected,
                pressedSprite = Pressed,
                disabledSprite = Disabled,
            };
    }
}
