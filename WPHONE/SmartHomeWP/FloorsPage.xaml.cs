﻿using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using SmartHomeWP.Common;
using SmartHomeWP.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace SmartHomeWP {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FloorsPage : Page {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public FloorsPage() {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper {
            get { return this.navigationHelper; }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            this.navigationHelper.OnNavigatedTo(e);
            FloorsListView.Items.Clear();
            LoadingProgressRing.Visibility = Visibility.Visible;
            if (e.Parameter != null) {
                int buildingID = (int)e.Parameter;


                if (App.connected) {
                    App.proxy.Invoke<String>("getFloors",buildingID).ContinueWith(async task => {
                        String result = task.Result;
                        ICollection<Models.Floor> floors = JsonConvert.DeserializeObject<ICollection<Models.Floor>>(result);
                        App.floors = floors;
                        await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () => {
                            FloorsListView.Items.Clear();
                            if (App.floors != null) {
                                foreach (Floor b in App.floors) {
                                    TextBlock textBlock2 = new TextBlock();
                                    textBlock2.FontSize = 24;
                                    textBlock2.Text = b.Name;
                                    textBlock2.Tag = b.ID;
                                    FloorsListView.Items.Add(textBlock2);
                                }
                            }
                            if (App.floors == null || App.floors.Count == 0) {
                                TextBlock textBlock2 = new TextBlock();
                                textBlock2.FontSize = 24;
                                textBlock2.Text = "It's empty here";
                                FloorsListView.IsEnabled = false;
                                FloorsListView.Items.Add(textBlock2);
                            }
                            LoadingProgressRing.Visibility = Visibility.Collapsed;

                        });
                    });

                }
            }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e) {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e) {
        }

        public async void showDisconnectedLabel(Boolean show) {
            if (show) {
                await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => DisconnectedLabel.Visibility = Visibility.Visible);
            } else {
                await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => DisconnectedLabel.Visibility = Visibility.Collapsed);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e) {
            if (!App.connected) {
                DisconnectedLabel.Visibility = Visibility.Visible;
            } else DisconnectedLabel.Visibility = Visibility.Collapsed;
            App.connection.StateChanged += ConnectionStateChanged;
        }

        private void ConnectionStateChanged(StateChange connection) {
            switch (connection.NewState) {
                case ConnectionState.Connected:
                    showDisconnectedLabel(false);
                    break;
                case ConnectionState.Connecting:
                    break;
                case ConnectionState.Disconnected:
                    showDisconnectedLabel(true);
                    break;
                case ConnectionState.Reconnecting:
                    break;
            }
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e) {
            Frame.Navigate(typeof(SettingsPage));
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e) {
            App.connection.StateChanged -= ConnectionStateChanged;
        }

        #region NavigationHelper registration


        protected override void OnNavigatedFrom(NavigationEventArgs e) {
            this.navigationHelper.OnNavigatedFrom(e);
        }
        public async void showMessageDialog(String message) {
            var dlg = new MessageDialog(message);
            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () => await dlg.ShowAsync());
        }

        #endregion


        private void FloorsListView_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            try {
                int floorID = (int)((TextBlock)FloorsListView.SelectedItem).Tag;
                Frame.Navigate(typeof(RoomsPage), floorID);
            } catch (InvalidCastException) {

            }
        }


    }
}
