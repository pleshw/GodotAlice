using System.Collections.Generic;
using Animation;
using Godot;

namespace Entity;

public interface IEntityLifeStats
{
  public int MaxHealth { get; }
  public int CurrentHealth { get; set; }
  public int MaxMana { get; }
  public int CurrentMana { get; set; }
}