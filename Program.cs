using Blackjack;

// Oyun ayarları
int baslangicBahis = 100;
int minimumBahis = 10;

// Deste oluşturulur.
List<Kart> deste = Kart.DesteOlustur();

// Oyuncu ve Kurpiyer nesneleri oluşturulur.
Oyuncu oyuncu = new Oyuncu(baslangicBahis);
Kurpiyer kurpiyer = new Kurpiyer();

// Oyun başlangıcında bahis alınır.
oyuncu.BahisKoy(baslangicBahis);

// Oyun döngüsü
bool oyunDevamEdiyor = true;
while (oyunDevamEdiyor)
{
    // Oyun oynanır.
    Blackjack.Blackjack blackjack = new Blackjack.Blackjack();
    blackjack.OyunBasladi += Blackjack_OyunBasladi;
    blackjack.OyunBitti += Blackjack_OyunBitti;
    oyuncu.KartCekildi += Oyuncu_KartCekildi;
    oyuncu.PuanHesaplandi += Oyuncu_PuanHesaplandi;
    kurpiyer.KartCekildi += Kurpiyer_KartCekildi;
    kurpiyer.PuanHesaplandi += Kurpiyer_PuanHesaplandi;
    blackjack.Oyna(deste, oyuncu, kurpiyer);

    // Oyunun sonucu belirlenir ve ekrana yazdırılır.
    KazanmaDurumu kazanmaDurumu = blackjack.KazanmaDurumunuBelirle(oyuncu.Puan, kurpiyer.Puan);
    oyuncu.KazanmaDurumunaGoreIslemYap(kazanmaDurumu);

    // Oyuncu tekrar oynamak ister mi?
    oyunDevamEdiyor = OyuncuTekrarOynamakIsterMi(oyuncu, minimumBahis);

    // Yeni oyun için deste ve bahis sıfırlanır.
    if (oyunDevamEdiyor)
    {
        deste = Kart.DesteOlustur();
        oyuncu.BahisKoy(baslangicBahis);
    }
}

void Kurpiyer_PuanHesaplandi(object sender, EventArgs e)
{
    Console.WriteLine($"Kurpiyer puanı yeniden hesaplandı. Puan: {kurpiyer.Puan}");
}

void Kurpiyer_KartCekildi(object sender, Kurpiyer.KartCekildiEventArgs e)
{
    Console.WriteLine($"Kurpiyer kart çekti. Çekilen Kart: {e.CekilenKart}");
}

void Oyuncu_PuanHesaplandi(object sender, EventArgs e)
{
    Console.WriteLine($"Oyuncu puanı yeniden hesaplandı. Puan: {oyuncu.Puan}");
}

void Oyuncu_KartCekildi(object sender, KartCekildiEventArgs e)
{
    Console.WriteLine($"oyuncu kart çekti. Çekilen Kart: {e.CekilenKart}");
}

void Blackjack_OyunBitti(object sender, OyunBittiEventArgs e)
{
    Console.WriteLine($"Oyun bitti. kazanan:{e.KazanmaDurumu}");
}

void Blackjack_OyunBasladi(object sender, EventArgs e)
{
    Console.WriteLine("Oyun başladı.");
}

Console.WriteLine("Tekrar oynamak için bir tuşa basın...");
Console.ReadKey();

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
