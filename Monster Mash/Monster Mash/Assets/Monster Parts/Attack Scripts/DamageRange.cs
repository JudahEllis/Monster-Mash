using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data structure to make working with the damage ranges easier. 
/// The struct contains predefined ranges for each damage rank so that you dont have to define them yourself.
/// If needed you can also define a new range using the constructor for monster parts that do not fit within one of the predefined ranges.
/// </summary>


public struct DamageRange
{
     public int Min { get; private set; }
     public int Max { get; private set; }

    /// <summary>
    /// Predefined damage range from 0 to 0. Use if the attack has no damage or as an initial range before the real range is set.
    /// </summary>
    public static readonly DamageRange Range0 = new(0, 0);
    /// <summary>
    /// Predefined damage range from 10 to 10
    /// </summary>
    public static readonly DamageRange Range1 = new(10, 10);
    /// <summary>
    /// Predefined damage range from 10 to 40
    /// </summary>
    public static readonly DamageRange Range2 = new(10, 40);
    /// <summary>
    /// Predefined damage range from 50 to 80
    /// </summary>
    public static readonly DamageRange Range3 = new(50, 80);
    /// <summary>
    /// Predefined damage range from 90 to 120
    /// </summary>
    public static readonly DamageRange Range4 = new(90, 120);
    /// <summary>
    /// Predefined damage range from 130 to 200
    /// </summary>
    public static readonly DamageRange Range5 = new(130, 200);

    public DamageRange(int min, int max)
    {
        this.Min = min;
        this.Max = max;
    }

    /// <summary>
    /// Automatically clamps the passed in value to the assigned range
    /// </summary>
    /// <param name="value">The value you want to clamp</param>
    /// <returns>The clamped value</returns>
    public int Clamp(int value)
    {
        return Mathf.Clamp(value, Min, Max);
    }
}
