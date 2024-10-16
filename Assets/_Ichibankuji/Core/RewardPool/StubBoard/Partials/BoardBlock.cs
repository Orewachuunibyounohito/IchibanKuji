using System;
using System.Collections.Generic;
using Ichibankuji.Core.RewardPools;
using Ichibankuji.SO;
using TMPro;
using UnityEngine;

namespace Ichibankuji.Generators
{
    public partial class StubBoard
    {
        [Serializable]
        public partial class BoardBlock
        {
            private const string REWARD_TEXT_TEMPLATE = "　{0}賞 - {1}";
            private const string AMOUNT_TEXT_TEMPLATE = "剩{0}個｜";
            private const float TEXT_AREA_HEIGHT_RATIO = 0.3f;
            private const float STUB_AREA_HEIGHT_RATIO = 0.7f;
            private const float CENTER_RATIO = 0.5f;
            private const float ITEM_NAME_AREA_WIDTH_RATIO = 0.7f;
            private const float AMOUNT_AREA_WIDTH_RATIO = 0.3f;
            private const float PASTED_POSITION_WIDTH_RATIO = 0.5f;

            private float textAreaHeightRatio = TEXT_AREA_HEIGHT_RATIO;
            private float stubAreaHeightRatio = STUB_AREA_HEIGHT_RATIO;
            private float itemNameAreaWidthRatio = ITEM_NAME_AREA_WIDTH_RATIO;
            private float amountAreaWidthRatio = AMOUNT_AREA_WIDTH_RATIO;
            private float pastedPositionWidthRatio = PASTED_POSITION_WIDTH_RATIO;
            public static GameObject Prefab { get; }

            public float Width{
                get => _width;
                set{
                    _width = value;
                    float itemNameWidth = value * itemNameAreaWidthRatio;
                    ItemNameText.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, itemNameWidth);
                    ItemNameText.rectTransform.anchoredPosition3D = new Vector3(
                        -value/2 + itemNameWidth/2,
                        ItemNameText.rectTransform.anchoredPosition3D.y,
                        value * (CENTER_RATIO - itemNameAreaWidthRatio/2)
                    );
                    float amountWidth = value * amountAreaWidthRatio;
                    AmountText.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, amountWidth);
                    AmountText.rectTransform.anchoredPosition3D = new Vector3(
                        value/2 - amountWidth/2,
                        AmountText.rectTransform.anchoredPosition3D.y,
                        AmountText.rectTransform.anchoredPosition3D.z
                    );
                    float pastedPositionX = value*pastedPositionWidthRatio - value*CENTER_RATIO;
                    PasteOriginalPosition.x = pastedPositionX;
                    StubSlotCollection.localPosition = PasteOriginalPosition;
                }
            }
            public float Height{
                get => _height;
                set{
                    _height = value;
                    ItemNameText.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, value * textAreaHeightRatio);
                    ItemNameText.rectTransform.anchoredPosition3D = new Vector3(
                        ItemNameText.rectTransform.anchoredPosition3D.x,
                        ItemNameText.rectTransform.anchoredPosition3D.y,
                        value * (CENTER_RATIO - textAreaHeightRatio/2)
                    );
                    AmountText.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, value * textAreaHeightRatio);
                    AmountText.rectTransform.anchoredPosition3D = new Vector3(
                        AmountText.rectTransform.anchoredPosition3D.x,
                        AmountText.rectTransform.anchoredPosition3D.y,
                        value * (CENTER_RATIO - textAreaHeightRatio/2)
                    );
                    PasteOriginalPosition.z = value * stubAreaHeightRatio/2 - value*CENTER_RATIO;
                    StubSlotCollection.localPosition = PasteOriginalPosition;
                }
            }
            public TextMeshPro ItemNameText;
            public TextMeshPro AmountText;
            public List<StubSlot> HoldingStubs;
            public Transform View;
            public Transform StubSlotCollection;
            public Vector3 PasteOriginalPosition;
            public float Spacing;
            public int CurrentAmount;

            private float _width, _height;

            static BoardBlock(){
                Prefab = Resources.Load<PrefabsConfig>(ResourcesPath.PREFABS_CONFIG).BoardBlock;
            }

            public BoardBlock(Transform blockTrans, Vector3 position, float spacing, Reward reward){
                View = Instantiate(Prefab, blockTrans).transform;
                View.localPosition = position;
                ItemNameText = View.Find("ItemName").GetComponent<TextMeshPro>();
                AmountText = View.Find("ItemAmount").GetComponent<TextMeshPro>();
                PasteOriginalPosition = new Vector3(0, 0.01f, 0);
                Spacing = spacing;
                View.name = $"{reward.Level}";
                string itemName = string.Format(REWARD_TEXT_TEMPLATE, reward.Level, reward.Item.Name);
                ItemNameText.SetText(itemName);
                string amountText = string.Format(AMOUNT_TEXT_TEMPLATE, reward.Amount);
                AmountText.SetText(amountText);
                StubSlotCollection = new GameObject("SlotCollection").transform;
                StubSlotCollection.parent = View;
                CurrentAmount = 0;
                HoldingStubs = new List<StubSlot>();
                for(int amount = 0; amount < reward.Amount; amount++){
                    HoldingStubs.Add(new StubSlot(StubSlotCollection, PasteOriginalPosition + new Vector3(1, 0.001f, 0)*Spacing*amount));
                }
            }
            public BoardBlock(Transform blockTrans,
                              Vector3 position,
                              Vector3 pasteOriginalPosition,
                              float spacing,
                              Reward reward) : this(blockTrans, position, spacing, reward){
                PasteOriginalPosition = pasteOriginalPosition;
            }
            public BoardBlock(Transform blockTrans,
                              Vector3 position,
                              float width,
                              float height,
                              float spacing,
                              Reward reward) : this(blockTrans, position, spacing, reward){
                Width = width;
                Height = height;
            }
            public BoardBlock(Transform blockTrans,
                              Vector3 position,
                              float width,
                              float height,
                              float spacing,
                              BoardBlockContent config,
                              Reward reward) : this(blockTrans, position, spacing, reward){
                textAreaHeightRatio = config.TextAreaHeightRatio == 0 ? TEXT_AREA_HEIGHT_RATIO : config.TextAreaHeightRatio;
                stubAreaHeightRatio = config.StubAreaHeightRatio == 0 ? STUB_AREA_HEIGHT_RATIO : config.StubAreaHeightRatio;
                itemNameAreaWidthRatio = config.ItemNameAreaWidthRatio == 0 ? ITEM_NAME_AREA_WIDTH_RATIO : config.ItemNameAreaWidthRatio;
                amountAreaWidthRatio = config.AmountAreaWidthRatio == 0 ? AMOUNT_AREA_WIDTH_RATIO : config.AmountAreaWidthRatio;
                pastedPositionWidthRatio = config.StubOriginalPosition;
                Width = width;
                Height = height;
            }

            public void AddStub(GameObject stub){
                HoldingStubs[CurrentAmount].EnterTheSlot(stub);
                CurrentAmount++;
                int remaining = HoldingStubs.Count - CurrentAmount;
                string amountText = string.Format(AMOUNT_TEXT_TEMPLATE, remaining); 
                AmountText.SetText(amountText);
            }
        }
    }
}