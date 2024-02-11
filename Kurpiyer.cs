using System;
using static Blackjack.Blackjack;

namespace Blackjack
{
    public class Kurpiyer
    {
        public List<Kart> Kartlari { get; set; }
        public int Puan { get; set; }

        public Kurpiyer()
        {
            Kartlari = new List<Kart>();
        }

        public void KartCek(List<Kart> deste)
        {
            Kart kart = Kart.RastgeleCek(deste);
            Kartlari.Add(kart);
            Puan = PuanHesapla();
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
    }
}

