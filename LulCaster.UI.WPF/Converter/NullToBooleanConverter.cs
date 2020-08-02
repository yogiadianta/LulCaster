﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace LulCaster.UI.WPF.Converter
{
  public class NullToBooleanConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return value != null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return value ?? null;
    }
  }
}