using Godot;

namespace Entity.Commands;

public class UICommandKeybindMap(Entity entity) : EntityCommandKeybind(entity)
{
  public UICommandController UI = new(entity);

  public override void BindDefaults()
  {
    BindKey(Key.Tab, UI.TogglePlayerMenu);
  }

  public void BindTogglePlayerMenu()
  {
    BindKey(Key.Tab, UI.TogglePlayerMenu);
  }
}