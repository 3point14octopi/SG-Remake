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

        public static void SwapCoord(List<Coord> l, int a, int b)
        {
            int ax = l[a].x;
            int ay = l[a].y;
            int az = l[a].z;

            l[a] = new Coord(l[b].x, l[b].y, l[b].z);
            l[b] = new Coord(ax, ay, az);
        }
    }


    public static class RNG {

        public static void SetSeed(int s)
        {
            UnityEngine.Random.InitState(s);
        }

        public static int GenRand(int min, int range)
        {
            return UnityEngine.Random.Range(min, (min + range));
        }
        public static Coord GenRandCoord(Space2D space, bool includeBorder = false)
        {
            int inclusive = (!includeBorder) ? 1 : 0;

            return new Coord((GenRand(inclusive, space.width + ((includeBorder)?0:-2))), (GenRand(inclusive, space.height + ((includeBorder) ? 0 : -2))));

        }
        public static Coord CircleSelect(List<Coord> coords, int start)
        {
            if(start < coords.Count)
            {
                int x = GenRand(start, coords.Count-start);
                Basic.SwapCoord(coords, start, x);
            }
            else
            {
                start = coords.Count - 1;
            }

            return coords[start];
        }

    }

}
