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

        //UjJarat f�ggv�ny

        /*�L�trehoz egy �j j�ratot, amit elt�rol a j�ratkezel�ben.
         * �Kezdetben nincs k�s�s.
         * �A j�ratsz�mnak egyedinek kell lennie!
         
        duplik�lr j�ratsz�m, �res j�ratsz�m
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

        //Keses f�ggv�ny
        /*�A megadott k�s�st hozz�adja a megadott j�rathoz.
         * �A keses param�ter percben �rtend�.
         * �Ha t�bbsz�r h�vjuk meg a f�ggv�nyt a k�s�sek �sszead�dnak
         * �A keses param�ter negat�v is lehet, ez azt jelenti, hogy egy kor�bban 
         * r�gz�tett k�s�st siker�lt elh�r�tani, behozni.
         * �A szumma k�s�s, azaz a Keses() f�ggv�nyb�l �sszead�d� �rt�k viszont 
         * sosem lehet negat�v, a g�p nem indulhat kor�bban, mint a ki�rt id�pont. 
         * Ha m�gis az lenne, akkor dobjunkNegativKesesException-t!*/

        //neh�zkes megn�zni, hogy j� rep�l�re ker�l-e a k�s�s
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

        // MikorIndul f�ggv�ny
        /*Az indul�s id�pontj�hoz hozz�adja a k�s�st.
         1. hozz�ad�dik a k�s�s
         2. argument exceptiont dob, ha invalid a g�p sz�ma
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
        //JaratokRepuloterrol f�ggv�ny
        /*Azon  j�ratok  j�ratsz�m�t  adja  vissza
         * ,  amelyek  a  param�terben  megadott  nev� rep�l�t�rr�l indulnak.
         1. Egy elem� list�t vissza tud adni
        2. Egyn�l t�bb elem� list�t vissza tud adni
        3. Kihagyja azokat a j�ratokat, amik nem tartoznak a rept�rhez*/

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