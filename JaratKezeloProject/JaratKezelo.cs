namespace JaratKezeloProject
{
    public class JaratKezelo
    {
        private List<Jarat> jaratLista;
        public List<Jarat> JaratLista { get => jaratLista; }
        public JaratKezelo()
        {
            this.jaratLista = new List<Jarat>();
        }
        public void UjJarat(string jaratSzam, string repterHonnan, string repterHova, DateTime indulas)
        {
            //francba! ellenőrizni kéne minden válrozóra, hogy null vagy üres-e
            //nincs időm megírni már
           
            Jarat hozzaadando = new Jarat(jaratSzam, repterHonnan, repterHova, indulas);
            if (jaratSzam is null  || repterHonnan is null || repterHova is null)
            {
                throw new ArgumentException();
            }
            if (jaratSzam == "" || repterHonnan == "" || repterHova == "")
            {
                throw new ArgumentException();
            }
            foreach (Jarat jarat in jaratLista)
            {
                if (jarat.jaratSzam == hozzaadando.jaratSzam)
                {
                    throw new ArgumentException();
                }
            }
            this.jaratLista.Add(hozzaadando);

        }
        public void Keses(string jaratSzam, int keses)
        {
            int i = 0;
            while (i<jaratLista.Count && jaratLista[i].jaratSzam!=jaratSzam)
            {
                i++;
            }
            if (i<jaratLista.Count)
            {
                jaratLista[i].keses += keses;
                
            }
            else
            {
            throw new ArgumentException();

            }          
            
        }
        public DateTime MikorIndul(string jaratSzam) 
        {
            int i = -1;
            bool megtalaltuk = false;
            while (i < jaratLista.Count && !megtalaltuk)
            {
                i++;
                megtalaltuk = jaratLista[i].jaratSzam == jaratSzam;
            }
            if (megtalaltuk)
            {
                return jaratLista[i].indulas.AddMinutes(Convert.ToDouble(jaratLista[i].keses));
            }
            else
            {
                throw new ArgumentException();

            }
        }
        public List<string> JaratokRepuloterrol(string repter)
        {
            throw new NotImplementedException();
        }
    }
}
