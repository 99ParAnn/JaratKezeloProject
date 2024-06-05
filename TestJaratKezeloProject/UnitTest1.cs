using JaratKezeloProject;
using System;
using System.Threading.Tasks.Sources;
namespace TestJaratKezeloProject
{
    public class Tests
    {
        DateTime vmikor;
        JaratKezelo jaratok;
        [SetUp]
        public void Setup()
        {
            vmikor = new DateTime(1955, 2, 9, 16,0,0);
            jaratok = new JaratKezelo();
        }

        [Test]
        public void KezeloLetezik()
        {
            Assert.That(jaratok is not null);
        }
        [Test]
        public void KezeloUresenJonLetre()
        {
            Assert.That(jaratok.JaratLista is not null);
        }

        //UjJarat függvény

        /*•Létrehoz egy új járatot, amit eltárol a járatkezelõben.
         * •Kezdetben nincs késés.
         * •A járatszámnak egyedinek kell lennie!
         
        duplikálr járatszám, üres járatszám
         */
        [Test]
        public void UjJaratLetezik()
        {
            jaratok.UjJarat("jaratszam", "repterhonnan", "repterhova", vmikor);
            Assert.That(jaratok.JaratLista.Count == 1);
        }
        [Test]
        public void KezdetbenNincsKeses()
        {
            jaratok.UjJarat("jaratszam", "repterhonnan", "repterhova", vmikor);
            Assert.That(jaratok.JaratLista[0].keses == 0);
        }
        [Test]
        public void KetUgyanolyanJaraszamHibaraFut()
        {
            jaratok.UjJarat("jaratszam", "repterhonnan", "repterhova", vmikor);
            Assert.Throws<ArgumentException>(() => { jaratok.UjJarat("jaratszam", "s", "d", vmikor); });
        }
        [Test]
        public void UresJaratszamHibaraFut()
        {
            Assert.Throws<ArgumentException>(() => { jaratok.UjJarat("", "s", "d", vmikor); });
        }

        //Keses függvény
        /*•A megadott késést hozzáadja a megadott járathoz.
         * •A keses paraméter percben értendõ.
         * •Ha többször hívjuk meg a függvényt a késések összeadódnak
         * •A keses paraméter negatív is lehet, ez azt jelenti, hogy egy korábban 
         * rögzített késést sikerült elhárítani, behozni.
         * •A szumma késés, azaz a Keses() függvénybõl összeadódó érték viszont 
         * sosem lehet negatív, a gép nem indulhat korábban, mint a kiírt idõpont. 
         * Ha mégis az lenne, akkor dobjunkNegativKesesException-t!*/

        //nehézkes megnézni, hogy jó repülõre kerül-e a késés
        [Test]
        public void KesesHozzaadodikAJoRepulohoz()
        {
            jaratok.UjJarat("elso", "repterhonnan", "repterhova", vmikor);
            jaratok.UjJarat("masodik", "repterhonnan", "repterhova", vmikor);
            jaratok.Keses("masodik", 30);
            Assert.That(() =>
            {
                bool output = true;
                foreach (var item in jaratok.JaratLista)
                {
                    if (item.jaratSzam == "elso" && item.keses != 0)
                    {
                        output = false;
                    }
                    if (item.jaratSzam == "masodik" && item.keses != 30)
                    {
                        output = false;
                    }
                }
                return output;
            });
        }
        [Test]
        public void KesesOsszeadodik()
        {
            jaratok.UjJarat("szam", "repterhonnan", "repterhova", vmikor);
            jaratok.Keses("szam", 30);
            jaratok.Keses("szam", 20);
            Assert.That(() =>
            {
                bool output = true;
                foreach (var item in jaratok.JaratLista)
                {
                    if (item.jaratSzam == "szam" && item.keses == 50)
                    {
                        return true;
                    }
                }
                return false;
            });
            Assert.Pass();
        }
        [Test]
        public void NegativKesesLevonodik()
        {
            jaratok.UjJarat("szam", "repterhonnan", "repterhova", vmikor);
            jaratok.Keses("szam", 30);
            jaratok.Keses("szam", -20);
            Assert.That(() =>
            {
                bool output = true;
                foreach (var item in jaratok.JaratLista)
                {
                    if (item.jaratSzam == "szam" && item.keses == 10)
                    {
                        return true;
                    }
                }
                return false;
            });
        }
        [Test]
        public void OsszesKesesNemLehetNegativ()
        {

            jaratok.UjJarat("szam", "repterhonnan", "repterhova", vmikor);
            //            Assert.Throws<NegativKesesException>(() => { jaratok.Keses("szam", -30); });
            //?? nem eszi meg a custom exceptionomet
            Assert.Throws<ArgumentException>(() => { jaratok.Keses("szam", -30); });
        }
        [Test]
        public void InvalidJaratszamhozKesesHibatDob()
        {
            //Assert.That(jaratok.JaratLista.Count == 1);
            Assert.Throws<ArgumentException>(() => { jaratok.Keses("szam", -30); });
        }

