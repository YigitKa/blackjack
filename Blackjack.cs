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

    public class Blackjack
	{
		public Blackjack()
		{
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

        public class Oyuncu
        {
            public List<Kart> Kartlari { get; set; }
            public int Puan { get; set; }

            public Oyuncu()
            {
                Kartlari = new List<Kart>();
            }

            public void KartCek(List<Kart> deste)
            {
                Kart kart = Kart.RastgeleCek(deste);
                Kartlari.Add(kart);
                Puan = PuanHesapla();
            }

            public int PuanHesapla()
            {
                int puan = 0;
                bool asVar = false;

                foreach (Kart kart in Kartlari)
                {
                    if (kart.Deger == KartDegeri.As)
                    {
                        asVar = true;
                        puan += 1;
                    }
                    else if ((int)kart.Deger <= 10)
                    {
                        puan += (int)kart.Deger;
                    }
                    else
                    {
                        puan += 10;
                    }
                }

                if (asVar && puan <= 11)
                {
                    puan += 10;
                }

                return puan;
            }
        }

        public class Kurpiyer
        {
            public List<Kart> Kartlari { get; set; }
            public int Puan { get; set; }

            public Kurpiyer()
            {
                Kartlari = new List<Kart>();
            }

            public void KartCek(List<Kart> deste)
            {
                Kart kart = Kart.RastgeleCek(deste);
                Kartlari.Add(kart);
                Puan = PuanHesapla();
            }

            public int PuanHesapla()
            {
                int puan = 0;
                bool asVar = false;

                foreach (Kart kart in Kartlari)
                {
                    if (kart.Deger == KartDegeri.As)
                    {
                        asVar = true;
                        puan += 1;
                    }
                    else if ((int)kart.Deger <= 10)
                    {
                        puan += (int)kart.Deger;
                    }
                    else
                    {
                        puan += 10;
                    }
                }

                if (asVar && puan <= 11)
                {
                    puan += 10;
                }

                return puan;
            }
        }

    }
}

