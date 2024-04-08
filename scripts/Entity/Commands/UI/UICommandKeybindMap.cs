using Godot;

namespace Entity.Commands.UI;

public class UICommandKeybindMap(Entity entity) : EntityCommandKeybind(entity)
{
  public UICommandController UI = new(entity);

  public override void BindDefaults()
  {
    BindKey(Key.Tab, UI.ToggleInventory);
  }

  public void BindToggleInventory()
  {
    BindKey(Key.Tab, UI.ToggleInventory);
  }
}