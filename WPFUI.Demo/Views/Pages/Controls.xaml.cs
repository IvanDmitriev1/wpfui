// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Collections.ObjectModel;
using System.Windows.Controls;
using WPFUI.Controls;
using WPFUI.DIControls.Interfaces;
using Icon = WPFUI.Common.Icon;

namespace WPFUI.Demo.Views.Pages;

public enum OrderStatus
{
    None,
    New,
    Processing,
    Shipped,
    Received
};

/// <summary>
/// Interaction logic for Controls.xaml
/// </summary>
public partial class Controls : Page
{
    private readonly App.DialogResolver _dialogResolver;
    private readonly App.SnackBarResolver _snackbarResolver;

    public class Customer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsMember { get; set; }
        public OrderStatus Status { get; set; }
    }

    public ObservableCollection<string> ListBoxItemCollection { get; set; }

    public ObservableCollection<Customer> DataGridItemCollection { get; set; }

    public Controls(App.DialogResolver dialogResolver, App.SnackBarResolver snackbarResolver)
    {
        _dialogResolver = dialogResolver;
        _snackbarResolver = snackbarResolver;

        InitializeComponent();

        ListBoxItemCollection = new ObservableCollection<string>()
        {
            "Somewhere over the rainbow",
            "Way up high",
            "And the dreams that you dream of",
            "Once in a lullaby, oh"
        };

        DataGridItemCollection = new ObservableCollection<Customer>()
        {
            new()
            {
                Email = "john.doe@example.com", FirstName = "John", LastName = "Doe", IsMember = true,
                Status = OrderStatus.Processing
            },
            new()
            {
                Email = "chloe.clarkson@example.com", FirstName = "Chloe", LastName = "Clarkson", IsMember = true,
                Status = OrderStatus.Processing
            },
            new()
            {
                Email = "eric.brown@example.com", FirstName = "Eric", LastName = "Brown", IsMember = false,
                Status = OrderStatus.New
            }
        };

        DataContext = this;
    }

    private async void Button_ShowDialog_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        await _dialogResolver.Invoke().ShowDialog("What is it like to be a scribe? Is it good? In my opinion it's not about being good or not good. If I were to say what I esteem the most in life, I would say - people. People, who gave me a helping hand when I was a mess, when I was alone. And what's interesting, the chance meetings are the ones that influence our lives. The point is that when you profess certain values, even those seemingly universal, you may not find any understanding which, let me say, which helps us to develop. I had luck, let me say, because I found it. And I'd like to thank life. I'd like to thank it - life is singing, life is dancing, life is love. Many people ask me the same question, but how do you do that? where does all your happiness come from? And i replay that it's easy, it's cherishing live, that's what makes me build machines today, and tomorrow... who knows, why not, i would dedicate myself to do some community working and i would be, wham, not least... planting .... i mean... carrots");

        //(((Container)System.Windows.Application.Current.MainWindow)!).RootDialog.Show = true;
    }

    private void Button_ShowSnackbar_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        _snackbarResolver.Invoke().Expand("Remember that the Heat Death of Universe is coming someday, no time to explain - let's go!", Icon.PuzzlePiece24, 2000);

        //(((Container)System.Windows.Application.Current.MainWindow)!).RootSnackbar.Expand();
    }

    private void Button_ShowBox_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        WPFUI.Controls.MessageBox messageBox = new WPFUI.Controls.MessageBox();

        messageBox.ButtonLeftName = "Hello World";
        messageBox.ButtonRightName = "Just close me";

        messageBox.ButtonLeftClick += MessageBox_LeftButtonClick;
        messageBox.ButtonRightClick += MessageBox_RightButtonClick;
        
        messageBox.Show("Something weird", "May happen");
    }

    private void Button_ShowContextMenu_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        //CREATE PROGRAMMATICALLY
        //Right now it is not possible to open the ContextMenu from CodeBehind (ContextMenu.IsOpen = true) because when it is not created again after a Theme-Change it will not use the new Theme
        //Without creating it in CodeBehing the Theme-Change during Runtime would therefore not have any affect on the ContextMenu until you force it to rebuild by opening it through a Right-Click onto it's host-card
        //Comment and temporary fix by https://github.com/ParzivalExe
        ContextMenu contextMenu = new ContextMenu();
        contextMenu.Items.Add(new MenuItem()
        {
            Header = "Normal Item"
        });
        contextMenu.Items.Add(new MenuItem()
        {
            Header = "With Icon",
            Icon = WPFUI.Common.Icon.TextBox16
        });
        contextMenu.Items.Add(new MenuItem()
        {
            Header = "Deactivated",
            IsEnabled = false
        });
        MenuItem subMenu = new MenuItem()
        {
            Header = "SubMenu"
        };
        subMenu.Items.Add(new MenuItem()
        {
            Header = "Item 1"
        }); 
        subMenu.Items.Add(new MenuItem()
        {
            Header = "Item 2"
        });
        contextMenu.Items.Add(subMenu);

        contextMenu.IsOpen = true;
    }

    private void MessageBox_LeftButtonClick(object sender, System.Windows.RoutedEventArgs e)
    {
        (sender as MessageBox)?.Close();
    }

    private static void MessageBox_RightButtonClick(object sender, System.Windows.RoutedEventArgs e)
    {
        (sender as WPFUI.Controls.MessageBox)?.Close();
    }
}