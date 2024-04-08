using System;
using Godot;

namespace Entity;

public partial class Item : Resource
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