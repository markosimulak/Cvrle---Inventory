using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Cvrle
{
    [Serializable]
    public class Proizvod
    {
        public ObservableCollection<Proizvod> proizvodi;
        public Dictionary<string, double> gramazaProizvoda;


        public Proizvod()
        {
            proizvodi = new ObservableCollection<Proizvod>();
            gramazaProizvoda = new Dictionary<string, double>();

            Ime = null;
            Ukupno =  -1;
            Porcija = -1;
        }


        public Proizvod(string ime, double uk, double g)
        {
            proizvodi = new ObservableCollection<Proizvod>();
            gramazaProizvoda = new Dictionary<string, double>();

            Ime = ime;
            Ukupno = uk;
            Porcija = g;
        }


        public String Ime { get; set; }


        public Double Ukupno { get; set; }


        public Double Porcija { get; set; }


        public bool Enable { get; set; }


        public Double Gramaza { get; set; }


        public String Sastav
        {
            get
            {
                string ret = "Sastojci:\n";
                if (proizvodi.Count > 0)
                {
                    foreach (Proizvod p in proizvodi)
                    {
                        ret += "\t" + p.Ime + "\t=\t" + gramazaProizvoda[p.Ime].ToString() + " g\n";
                    }
                }

                if (Porcija > 0)
                    ret = "Porcija:\t\t: " + Porcija.ToString() + " g";

                if (ret == "Sastojci:\n")
                    return "Ne postoji sastav.";

                return ret;
            }

            set
            {

            }
        }
    }
}
