using Blackjack;

public class Kurpiyer
{
    public class KartCekildiEventArgs : EventArgs
    {
        public Kart CekilenKart { get; set; }
    }

    public delegate void PuanHesaplandiEventHandler(object sender, EventArgs e);
    public delegate void KartCekildiEventHandler(object sender, KartCekildiEventArgs e);

    public List<Kart> Kartlar { get; set; }
    public int Puan { get; private set; }

    public event KartCekildiEventHandler KartCekildi;
    public event PuanHesaplandiEventHandler PuanHesaplandi;

    public Kurpiyer()
    {
        Kartlar = new List<Kart>();
    }

    public void KartCek(List<Kart> deste, bool ilkKart = false)
    {
        if (!ilkKart)
        {
            Kart kart = Kart.RastgeleCek(deste);
            Kartlar.Add(kart);

            KartCekildi?.Invoke(this, new KartCekildiEventArgs { CekilenKart = kart });
        }

        PuanHesapla();

        while (Puan < 17 && !ilkKart)
        {
            Kart kart = Kart.RastgeleCek(deste);
            Kartlar.Add(kart);

            KartCekildi?.Invoke(this, new KartCekildiEventArgs { CekilenKart = kart });

            PuanHesapla();
        }
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

