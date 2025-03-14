﻿using System;
using System.Reflection;
using System.Xml;
using System.Windows;
using System.Globalization;
using System.Diagnostics;
using AutoDarkModeSvc.Communication;
using AutoDarkModeComms;
using AutoDarkModeConfig;

namespace AutoDarkModeApp
{
    class Updater
    {
        ApiResponse response = new();
        private readonly ICommandClient commandClient;

        public Updater()
        {
            commandClient = new ZeroMQClient(Address.DefaultPort);
        }

        public bool CheckNewVersion()
        {
            response = ApiResponse.FromString(commandClient.SendMessageAndGetReply(Command.CheckForUpdateNotify));
            return UpdateAvailable();
        }

        public bool UpdateAvailable()
        {
            if (response.StatusCode == StatusCode.New)
            {
                return true;
            }
            return false;
        }

        public bool CanUseUpdater()
        {
            return response.StatusCode != StatusCode.Disabled;
        }

        public void Update()
        {
            UpdateInfo info = UpdateInfo.Deserialize(response.Details);
            ApiResponse updatePrepResponse = ApiResponse.FromString(commandClient.SendMessageAndGetReply(Command.Update));
            if (updatePrepResponse.StatusCode == StatusCode.New)
            {
                StartProcessByProcessInfo(info.GetUpdateInfoPage());
            }
        }

        /*
        public void MessageBoxHandler(Window owner = null)
        {
            CultureInfo.CurrentUICulture = new CultureInfo(Properties.Settings.Default.Language, true);
            if (UpdateAvailable())
            {
                if (!silent)
                {
                    UpdateInfo info = UpdateInfo.Deserialize(response.Details);
                    string text = string.Format(Properties.Resources.msgUpdaterText, response.Message, info.Tag);
                    MsgBox msgBox = new(text, "Auto Dark Mode Updater", "update", "yesno")
                    {
                        WindowStartupLocation = WindowStartupLocation.CenterScreen,
                        Topmost = true
                    };
                    if (owner != null)
                    {
                        msgBox.Owner = owner;
                    }
                    msgBox.ShowDialog();
                    bool? result = msgBox.DialogResult;
                    if (result == true)
                    {
                        Update();
                    }
                }
            }
        }
        */

        private static void ShowErrorMessage(Exception ex, string location)
        {
            string error = Properties.Resources.errorThemeApply + $"\n\nError ocurred in: {location}" + ex.Source + "\n\n" + ex.Message;
            MsgBox msg = new(error, Properties.Resources.errorOcurredTitle, "error", "yesno");
            msg.ShowDialog();
            var result = msg.DialogResult;
            if (result == true)
            {
                string issueUri = @"https://github.com/Armin2208/Windows-Auto-Night-Mode/issues";
                Process.Start(new ProcessStartInfo(issueUri)
                {
                    UseShellExecute = true,
                    Verb = "open"
                });
            }
            return;
        }

        private static void StartProcessByProcessInfo(string message)
        {
            Process.Start(new ProcessStartInfo(message)
            {
                UseShellExecute = true,
                Verb = "open"
            });
        }
    }
}