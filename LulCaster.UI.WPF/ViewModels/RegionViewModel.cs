﻿using LulCaster.Utility.Common.Logic;
using System;
using System.Drawing;

namespace LulCaster.UI.WPF.ViewModels
{
  public class RegionViewModel
  {
    public Guid Id { get; set; }
    public LogicSets LogicSet { get; set; }
    public string Label { get; set; }
    public string TriggerValue { get; set; }
    public string SoundFilePath { get; set; }
    public Rectangle BoundingBox { get; set; }
  }
}