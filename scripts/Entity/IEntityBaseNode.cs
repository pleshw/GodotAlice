using Godot;

namespace Entity;

public interface IEntityBaseNode
{
  public Camera2D Camera { get; set; }
  public AnimatedSprite2D Sprite { get; set; }
  public CharacterBody2D CollisionBody { get; set; }
  public CollisionShape2D[] CollisionShapes { get; set; }
}