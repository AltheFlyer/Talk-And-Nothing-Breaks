using System;
using UnityEngine;

public static class StaticRandom 
{
    static System.Random random = new System.Random();

    public static double Next() {
        return random.NextDouble();
    }

    public static int NextInt(int a) {
        int n = random.Next(a);
        return n;
    }

    public static int NextInt(int low, int a) {
        int n = random.Next(a - low) + low;
        return n;
    }
}



