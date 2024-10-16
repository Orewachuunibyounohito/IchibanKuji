using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Ichibankuji.SO
{
    [CreateAssetMenu(menuName = "Ichibankuji/Config/Material", fileName = "New Material Config")]
    public class MaterialsConfig : ScriptableObject
    {
        [FoldoutGroup("Stub")]
        public List<StubMaterialContent> StubMaterials;
    }
}