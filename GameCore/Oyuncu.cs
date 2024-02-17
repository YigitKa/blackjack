
namespace GameCore
{
    public class KartCekildiEventArgs : EventArgs
    {
        public Kart CekilenKart { get; set; }
    }

    public class SplitYapildiEventArgs : EventArgs
    {
        public List<Kart> Kartlar { get; set; }
        public List<Kart> SplitKartlar { get; set; }
    }

    public delegate void PuanHesaplandiEventHandler(object sender, EventArgs e);
    public delegate void KartCekildiEventHandler(object sender, KartCekildiEventArgs e);
    public delegate void SplitYapildiEventHandler(object sender, SplitYapildiEventArgs e);

    public class Oyuncu
    {
        public decimal Bahis { get; set; }
        public decimal ToplamKasa { get; set; }
        public decimal SplitBahis { get; set; }
        public List<Kart> Kartlar { get; set; }
        public List<Kart> SplitKartlar { get; set; }
        public int Puan { get; private set; }

        public event KartCekildiEventHandler KartCekildi;
        public event PuanHesaplandiEventHandler PuanHesaplandi;
        public event SplitYapildiEventHandler SplitYapildi;

        public Oyuncu(int baslangicBahis)
        {
            Kartlar = new List<Kart>();
            Bahis = baslangicBahis;
        }

        public void SplitYap()
        {
            // oyuncuda 2 adet kart olması ve kartların değerlerinin aynı olması gerekmektedir
            if (Kartlar.Count == 2 && Kartlar[0].Deger == Kartlar[1].Deger)
            {
                SplitKartlar = new List<Kart> { Kartlar[1] };
                Kartlar.Remove(Kartlar[1]);
                SplitYapildi?.Invoke(this, new SplitYapildiEventArgs { Kartlar = Kartlar, SplitKartlar = SplitKartlar});
                SplitBahis = Bahis;
            }
        }

        public void BahisKoy(decimal miktar)
        {
            if (miktar > 0)
            {
                Bahis = miktar;
            }
        }

        public void KartCek(List<Kart> deste)
        {
            Kart kart = Kart.RastgeleCek(deste);
            Kartlar.Add(kart);

            PuanHesapla();

            KartCekildi?.Invoke(this, new KartCekildiEventArgs { CekilenKart = kart });
        }

        public void PuanHesapla()
        {
            int puan = 0;
            bool asVar = false;

            foreach (Kart kart in Kartlar)
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

            Puan = puan;
            PuanHesaplandi?.Invoke(this, EventArgs.Empty);
        }
    }

}


