﻿using System;
namespace Blackjack
{
    public enum KartDegeri
    {
        As = 1,
        Ikı = 2,
        Uc = 3,
        Dort = 4,
        Bes = 5,
        Alti = 6,
        Yedi = 7,
        Sekiz = 8,
        Dokuz = 9,
        On = 10,
        Vale = 10,
        Kız = 10,
        Papaz = 10,
    }

    public enum KartRengi
    {
        Sinek,
        Karo,
        Maça,
        Kupa,
    }

    public delegate void KartCekildiEventHandler(object sender, EventArgs e);
    public delegate void PuanHesaplandiEventHandler(object sender, EventArgs e);
    public delegate void BahisKonulduEventHandler(object sender, EventArgs e);
    public delegate void KazandiEventHandler(object sender, EventArgs e);
    public delegate void KaybettiEventHandler(object sender, EventArgs e);

    public class KartCekildiIsleyici
    {
        public void OnKartCekildi(object sender, EventArgs e)
        {
            // Kart çekme işlemini gerçekleştirir.
        }
    }

    public class PuanHesaplandiIsleyici
    {
        public void OnPuanHesaplandi(object sender, EventArgs e)
        {
            // Oyuncunun puanını hesaplar.
        }
    }

    public class BahisKonulduIsleyici
    {
        public void OnBahisKonuldu(object sender, EventArgs e)
        {
            // Oyuncunun bahis koyma işlemini gerçekleştirir.
        }
    }

    public class KazandiIsleyici
    {
        public void OnKazandi(object sender, EventArgs e)
        {
            // Oyunu kazanan oyuncuya ilişkin işlemleri gerçekleştirir.
        }
    }

    public class KaybettiIsleyici
    {
        public void OnKaybetti(object sender, EventArgs e)
        {
            // Oyunu kaybeden oyuncuya ilişkin işlemleri gerçekleştirir.
        }
    }

    public class Blackjack
	{
		public Blackjack()
		{
		}
    }
}

