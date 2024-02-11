using Blackjack;

class Program
{
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
                Thread.Sleep(1000);
                Console.WriteLine($"Oyuncu Dağıtılan Kart: {oyuncu.Kartlari[i].Renk} {oyuncu.Kartlari[i].Deger}");
            }

            Thread.Sleep(1000);
            Console.WriteLine($"Oyuncu Puan: {oyuncu.Puan}");

            kurpiyer.KartCek(deste);
            kurpiyer.KartCek(deste);

            for (int i = 0; i < kurpiyer.Kartlari.Count; i++)
            {
                Thread.Sleep(1000);
                Console.WriteLine($"Kurpiyer Dağıtılan Kart: {kurpiyer.Kartlari[i].Renk} {kurpiyer.Kartlari[i].Deger}");
            }

            Thread.Sleep(1000);
            Console.WriteLine($"Kurpiyer Puan: {kurpiyer.Puan}");

            kalanPara = kalanPara + bahis;

            // Oyuncu 21'i geçtiyse
            if (oyuncu.PuanHesapla() > 21)
            {
                kurpiyerKazandi = true;
                Thread.Sleep(1000);
                Console.WriteLine($"Oyuncu kartları 21i geçti. Oyuncu kaybetti.");

                kalanPara = kalanPara - bahis;
                continue;
            }

            // 21 YAPTI!
            if (oyuncu.PuanHesapla() == 21)
            {
                oyuncuKazandi = true;
                Thread.Sleep(1000);
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

            if (kurpiyer.PuanHesapla() < 17)
            {
                Thread.Sleep(1000);
                Console.WriteLine($"Kurpiyer 17'ye ulaşana kadar kart çekmeye devam eder.");
            }

            Thread.Sleep(1000);
            Console.WriteLine($"Kurpiyer mevcut skoru {kurpiyer.PuanHesapla()}.");
            // Kurpiyer 17'ye ulaşana kadar kart çekmeye devam eder
            while (kurpiyer.PuanHesapla() < 17)
            {
                kurpiyer.KartCek(deste);
                Thread.Sleep(1000);
                Console.WriteLine($"Kurpiyer mevcut skoru {kurpiyer.PuanHesapla()}.");
            }

            for (int i = 0; i < kurpiyer.Kartlari.Count; i++)
            {
                Thread.Sleep(1000);
                Console.WriteLine($"Kurpiyer Dağıtılan Kart: {kurpiyer.Kartlari[i].Renk} {kurpiyer.Kartlari[i].Deger}");
            }

            // Kazanma durumunu kontrol etme
            if (kurpiyer.PuanHesapla() > 21)
            {
                Thread.Sleep(1000);
                Console.WriteLine($"kurpiyerPuan > 21. Kurpiyer kaybetti. Kurpiyer puanı: {kurpiyer.PuanHesapla()}. Oyuncu Puanı: {oyuncu.PuanHesapla()}");
                oyuncuKazandi = true;
                kalanPara = kalanPara + (bahis * 2);
                continue;
            }

            Thread.Sleep(1000);
            Console.WriteLine($"Kurpiyer Puanı: {kurpiyer.PuanHesapla()}");
            Thread.Sleep(1000);
            Console.WriteLine($"Oyuncu Puanı: {oyuncu.PuanHesapla()}");
            Thread.Sleep(1000);
            Console.WriteLine("kart çekmek ister misiniz? (E/H)");

            if (Console.ReadLine().ToString().ToLower() == "e")
            {
                oyuncu.KartCek(deste);
            }

            if (oyuncu.PuanHesapla() > kurpiyer.PuanHesapla())
            {
                oyuncuKazandi = true;
                Thread.Sleep(1000);
                Console.WriteLine($"OYUNCU KAZANDI. kurpiyerPuan {kurpiyer.Puan}. Oyuncu Puan: {oyuncu.Puan}");
                kalanPara = kalanPara + bahis;
                continue;
            }

            if (oyuncu.PuanHesapla() < kurpiyer.PuanHesapla())
            {
                kurpiyerKazandi = true;
                Thread.Sleep(1000);
                Console.WriteLine($"KUPIYER KAZANDI. kurpiyerPuan {kurpiyer.Puan}. Oyuncu Puan: {oyuncu.Puan}");
                kalanPara = kalanPara - bahis;
                continue;
            }

            // Oyunu bitirme
            oyunDevamEdiyor = false;

            // Oyun sonucunu gösterme
            Thread.Sleep(1000);
            Console.WriteLine("Oyuncu Puanı: {0}", oyuncu.PuanHesapla());
            Thread.Sleep(1000);
            Console.WriteLine("Kurpiyer Puanı: {0}", kurpiyer.PuanHesapla());

            if (oyuncuKazandi)
            {

            }
            else if (kurpiyerKazandi)
            {
                if (sigortaYapildi)
                {
                    Thread.Sleep(1000);
                    Console.WriteLine("Kaybettiniz, ancak sigortadan {0} kazandınız.", sigortaBahis * 2);
                    kalanPara = (kalanPara - bahis) + sigortaBahis * 2;
                }
                else
                {
                    Thread.Sleep(1000);
                    Console.WriteLine("Kaybettiniz.");
                }
            }
            else
            {
                Thread.Sleep(1000);
                Console.WriteLine("Beraberlik! Bahsiniz iade edildi.");
            }

            // Tekrar oynama seçeneği
            Thread.Sleep(1000);
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
