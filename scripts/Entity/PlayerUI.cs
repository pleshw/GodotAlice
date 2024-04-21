using System;
using Godot;

namespace Entity;

public partial class Player
{
  public Control UI
  {
    get
    {
      return MainScene.GetNode<Control>("UI");
    }
  }

  public Control UIMenu
  {
    get
    {
      return UI.GetNode<Control>("PlayerMenu");
    }
  }

  public Control UIMenuMarginContainer
  {
    get
    {
      return UI.GetNode<Control>("MarginContainer");
    }
  }

  public Control UIMenuTabContainer
  {
    get
    {
      return UIMenuMarginContainer.GetNode<Control>("TabContainer");
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
      return UIMenuPlayerTab.GetNode<Control>("AttributesPanel");
    }
  }

  public Control UIEquippedItemsControlNode
  {
    get
    {
      return UIMenuPlayerTab.GetNode<Control>("EquipmentPanel");
    }
  }

  public Control UIStatusControlNode
  {
    get
    {
      return UIMenuPlayerTab.GetNode<Control>("StatusPanel");
    }
  }

  public Control UIInventoryControlNode
  {
    get
    {
      return UIMenuPlayerTab.GetNode<Control>("InventoryPanel");
    }
  }

  public Control UIQuestsControlNode
  {
    get
    {
      return UIMenuPlayerTab.GetNode<Control>("QuestsPanel");
    }
  }
}