        // MikorIndul függvény
        /*Az indulás idõpontjához hozzáadja a késést.
         1. hozzáadódik a késés
         2. argument exceptiont dob, ha invalid a gép száma
         */

        [Test]
        public void MikorIndulJoSzamotAdVissza()
        {
            DateTime indulas = vmikor;
            jaratok.UjJarat("szam", "repterhonnan", "repterhova", indulas);
            jaratok.Keses("szam", 42);
            indulas= indulas.AddMinutes(Convert.ToDouble(42));
            Assert.That(jaratok.MikorIndul("szam") == indulas);
        }
        [Test]
        public void InvalidJaratszamhozMikorindulHibatDob()
        {
            Assert.Throws<ArgumentException>(() => { jaratok.MikorIndul(":("); });
        }
        //JaratokRepuloterrol függvény
        /*Azon  járatok  járatszámát  adja  vissza
         * ,  amelyek  a  paraméterben  megadott  nevû repülõtérrõl indulnak.
         1. Egy elemû listát vissza tud adni
        2. Egynél több elemû listát vissza tud adni
        3. Kihagyja azokat a járatokat, amik nem tartoznak a reptérhez*/

        [Test]
        public void JaratokRepuloterrolUreslista()
        {
            Assert.That(jaratok.JaratokRepuloterrol("akdhgsgh").Count == 0);
        }
        [Test]
        public void JaratokRepuloterrolEgyElem()
        {
            jaratok.UjJarat("elso", "repterhonnan", "repterhova", vmikor);
            Assert.That(jaratok.JaratokRepuloterrol("repterhonnan").Count == 1
						&& jaratok.JaratokRepuloterrol("repterhonnan")[0] == "elso");
        }  
        [Test]
        public void JaratokRepuloterrolTobbElem()
        {
            jaratok.UjJarat("elso", "repterhonnan", "repterhova", vmikor);
            jaratok.UjJarat("masodik", "repterhonnan", "repterhova", vmikor);
            List<string> lekerdezettLista = jaratok.JaratokRepuloterrol("repterhonnan");
            Assert.That(lekerdezettLista.Count == 2 && lekerdezettLista.Contains("elso") && lekerdezettLista.Contains("masodik"));

            Assert.Pass();
        }
        [Test]
        public void JaratokRepuloterrolCsakAJoRepter()
        { 
            jaratok.UjJarat("elso", "repterhonnan", "repterhova", vmikor);
            jaratok.UjJarat("masodik", "repterhonnan", "repterhova", vmikor);
            jaratok.UjJarat("harmadik", "masikrepter", "repterhova", vmikor);
            List<string> lekerdezettLista = jaratok.JaratokRepuloterrol("repterhonnan");
            Assert.That(!lekerdezettLista.Contains("harmadik"));
        }
    }
}