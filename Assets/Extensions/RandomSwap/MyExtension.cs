using System.Collections.Generic;
using UnityEngine;

namespace ChuuniExtension
{
    public static class MyExtension
    {
        public static List<T> RandomSwap<T>(this List<T> source, int swapCount = 500){
            T tempElement;
            for(int count = 0, randomedA, randomedB; count < swapCount; count++){
                randomedA = Random.Range(0, source.Count);
                randomedB = Random.Range(0, source.Count);
                if(randomedA == randomedB){ count--; continue; }
                tempElement = source[randomedA];
                source[randomedA] = source[randomedB];
                source[randomedB] = tempElement;
            }
            return source;
        }
        public static List<T> RandomSwap<T>(this List<T> source, int swapCount, int seed){
            Random.InitState(seed);
            T tempElement;
            for(int count = 0, randomedA, randomedB; count < swapCount; count++){
                randomedA = Random.Range(0, source.Count);
                randomedB = Random.Range(0, source.Count);
                if(randomedA == randomedB){ count--; continue; }
                tempElement = source[randomedA];
                source[randomedA] = source[randomedB];
                source[randomedB] = tempElement;
            }
            return source;
        }
    }
}