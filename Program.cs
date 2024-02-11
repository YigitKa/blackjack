﻿using Blackjack;

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
    Blackjack.Blackjack blackjack = new Blackjack.Blackjack();
    blackjack.OyunBasladi += Blackjack_OyunBasladi;
    blackjack.OyunBitti += Blackjack_OyunBitti;
    oyuncu.KartCekildi += Oyuncu_KartCekildi;
    oyuncu.PuanHesaplandi += Oyuncu_PuanHesaplandi;
    kurpiyer.KartCekildi += Kurpiyer_KartCekildi;
    kurpiyer.PuanHesaplandi += Kurpiyer_PuanHesaplandi;
    blackjack.Oyna(deste, oyuncu, kurpiyer);

    if (kurpiyer.Puan > 21)
    {
        blackjack.OyunuBitir(oyuncu, kurpiyer);
    }

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

    while (kurpiyer.Puan < 17 || (kurpiyer.Puan < oyuncu.Puan && kurpiyer.Puan <= 21))
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
    if (oyuncu.Kartlar.Count < 1)
    {
        return;
    }

    Console.WriteLine($"Oyuncuya kart çekildi! Kart: {e.CekilenKart}");
    Console.WriteLine("Oyuncu puanı: " + oyuncu.Puan);
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
