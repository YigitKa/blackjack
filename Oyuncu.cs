using System;
using static Blackjack.Blackjack;

namespace Blackjack
{
	public class Oyuncu
	{
            public List<Kart> Kartlari { get; set; }
            public int Puan { get; set; }
            public decimal ToplamPara;
            public decimal BahisMiktari;
            public event KartCekildiEventHandler KartCekildi;
            public event PuanHesaplandiEventHandler PuanHesaplandi;
            public event BahisKonulduEventHandler BahisKonuldu;
            public event KazandiEventHandler Kazandi;
            public event KaybettiEventHandler Kaybetti;

            public void KartCek(List<Kart> deste)
            {
                if (BahisMiktari > ToplamPara)
                {
                    throw new Exception("Yetersiz bakiye!");
                }

                ToplamPara -= BahisMiktari;

                Kart kart = Kart.RastgeleCek(deste);
                Kartlari.Add(kart);

                KartCekildi?.Invoke(this, EventArgs.Empty);

                Puan = PuanHesapla();
                PuanHesaplandi?.Invoke(this, EventArgs.Empty);
            }

            public int PuanHesapla()
            {
                int puan = 0;
                bool asVar = false;

                foreach (Kart kart in Kartlari)
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

            public void BahisKoy(decimal bahisMiktari)
            {
                if (bahisMiktari > ToplamPara)
                {
                    throw new Exception("Yetersiz bakiye!");
                }

                BahisMiktari = bahisMiktari;
                BahisKonuldu?.Invoke(this, EventArgs.Empty);
            }

            public void Kazan(decimal kazanc)
            {
                ToplamPara += BahisMiktari + kazanc;
                Kazandi?.Invoke(this, EventArgs.Empty);
            }

            public void Kaybet()
            {
                // Bahis miktarını kaybeder.
                Kaybetti?.Invoke(this, EventArgs.Empty);
            }
        }
    }


