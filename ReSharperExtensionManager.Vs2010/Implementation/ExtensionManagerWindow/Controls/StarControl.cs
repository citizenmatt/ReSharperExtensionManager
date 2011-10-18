#region license
// Copyright 2011 Matt Ellis (@citizenmatt)
//
// This file is part of ReSharper Extension Manager.
//
// ReSharper Extension Manager is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// ReSharper Extension Manager is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with ReSharper Extension Manager.  If not, see <http://www.gnu.org/licenses/>.
#endregion

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CitizenMatt.ReSharper.ExtensionManager.Implementation.ExtensionManagerWindow.Controls
{
    public class StarControl : Control
    {
        private FrameworkElement _starPath;
        private double _value;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
        static StarControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(StarControl), new FrameworkPropertyMetadata(typeof(StarControl)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _starPath = GetTemplateChild("StarPath") as FrameworkElement;
            _starPath.Loaded += new RoutedEventHandler(_starPath_Loaded);
        }

        void _starPath_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateVisuals();
        }

        public void UpdateVisuals()
        {
            if (_starPath == null)
            {
                return;
            }

            if (_value < 0 || _value > 1)
            {
                return;
            }

            double width = _value * _starPath.ActualWidth;
            Rect rect = new Rect(0, 0, width, _starPath.ActualHeight);
            _starPath.Clip = new RectangleGeometry()
            {
                Rect = rect
            };
        }

        private static double RoundValue(double value)
        {
            if (value < 0)
            {
                value = 0;
            }
            else if (value > 1)
            {
                value = 1;
            }

            return value;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods")]
        public double Value
        {
            get
            {
                return (double)GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        /// <summary>
        /// Value Dependency Property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
            "Value",
            typeof(double),
            typeof(StarControl),
            new PropertyMetadata(new PropertyChangedCallback(ValueChanged)));

        private static void ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            StarControl starControl = d as StarControl;
            if (starControl != null)
            {
                starControl._value = RoundValue((double)e.NewValue);
                starControl.UpdateVisuals();
            }
        }
    }
}