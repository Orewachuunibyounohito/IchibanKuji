using System;
using Ichibankuji.SO;
using TMPro;
using UnityEngine;

namespace Ichibankuji.Generators
{
    public partial class StubBoard
    {
        [Serializable]
        public partial class BoardBlockForTitle
        {
            public static GameObject Prefab{ get; }

            private float _width;
            private float _height;

            public TextMeshPro TitleText;
            public GameObject View;
            public float Width{
                get => _width;
                set{
                    _width = value;
                    TitleText.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, value);
                }
            }
            public float Height{
                get => _height;
                set{
                    _height = value;
                    TitleText.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, value);
                }
            }

            static BoardBlockForTitle(){
                Prefab = Resources.Load<PrefabsConfig>(ResourcesPath.PREFABS_CONFIG).BoardBlockForTitle;
            }

            public BoardBlockForTitle(Transform blockTrans, string title){
                View = Instantiate(Prefab, blockTrans);
                View.name = "TitleName";
                TitleText = View.GetComponent<TextMeshPro>();
                TitleText.SetText(title);
            }
            public BoardBlockForTitle(Transform blockTrans, string title, float width, float height) : this(blockTrans, title){
                Width = width;
                Height = height;
            }
        }
    }
}