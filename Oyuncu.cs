using System;
using static Blackjack.Blackjack;

namespace Blackjack
{
    public delegate void PuanHesaplandiEventHandler(object sender, EventArgs e);
    public delegate void KartCekildiEventHandler(object sender, EventArgs e);

    public class Oyuncu
    {
        public int Bahis { get; set; }
        public List<Kart> Kartlar { get; set; }
        public int Puan { get; private set; }

        public event KartCekildiEventHandler KartCekildi;
        public event PuanHesaplandiEventHandler PuanHesaplandi;

        public Oyuncu(int baslangicBahis)
        {
            Kartlar = new List<Kart>();
            Bahis = baslangicBahis;
        }

        public void BahisKoy(int miktar)
        {
            if (miktar > 0)
            {
                Bahis = miktar;
            }
        }

        public void KazanmaDurumunaGoreIslemYap(KazanmaDurumu kazanmaDurumu)
        {
            switch (kazanmaDurumu)
            {
                case KazanmaDurumu.OyuncuKazandi:
                    Bahis *= 2;
                    Console.WriteLine("Kazandınız! Bahsiniz iki katına çıktı: " + Bahis);
                    break;
                case KazanmaDurumu.KurpiyerKazandi:
                    Bahis = 0;
                    Console.WriteLine("Kaybettiniz. Bahsinizi kaybettiniz.");
                    break;
                case KazanmaDurumu.Beraberlik:
                    Console.WriteLine("Beraberlik! Bahsiniz iade edildi.");
                    break;
            }
        }

        public void KartCek(List<Kart> deste)
        {
            Kart kart = Kart.RastgeleCek(deste);
            Kartlar.Add(kart);

            KartCekildi?.Invoke(this, EventArgs.Empty);

            PuanHesapla();

            if (Puan > 21)
            {
                Console.WriteLine("Puanınız 21'i aştı. Kaybettiniz.");
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

}


