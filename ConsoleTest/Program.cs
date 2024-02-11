using GameCore;

Oyuncu oyuncu;
Kurpiyer kurpiyer;

// Oyun döngüsü
bool oyunDevamEdiyor = true;
while (oyunDevamEdiyor)
{
    Console.WriteLine($"Lütfen bahsinizi girin.");
    // Oyun ayarları
    int baslangicBahis = int.Parse(Console.ReadLine());

    // Deste oluşturulur.
    List<Kart> deste = Kart.DesteOlustur();

    // Oyuncu ve Kurpiyer nesneleri oluşturulur.
    oyuncu = new Oyuncu(baslangicBahis);
    kurpiyer = new Kurpiyer();

    // Oyun başlangıcında bahis alınır.
    oyuncu.BahisKoy(baslangicBahis);

    // Oyun oynanır.
    Blackjack blackjack = new Blackjack();
    blackjack.OyunBasladi += Blackjack_OyunBasladi;
    blackjack.OyunBitti += Blackjack_OyunBitti;
    oyuncu.KartCekildi += Oyuncu_KartCekildi;
    oyuncu.PuanHesaplandi += Oyuncu_PuanHesaplandi;
    kurpiyer.KartCekildi += Kurpiyer_KartCekildi;
    kurpiyer.PuanHesaplandi += Kurpiyer_PuanHesaplandi;
    blackjack.Oyna(deste, oyuncu, kurpiyer);

    while (oyuncu.Puan < 21)
    {
        Console.WriteLine("Kart çekmek ister misiniz? (E/H)");
        string cevap = Console.ReadLine().ToUpper();

        if (cevap == "E")
        {
            oyuncu.KartCek(deste);
        }
        else
        {
            break;
        }
    }

    if (oyuncu.Puan > 21)
    {
        blackjack.OyunuBitir(oyuncu, kurpiyer);
        return;
    }

    while (kurpiyer.Puan < 17)
    {
        kurpiyer.KartCek(deste);
    }

    blackjack.OyunuBitir(oyuncu, kurpiyer);

    Console.WriteLine("Tekrar oynamak ister misiniz? (E/H)");
    if (Console.ReadLine().ToLower() == "h")
    {
        oyunDevamEdiyor = false;
    }
}

void Kurpiyer_PuanHesaplandi(object sender, EventArgs e)
{
    if (kurpiyer.Kartlar.Count == 2 || kurpiyer.Kartlar.Count < 1)
    {
        return;
    }

    Console.WriteLine($"Kurpiyer puanı yeniden hesaplandı. Puan: {kurpiyer.Puan}");
}

void Kurpiyer_KartCekildi(object sender, Kurpiyer.KartCekildiEventArgs e)
{
    if (kurpiyer.Kartlar.Count == 2)
    {
        Console.WriteLine($"Kurpiyer kapalı kart çekti.");
        return;
    }

    Console.WriteLine($"Kurpiyer kart çekti. Çekilen Kart: {e.CekilenKart}");
}

void Oyuncu_PuanHesaplandi(object sender, EventArgs e)
{
    Console.WriteLine($"Oyuncu puanı yeniden hesaplandı. Puan: {oyuncu.Puan}");
}

void Oyuncu_KartCekildi(object sender, KartCekildiEventArgs e)
{
    if (oyuncu.Kartlar.Count < 1)
    {
        return;
    }

    Console.WriteLine($"Oyuncuya kart çekildi! Kart: {e.CekilenKart}");
    Console.WriteLine("Oyuncu puanı: " + oyuncu.Puan);
}

void Blackjack_OyunBitti(object sender, OyunBittiEventArgs e)
{
    Console.WriteLine($"Oyun bitti.");
    Console.WriteLine($"Oyuncu Puan: {oyuncu.Puan}");

    Console.WriteLine($"Oyuncu Kartları: ");
    foreach (Kart kart in kurpiyer.Kartlar)
    {
        Console.WriteLine($"{kart}");

    }
    Console.WriteLine($"Kurpiyer Puan: {kurpiyer.Puan}");

    Console.WriteLine($"Kurpiyer Kartları: ");
    foreach (Kart kart in oyuncu.Kartlar)
    {
        Console.WriteLine($"{kart}");
    }

    switch (e.KazanmaDurumu)
    {
        case KazanmaDurumu.OyuncuKazandi:
            Console.ForegroundColor = ConsoleColor.Green;
            break;
        case KazanmaDurumu.KurpiyerKazandi:
            Console.ForegroundColor = ConsoleColor.Red;
            break;
        case KazanmaDurumu.Beraberlik:
            Console.ForegroundColor = ConsoleColor.Blue;
            break;
    }

    Console.WriteLine($"Kazanan:{e.KazanmaDurumu}");
    Console.ForegroundColor = ConsoleColor.White;
}

void Blackjack_OyunBasladi(object sender, EventArgs e)
{
    Console.WriteLine("Oyun başladı.");
}

bool OyuncuTekrarOynamakIsterMi(Oyuncu oyuncu, int minimumBahis)
{
    bool tekrarOynamakIstiyor = false;

    while (true)
    {
        Console.WriteLine("Tekrar oynamak ister misiniz? (E/H)");
        string cevap = Console.ReadLine().ToUpper();

        if (cevap == "E")
        {
            if (oyuncu.Bahis >= minimumBahis)
            {
                tekrarOynamakIstiyor = true;
                break;
            }
            else
            {
                Console.WriteLine("Yeterli bakiyeniz yok. Minimum bahis: " + minimumBahis);
            }
        }
        else if (cevap == "H")
        {
            break;
        }
        else
        {
            Console.WriteLine("Geçersiz cevap. Lütfen E veya H giriniz.");
        }
    }

    return tekrarOynamakIstiyor;
}
