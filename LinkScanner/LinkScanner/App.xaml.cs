using System;
using System.Collections.Generic;
using LinkScanner.Views;
using Xam.Plugin.SimpleAppIntro;
using Xamarin.Forms;

namespace LinkScanner
{
    public partial class App : Application
    {
        /// <summary>
        /// Shows the introduction page on first launch of the application
        /// </summary>
        public App()
        {
            InitializeComponent();

            if (Current.Properties.ContainsKey("FirstStart"))
            {
                MainPage = new NavigationPage(new MainPage())
                {
                    BarBackgroundColor = Color.FromHex("#2c3e50")
                };
            }
            else
            {
                MainPage = new SimpleAppIntro(new List<Slide> {
                    new Slide(new SlideConfig("Scan URL", "Scan URL by capturing or loading image from storage", 
                        "scan.png", "#2c3e50")),
                    new Slide(new SlideConfig("Internet", "Make sure that you have internet connection", 
                        "connection.png", "#2c3e50")),
                    new Slide(new SlideConfig("Browsing", "Open scanned URL in browser", 
                        "browsing.png", "#2c3e50"))})
                {
                    // Properties
                    ShowPositionIndicator = true,
                    ShowSkipButton = true,
                    ShowNextButton = true,
                    DoneText = "Finish",
                    NextText = "Next",
                    SkipText = "Skip",

                    // Theming
                    BarColor = "#607D8B",
                    SkipButtonBackgroundColor = "#2c3e50",
                    DoneButtonBackgroundColor = "#2c3e50",
                    NextButtonBackgroundColor = "#2c3e50",

                    // Callbacks
                    OnSkipButtonClicked = OnSkipButtonClicked,
                    OnDoneButtonClicked = OnDoneButtonClicked,
                    OnPositionChanged = OnPositionChanged,
                };
            }
        }

        /// <summary>
        /// On skip button clicked
        /// </summary>
        private void OnSkipButtonClicked()
        {
            MainPage = new NavigationPage(new MainPage())
            {
                BarBackgroundColor = Color.FromHex("#2c3e50")
            };

            Current.Properties["FirstStart"] = false;
        }

        /// <summary>
        /// On done button clicked
        /// </summary>
        private void OnDoneButtonClicked()
        {
            MainPage = new NavigationPage(new MainPage())
            {
                BarBackgroundColor = Color.FromHex("#2c3e50")
            };

            Current.Properties["FirstStart"] = false;
        }

        /// <summary>
        /// On slide changed event
        /// </summary>
        private void OnPositionChanged(int page)
        {
            Console.Write($"Slide changed to page {page}");
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
