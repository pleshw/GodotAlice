using System;
using Godot;

namespace Entity;

[GlobalClass]
public partial class EntityInventoryItem : Resource
{
  [Export]
  public string Id;

  [Export]
  public string Name;

  [Export]
  public string Description;

  [Export]
  public Texture2D Texture;
}