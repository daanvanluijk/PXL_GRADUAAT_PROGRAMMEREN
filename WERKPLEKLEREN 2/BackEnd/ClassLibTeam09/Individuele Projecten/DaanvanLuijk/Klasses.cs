using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibTeam09.DaanvanLuijk_Individueel_Project
{
    public class DaanvanLuijk
    {
        // variabelen
        public string voornaam = "Daan";
        public string achternaam = "van Luijk;";

        // properties
        public List<Hobby> Hobbies
        {
            get { return Hobbies; }
        }

        public DaanvanLuijk()
        {

        }

    }

    public class Hobby
    {
        public string naam;
    }
}
