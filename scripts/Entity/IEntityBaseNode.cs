using System.Collections.Generic;
using Animation;
using Godot;

namespace Entity;

public interface IEntityBaseNode
{
  public Camera2D Camera { get; set; }
  public Dictionary<StringName, AnimationData> Animations { get; set; }
  public CharacterBody2D CollisionBody { get; set; }
  public CollisionShape2D[] CollisionShapes { get; set; }
  public Control PlayerMenu { get; set; }
}