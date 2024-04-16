using System;
using Godot;

namespace Entity;

public static partial class EntityAttributes
{
  public class Intelligence : EntityAttributeBase
  {
    public override string Abbreviation { get; } = "int";
    public override string Name { get; } = "Intelligence";
  }
}