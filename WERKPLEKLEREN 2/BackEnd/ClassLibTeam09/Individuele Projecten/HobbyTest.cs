using NUnit.Framework;
using ClassLibTeam09;

namespace ClassLibTeam09.Individuele_Projecten.StefWouters
{
    public class HobbyTest
    {
        private Hobby defaultHobby;
        private Hobby voetbalHobby;
        private const string voetbalNaam = "Voetbal";
        private const string voetbalBeschrijving = "Twee teams proberen een bal in het doel te trappen van de tegenstanders.";
        private const string gamingNaam = "Gaming";
        private const string gamingBeschrijving = "Het spelen van een videospel op een elektronisch systeem.";


        [SetUp]
        public void Setup()
        {
            defaultHobby = new Hobby();
            voetbalHobby = new Hobby(voetbalNaam, voetbalBeschrijving);
        }

        [Test]
        public void HasDefaultValueGaming()
        {
            Assert.AreEqual(
                defaultHobby.Naam, "Gaming",
                "De standaard waarde voor Naam van een object aangemaakt met een constructor zonder parameters is niet correct.");
            Assert.AreEqual(
                defaultHobby.Beschrijving, "Het spelen van een videospel op een elektronisch systeem.",
                "De standaard waarde voor Beschrijving van een object aangemaakt met een constructor zonder parameters is niet correct.");
        }

        [Test]
        public void HasTwoConstructors()
        {
            Assert.IsNotNull(new Hobby(), "Er ontbreekt een constructor zonder parameters");
            Assert.IsNotNull(new Hobby("", ""), "Er ontbreekt een constructor met twee parameters");
        }

        [Test]
        public void IsNaamGetterWorking()
        {
            Assert.AreEqual(voetbalHobby.Naam, voetbalNaam);
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase(" ")]
        [TestCase("\t")]
        public void IsNaamSetterWorking(string value)
        {
            Assert.AreEqual(defaultHobby.Naam, gamingNaam,
                "Ongeldige input voor de property Naam wordt niet correct verwerkt.");
        }


        [Test]
        public void IsBeschrijvingGetterWorking()
        {
            Assert.AreEqual(voetbalHobby.Beschrijving, voetbalBeschrijving);
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase(" ")]
        [TestCase("\t")]
        public void IsBeschrijvingSetterWorking(string value)
        {
            Assert.AreEqual(defaultHobby.Beschrijving, gamingBeschrijving, 
                "Ongeldige input voor de property Beschrijving wordt niet correct verwerkt.");
        }

        [Test]
        public void IsAantalUrenPerWeekPropertyAanwezig()
        {
            int value = 9001;
            defaultHobby.AantalUrenPerWeek = value;
            Assert.AreEqual(defaultHobby.AantalUrenPerWeek, value);
        }


        [TestCase(new int[0] { }, 2)]
        [TestCase(null, 2)]
        [TestCase(new int[] { 2, 3, 6, 8, 9, 0, 1 }, 4)]
        [TestCase(new int[] { 4, 4, 4, 4, 4, 4, 3 }, 3)]
        public void AantalUrenPerWeek(int[] values, int result)
        {
            defaultHobby.SetAantalUrenPerWeek(values);
            Assert.AreEqual(defaultHobby.AantalUrenPerWeek, result, 
                "De methode SetaantalUrenPerWeek werkt niet correct.");
        }
    }
}