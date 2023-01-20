using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibTeam09.Individuele_Projecten.KristofPrzybylak
{
   public class Hobbies
    {
        private string naam;
        private string beschrijving;
        public int AantalUrenPerWeek { get; set; }

        public Hobbies()
        {

        }
        public Hobbies(string naam , string beschrijving)
        {
            Naam = naam;
            Beschrijving = beschrijving;
        }

        public string Beschrijving
        {
            get { return beschrijving; }
            set
            { 
                if(string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                {
                    beschrijving = "“Het spelen van een videospel op een elektronisch systeem.";

                }
                else
                {
                    beschrijving = value;
                }


                

            }
        }

        public string Naam
        {
            get { return naam; }
            set
            {
                if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                {
                    naam = "Gaming";
                }
                else
                {
                    naam = value;
                }
               
            }
        }


        public void SetAantalUrenPerWeek (int[] gemiddeldeUren)
        {
         
            int gemiddelde = 0;
            int result;

            if(gemiddeldeUren.Length == -1)
            {
                result = 2;
            }
            else
            {
                foreach (var item in gemiddeldeUren)
                {
                    gemiddelde += item;

                }
                result = gemiddelde / gemiddeldeUren.Length;
            }

        
            
        }

    }
}
