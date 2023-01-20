using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibTeam09.Individuele_Projecten.StefWouters
{
    public class Hobby
    {
        private int aantalUrenPerWeek = 0;
        public int AantalUrenPerWeek
        { 
            get { return aantalUrenPerWeek; }
            set { aantalUrenPerWeek = value; }
        }

        private string naam;

        public string Naam
        {
            get { return naam; }
            set
            {
                if (string.IsNullOrWhiteSpace(value) || value == string.Empty)
                {
                    naam = "Gaming";
                }
                else
                {
                    naam = value;
                }           
            }

        }

        private string beschrijving;

        public string Beschrijving
        {
            get { return beschrijving; }
            set 
            { 
                if (value == null || value == string.Empty)
                {
                    beschrijving = "Het spelen van een videospel op een elektronisch systeem.";
                }
                else
                {
                    beschrijving = value;
                }
                
            }
        }

        public Hobby(string naam, string beschrijving)
        {
            Naam = naam;
            Beschrijving = beschrijving;
            AantalUrenPerWeek = 0;
        }
        public Hobby() : this("", "")
        {
            //Naam = "";
            //Beschrijving = "";
            //AantalUrenPerWeek = 0;
        }

        public void SetAantalUrenPerWeek(int[] uren)
        {
            int teller = 0;
            int arrayAantal = 0;
            int resultaat;

            if (uren == null || uren.Length == 0)
            {
                resultaat = 2;
            }
            else
            {
                for (int i = 0; i < uren.Length; i++)
                {
                    teller += uren[i];
                    arrayAantal++;
                }

                resultaat = teller / arrayAantal;
            }
               
            aantalUrenPerWeek = resultaat;

        }
    }
}
