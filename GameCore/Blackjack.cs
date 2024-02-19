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

    public class BitenOyun
    {
        public List<Kart> Deste { get; set; }
        public Oyuncu Oyuncu { get; set; }
        public Kurpiyer Kurpiyer { get; set; }
        public KazanmaDurumu Sonuc { get; set; }
    }

    public class Blackjack
    {
        public event OyunBasladiEventHandler OyunBasladi;
        public event OyunBittiEventHandler OyunBitti;
        public List<BitenOyun> BitenOyunlar = new List<BitenOyun>();
        List<Kart> _deste;
        Oyuncu _oyuncu;
        Kurpiyer _kurpiyer;

        public void Oyna(List<Kart> deste, Oyuncu oyuncu, Kurpiyer kurpiyer)
        {
            _deste = deste;
            _oyuncu = oyuncu;
            _kurpiyer = kurpiyer;

            // İlk iki kart dağıtılır.
            _oyuncu.KartCek(_deste);
            _oyuncu.KartCek(_deste);
            _kurpiyer.KartCek(_deste);
            _kurpiyer.KartCek(_deste);

            // Oyun başlangıcında event tetiklenir.
            OyunBasladi?.Invoke(this, EventArgs.Empty);

            // oyuncu blackjack yaptı
            if (_oyuncu.Kartlar.Count == 2 && oyuncu.Puan == 21)
            {
                OyunuBitir();
            }
        }

        public void OyunuBitir()
        {
            // Oyunun kazananı belirlenir ve event tetiklenir.
            KazanmaDurumu kazanmaDurumu = KazanmaDurumunuBelirle();
            OyunBitti?.Invoke(this, new OyunBittiEventArgs
            {
                KazanmaDurumu = kazanmaDurumu,
                OyuncuPuan = _oyuncu.Puan,
                KurpiyerPuan = _kurpiyer.Puan
            });

            BitenOyunlar.Add(new BitenOyun {Deste = _deste, Kurpiyer = _kurpiyer, Oyuncu = _oyuncu, Sonuc = kazanmaDurumu });
        }

        private KazanmaDurumu KazanmaDurumunuBelirle()
        {
            if (_oyuncu.Kartlar.Count == 2 && _oyuncu.Puan == 21)
            {
                _oyuncu.ToplamKasa = _oyuncu.ToplamKasa + (_oyuncu.Bahis * 2);
                return KazanmaDurumu.OyuncuBlackjackYapti;
            }
            else if (_oyuncu.Puan > 21)
            {
                _oyuncu.ToplamKasa = _oyuncu.ToplamKasa - _oyuncu.Bahis;
                return KazanmaDurumu.KurpiyerKazandi;
            }
            else if (_kurpiyer.Puan > 21)
            {
                _oyuncu.ToplamKasa = _oyuncu.ToplamKasa + _oyuncu.Bahis;
                return KazanmaDurumu.OyuncuKazandi;
            }
            else if (_oyuncu.Puan > _kurpiyer.Puan)
            {
                _oyuncu.ToplamKasa = _oyuncu.ToplamKasa + _oyuncu.Bahis;
                return KazanmaDurumu.OyuncuKazandi;
            }
            else if (_kurpiyer.Puan > _oyuncu.Puan)
            {
                _oyuncu.ToplamKasa = _oyuncu.ToplamKasa - _oyuncu.Bahis;
                return KazanmaDurumu.KurpiyerKazandi;
            }
            else
            {
                return KazanmaDurumu.Beraberlik;
            }
        }
    }

}

