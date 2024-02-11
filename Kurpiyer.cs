using Blackjack;

public class KurpiyerKartCekildiHandler
{
    public void OnKurpiyerKartCekildi(object sender, EventArgs e)
    {
        // Kurpiyer kart çekme işlemini gerçekleştirir.
    }
}

public class KurpiyerPuanHesaplandiHandler
{
    public void OnKurpiyerPuanHesaplandi(object sender, EventArgs e)
    {
        // Kurpiyerin puanını hesaplar.
    }
}

public delegate void KurpiyerKartCekildiEventHandler(object sender, EventArgs e);
public delegate void KurpiyerPuanHesaplandiEventHandler(object sender, EventArgs e);

public class Kurpiyer
{
    public event KurpiyerKartCekildiEventHandler KurpiyerKartCekildi;
    public event KurpiyerPuanHesaplandiEventHandler KurpiyerPuanHesaplandi;

    private List<Kart> _kartlar;
    private int _puan;

    public List<Kart> Kartlari => _kartlar;

    public int Puan => _puan;

    public Kurpiyer()
    {
        _kartlar = new List<Kart>();
    }

    public void KartCek(List<Kart> deste)
    {
        Kart kart = Kart.RastgeleCek(deste);
        _kartlar.Add(kart);

        KurpiyerKartCekildi?.Invoke(this, EventArgs.Empty);

        _puan = PuanHesapla();
        KurpiyerPuanHesaplandi?.Invoke(this, EventArgs.Empty);
    }

    public int PuanHesapla()
    {
        int puan = 0;
        bool asVar = false;

        foreach (Kart kart in _kartlar)
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
