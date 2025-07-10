using System;

// These indexes need to match the right side of the indexes in MonsterPartConnectionMask
public enum MonsterPartConnectionPoint
{
    None = -1,
    TopHeadConnection = 0,
    BackHeadConnection = 1,
    LeftHeadConnection = 2,
    RightHeadConnection = 3,
    FaceConnection = 4,
    NeckTorsoConnection = 5,
    LeftUpperTorsoConnection = 6,
    RightUpperTorsoConnection = 7,
    ChestTorsoConnection = 8,
    ShoulderBladeTorsoConnection = 9,
    BellyTorsoConnection = 10,
    LeftLowerTorsoConnection = 11,
    RightLowerTorsoConnection = 12,
    TailTorsoConnection = 13,
}

// The indexes for flag enums need to be powers of 2 in order to avoid overlaping binary values.
// each item is given an index of 1 by default then the binary value is shifted n + 1 places to make it a power of 2.
[Flags]
public enum MonsterPartConnectionMask
{
    None = 0,
    TopHeadLimb = 1 << 0,
    BacksideHeadLimb = 1 << 1,
    LeftEarLimb = 1 << 2,
    RightEarLimb = 1 << 3,
    FacialLimb = 1 << 4,
    NeckLimb = 1 << 5,
    LeftShoudlerLimb = 1 << 6,
    RightShoulderLimb = 1 << 7,
    ChestLimb = 1 << 8,
    ShoulderBladeLimb = 1 << 9,
    BellyLimb = 1 << 10,
    LeftPelvisLimb = 1 << 11,
    RightPelvisLimb = 1 << 12,
    TailLimb = 1 << 13,
}

public static class MonsterPartConnection
{
    public static bool Matches(this MonsterPartConnectionMask mask, MonsterPartConnectionPoint connectionPoint)
    {
        if (connectionPoint == MonsterPartConnectionPoint.None || (int)connectionPoint < 0)
            return false;

        // Create a bitmask from the ConnectionPoint index
        int bit = 1 << (int)connectionPoint;
        return (mask & (MonsterPartConnectionMask)bit) != 0;
    }
}
