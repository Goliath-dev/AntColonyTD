using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Class representing 2-dimensional point or 2-dimensional vector. 
/// </summary>
public class TwoDimPoint
{
    public int X { get; private set; } = 0;
    public int Y { get; private set; } = 0;

    public TwoDimPoint(int x, int y)
    {
        X = x;
        Y = y;
    }

    public override string ToString()
    {
        return $"({X}, {Y})";
    }
    /// <summary>
    /// Creates new point by shifting the given one to the chosen direction.
    /// </summary>
    /// <param name="point"></param>
    /// <param name="vector"></param>
    /// <returns></returns>
    public TwoDimPoint ShiftPointTo(TwoDimPoint vector)
    {
        return new TwoDimPoint(this.X + vector.X, this.Y + vector.Y);
    }

    public static TwoDimPoint Up = new TwoDimPoint(0, 1);
    public static TwoDimPoint Down = new TwoDimPoint(0, -1);
    public static TwoDimPoint Left = new TwoDimPoint(-1, 0);
    public static TwoDimPoint Right = new TwoDimPoint(1, 0);
    public static TwoDimPoint operator *(int factor, TwoDimPoint point) => new TwoDimPoint(factor * point.X, factor * point.Y);
}
