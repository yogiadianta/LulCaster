﻿namespace LulCaster.UI.WPF.Dialogs.ViewModels
{
  public abstract class DialogViewModelBase
  {
    public string Title { get; set; }
    public string Message { get; set; }
    public DialogButtons MessageBoxButtons { get; set; }
  }
}