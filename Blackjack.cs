using System;
namespace Blackjack
{
    public delegate void OyunBasladiEventHandler(object sender, EventArgs e);
    public delegate void OyuncuKartCekildiEventHandler(object sender, EventArgs e);
    public delegate void OyuncuPuanHesaplandiEventHandler(object sender, EventArgs e);
    public delegate void KurpiyerKartCekildiEventHandler(object sender, EventArgs e);
    public delegate void KurpiyerPuanHesaplandiEventHandler(object sender, EventArgs e);
    public delegate void OyunBittiEventHandler(object sender, OyunBittiEventArgs e);

    public class OyunBittiEventArgs : EventArgs
    {
        public KazanmaDurumu KazanmaDurumu { get; set; }
        public int OyuncuPuan { get; set; }
        public int KurpiyerPuan { get; set; }
    }

    public enum KazanmaDurumu
    {
        OyuncuKazandi,
        KurpiyerKazandi,
        Beraberlik
    }

    public class Blackjack
    {
        public event OyunBasladiEventHandler OyunBasladi;
        public event OyuncuKartCekildiEventHandler OyuncuKartCekildi;
        public event OyuncuPuanHesaplandiEventHandler OyuncuPuanHesaplandi;
        public event KurpiyerKartCekildiEventHandler KurpiyerKartCekildi;
        public event KurpiyerPuanHesaplandiEventHandler KurpiyerPuanHesaplandi;
        public event OyunBittiEventHandler OyunBitti;

        public void Oyna(List<Kart> deste, Oyuncu oyuncu, Kurpiyer kurpiyer)
        {
            // Oyun başlangıcında event tetiklenir.
            OyunBasladi?.Invoke(this, EventArgs.Empty);


            // İlk iki kart dağıtılır.
            oyuncu.KartCek(deste);
            oyuncu.KartCek(deste);
            kurpiyer.KartCek(deste, true);
            kurpiyer.KartCek(deste);

            // Oyuncu kart çekmeye devam edebilir.
            while (oyuncu.Puan < 21 && oyuncu.IstekDevam)
            {
                oyuncu.KartCek(deste);
                oyuncu.PuanHesapla();
            }

            // Kurpiyer kart çekmeye başlar.
            if (oyuncu.Puan <= 21)
            {
                while (kurpiyer.Puan < 17 || (kurpiyer.Puan < oyuncu.Puan && kurpiyer.Puan <= 21))
                {
                    kurpiyer.KartCek(deste);
                    kurpiyer.PuanHesapla();
                }
            }

            // Oyunun kazananı belirlenir ve event tetiklenir.
            KazanmaDurumu kazanmaDurumu = KazanmaDurumunuBelirle(oyuncu.Puan, kurpiyer.Puan);
            OyunBitti?.Invoke(this, new OyunBittiEventArgs
            {
                KazanmaDurumu = kazanmaDurumu,
                OyuncuPuan = oyuncu.Puan,
                KurpiyerPuan = kurpiyer.Puan
            });
        }

        public KazanmaDurumu KazanmaDurumunuBelirle(int oyuncuPuan, int kurpiyerPuan)
        {
            if (oyuncuPuan > 21)
            {
                return KazanmaDurumu.KurpiyerKazandi;
            }
            else if (kurpiyerPuan > 21)
            {
                return KazanmaDurumu.OyuncuKazandi;
            }
            else if (oyuncuPuan > kurpiyerPuan)
            {
                return KazanmaDurumu.OyuncuKazandi;
            }
            else if (kurpiyerPuan > oyuncuPuan)
            {
                return KazanmaDurumu.KurpiyerKazandi;
            }
            else
            {
                return KazanmaDurumu.Beraberlik;
            }
        }
    }

}

