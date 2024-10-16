using UnityEngine;

namespace Ichibankuji.SO
{
    [CreateAssetMenu(menuName = "Ichibankuji/Config/Prefabs", fileName = "New Prefabs Config")]
    public class PrefabsConfig : ScriptableObject
    {
        public GameObject Ticket;
        public GameObject StubBoard;
        public GameObject BoardBlock;
        public GameObject BoardBlockForTitle;
    }
}
