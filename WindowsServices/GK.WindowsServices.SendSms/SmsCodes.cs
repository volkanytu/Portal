using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GK.WindowsServices.SendSms
{
    public class SmsProviderCodes
    {
        public static Dictionary<string, string> SmsErrorCodes
        {
            get
            {
                Dictionary<string, string> returnValue = new Dictionary<string, string>();

                returnValue.Add("ERR1010", "İstek Hatası. Veritabanı bağlantı işlemi yapılamadı");
                returnValue.Add("ERR1020", "İstek Hatası. Veritabanına bağlanamadı.");
                returnValue.Add("ERR1030", "İstek Hatası. İstek tarihçe kaydı tutalamadı.");
                returnValue.Add("ERR1040", "İstek Hatası. Kayıt bulunamadı.");
                returnValue.Add("ERR1050", "İstek Hatası. İstek değerlendirilemedi. Metot Belirlenemedi.");
                returnValue.Add("ERR1060", "Alicilar bölümü boş olamaz.");
                returnValue.Add("ERR1070", "Mesajlar bölümü boş olamaz.");
                returnValue.Add("ERR1080", "Alicilar ve Mesajlar dizileri esit boyutta olmalidir.");
                returnValue.Add("ERR1090", "Maksimum Alıcı Sayısı Aşıldı");
                returnValue.Add("ERR2020", "Durum Sorgulama Veritabanı Hatası");
                returnValue.Add("ERR2030", "Durum Sorgulama Tarihçe Kaydı Hatası");
                returnValue.Add("ERR2040", "Durum Sorgulama Kayıt Bulunamadı Hatası");
                returnValue.Add("ERR2050", "Durum Sorgulama Veritabanı Metot Hatası");
                returnValue.Add("ERR2060", "Mesaj bölümü boş olamaz");
                returnValue.Add("ERR7020", "Kredi Sorgulama Veritabanı Hatası");
                returnValue.Add("ERR7030", "Kredi Sorgulama Tarihçe Kaydı Hatası");
                returnValue.Add("ERR7040", "Kredi Sorgulama Kayıt Bulunamadı Hatası");
                returnValue.Add("ERR7050", "Kredi Sorgulama Veritabanı Metot Hatası");
                returnValue.Add("ERR8020", "Alfanumerik Sorgulama Veritabanı Hatası");
                returnValue.Add("ERR8030", "Alfanumerik Sorgulama Tarihçe Kaydı Hatası");
                returnValue.Add("ERR8040", "Alfanumerik Sorgulama Kayıt Bulunamadı Hatası");
                returnValue.Add("ERR8050", "Alfanumerik Sorgulama Veritabanı Metot Hatası");

                return returnValue;
            }
        }

        public static Dictionary<string, string> SmsStatusCodes
        {
            get
            {
                Dictionary<string, string> returnValue = new Dictionary<string, string>();

                returnValue.Add("110", "Kredi yüklenmesini bekliyor");
                returnValue.Add("120", "Gönderilme saatini bekliyor");
                returnValue.Add("200", "SMSC ye gönderilmek üzere");
                returnValue.Add("300", "SMSC ye gönderildi");
                returnValue.Add("400", "Alıcıya ulaştı");
                returnValue.Add("401", "Alıcıya gönderilecek");
                returnValue.Add("402", "Alıcıya ulaşmadı");
                returnValue.Add("403", "Alıcıya ulaşmadı (zaman aşımı)");
                returnValue.Add("404", "Alıcıya ulaşmadı (bilinmeyen hat)");
                returnValue.Add("405", "Alıcıya ulaşmadı (sistem hatası)");
                returnValue.Add("406", "Alıcıya ulaşmadı (SMS ‘e kapalı)");
                returnValue.Add("407", "Cihaz Hatası");
                returnValue.Add("408", "Cihaza Ulaşılamadı");
                returnValue.Add("409", "Operatör kısıtlama durumu");
                returnValue.Add("410", "Mesaj SMSC de bulunamadı");
                returnValue.Add("411", "Cihaz hafızası dolu");
                returnValue.Add("412", "Ulaşılamayan abone");
                returnValue.Add("413", "Abone profilindeki yetkilendirme yetersiz");
                returnValue.Add("414", "Bilinmeyen Abone Veya Yetersiz Kontor");
                returnValue.Add("420", "Mesaj sunucuda bulunamadı");
                returnValue.Add("-20", "Bu mesaj size a ait değil");
                returnValue.Add("-201", "Silindi (Geçersiz cep no.)");
                returnValue.Add("-202", "Silindi (Mesaj tekrarı)");
                returnValue.Add("-203", "Silindi (Geçersiz son gönderim tarihi)");
                returnValue.Add("-205", "Silindi (Mesaj çok uzun)");
                returnValue.Add("-207", "Silindi (Smsc redetti)");
                returnValue.Add("-210", "Silindi (Zaman aşımı)");
                returnValue.Add("-220", "Silindi (Yetersiz kredi zaman aşımı)");
                returnValue.Add("-222", "Silindi (Abone mesajın bloke edilmesi tercih ettiğinden)");
                returnValue.Add("-250", "Silindi (Müşteri tarafından)");
                returnValue.Add("-260", "Silindi (Fatura borcu)");
                returnValue.Add("-241", "Silindi (Alıcı RET İşlemi)");
                returnValue.Add("-242", "Silindi (IVT Blacklist)");
                returnValue.Add("-243", "Silindi (IVT Whitelist)");
                returnValue.Add("-201..-299", "Silindi");


                return returnValue;
            }
        }
    }
}
