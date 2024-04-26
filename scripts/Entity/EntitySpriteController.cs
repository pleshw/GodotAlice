
using Entity;
using GameManager;
using Godot;
using Scene;

namespace Entity;

public partial class EntitySpriteController(EntityAnimated entity) : GameResourceManagerBase<SpriteFrames>()
{
  public EntityAnimated Entity = entity;

  public void ChangeHat(SpriteFrames newSprite)
  {
    Entity.AnimatedBody.ChangePart("Hat", newSprite);
  }

  public void ChangeShirt(SpriteFrames newSprite)
  {
    Entity.AnimatedBody.ChangePart("Shirt", newSprite);
  }

  public void ChangePants(SpriteFrames newSprite)
  {
    Entity.AnimatedBody.ChangePart("Pants", newSprite);
  }

  public void ChangeBody(SpriteFrames newSprite)
  {
    Entity.AnimatedBody.ChangePart("Body", newSprite);
  }
}