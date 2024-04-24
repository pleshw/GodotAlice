
using Entity;
using GameManager;
using Godot;
using Scene;

namespace Entity;

public partial class EntitySpriteController : GameResourceManagerBase<SpriteFrames>
{
  public EntityAnimated Entity;

  public EntitySpriteController(EntityAnimated entity) : base()
  {
    Entity = entity;
  }

  public void ChangeHead(SpriteFrames newSprite)
  {
    Entity.AnimatedBody.ChangePart("Head", newSprite);
  }

  public void ChangeBody(SpriteFrames newSprite)
  {
    Entity.AnimatedBody.ChangePart("Body", newSprite);
  }

  public void ChangeLegs(SpriteFrames newSprite)
  {
    Entity.AnimatedBody.ChangePart("Legs", newSprite);
  }
}