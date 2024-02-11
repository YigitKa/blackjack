using System;
namespace Blackjack
{
    public enum KartDegeri
    {
        As = 1,
        Ikı = 2,
        Uc = 3,
        Dort = 4,
        Bes = 5,
        Alti = 6,
        Yedi = 7,
        Sekiz = 8,
        Dokuz = 9,
        On = 10,
        Vale = 10,
        Kız = 10,
        Papaz = 10,
    }

    public enum KartRengi
    {
        Sinek,
        Karo,
        Maça,
        Kupa,
    }

    public delegate void KartCekildiEventHandler(object sender, EventArgs e);

    public class KartCekildiIsleyici
    {
        public void OnKartCekildi(object sender, EventArgs e)
        {
            // Kart çekme işlemini gerçekleştirir.
        }
    }

  
    public class Kart
    {
        public KartDegeri Deger { get; set; }
        public KartRengi Renk { get; set; }

        public Kart(KartDegeri deger, KartRengi renk)
        {
            Deger = deger;
            Renk = renk;
        }

        public override string ToString()
        {
            return $"{Deger} {Renk}";
        }

        public static List<Kart> DesteyiKar(List<Kart> deste)
        {
            // Kartları rastgele sıralayacak bir `Random` nesnesi oluşturma
            Random rnd = new Random();
            deste = new List<Kart>();

            foreach (KartDegeri deger in Enum.GetValues(typeof(KartDegeri)))
            {
                foreach (KartRengi renk in Enum.GetValues(typeof(KartRengi)))
                {
                    deste.Add(new Kart(deger, renk));
                }
            }

            // Destedeki her kart için
            for (int i = 0; i < deste.Count; i++)
            {
                // Rastgele bir kart seçme indeksi
                int rastgeleIndex = rnd.Next(deste.Count);

                // Seçilen kartı en sona taşıma
                Kart tempKart = deste[i];
                deste[i] = deste[rastgeleIndex];
                deste[rastgeleIndex] = tempKart;
            }

            return deste;
        }

        public static Kart RastgeleCek(List<Kart> deste)
        {
            // Rastgele kart seçme
            Random rnd = new Random();
            int kartIndex = rnd.Next(deste.Count);

            // Kartı destenden çıkarma
            Kart kart = deste[kartIndex];
            deste.RemoveAt(kartIndex);

            return kart;
        }
    }
}

