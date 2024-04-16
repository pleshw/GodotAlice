using System;
using Godot;

namespace Entity;

public static partial class EntityAttributes
{
  public class Dexterity : EntityAttributeBase
  {
    public override string Abbreviation { get; } = "dex";
    public override string Name { get; } = "Dexterity";
  }
}