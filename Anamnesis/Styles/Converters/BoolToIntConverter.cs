﻿// © Anamnesis.
// Developed by W and A Walsh.
// Licensed under the MIT license.

namespace Anamnesis.Styles.Converters
{
	using System;
	using System.Globalization;
	using System.Windows.Data;

	[ValueConversion(typeof(bool), typeof(int))]
	public class BoolToIntConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool v = (bool)value;
			return v ? 1 : 0;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
