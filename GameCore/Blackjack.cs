﻿using System;
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
        Beraberlik
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
        }

        public void OyunuBitir(Oyuncu oyuncu, Kurpiyer kurpiyer)
        {
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
