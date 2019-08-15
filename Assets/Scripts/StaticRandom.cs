using System;

public static class StaticRandom 
{
    static Random random = new Random();

    public static double Next() {
        return random.NextDouble();
    }

    public static int NextInt(int a) {
        return random.Next(a);
    }
}



