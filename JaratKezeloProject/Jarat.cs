using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaratKezeloProject
{
    public class Jarat
    {
        public Jarat(string jaratSzam, string honnanRepter, string hovaRepter, DateTime indulas)
        {
            this.jaratSzam = jaratSzam;
            this.honnanRepter = honnanRepter;
            this.hovaRepter = hovaRepter;
            this.indulas = indulas;
            this.keses = 0;
        }

        public string jaratSzam { get; set; }
       public string honnanRepter { get; set; }
        public string hovaRepter { get; set; }
        public DateTime indulas { get; set; }
       public int keses { get; set; }

    }
}