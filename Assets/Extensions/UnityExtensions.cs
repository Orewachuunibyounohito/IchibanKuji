using UnityEngine;

namespace ChuuniExtension
{
    public static class UnityExtensions
    {
        public static Vector3 InXZ(this Vector3 source) => new Vector3(source.x, 0, source.z);
        public static Vector3 InY(this Vector3 source) => new Vector3(0, source.y, 0);
        public static Vector3 AsVector3InXZ(this Vector2 source) => new Vector3(source.x, 0, source.y);
        public static Vector2 AsVector2(this Vector2Int source) => new Vector2(source.x, source.y);
        public static Vector2Int AsRoundVector2Int(this Vector2 source) => new Vector2Int(Mathf.RoundToInt(source.x), Mathf.RoundToInt(source.y));
    }
}