using System;
using System.Collections.Generic;

namespace GameCore
{
    public enum KartDegeri
    {
        As,
        Ikı = 2,
        Üç = 3,
        Dört = 4,
        Beş = 5,
        Altı = 6,
        Yedi = 7,
        Sekiz = 8,
        Dokuz = 9,
        On = 10,
        Vale,
        Kralice,
        Papaz
    }

    public enum Renk
    {
        Sinek,
        Karo,
        Maça,
        Kupa
    }

    public class Kart
    {
        public KartDegeri Deger { get; set; }
        public Renk Renk { get; set; }

        public Kart(KartDegeri deger, Renk renk)
        {
            Deger = deger;
            Renk = renk;
        }

        public static List<Kart> DesteOlustur()
        {
            List<Kart> deste = new List<Kart>();

            foreach (KartDegeri deger in Enum.GetValues(typeof(KartDegeri)))
            {
                foreach (Renk renk in Enum.GetValues(typeof(Renk)))
                {
                    deste.Add(new Kart(deger, renk));
                }
            }

            return deste;
        }

        public static Kart RastgeleCek(List<Kart> deste)
        {
            if (deste.Count == 0)
            {
                deste = DesteOlustur();
            }

            Random random = new Random();
            int index = random.Next(deste.Count);

            Kart kart = deste[index];
            deste.RemoveAt(index);

            return kart;
        }

        public override string ToString()
        {
            string degerStr = "";
            switch (Deger)
            {
                case KartDegeri.As:
                    degerStr = "As";
                    break;
                case KartDegeri.Ikı:
                    degerStr = "2";
                    break;
                case KartDegeri.Üç:
                    degerStr = "3";
                    break;
                case KartDegeri.Dört:
                    degerStr = "4";
                    break;
                case KartDegeri.Beş:
                    degerStr = "5";
                    break;
                case KartDegeri.Altı:
                    degerStr = "6";
                    break;
                case KartDegeri.Yedi:
                    degerStr = "7";
                    break;
                case KartDegeri.Sekiz:
                    degerStr = "8";
                    break;
                case KartDegeri.Dokuz:
                    degerStr = "9";
                    break;
                case KartDegeri.On:
                    degerStr = "10";
                    break;
                case KartDegeri.Vale:
                    degerStr = "Vale";
                    break;
                case KartDegeri.Kralice:
                    degerStr = "Kralice";
                    break;
                case KartDegeri.Papaz:
                    degerStr = "Papaz";
                    break;
            }

            string renkStr = "";
            switch (Renk)
            {
                case Renk.Sinek:
                    renkStr = "Sinek";
                    break;
                case Renk.Karo:
                    renkStr = "Karo";
                    break;
                case Renk.Maça:
                    renkStr = "Maça";
                    break;
                case Renk.Kupa:
                    renkStr = "Kupa";
                    break;
            }

            return degerStr + " " + renkStr;
        }
    }
}
