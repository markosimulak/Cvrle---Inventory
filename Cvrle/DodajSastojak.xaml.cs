using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Cvrle
{
    /// <summary>
    /// Interaction logic for DodajSastojak.xaml
    /// </summary>
    public partial class DodajSastojak : Window
    {
        ListBox lb;

        public DodajSastojak(ListBox lista)
        {
            InitializeComponent();
            lb = new ListBox();
            lb = lista;
            int x = 0;
            var panel = new Grid { HorizontalAlignment = HorizontalAlignment.Center };
            foreach (Proizvod p in lista.SelectedItems)
            {
                panel.Children.Add(new Label
                {
                    Content = p.Ime,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Height = 33,
                    Margin = new Thickness(90, 0 + x, 0, 0),
                    VerticalAlignment = VerticalAlignment.Top,
                    Width = 53,
                    FontSize = 16
                });

                panel.Children.Add(new TextBox
                {
                    Text = "",
                    Name = "newS",
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Height = 28,
                    Margin = new Thickness(197, 0 + x, 0, 0),
                    VerticalAlignment = VerticalAlignment.Top,
                    Width = 94,
                    FontSize = 16
                });

                panel.Children.Add(new Label
                {
                    Content = "g",
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Height = 33,
                    Margin = new Thickness(352, 0 + x, 0, 0),
                    VerticalAlignment = VerticalAlignment.Top,
                    Width = 24,
                    FontSize = 16
                });

                x += 35;
            }

            //this.Content = panel;
            grid2.Children.Add(panel);
            
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
