﻿using System;
using System.Windows;
using System.Windows.Shell;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Navigation;

namespace AutoThemeChanger
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            Console.WriteLine("--------- AppStart");
            LanguageHelper();
            InitializeComponent();

            if (Properties.Settings.Default.FirstRun)
            {
                SystemTimeFormat();
                AddJumpList();
                Properties.Settings.Default.FirstRun = false;
            }
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            LanguageHelper();
            ButtonNavarTime_Click(this, null);
            DonationScreen();
            Updater updater = new Updater();
            updater.CheckNewVersion();
        }

        private void DonationScreen()
        {
            Random rdmnumber = new Random();
            int generatedNumber = rdmnumber.Next(1, 100);
            if (generatedNumber == 50)
            {
                MsgBox msgBox = new MsgBox(Properties.Resources.donationDescription, Properties.Resources.donationTitle, "smiley", "yesno")
                {
                    Owner = GetWindow(this)
                };
                msgBox.ShowDialog();
                var result = msgBox.DialogResult;
                if (result == true)
                {
                    System.Diagnostics.Process.Start("https://www.paypal.me/arminosaj");
                }
            }

        }

        private void LanguageHelper()
        {
            if (String.IsNullOrWhiteSpace(Properties.Settings.Default.Language.ToString()))
            {
                Properties.Settings.Default.Language = CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToString();
            }
            CultureInfo.CurrentUICulture = new CultureInfo(Properties.Settings.Default.Language, true);
        }

        private void SystemTimeFormat()
        {
            try
            {
                string sysFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern;
                sysFormat = sysFormat.Substring(0, sysFormat.IndexOf(":"));
                if (sysFormat.Equals("hh") | sysFormat.Equals("h"))
                {
                    Properties.Settings.Default.AlterTime = true;
                }
            }
            catch
            {

            }
        }

        private void AddJumpList()
        {
            JumpTask darkJumpTask = new JumpTask
            {
                Title = Properties.Resources.lblDarkTheme,//Dark theme
                Arguments = "/dark",
                CustomCategory = Properties.Resources.lblSwitchTheme//Switch current theme
            };
            JumpTask lightJumpTask = new JumpTask
            {
                Title = Properties.Resources.lblLightTheme,//Light theme
                Arguments = "/light",
                CustomCategory = Properties.Resources.lblSwitchTheme//Switch current theme
            };

            JumpList jumpList = new JumpList();
            jumpList.JumpItems.Add(darkJumpTask);
            jumpList.JumpItems.Add(lightJumpTask);
            jumpList.ShowFrequentCategory = false;
            jumpList.ShowRecentCategory = false;

            JumpList.SetJumpList(Application.Current, jumpList);
        }

        //application close behaviour
        private void Window_Closed(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
            Application.Current.Shutdown();
            Process.GetCurrentProcess().Kill();
        }

        //navigation bar
        private void ButtonNavarTime_Click(object sender, RoutedEventArgs e)
        {
            FrameNavbar.Navigate(new Uri(@"/Pages/PageTime.xaml", UriKind.Relative));
            NavbarRectangle.Margin = new Thickness(0, 45, 0, 0);
        }

        private void ButtonNavbarApps_Click(object sender, RoutedEventArgs e)
        {
            FrameNavbar.Navigate(new Uri(@"/Pages/PageApps.xaml", UriKind.Relative));
            NavbarRectangle.Margin = new Thickness(0,90,0,0);
        }

        private void ButtonNavbarWallpaper_Click(object sender, RoutedEventArgs e)
        {
            FrameNavbar.Navigate(new Uri(@"/Pages/PageWallpaper.xaml", UriKind.Relative));
            NavbarRectangle.Margin = new Thickness(0, 135, 0, 0);
        }

        private void ButtonNavbarSettings_Click(object sender, RoutedEventArgs e)
        {
            FrameNavbar.Navigate(new Uri(@"/Pages/PageSettings.xaml", UriKind.Relative));
            NavbarRectangle.Margin = new Thickness(0, 180, 0, 0);
        }

        private void ButtonNavbarAbout_Click(object sender, RoutedEventArgs e)
        {
            FrameNavbar.Navigate(new Uri(@"/Pages/PageAbout.xaml", UriKind.Relative));
            NavbarRectangle.Margin = new Thickness(0, 490, 0, 0);
        }

        private void FrameNavbar_Navigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Forward | e.NavigationMode == NavigationMode.Back)
            {
                e.Cancel = true;
            }
        }
    }
}