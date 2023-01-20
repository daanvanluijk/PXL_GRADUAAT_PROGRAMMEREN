using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibTeam09.Individuele_Projecten.StefWouters
{
    public class StefWouters
    {

        public List<Hobby> Hobbies  { get; set; }
        public string Voornaam;
        public string Achternaam;
        // class var > private static string voornaam;

        private string naam;
        public string Naam
        {
            get
            {
                return naam;
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    naam = "Stef Wouters";
                }
                else
                {
                    naam = value;
                }
            }
        }
        public string Hobby { get; set; }
       

        public StefWouters()
        {
            Voornaam = "Stef";
            Achternaam = "Wouters";
            Hobby hobby = new Hobby("Gamen", "Games spelen op een pc");
            Hobby hobby1 = new Hobby("Airsoft", "Met bb's schieten");
            Hobby hobby2 = new Hobby("Slapen", "Meestal in een bed");
            
            Hobbies = new List<Hobby>();

            Hobbies.Add(hobby);
            Hobbies.Add(hobby1);
            Hobbies.Add(hobby2);
            
        }
        
        

    }
}
