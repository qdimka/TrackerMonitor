﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UDPTestUIWPF
{
    public class SettingModel : DependencyObject
    {
        public static DependencyProperty SendReceiveProperty;
        public static DependencyProperty CycleProperty;
        public static DependencyProperty WriteToDBProperty;

        static SettingModel()
        {
            SendReceiveProperty = DependencyProperty.Register("SendReceive",
                                                                typeof(bool),
                                                                typeof(SettingModel),
                                                                new PropertyMetadata(true));

            CycleProperty = DependencyProperty.Register("Cycle",
                                                                typeof(bool),
                                                                typeof(SettingModel),
                                                                new PropertyMetadata(false));

            WriteToDBProperty = DependencyProperty.Register("WriteToDB",
                                                                typeof(bool),
                                                                typeof(SettingModel),
                                                                new PropertyMetadata(false));
        }

        public bool SendReceive
        {
            set { SetValue(SendReceiveProperty, value); }
            get { return (bool)GetValue(SendReceiveProperty); }
        }

        public bool Cycle
        {
            set { SetValue(CycleProperty, value); }
            get { return (bool)GetValue(CycleProperty); }
        }

        public bool WriteToDB
        {
            set { SetValue(WriteToDBProperty, value); }
            get { return (bool)GetValue(WriteToDBProperty); }
        }
    }
}
