using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Ichibankuji.Generators
{
    [Serializable]
    public class BoardBlockContent
    {
        [LabelText("Text Height")]
        [Range(0, 1)]
        public float TextAreaHeightRatio;
        [LabelText("Stub Height")]
        [Range(0, 1)]
        public float StubAreaHeightRatio;
        [LabelText("Name Width")]
        [Range(0, 1)]
        public float ItemNameAreaWidthRatio;
        [LabelText("Amount Width")]
        [Range(0, 1)]
        public float AmountAreaWidthRatio;
        [LabelText("Stub Original")]
        [Range(0, 1)]
        public float StubOriginalPosition;
    }
}