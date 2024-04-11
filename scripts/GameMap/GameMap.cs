using Godot;

namespace GameMap;

public partial class GameMap : Node2D
{
  [Export]
  public Camera2D Camera { get; set; }
}