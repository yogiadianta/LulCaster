﻿using LulCaster.UI.WPF.Dialogs;
using LulCaster.UI.WPF.Dialogs.Models;
using LulCaster.UI.WPF.Dialogs.Services;
using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace LulCaster.UI.WPF.Controls
{
  /// <summary>
  /// Interaction logic for InteractableListControl.xaml
  /// </summary>
  public partial class InteractableListControl : UserControl
  {
    #region "Public Events"
    public event EventHandler<InputDialogResult> NewItemDialogExecuted;

    public event EventHandler<LulDialogResult> DeleteItemDialogExecuted;

    public event EventHandler<IListItem> SelectionChanged;

    #endregion "Public Events"

    #region "Dependency Properties"

    public static readonly DependencyProperty TitleProperty =
    DependencyProperty.Register
    (
        "Title",
        typeof(string),
        typeof(InteractableListControl)
    );

    public static readonly DependencyProperty ItemDescriptorProperty =
    DependencyProperty.Register
    (
        "ItemDescriptor",
        typeof(string),
        typeof(InteractableListControl)
    );

    public static readonly DependencyProperty ItemListProperty =
    DependencyProperty.Register
    (
        "ItemList",
        typeof(IList),
        typeof(InteractableListControl)
    );

    public static readonly DependencyProperty SelectedItemProperty =
    DependencyProperty.Register
    (
        "SelectedItem",
        typeof(IListItem),
        typeof(InteractableListControl),
        new FrameworkPropertyMetadata(SelectionDependancyPropertyChanged)
    );

    private static void SelectionDependancyPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
      if (sender is InteractableListControl thisControl)
      {
        thisControl.SelectedItem = (IListItem)e.NewValue;
      }
    }

    #endregion "Dependency Properties"

    #region "Properties"

    public string Title
    {
      get { return (string)GetValue(TitleProperty); }
      set { SetValue(TitleProperty, value); }
    }

    /// <summary>
    /// This is the word that will be used to describe a item in the list.
    /// </summary>
    public string ItemDescriptor
    {
      get { return (string)GetValue(ItemDescriptorProperty); }
      set { SetValue(ItemDescriptorProperty, value); }
    }

    public string AddToolTip
    {
      get => $"Add New {ItemDescriptor}";
    }

    public string DeleteToolTip
    {
      get => $"Delete Selected {ItemDescriptor}";
    }

    public IList ItemList
    {
      get { return (IList)GetValue(ItemListProperty); }
      set { SetValue(ItemListProperty, value); }
    }

    public IListItem SelectedItem
    {
      get { return (IListItem)GetValue(SelectedItemProperty); }
      set 
      { 
        SetValue(SelectedItemProperty, value);
        SelectionChanged?.Invoke(this, value);
      }
    }

    public IDialogService<InputDialog, InputDialogResult> InputDialog { get; set; }
    public IDialogService<MessageBoxDialog, LulDialogResult> MessageBoxService { get; set; }

    #endregion "Properties"

    public InteractableListControl()
    {
      InitializeComponent();
    }

    private void Button_btnAddPreset(object sender, RoutedEventArgs e)
    {
      if (InputDialog.Show($"New {ItemDescriptor}", $"Enter New {ItemDescriptor} Name: ", DialogButtons.OkCancel) is InputDialogResult dialogResult)
      {
        if (dialogResult.DialogResult == DialogResults.Ok && string.IsNullOrWhiteSpace(dialogResult.Input))
        {
          MessageBoxService.Show("Empty Preset Name", "No Preset Name Provided!!", Dialogs.DialogButtons.Ok);
          return;
        }

        NewItemDialogExecuted?.Invoke(this, dialogResult);
      }
    }

    private void Button_BtnDeletePreset(object sender, RoutedEventArgs e)
    {
      if (MessageBoxService.Show($"Delete {ItemDescriptor}", $"Do you want to delete selected {ItemDescriptor}(s)?", DialogButtons.YesNo) is LulDialogResult dialogResult)
      {
        DeleteItemDialogExecuted?.Invoke(this, dialogResult);
      }
    }
  }
}