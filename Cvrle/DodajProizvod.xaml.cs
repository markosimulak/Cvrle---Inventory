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
    /// Interaction logic for DodajProizvod.xaml
    /// </summary>
    public partial class DodajProizvod : Window
    {
        // proizvod za dodavanje
        private Proizvod proizvod;

        // poruka o gresci
        private string errorMsg = "";

        // poruka o gresci za tb
        private string errorMsgTb = "";

        // var za proveru greske
        private bool retTb = true;


        //
        // Konstruktor - Init
        //
        public DodajProizvod()
        {
            InitializeComponent();
            proizvod = new Proizvod();
            foreach (Proizvod p in MainWindow.proizvodiColl)
            {
                p.Enable = false;
            }

            listBox.ItemsSource = MainWindow.proizvodiColl;

        }


        //
        // Ako se klikne na "potvrdi" i ako je sve u redu...kreiraj novi proizvod
        //
        private void potvrdiBTN_Click(object sender, RoutedEventArgs e)
        {

            if (Validation() && retTb)
            {
                foreach (Proizvod p in listBox.SelectedItems)
                {
                    proizvod.proizvodi.Add(p);
                }

                MainWindow.proizvodiColl.Add(proizvod);
                this.Close();
            }
            else
                MessageBox.Show(errorMsg, "Greska!", MessageBoxButton.OK, MessageBoxImage.Error);
        }


        //
        // Ako je checkbox kliknut proveri njegovo stanje i unesi / obrisi iz liste selektovanih...
        //
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var cb = sender as CheckBox;
            var item = cb.DataContext;
            Proizvod p = item as Proizvod;

            if (cb.IsChecked == true)
            {
                listBox.SelectedItems.Add(item);
                p.Enable = true;
            }
            else
            {
                listBox.SelectedItems.Remove(item);
                p.Enable = false;

                if (p.gramazaProizvoda.ContainsKey(p.Ime))
                    p.gramazaProizvoda.Remove(p.Ime);
            }
        }
        

        //
        // Funkcija za proveru validnosti unetih podataka
        //
        private bool Validation()
        {
            bool ret = true;
            errorMsg = "";

            if (imeTB.Text.Trim().Equals(""))
            {
                imeTB.BorderBrush = Brushes.Red;
                imeTB.BorderThickness = new Thickness(1);
                errorMsg += "\nMorate uneti ime!";
                ret = false;
            }
            else
                proizvod.Ime = imeTB.Text;

            try
            {
                double ukupno = Double.Parse(ukTB.Text);

                if (ukTB.Text.Trim().Equals("") || ukupno < 0)
                {
                    ukTB.BorderBrush = Brushes.Red;
                    ukTB.BorderThickness = new Thickness(1);
                    errorMsg += "\nMorate uneti ukupnu kolicinu [ >= 0 ]!";
                    ret = false;
                }

                proizvod.Ukupno = ukupno;
            }
            catch (Exception ex)
            {
                ukTB.BorderBrush = Brushes.Red;
                ukTB.BorderThickness = new Thickness(1);
                errorMsg += "\nUkupna kolicina mora biti broj!";
                ret = false;
            }

            try
            {
                double porcija = Double.Parse(gramazaTB.Text);

                if (gramazaTB.Text.Trim().Equals("") || porcija < 0)
                {
                    gramazaTB.BorderBrush = Brushes.Red;
                    gramazaTB.BorderThickness = new Thickness(1);
                    errorMsg += "\nMorate uneti porciju [ >= 0 ]!";
                    ret = false;
                }

                proizvod.Porcija = porcija;
            }
            catch (Exception ex)
            {
                gramazaTB.BorderBrush = Brushes.Red;
                gramazaTB.BorderThickness = new Thickness(1);
                errorMsg += "\nPorcija mora biti broj!";
                ret = false;
            }

            return ret;
        }


        //
        // Na fokusiranje textboxa ocisti njegov sadrzaj
        //
        private void clearTB(object sender, RoutedEventArgs e)
        {
            string ime = (sender as TextBox).Name;

            if (ime == imeTB.Name)
            {
                imeTB.Text = "";
                imeTB.ClearValue(TextBox.BorderBrushProperty);
                imeTB.ClearValue(TextBox.BorderThicknessProperty);
            }
            else if (ime == ukTB.Name)
            {
                ukTB.Text = "";
                ukTB.ClearValue(TextBox.BorderBrushProperty);
                ukTB.ClearValue(TextBox.BorderThicknessProperty);
            }
            else
            {
                gramazaTB.Text = "";
                gramazaTB.ClearValue(TextBox.BorderBrushProperty);
                gramazaTB.ClearValue(TextBox.BorderThicknessProperty);
            }
        }


        //
        // Ako se klikne na "otkazi", zatvori prozor
        //
        private void otkaziBTN_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        //
        // Provera za unos gramaze sastojka
        //
        private void Gramaza_Enter(object sender, KeyEventArgs e)
        {
            var tb = sender as TextBox;
            var item = tb.DataContext;
            Proizvod p = item as Proizvod;
            retTb = true;

            if (e.Key == Key.Enter)
            {
                try
                {
                    if (tb.Text.Split().Equals(""))
                    {
                        tb.BorderBrush = Brushes.Red;
                        tb.BorderThickness = new Thickness(1);
                        MessageBox.Show("Morate uneti gramazu sastojka!", "Greska!", MessageBoxButton.OK, MessageBoxImage.Error);
                        retTb = false;
                    }
                    else
                    {
                        proizvod.gramazaProizvoda[p.Ime] = Double.Parse(tb.Text);
                        tb.ClearValue(TextBox.BorderBrushProperty);
                        tb.ClearValue(TextBox.BorderThicknessProperty);
                        tb.Background = Brushes.LightGreen;
                        retTb = true;
                    }
                }
                catch (Exception ex)
                {
                    tb.BorderBrush = Brushes.Red;
                    tb.BorderThickness = new Thickness(1);
                    MessageBox.Show("Porcija sastojka mora biti broj!", "Greska!", MessageBoxButton.OK, MessageBoxImage.Error);
                    retTb = false;
                }
            }
        }


        //
        // Provera da li je selekovanom itemu chekbox otkacen, ako jeste sve ok, ako nije, deselektuj ga
        //
        private void proveri_select(object sender, SelectionChangedEventArgs e)
        {
            int i = listBox.SelectedItems.Count - 1;
            if (i >= 0)
            {
                Proizvod p = listBox.SelectedItems[i] as Proizvod;

                if (!p.Enable)
                    listBox.SelectedItems.Remove(p);
            }
        }
    }
}
