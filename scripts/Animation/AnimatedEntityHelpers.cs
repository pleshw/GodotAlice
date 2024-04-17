using Animation;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Entity;

public partial class AnimatedEntity
{

  public Node2D AnimationsNode
  {
    get
    {
      return GetNode<Node2D>("Animations");
    }
  }

  public Node2D IdleAnimationsNode
  {
    get
    {
      return AnimationsNode.GetNode<Node2D>("Idle");
    }
  }

  public Node2D MovementAnimationsNode
  {
    get
    {
      return AnimationsNode.GetNode<Node2D>("Movement");
    }
  }

  public Node2D AttackAnimationsNode
  {
    get
    {
      return AnimationsNode.GetNode<Node2D>("Attack");
    }
  }

  public Dictionary<StringName, AnimatedBody> MovementRelatedSpritesByName
  {
    get
    {
      return GetMovementSpritesByName(MovementAnimationsNode);
    }
  }

  public Dictionary<StringName, AnimatedBody> IdleSpritesByName
  {
    get
    {
      return GetMovementSpritesByName(IdleAnimationsNode);
    }
  }

  public Dictionary<StringName, List<AnimatedSprite2D>> AttackSpritesByName
  {
    get
    {
      return GetAttackSpritesByName(AttackAnimationsNode);
    }
  }


  public Dictionary<StringName, AnimatedBody> AnimationsByName
  {
    get
    {
      return _animationsByName;
    }
  }

  public static Dictionary<StringName, AnimatedBody> GetMovementSpritesByName(Node2D node)
  {
    return node.GetChildren()
        .Select(c => c as AnimatedBody)
        .ToDictionary(sprite => sprite.Name, sprite => sprite);
  }

  public static Dictionary<StringName, List<AnimatedSprite2D>> GetAttackSpritesByName(Node2D node)
  {
    return node.GetChildren()
        .OfType<Node2D>()
        .Select(weapon => new
        {
          WeaponName = weapon.Name,
          Sprites = weapon.GetChildren().OfType<AnimatedSprite2D>().ToList()
        })
        .Where(weapon => weapon.Sprites.Count != 0)
        .ToDictionary(weapon => weapon.WeaponName, weapon => weapon.Sprites);
  }

  private void AddAnimationSprites(Dictionary<StringName, AnimatedBody> dictAnimations)
  {
    foreach (var kvp in dictAnimations)
    {
      _animationsByName[kvp.Key] = kvp.Value;
    }
  }
}