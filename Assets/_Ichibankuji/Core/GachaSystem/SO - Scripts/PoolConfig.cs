using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Ichibankuji.SO
{
    [CreateAssetMenu(menuName = "Ichibankuji/Config/Pool", fileName = "New Pool Config")]
    public class PoolConfig : ScriptableObject
    {
        [InlineEditor]
        public List<PoolData> Pools;
    }
}