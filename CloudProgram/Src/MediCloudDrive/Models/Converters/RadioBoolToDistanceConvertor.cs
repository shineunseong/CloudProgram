﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace MediCloudDrive.Models.Converters
{
    public class RadioBoolToDistanceConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.Equals(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.Equals(true) == true ? parameter : Binding.DoNothing;
        }
    }
}