using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ClassLibTeam09.Individuele_Projecten.StefWouters
{
    class StudentTest
    {

        [Test]
        public void IsLegeStudentStefWouters()
        {
            StefWouters student = new StefWouters();
            student.Naam = "";

            Assert.AreEqual(student.Naam, "Stef Wouters");
        }
    }
}
