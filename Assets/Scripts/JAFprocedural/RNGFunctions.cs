using System;
using System.Collections.Generic;

namespace JAFprocedural
{
    public static class Basic {
        public static void Swap<T>(T a, T b)
        {
            T temp = b;
            b = a;
            a = temp;
        }
    }



    public static class RNG {

        public static void SetSeed(int s)
        {
            UnityEngine.Random.InitState(s);
        }

        public static int GenRand(int min, int range)
        {
            return UnityEngine.Random.Range(min, (min + range) - ((min == 0)?1:0));
        }
        public static Coord GenRandCoord(Space2D space, bool includeBorder = false)
        {
            int inclusive = (!includeBorder) ? 1 : 0;

            return new Coord((GenRand(inclusive, space.width + ((includeBorder)?1:-2))), (GenRand(inclusive, space.height + ((includeBorder) ? 1 : -2))));

        }
        public static Coord CircleSelect(List<Coord> coords, int start)
        {
            if(start < coords.Count)
            {
                int x = GenRand(start, (coords.Count - 1)-start);
                Basic.Swap(coords[start], coords[x]);
            }
            else
            {
                start = coords.Count - 1;
            }

            return coords[start];
        }

    }

}
