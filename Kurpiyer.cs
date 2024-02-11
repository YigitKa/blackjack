using Blackjack;

public class Kurpiyer
{
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

            KartCekildi?.Invoke(this, EventArgs.Empty);
        }

        PuanHesapla();

        while (Puan < 17 && !ilkKart)
        {
            Kart kart = Kart.RastgeleCek(deste);
            Kartlar.Add(kart);

            KartCekildi?.Invoke(this, EventArgs.Empty);

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

