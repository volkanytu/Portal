using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK.Library.Entities
{
    public enum ResponseMessageDefinitionEnum
    {
        [Description("İşlem Başarılı.")]
        Success = 0,
        [Description("Genel Hata.")]
        GeneralException = 100,
        [Description("Giriş Başarısız.")]
        AuthenticationFailure = 101,
        [Description("Yetkisiz İşlem.")]
        AuthorizationFailure = 102,
        [Description("Kullanıcı için hedef bulunamadı.")]
        UserGoalNotFound = 110,
        [Description("Kullanıcı için birden fazla hedef tanımlanmış.")]
        UserGoalMultipleFound = 111,
        [Description("Dosya yüklenirken hata meydana geldi.")]
        FileUploadFailure = 200,
        [Description("Girdiğiniz bilgileri kontrol ediniz: {0}")]
        ValidationFailure = 300,
        [Description("İlgili kaydın daha önce girişi yapılmış.")]
        DuplicateValidationFailure = 301,
    }
}
