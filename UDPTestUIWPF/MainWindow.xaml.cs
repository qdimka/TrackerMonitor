﻿using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UDPServer;

namespace UDPTestUIWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        UDPDataModel dataUDP;
        UDPnet udpServer;

        bool stopSend = true;
        Random rnd = new Random();

        public MainWindow()
        {
            this.udpServer = new UDPnet();
            udpServer.eventReceivedMessage += OnShowReceivedMessage;

            InitializeComponent();
        }

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            var udpModel = (UDPDataModel) this.FindResource("UdpModel");

            // Нажата кнопка "Старт"
            if (StartButton.Content.ToString() == "Start")
            {
                // Выбрана отправка сообщения
                if(SendRadioButton.IsChecked == true)
                {
                    // Выбрана мультиотправка в цикле
                    if (CycleCheckBox.IsChecked == true)
                    {
                        MessageTextBox.Text = string.Empty;

                        CycleSendMessage(udpModel.IPAddress, udpModel.Port);
                    }
                    // Выбрана одиночная отправка
                    else
                    {
                        StatusLabel.Content = (string)await udpServer.SendMessageAsync(Encoding.ASCII.GetBytes(udpModel.Message), udpModel.IPAddress, udpModel.Port);
                    }
                }
                // Выбрано "Принимать сообщения"
                else
                {
                    StartButton.Content = "Stop";
                    StatusLabel.Content = "Принимаем сообщения...";

                    if (CycleCheckBox.IsChecked == true)
                    {
                        udpServer.StartReceiveAsync(udpModel.Port);
                    }
                    else
                    {
                        byte[] receiveMessage = (byte[])await udpServer.ReceiveSingleMessageAsync(udpModel.Port);
                        MessageTextBox.Dispatcher.Invoke(new Action(() => MessageTextBox.Text += Encoding.ASCII.GetString(receiveMessage) + "\n"));

                        StartButton.Content = "Start";
                        StatusLabel.Content = "Статус соединения...";
                    }
                }                
            }
            // Нажата кнопка "Стоп"
            else
            {
                StartButton.Content = "Start";
                StatusLabel.Content = "Статус соединения";

                stopSend = true; // Останавливаем мультиотправку
                udpServer.StopReceive();  // Останавливаем прием сообщений
            }
        }

        /// <summary>
        /// Отправляет случайные сообщения в цикле
        /// </summary>
        /// <param name="ipAddress">IP адрес получателя</param>
        /// <param name="port">Порт</param>
        private void CycleSendMessage(IPAddress ipAddress, int port)
        {
            StartButton.Content = "Stop";
            StatusLabel.Content = "Отправляем сообщения в цикле...";

            stopSend = false;
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    byte[] message = Encoding.ASCII.GetBytes(rnd.Next(1000, 10000).ToString());
                    udpServer.SendMessageAsync(message, ipAddress, port);

                    MessageTextBox.Dispatcher.Invoke(new Action(() => MessageTextBox.Text += string.Format("отправлено: {0}\n", Encoding.ASCII.GetString(message))));
                    Thread.Sleep(2000);

                    if (stopSend) break;
                }
            });
        }

        private void OnShowReceivedMessage(object sender, EventArgs e)
        {
            //GPSTrackerMessage message = new GPSTrackerMessage();

            MessageTextBox.Text += "\n";
            MessageTextBox.Text += (e as UDPMessage == null)? string.Empty : ((UDPMessage)e).ToString();
        }
    }
}
