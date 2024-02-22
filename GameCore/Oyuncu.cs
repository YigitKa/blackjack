
namespace GameCore
{
    public class KartCekildiEventArgs : EventArgs
    {
        public Kart CekilenKart { get; set; }
        public bool SplitKartMi { get; set; } = false;
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
        public bool SplitYapabilir { get; set; }
        public bool SplitYapiyor { get; set; }
        public List<Kart> Kartlar { get; set; }
        public List<Kart> SplitKartlar { get; set; }
        public int Puan { get; private set; }
        public int SplitPuan { get; private set; }

        public event KartCekildiEventHandler KartCekildi;
        public event PuanHesaplandiEventHandler PuanHesaplandi;
        public event SplitYapildiEventHandler SplitYapildi;

        public Oyuncu(int baslangicBahis)
        {
            Kartlar = new List<Kart>();
            SplitKartlar = new List<Kart>();
            Bahis = baslangicBahis;
        }

        public void SplitYap()
        {
            // oyuncuda 2 adet kart olması ve kartların değerlerinin aynı olması gerekmektedir
            if (SplitYapabilir)
            {
                SplitKartlar = new List<Kart> { Kartlar[1] };
                Kartlar.Remove(Kartlar[1]);
                SplitBahis = Bahis;
                SplitYapiyor = true;
                SplitYapildi?.Invoke(this, new SplitYapildiEventArgs { Kartlar = Kartlar, SplitKartlar = SplitKartlar});
            }
        }

        public void BahisKoy(decimal miktar)
        {
            if (miktar > 0)
            {
                Bahis = miktar;
            }
            SplitYapabilir = false;
            SplitYapiyor = false;
        }

        public void KartCek(List<Kart> deste)
        {
            bool splitKartMi = false;
            Kart kart = Kart.RastgeleCek(deste);
            if (SplitYapiyor)
            {
                if (Kartlar.Count == SplitKartlar.Count)
                {
                    Kartlar.Add(kart);
                }
                SplitKartlar.Add(kart);
                splitKartMi = true;
            }
            else
            {
                Kartlar.Add(kart);
            }

            PuanHesapla();

            if (Kartlar.Count == 2 && Kartlar[0].Deger == Kartlar[1].Deger)
            {
                SplitYapabilir = true;
            }

            KartCekildi?.Invoke(this, new KartCekildiEventArgs { CekilenKart = kart, SplitKartMi = splitKartMi });
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

            puan = 0;
            asVar = false;
            foreach (Kart kart in SplitKartlar)
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

            SplitPuan = puan;
            PuanHesaplandi?.Invoke(this, EventArgs.Empty);
        }
    }

}


