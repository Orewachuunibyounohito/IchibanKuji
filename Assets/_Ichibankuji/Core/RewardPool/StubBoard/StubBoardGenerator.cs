using System;
using System.Collections;
using System.Collections.Generic;
using Ichibankuji.Core;
using Ichibankuji.Core.RewardPools;
using Ichibankuji.SO;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Ichibankuji.Generators
{
    public class StubBoardGenerator : MonoBehaviour
    {
        private const float DEFAULT_RARE_BLOCK_HEIGHT_RATIO = 0.4f;
        private const float DEFAULT_NORMAL_BLOCK_HEIGHT_RATIO = 0.3f;
        
        public static GameObject BoardPrefab;

        [Title("Board Config")]
        public float Width;
        public float Height;
        public float Spacing;
        [Range(0, 1)]
        public float RareBlockHeightRatio;
        [Range(0, 1)]
        public float NormalBlockHeightRatio;
        public float TitleBlockHeightRatio => 1 - RareBlockHeightRatio - NormalBlockHeightRatio;
        public bool UseCustomLayout = false;
        [ShowIf("@UseCustomLayout")]
        [BoxGroup("Detail Config")]
        [LabelText("Rare Block Ratio")]
        public BoardBlockContent RareBlockDetailConfig;
        [ShowIf("@UseCustomLayout")]
        [BoxGroup("Detail Config")]
        [LabelText("Normal Block Ratio")]
        public BoardBlockContent NormalBlockDetailConfig;

        [Title("Debug Info")]
        [ReadOnly]
        public StubBoard generatedBoard;

        void Awake(){
            BoardPrefab = Resources.Load<PrefabsConfig>(ResourcesPath.PREFABS_CONFIG).StubBoard;           
        }

        public StubBoard GenerateBoard(RewardPool pool){
            HeightRatioToValid();
            StubBoard board = Instantiate(BoardPrefab).GetComponent<StubBoard>();
            board.Width = Width;
            board.Height = Height;
            board.Spacing = Spacing;
            var rareRewards = pool.GetRareRewards();
            var normalRewards = pool.GetNormalRewards();
            board.TitleBlock = GenerateTitleBlock(board, pool.Name);
            board.RareBlocks = GenerateBoardBlock(board, rareRewards, BoardComponentType.Rare);
            board.NormalBlocks = GenerateBoardBlock(board, normalRewards, BoardComponentType.Normal);
            generatedBoard = board;
            return board;
        }

        private void HeightRatioToValid(){
            RareBlockHeightRatio = RareBlockHeightRatio == 0 ? DEFAULT_RARE_BLOCK_HEIGHT_RATIO
                                                             : RareBlockHeightRatio;
            NormalBlockHeightRatio = NormalBlockHeightRatio == 0 ? DEFAULT_NORMAL_BLOCK_HEIGHT_RATIO
                                                             : NormalBlockHeightRatio;
            if(RareBlockHeightRatio + NormalBlockHeightRatio >= 1){
                RareBlockHeightRatio = DEFAULT_RARE_BLOCK_HEIGHT_RATIO;
                NormalBlockHeightRatio = DEFAULT_NORMAL_BLOCK_HEIGHT_RATIO;
            }
        }

        private StubBoard.BoardBlockForTitle GenerateTitleBlock(StubBoard board, string title){
            float heightRatio = TitleBlockHeightRatio;
            return new StubBoard.BoardBlockForTitle(board.TitleBlockTrans, title, board.Width, board.Height*heightRatio);
        }

        private Dictionary<Level, StubBoard.BoardBlock> GenerateBoardBlock(StubBoard board, List<Reward> rewards, BoardComponentType type){
            Dictionary<Level, StubBoard.BoardBlock> blocks = new();
            Transform blockTrans = default;
            float heightRatio = 1;
            int column = 1;
            int row = 1;
            BoardBlockContent blockConfig = null;
            switch(type){
                case BoardComponentType.Title:
                    throw new NotSupportedException("Please using GenerateTitleBlock method.");
                case BoardComponentType.Rare:
                    blockTrans = board.RareBlockTrans;
                    heightRatio = RareBlockHeightRatio;
                    column = rewards.Count > 1 ? 2 : 1;
                    row = rewards.Count/column;
                    row += rewards.Count % column == 0? 0 : 1;
                    if(UseCustomLayout){ blockConfig = RareBlockDetailConfig; }
                    break;
                case BoardComponentType.Normal:
                    blockTrans = board.NormalBlockTrans;
                    heightRatio = NormalBlockHeightRatio;
                    column = 1;
                    row = rewards.Count/column;
                    row += rewards.Count % column == 0? 0 : 1;
                    if(UseCustomLayout){ blockConfig = NormalBlockDetailConfig; }
                    break;
            }
            int currentIndex = 0;
            foreach(Reward reward in rewards){
                int currentColumn = currentIndex % column;
                int currentRow = currentIndex / column;
                float blockWidth = board.Width / column;
                float blockHeight = board.Height * heightRatio / row;
                float totalWidth = blockWidth * column;
                float totalHeight = blockHeight * row;
                Vector3 positionOffset = new Vector3(-totalWidth/2 + blockWidth/2,
                                                     0,
                                                     totalHeight/2 - blockHeight/2);
                Vector3 currentPosition = positionOffset + new Vector3(blockWidth * currentColumn, 0, -blockHeight * currentRow);
                if(UseCustomLayout){
                    blocks.Add(reward.Level, 
                               new StubBoard.BoardBlock(blockTrans, currentPosition, blockWidth, blockHeight, board.Spacing, blockConfig, reward));
                }else{
                    blocks.Add(reward.Level, 
                               new StubBoard.BoardBlock(blockTrans, currentPosition, blockWidth, blockHeight, board.Spacing, reward));
                }
                currentIndex++;
            }
            return blocks;
        }
    }
}