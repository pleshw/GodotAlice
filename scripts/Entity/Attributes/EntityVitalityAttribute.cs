using System;
using Godot;

namespace Entity;

public static partial class EntityAttributes
{
  public class Vitality : EntityAttributeBase
  {
    public override string Abbreviation { get; } = "vit";
    public override string Name { get; } = "Vitality";
  }
}