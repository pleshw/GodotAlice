using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Extras;

public static class Utils
{
  public static int Abs(this int val)
  {
    return Math.Abs(val);
  }

  public static int DistanceInCells(this Node2D origin, Node2D target2, int cellWidth) => origin.Position.DistanceInCells(target2, cellWidth);
  public static int DistanceInCells(this Node2D origin, Vector2 target2, int cellWidth) => origin.Position.DistanceInCells(target2, cellWidth);
  public static int DistanceInCells(this Vector2 origin, Node2D target2, int cellWidth) => origin.DistanceInCells(target2.Position, cellWidth);
  public static int DistanceInCells(this Vector2 origin, Vector2 target2, int cellWidth)
  {
    float distanceInPixels = origin.DistanceTo(target2);

    int distanceInCells = Mathf.RoundToInt(distanceInPixels / cellWidth);

    return distanceInCells;
  }

  public static double Remap(float inputMin, float inputMax, float outputMin, float outputMax, float v)
  {
    float t = Mathf.InverseLerp(inputMin, inputMax, v);
    return Mathf.Lerp(outputMin, outputMax, t);
  }

  public static double NextDoubleInRange(this Random random, double min, double max)
  {
    double randomDouble = random.NextDouble();
    double result = min + (randomDouble * (max - min));
    return result;
  }

  public static float NextFloatInRange(this Random random, float min, float max)
  {
    float randomFloat = (float)random.NextDouble();
    float result = min + (randomFloat * (max - min));
    return result;
  }

  public static Vector2 RandomVector(float minX, float maxX, float minY, float maxY)
  {
    Random rand = new();
    float randomX = (float)GD.RandRange(minX, maxX);
    float randomY = (float)GD.RandRange(minY, maxY);
    return new Vector2(randomX, randomY);
  }
}



/// <summary>
/// A dictionary will throw an error if the sum of its elements is not 1
/// </summary>
public class ProportionDictionary<T> : SortedDictionary<T, float>
{
  public new void Add(T key, float value)
  {
    if (!IsValidToAdd(value))
      throw new ArgumentException("The sum of values would exceed 1.");

    base.Add(key, value);
  }

  public new void Remove(T key)
  {
    base.Remove(key);
  }

  public new void Clear()
  {
    base.Clear();
  }

  public new float this[T key]
  {
    get { return base[key]; }
    set
    {
      if (!IsValidToSet(key, value))
        throw new ArgumentException("The sum of values would exceed 1(100%).");

      base[key] = value;
    }
  }

  private bool IsValidToAdd(float value)
  {
    float sum = Values.Sum() + value;
    return sum <= 1;
  }

  private bool IsValidToSet(T key, float value)
  {
    float currentSum = Values.Sum() - this[key];
    float sum = currentSum + value;
    return sum <= 1;
  }
}
