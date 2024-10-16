using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ChuuniExtension
{
    [Serializable]
    public class Bound3D
    {
        public Range X;
        public Range Y;
        public Range Z;

        public Bound3D(){}
        public Bound3D(Range x, Range y, Range z) : this(x.Max, x.Min, y.Max, y.Min, z.Max, z.Min){}
        public Bound3D(float xMax, float xMin, float yMax, float yMin, float zMax, float zMin){
            X = new Range(xMax, xMin);
            Y = new Range(yMax, yMin);
            Z = new Range(zMax, zMin);
        }

        public Vector3 GenerateInsidePositonByRandom() => new Vector3(
            Random.Range(X.Min, X.Max),
            Random.Range(Y.Min, Y.Max),
            Random.Range(Z.Min, Z.Max)
        );
    }
}