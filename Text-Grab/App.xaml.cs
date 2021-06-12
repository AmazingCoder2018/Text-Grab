﻿using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using Text_Grab.Properties;
using Text_Grab.Views;
using Windows.System.UserProfile;

namespace Text_Grab
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        public static List<string> InstalledLanguages => GlobalizationPreferences.Languages.ToList();

        void appStartup(object sender, StartupEventArgs e)
        {
            // Register COM server and activator type
            DesktopNotificationManagerCompat.RegisterActivator<TextGrabNotificationActivator>();

            for (int i = 0; i != e.Args.Length; ++i)
            {
                if (e.Args[i] == "Settings")
                {
                    SettingsWindow sw = new SettingsWindow();
                    sw.Show();
                }
                if (e.Args[i] == "GrabFrame")
                {
                    GrabFrame gf = new GrabFrame();
                    gf.Show();
                }
                if (e.Args[i] == "Fullscreen")
                {
                    NormalLaunch();
                }
                if (e.Args[i] == "EditText")
                {
                    ManipulateTextWindow manipulateTextWindow = new ManipulateTextWindow();
                    manipulateTextWindow.Show();
                }
            }

            if(e.Args.Length == 0 && Settings.Default.FirstRun == false)
            {
                switch (Settings.Default.DefaultLaunch)
                {
                    case "Fullscreen":
                        NormalLaunch();
                        break;
                    case "GrabFrame":
                        GrabFrame gf = new GrabFrame();
                        gf.Show();
                        break;
                    default:
                        NormalLaunch();
                        break;
                }
            }

            // if (true)
            if (Settings.Default.FirstRun)
            {
                FirstRunWindow frw = new FirstRunWindow();
                frw.Show();

                Settings.Default.FirstRun = false;
                Settings.Default.Save();
            }
        }

        public void NormalLaunch()
        {
            // base.OnActivated(e);

            Screen[] allScreens = Screen.AllScreens;
            WindowCollection allWindows = Current.Windows;

            foreach (Screen screen in allScreens)
            {
                bool screenHasWindow = true;

                foreach (Window window in allWindows)
                {
                    System.Drawing.Point windowCenter =
                        new System.Drawing.Point(
                            (int)(window.Left + (window.Width / 2)),
                            (int)(window.Top + (window.Height / 2)));
                    screenHasWindow = screen.Bounds.Contains(windowCenter);
                }

                if (allWindows.Count < 1)
                    screenHasWindow = false;

                if (screenHasWindow == false)
                {
                    FullscreenGrab mw = new FullscreenGrab
                    {
                        WindowStartupLocation = WindowStartupLocation.Manual,
                        Width = 200,
                        Height = 200,

                        WindowState = WindowState.Normal
                    };

                    if (screen.WorkingArea.Left >= 0)
                        mw.Left = screen.WorkingArea.Left;
                    else
                        mw.Left = screen.WorkingArea.Left + (screen.WorkingArea.Width / 2);

                    if (screen.WorkingArea.Top >= 0)
                        mw.Top = screen.WorkingArea.Top;
                    else
                        mw.Top = screen.WorkingArea.Top + (screen.WorkingArea.Height / 2);

                    mw.Show();
                }
            }
        }
    }
}
