using System;
using GameManagers;
using Godot;

namespace Entity;

[GlobalClass]
public partial class EntityInventoryItem : Resource, IGameResource
{
  [Export]
  public StringName ItemId { get; set; }

  [Export]
  public StringName ItemName { get; set; }

  [Export]
  public string ItemDescription { get; set; }

  [Export]
  public SpriteFrames Sprite { get; set; }
}