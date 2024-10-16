using System;
using Ichibankuji.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Ichibankuji.SO
{
    [Serializable]
    public class StubMaterialContent
    {
        [HorizontalGroup("Horizontal", Width = 150)]
        public Level Level;
        [HorizontalGroup("Horizontal")]
        [HideLabel]
        public Material Material;
    }
}