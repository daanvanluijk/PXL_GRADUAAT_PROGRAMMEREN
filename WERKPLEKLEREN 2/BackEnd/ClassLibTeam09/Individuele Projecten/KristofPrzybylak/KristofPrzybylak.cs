using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ClassLibTeam09.Individuele_Projecten.KristofPrzybylak
{
    public class KristofPrzybylak
    {
        public List<Hobbies> Hobbylist { get; set; }
        public string Voornaam { get; set; }
        public string Achternaam { get; set; }


        public KristofPrzybylak()
        {

            Hobbies hobby1 = new Hobbies("Lopen", "in deze hobby lopen we ");
            Hobbies hobby2 = new Hobbies("Dansen", "in deze hobby dansen we ");
            Hobbies hobby3 = new Hobbies("Dansen", "in deze hobby dansen we ");
            Hobbylist.Add(hobby1);
            Hobbylist.Add(hobby2);
            Hobbylist.Add(hobby3);
            
        }
    
      
        public void VoornaamNaam()
        {


        }
    }
}


    

