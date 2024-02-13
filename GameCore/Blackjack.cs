using System;
namespace GameCore
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
        Beraberlik,
        OyuncuBlackjackYapti
    }

    public class Blackjack
    {
        public event OyunBasladiEventHandler OyunBasladi;
        public event OyunBittiEventHandler OyunBitti;
        public bool oyunuBitir = false;

        public void Oyna(List<Kart> deste, Oyuncu oyuncu, Kurpiyer kurpiyer)
        {
            // İlk iki kart dağıtılır.
            oyuncu.KartCek(deste);
            oyuncu.KartCek(deste);
            kurpiyer.KartCek(deste, true);
            kurpiyer.KartCek(deste);

            // Oyun başlangıcında event tetiklenir.
            OyunBasladi?.Invoke(this, EventArgs.Empty);

            // oyuncu blackjack yaptı
            if (oyuncu.Kartlar.Count == 2 && oyuncu.Puan == 21)
            {
                OyunuBitir(oyuncu, kurpiyer);
            }
        }

        public void OyunuBitir(Oyuncu oyuncu, Kurpiyer kurpiyer)
        {
            // Oyunun kazananı belirlenir ve event tetiklenir.
            KazanmaDurumu kazanmaDurumu = KazanmaDurumunuBelirle(oyuncu, kurpiyer);
            OyunBitti?.Invoke(this, new OyunBittiEventArgs
            {
                KazanmaDurumu = kazanmaDurumu,
                OyuncuPuan = oyuncu.Puan,
                KurpiyerPuan = kurpiyer.Puan
            });
        }

        private KazanmaDurumu KazanmaDurumunuBelirle(Oyuncu oyuncu, Kurpiyer kurpiyer)
        {
            if (oyuncu.Kartlar.Count == 2 && oyuncu.Puan == 21)
            {
                return KazanmaDurumu.OyuncuBlackjackYapti;
            }
            else if (oyuncu.Puan > 21)
            {
                return KazanmaDurumu.KurpiyerKazandi;
            }
            else if (kurpiyer.Puan > 21)
            {
                return KazanmaDurumu.OyuncuKazandi;
            }
            else if (oyuncu.Puan > kurpiyer.Puan)
            {
                return KazanmaDurumu.OyuncuKazandi;
            }
            else if (kurpiyer.Puan > oyuncu.Puan)
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

