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
    /// Interaction logic for IzmeniProizvod.xaml
    /// </summary>
    public partial class IzmeniProizvod : Window
    {
        // proizvod za izmenu
        private Proizvod proizvod;

        // poruka greske
        private string errorMsg = "";

        // indeks proizvoda iz kolekcije
        private int index = 0;

        // provera za gramazu
        private bool retTb = true;

        //
        // Konstruktor - Init
        //
        public IzmeniProizvod(Proizvod p)
        {
            InitializeComponent();

            proizvod = new Proizvod();
            this.DataContext = p;

            bool tempIndex = true;

            // LOSE !!!
            foreach (Proizvod pp in MainWindow.proizvodiColl)
            {
                if (!p.proizvodi.Contains(pp))
                    pp.Enable = false;
                else
                {
                    pp.Enable = true;
                    listBox.SelectedItems.Add(pp);
                }
                if (pp.Ime == p.Ime)
                    tempIndex = false;

                if (tempIndex)
                    index++;
            }
            // LOSE !!

            listBox.ItemsSource = MainWindow.proizvodiColl;
            proizvod = p;
        }


        //
        // Ako se klikne na "otkazi", zatvori prozor
        //
        private void otkaziBTN_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        //
        // Ako se klikne na "potvrdi" i ako je sve u redu..izmeni proizvod
        //
        private void potvrdiBTN_Click(object sender, RoutedEventArgs e)
        {
            if (Validation() && retTb)
            {
                proizvod.proizvodi.Clear();

                foreach (Proizvod p in listBox.SelectedItems)
                {
                    proizvod.proizvodi.Add(p);
                }

                MainWindow.proizvodiColl[index] = proizvod;
                this.Close();
            }
            else
                MessageBox.Show(errorMsg, "Greska!", MessageBoxButton.OK, MessageBoxImage.Error);
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
                    errorMsg += "\nMorate uneti ukupnu kolicinu [ > 0 ]!";
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
                double gramaza = Double.Parse(gramazaTB.Text);

                if (gramazaTB.Text.Trim().Equals("") || gramaza < 0)
                {
                    gramazaTB.BorderBrush = Brushes.Red;
                    gramazaTB.BorderThickness = new Thickness(1);
                    errorMsg += "\nMorate uneti gramazu [ > 0 ]!";
                    ret = false;
                }

                proizvod.Porcija = gramaza;
            }
            catch (Exception ex)
            {
                gramazaTB.BorderBrush = Brushes.Red;
                gramazaTB.BorderThickness = new Thickness(1);
                errorMsg += "\nGramaza mora biti broj!";
                ret = false;
            }

            return ret;
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
