
class Program
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

    public class Kart
    {
        public KartDegeri Deger { get; set; }
        public KartRengi Renk { get; set; }

        public Kart(KartDegeri deger, KartRengi renk)
        {
            Deger = deger;
            Renk = renk;
        }

        public override string ToString()
        {
            return $"{Deger} {Renk}";
        }

        public static List<Kart> DesteyiKar(List<Kart> deste)
        {
            // Kartları rastgele sıralayacak bir `Random` nesnesi oluşturma
            Random rnd = new Random();
            deste = new List<Kart>();

            foreach (KartDegeri deger in Enum.GetValues(typeof(KartDegeri)))
            {
                foreach (KartRengi renk in Enum.GetValues(typeof(KartRengi)))
                {
                    deste.Add(new Kart(deger, renk));
                }
            }

            // Destedeki her kart için
            for (int i = 0; i < deste.Count; i++)
            {
                // Rastgele bir kart seçme indeksi
                int rastgeleIndex = rnd.Next(deste.Count);

                // Seçilen kartı en sona taşıma
                Kart tempKart = deste[i];
                deste[i] = deste[rastgeleIndex];
                deste[rastgeleIndex] = tempKart;
            }

            return deste;
        }

        public static Kart RastgeleCek(List<Kart> deste)
        {
            // Rastgele kart seçme
            Random rnd = new Random();
            int kartIndex = rnd.Next(deste.Count);

            // Kartı destenden çıkarma
            Kart kart = deste[kartIndex];
            deste.RemoveAt(kartIndex);

            return kart;
        }

    }

    public class Oyuncu
    {
        public List<Kart> Kartlari { get; set; }
        public int Puan { get; set; }

        public Oyuncu()
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



    static void Main(string[] args)
    {

        // Oyun değişkenleri
        bool oyuncuKazandi = false;
        bool blackJack = false;
        bool kurpiyerKazandi = false;
        bool oyunDevamEdiyor = true;
        bool sigortaYapildi = false;
        int sigortaBahis = 0;


        int bahis = -1;
        int kalanPara = 0;

        // Kart destesi oluşturma
        List<Kart> deste = new List<Kart>();

        Oyuncu oyuncu = new Oyuncu();
        Kurpiyer kurpiyer = new Kurpiyer();

        // Oyun başlangıcı
        while (oyunDevamEdiyor)
        {
            Console.WriteLine($"****************************************");
            Console.WriteLine($"Oyun başlıyor.");
            bahis = bahis < 0 ? BahisAl() : bahis;

            // deste karma
            deste = Kart.DesteyiKar(deste);

            oyuncu = new Oyuncu();
            kurpiyer = new Kurpiyer();

            // İlk iki kart dağıtma
            oyuncu.KartCek(deste);
            oyuncu.KartCek(deste);

            for (int i = 0; i < oyuncu.Kartlari.Count; i++)
            {
                Console.WriteLine($"Oyuncu Dağıtılan Kart: {oyuncu.Kartlari[i].Renk} {oyuncu.Kartlari[i].Deger}");
            }
            Console.WriteLine($"Oyuncu Puan: {oyuncu.Puan}");

            kurpiyer.KartCek(deste);
            kurpiyer.KartCek(deste);

            for (int i = 0; i < kurpiyer.Kartlari.Count; i++)
            {
                Console.WriteLine($"Kurpiyer Dağıtılan Kart: {kurpiyer.Kartlari[i].Renk} {kurpiyer.Kartlari[i].Deger}");
            }
            Console.WriteLine($"Kurpiyer Puan: {kurpiyer.Puan}");

            kalanPara = kalanPara + bahis;

            // Oyuncu 21'i geçtiyse
            if (oyuncu.PuanHesapla() > 21)
            {
                kurpiyerKazandi = true;
                Console.WriteLine($"Oyuncu kartları 21i geçti. Oyuncu kaybetti.");

                kalanPara = kalanPara - bahis;
                continue;
            }

            // 21 YAPTI!
            if (oyuncu.PuanHesapla() == 21)
            {
                oyuncuKazandi = true;
                Console.WriteLine($"Oyuncu BLACKJACK yaptı. Oyuncu kazandı.");
                kalanPara = kalanPara + (bahis * 2);
                blackJack = true;
                continue;
            }

            // Sigorta teklifi
            if (oyuncu.PuanHesapla() == 11 && !sigortaYapildi)
            {
                sigortaYapildi = SigortaTeklifEt(bahis);
                if (sigortaYapildi)
                {
                    sigortaBahis = bahis / 2;
                }
            }

            Console.WriteLine($"Kurpiyer 17'ye ulaşana kadar kart çekmeye devam eder.");
            Console.WriteLine($"Kurpiyer mevcut skoru {kurpiyer.PuanHesapla()}.");
            // Kurpiyer 17'ye ulaşana kadar kart çekmeye devam eder
            while (kurpiyer.PuanHesapla() < 17)
            {
                Console.WriteLine($"kurpiyerPuan < 17.");
                kurpiyer.KartCek(deste);
                Console.WriteLine($"Kurpiyer mevcut skoru {kurpiyer.PuanHesapla()}.");
            }

            for (int i = 0; i < kurpiyer.Kartlari.Count; i++)
            {
                Console.WriteLine($"Kurpiyer Dağıtılan Kart: {kurpiyer.Kartlari[i].Renk} {kurpiyer.Kartlari[i].Deger}");
            }

            // Kazanma durumunu kontrol etme
            if (kurpiyer.PuanHesapla() > 21)
            {
                Console.WriteLine($"kurpiyerPuan > 21. Kurpiyer kaybetti. Kurpiyer puanı: {kurpiyer.PuanHesapla()}. Oyuncu Puanı: {oyuncu.PuanHesapla()}");
                oyuncuKazandi = true;
                kalanPara = kalanPara + (bahis * 2);
                continue;
            }

            Console.WriteLine($"kurpiyerPuan < 17.");
            Console.WriteLine($"Kurpiyer Puanı: {kurpiyer.PuanHesapla()}");
            Console.WriteLine($"Oyuncu Puanı: {oyuncu.PuanHesapla()}");
            Console.WriteLine("kart çekmek ister misiniz? (E/H)");

            if (Console.ReadLine().ToString().ToLower() == "e")
            {
                oyuncu.KartCek(deste);
            }

            if (oyuncu.PuanHesapla() > kurpiyer.PuanHesapla())
            {
                oyuncuKazandi = true;
                Console.WriteLine($"OYUNCU KAZANDI. kurpiyerPuan {kurpiyer.Puan}. Oyuncu Puan: {oyuncu.Puan}");
                kalanPara = kalanPara + bahis;
                continue;
            }

            if (oyuncu.PuanHesapla() < kurpiyer.PuanHesapla())
            {
                kurpiyerKazandi = true;
                Console.WriteLine($"KUPIYER KAZANDI. kurpiyerPuan {kurpiyer.Puan}. Oyuncu Puan: {oyuncu.Puan}");
                kalanPara = kalanPara - bahis;
                continue;
            }

            // Oyunu bitirme
            oyunDevamEdiyor = false;

            // Oyun sonucunu gösterme
            Console.WriteLine("Oyuncu Puanı: {0}", oyuncu.PuanHesapla());
            Console.WriteLine("Kurpiyer Puanı: {0}", kurpiyer.PuanHesapla());

            if (oyuncuKazandi)
            {

            }
            else if (kurpiyerKazandi)
            {
                if (sigortaYapildi)
                {
                    Console.WriteLine("Kaybettiniz, ancak sigortadan {0} kazandınız.", sigortaBahis * 2);
                    kalanPara = (kalanPara - bahis) + sigortaBahis * 2;
                }
                else
                {
                    Console.WriteLine("Kaybettiniz.");
                }
            }
            else
            {
                Console.WriteLine("Beraberlik! Bahsiniz iade edildi.");
            }

            // Tekrar oynama seçeneği
            Console.WriteLine("Tekrar oynamak ister misiniz? (E/H)");
            string cevap = Console.ReadLine();
            if (cevap.ToLower() == "e")
            {
                Main(args);
            }
        }
    }

    static bool SigortaTeklifEt(int bahis)
    {
        Console.WriteLine("Sigorta yaptırmak ister misiniz? (E/H)");
        string cevap = Console.ReadLine();

        if (cevap.ToLower() == "e")
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    static int BahisAl()
    {
        Console.WriteLine("Bahis miktarını giriniz: ");
        return int.Parse(Console.ReadLine());

    }
}
