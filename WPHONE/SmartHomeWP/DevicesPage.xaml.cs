using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SmartHomeWP.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace SmartHomeWP {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page {




        public MainPage() {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        class ControlTag {
            public int deviceID;
            public int lastValue;
            public bool changing = false;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e) {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
            DevicesListView.Items.Clear();
            if (e.Parameter != null) {
                try {
                    Room room = (Room)e.Parameter;
                    RoomTextBlock.Text = "Room " + room.Name;
                    Rect.Fill = new SolidColorBrush(Color.FromArgb(15,15,23,0));

                    if (App.connected) {

                        App.proxy.Invoke<String>("getDevices",room.ID).ContinueWith(async task => {
                            String result = task.Result;
                            ICollection<Models.Device> devices = JsonConvert.DeserializeObject<ICollection<Models.Device>>(result);
                            App.devices = devices;
                            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () => {
                                
                                if (App.devices != null) {
                                    
                                    ChooseAnotherTextBlock.Visibility = Visibility.Visible;
                                    foreach (Device b in App.devices) {

                                        switch (b.DeviceType){
                                            case DeviceType.DIMMER:
                                                Slider slider = new Slider();
                                                slider.MinWidth = 280;
                                                slider.Margin = new Thickness(10, 0, 10, 0);
                                                slider.Header = b.Name;
                                                slider.Minimum = 0;
                                                slider.Maximum = 255;
                                                ControlTag tag = new ControlTag();
                                                tag.deviceID = b.ID;
                                                tag.changing = false;
                                                slider.Tag = tag;
                                                slider.ValueChanged += slider_ValueChanged;
                                                slider.PointerCaptureLost += slider_PointerCaptureLost;
                                                DevicesListView.Items.Add(slider);
                                                App.proxy.Invoke<int>("getDeviceValue", b.ID).ContinueWith(async task2 => {
                                                    int value = task2.Result;
                                                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                                                        slider.Value = value;
                                                        ((ControlTag)slider.Tag).lastValue = value;
                                                    });
                                                });
                                                break;
                                            case DeviceType.POWER_SWITCH:
                                                ToggleSwitch newSwitch = new ToggleSwitch();
                                                newSwitch.Header = b.Name;
                                                newSwitch.MinWidth = 280;
                                                tag = new ControlTag();
                                                tag.deviceID = b.ID;
                                                tag.changing = false;
                                                newSwitch.Tag = tag;
                                                newSwitch.Toggled += newSwitch_Toggled;
                                                DevicesListView.Items.Add(newSwitch);
                                                App.proxy.Invoke<int>("getDeviceValue", b.ID).ContinueWith(async task2 => {
                                                    int value = task2.Result;
                                                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                                                        if (value == 0) {
                                                            newSwitch.IsOn = false;
                                                        } else {
                                                            newSwitch.IsOn = true;
                                                        }
                                                    });
                                                });
                                                break;
                                            case DeviceType.THERMOMETER:
                                                TextBlock NameTextBlock = new TextBlock();
                                                NameTextBlock.FontSize = 24;
                                                NameTextBlock.Text = b.Name;
                                                tag = new ControlTag();
                                                tag.deviceID = -1;
                                                tag.changing = false;
                                                NameTextBlock.Tag = tag;
                                                TextBlock tempTextBlock = new TextBlock();
                                                tempTextBlock.FontSize = 20;
                                                tempTextBlock.Text = "0";
                                                tempTextBlock.MinWidth = 300;
                                                tempTextBlock.HorizontalAlignment = HorizontalAlignment.Stretch;
                                                tempTextBlock.TextAlignment = TextAlignment.Center;
                                                tag = new ControlTag();
                                                tag.deviceID = b.ID;
                                                tag.changing = false;
                                                tempTextBlock.Tag = tag;
                                                DevicesListView.Items.Add(NameTextBlock);
                                                DevicesListView.Items.Add(tempTextBlock);
                                                App.proxy.Invoke<int>("getDeviceValue", b.ID).ContinueWith(async task2 => {
                                                    int value = task2.Result;
                                                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                                                        tempTextBlock.Text = value.ToString();
                                                    });
                                                });
                                                break;
                                        }
                                    }
                                }
                                if (App.devices == null || App.devices.Count == 0) {
                                    TextBlock textBlock2 = new TextBlock();
                                    textBlock2.FontSize = 24;
                                    textBlock2.Text = "It's empty here";
                                    DevicesListView.IsEnabled = false;
                                    DevicesListView.Items.Add(textBlock2);
                                }
                            });
                        });

                    }
                    ChooseRoomLabel.Visibility = Visibility.Collapsed;
                } catch (InvalidCastException) {
                    ChooseRoomLabel.Visibility = Visibility.Visible;
                    RoomTextBlock.Text = "";
                    ChooseAnotherTextBlock.Visibility = Visibility.Collapsed;
                    Rect.Fill = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));

                }

            } else {
                Rect.Fill = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));

                ChooseRoomLabel.Visibility = Visibility.Visible;
                RoomTextBlock.Text = "";
                ChooseAnotherTextBlock.Visibility = Visibility.Collapsed;
            }

        }

        void newSwitch_Toggled(object sender, RoutedEventArgs e) {
            App.proxy.Invoke("setDeviceValue", ((ControlTag)((ToggleSwitch)sender).Tag).deviceID, ((((ToggleSwitch)sender).IsOn)?1:0));
        }

        void slider_PointerCaptureLost(object sender, PointerRoutedEventArgs e) {
            App.proxy.Invoke("setDeviceValue", ((ControlTag)((Slider)sender).Tag).deviceID, (int)((Slider)sender).Value);
            ((ControlTag)((Slider)sender).Tag).changing = false;
        }


        void slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e) {
            ((ControlTag)((Slider)sender).Tag).changing = true;
            if (Math.Abs(((ControlTag)((Slider)sender).Tag).lastValue - (int)((Slider)sender).Value) > 5) {
                App.proxy.Invoke("setDeviceValue", ((ControlTag)((Slider)sender).Tag).deviceID, (int)((Slider)sender).Value);
                ((ControlTag)((Slider)sender).Tag).lastValue = (int)((Slider)sender).Value;
            }
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e) {


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
            App.proxy.Subscribe("DeviceValueChanged").Received += SettingsPage_Received;
        }

        private async void SettingsPage_Received(IList<Newtonsoft.Json.Linq.JToken> obj) {
            int DeviceID = JsonConvert.DeserializeObject<int>(obj[0].ToString());
            int value = JsonConvert.DeserializeObject<int>(obj[1].ToString());
            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                foreach (var item in DevicesListView.Items) {
                    if (((FrameworkElement)item).Tag != null && ((ControlTag)((FrameworkElement)item).Tag).deviceID == DeviceID) {
                        if (item is TextBlock) {
                            TextBlock textBlock = (TextBlock)item;
                            textBlock.Text = value.ToString();
                        } else if (item is Slider) {
                            if (!((ControlTag)((FrameworkElement)item).Tag).changing) {
                                Slider slider = (Slider)item;
                                slider.ValueChanged -= slider_ValueChanged;
                                slider.Value = value;
                                slider.ValueChanged += slider_ValueChanged;
                            }
                        } else if (item is ToggleSwitch) {
                            ToggleSwitch toggleSwitch = (ToggleSwitch)item;
                            if (value == 0) {
                                toggleSwitch.IsOn = false;
                            } else {
                                toggleSwitch.IsOn = true;
                            }
                        }
                    }
                }
            });
            
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

        public async Task showMessageDialog(String message) {
            var dlg = new MessageDialog(message);
            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () => await dlg.ShowAsync());
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e) {
            Frame.Navigate(typeof(SettingsPage));
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e) {
            App.connection.StateChanged -= ConnectionStateChanged;
            App.proxy.Subscribe("DeviceValueChanged").Received -= SettingsPage_Received;
        }

        private void ChooseRoomLabel_Tapped(object sender, TappedRoutedEventArgs e) {
            if (App.connected) {
                Frame.Navigate(typeof(BuildingsPage));
            }
        }

        private void ChooseAnotherTextBlock_Tapped(object sender, TappedRoutedEventArgs e) {
            if (App.connected) {
                Frame.Navigate(typeof(BuildingsPage));
            }
        }
    }
}
