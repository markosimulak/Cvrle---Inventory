using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// Interaction logic for RezultatObrade.xaml
    /// </summary>
    public partial class RezultatObrade : Window
    {
        public RezultatObrade(string obrada)
        {
            InitializeComponent();

            textBlock.Text = obrada;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                this.Close();
        }
    }
}
