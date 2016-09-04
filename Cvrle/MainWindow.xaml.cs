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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;

namespace Cvrle
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // kolekcija proizvoda
        public static ObservableCollection<Proizvod> proizvodiColl;

        // var za serijalizaciju podataka 
        private DataSerialize dataSeralize;


        //
        // Init 
        //
        public MainWindow()
        {
            InitializeComponent();

            proizvodiColl = new ObservableCollection<Proizvod>();
            dataSeralize = new DataSerialize();

            if (File.Exists("produkti.dat"))
                proizvodiColl = dataSeralize.DeSerializeObject<ObservableCollection<Proizvod>>("produkti.dat");

            dataGrid.ItemsSource = proizvodiColl;
            comboBox.ItemsSource = proizvodiColl;           
        }


        //
        // Na dugme  "+" otvori prozor za kreiranje novog proizvoda
        //
        private void button_Click(object sender, RoutedEventArgs e)
        {
            DodajProizvod window = new DodajProizvod();
            window.Owner = this;
            window.ShowDialog();
        }


        //
        // Refresuje iteme iz tabele
        //
        public void RefreshTable()
        {
            dataGrid.Items.Refresh();
        }


        //
        // Generisi tabele tabele da popune prazan prostor
        //
        private void generateColumnWidth(object sender, RoutedEventArgs e)
        {
            foreach (var column in dataGrid.Columns)
            {
                column.MinWidth = column.ActualWidth;
                column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }

        }


        //
        // Na dugme "-" brise proizvod iz kolekcije
        //
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Proizvod proizvod = dataGrid.SelectedItem as Proizvod;

            if (proizvod != null)
            {
                foreach (Proizvod p in proizvodiColl)
                {
                    p.proizvodi.Remove(proizvod);
                    p.gramazaProizvoda.Remove(proizvod.Ime);
                }

                proizvodiColl.Remove(proizvod);
            }
        }


        //
        // Ako je checkbox otkacen prikazi sastav proizvoda u tabeli
        //
        private void prikaziSastav(object sender, RoutedEventArgs e)
        {
            var cb = sender as CheckBox;
            dataGrid.SelectedIndex = -1;

            if (cb.IsChecked == true)
                dataGrid.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.VisibleWhenSelected;
            else
                dataGrid.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.Collapsed;
        }


        //
        // Izmena selektovanog proizvoda iz tabele
        //
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Proizvod proizvod = dataGrid.SelectedItem as Proizvod;

            if (proizvod != null)
            {
                IzmeniProizvod window = new IzmeniProizvod(proizvod);
                window.Owner = this;
                window.ShowDialog();
                RefreshTable();
            }
            else
                MessageBox.Show("Morate izabrati proizvod za izmenu.", "Obavestenje", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        //
        // Pri zatvaranju aplikacije snimi sve podatke
        //
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            dataSeralize.SerializeObject<ObservableCollection<Proizvod>>(proizvodiColl, "produkti.dat");
        }


        //
        // Izmena sadrzaja proizvoda - azuriranje
        //
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                uint vrednost = UInt32.Parse(textBox.Text);
                Proizvod proizvod = comboBox.SelectedItem as Proizvod;
                string novSastav = "";

                novSastav = NadjiSastav(proizvod, vrednost, "");

                if (proizvod.Porcija > 0)
                {
                    proizvod.Ukupno -= proizvod.Porcija * vrednost;
                    novSastav += "\t" + proizvod.Ime + "\t=\t" + proizvod.Ukupno + "\n";
                }

                RezultatObrade window = new RezultatObrade(novSastav);
                window.Owner = this;
                window.ShowDialog();

                RefreshTable();
                textBox.Clear();
            }
            catch (Exception ex)
            {
                textBox.BorderBrush = Brushes.Red;
                textBox.BorderThickness = new Thickness(1);
                MessageBox.Show("Kolicina mora biti ceo pozitivan broj i proizvod mora biti izabran!", "Greska!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        //
        // Rekurzivna funkcija za nalazenje svih sastojaka proizvoda
        //
        private string NadjiSastav(Proizvod proizvod, uint vrednost, string sastav)
        {
            string novSastav = sastav;

            foreach (Proizvod p in proizvod.proizvodi)
            {
                p.Ukupno -= vrednost * proizvod.gramazaProizvoda[p.Ime];
                novSastav += "\t" + p.Ime + "\t=\t" + p.Ukupno + "\n";
                novSastav = NadjiSastav(p, vrednost, novSastav);
            }

            return novSastav;
        }


        //
        // Na izmenu window state - maximize / minimize
        //
        private void Window_StateChanged(object sender, EventArgs e)
        {/*
            if (this.WindowState == WindowState.Maximized)
                dock.Visibility = Visibility.Visible;
            else if (this.WindowState == WindowState.Normal)
                dock.Visibility = Visibility.Hidden;*/
        }


        //
        // Kada se selektuje proizvod iz tabele da bude selektovan i u combobox-u
        //
        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            comboBox.SelectedItem = dataGrid.SelectedItem;
        }


        //
        // Snimanje / ucitavanje podataka
        //
        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();

                dlg.FileName = "Cvrle - Evidencija robe"; // Default file name
                dlg.DefaultExt = ".dat"; // Default file extension
                dlg.Filter = "Binary files (.dat)|*.dat"; // Filter files by extension

                // Show save file dialog box
                Nullable<bool> result = dlg.ShowDialog();

                // Process save file dialog box results
                if (result == true)
                {
                    // Save document
                    dataSeralize.SerializeObject<ObservableCollection<Proizvod>>(proizvodiColl, dlg.FileName);
                }
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

                dlg.DefaultExt = ".dat"; // Default file extension
                dlg.Filter = "Binary files (.dat)|*.dat"; // Filter files by extension

                // Show save file dialog box
                Nullable<bool> result = dlg.ShowDialog();

                if (result == true)
                {
                    // load file
                    proizvodiColl = dataSeralize.DeSerializeObject<ObservableCollection<Proizvod>>(dlg.FileName);
                    dataGrid.ItemsSource = proizvodiColl;
                    comboBox.ItemsSource = proizvodiColl;
                }
            }

            comboBox1.SelectedIndex = -1;
        }
    }
}
