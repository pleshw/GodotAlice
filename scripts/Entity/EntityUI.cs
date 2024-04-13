using Godot;

namespace Entity;

public partial class Entity
{
  public Control UIMenu
  {
    get
    {
      return PlayerMenu.GetNode<Control>("Menu");
    }
  }

  public Control UIMenuTabContainer
  {
    get
    {
      return UIMenu.GetNode<Control>("TabContainer");
    }
  }

  public Control UIMenuPlayerTab
  {
    get
    {
      return UIMenuTabContainer.GetNode<Control>("PlayerTab");
    }
  }

  public Control UIAttributesControlNode
  {
    get
    {
      return UIMenuPlayerTab.GetNode<Control>("Attributes");
    }
  }

  public Control UIEquippedItemsControlNode
  {
    get
    {
      return UIMenuPlayerTab.GetNode<Control>("EquippedItems");
    }
  }

  public Control UIStatusControlNode
  {
    get
    {
      return UIMenuPlayerTab.GetNode<Control>("Status");
    }
  }

  public Control UIInventoryControlNode
  {
    get
    {
      return UIMenuPlayerTab.GetNode<Control>("Inventory");
    }
  }

  public Control UIQuestsControlNode
  {
    get
    {
      return UIMenuPlayerTab.GetNode<Control>("Quests");
    }
  }
}