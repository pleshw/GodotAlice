using System;
using Godot;

namespace Entity;

public static partial class EntityAttributes
{
  public class Luck : EntityAttributeBase
  {
    public override string Abbreviation { get; } = "luk";
    public override string Name { get; } = "Luck";
  }
}