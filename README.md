# HWA-NetCore-MemoryCache-And-Logging

<img src="https://www.aihr.com/wp-content/uploads/performance-management-cover.png" width="1000" height="553" alt="">

Bu proje, 'log', 'cache' ve 'middleware' yapılarının CQRS tasarım deseninde nasıl kullanılabileceğini göstermektedir.

## Kullanım

Projenin çalıştırılması ve kullanılması için aşağıdaki adımları izleyin:

1. Paketleri Yükleme: Proje dosyalarınızın bulunduğu dizinde terminali açın ve aşağıdaki komutu çalıştırarak gerekli paketleri yükleyin:

   ```dotnet restore```

2. Uygulamayı Başlatma: Projeyi ayağa kaldırmak için terminalde aşağıdaki komutu çalıştırın:

   ```dotnet run```

3. API'ye Erişim: Uygulama başarıyla çalıştıktan sonra, tarayıcınızda veya API test aracınızda `https://localhost:5163` adresine giderek API'ye erişebilirsiniz.

## Proje Yapısı

Projede kullanılan ana klasörler ve dosyalar aşağıdaki gibidir:

- **Core**: Temel iş mantığı ve çapraz kesen endişelerin (cross-cutting concerns) ele alındığı klasör.
  - **CrossCuttingConcerns**: Uygulamanın farklı katmanları arasında tekrar kullanılan kodları içerir.
    - **Exceptions**: Uygulama genelinde fırlatılan ve ele alınan istisna (exception) türlerini ve bunları işleyen kodları içerir.
    - **Logging**: Uygulamanın farklı bölümlerinden gelen log girişlerini işleyen kodları içerir.
   - **Application**: Uygulamanın iş mantığına özgü ara katman davranışlarını içerir.
     - **Caching**: Veri önbellekleme işlemlerini ve bu işlemleri yöneten kodları içerir.
     - **Logging**: Uygulamanın loglama işlemlerini gerçekleştirmek için gerekli alt yapıyı ve konfigurasyon işlemlerini bulundurur.
- **Application**: Uygulamanın iş mantığına özgü kodları içerir.
- **Domain**: Uygulamada kullanılan veri tabanı modellerini içerir.
- **Persistence**: Uygulamanın veri tabanı ile olan iletişimini sağlar.
- **WebAPI**: Uygulamanın sunum katmanı.
  
## Ara Katman Sınıflarının Kullanımı

### Cache Davranışları
***CacheBehavior:*** MediatR pipeline'ındaki bir davranıştır. MediatR isteklerinin sonuçlarını önbelleğe alır ve gelecekteki benzer istekler için sonuçları hızlı bir şekilde döndürmek için kullanılır.

***CacheRemovingBehavior:*** MediatR pipeline'ındaki bir davranıştır. Önceden önbelleğe alınmış sonuçları ve ilgili grupları temizler. Özellikle veri güncelleme işlemleri gerçekleştirildiğinde önbelleğin güncellenmesini sağlar.

### Logging Davranışı
***LoggingBehavior:*** MediatR pipeline'ındaki bir davranıştır. MediatR isteklerini ve yanıtlarını günlüğe kaydeder. Bu, uygulamanın çalışma sürecini izlemek, hataları bulmak ve performansı değerlendirmek için kullanılır.
İstisna İşleme ve Kayıt
***ExceptionMiddleware:*** ASP.NET Core pipeline'ındaki bir ara katman olarak kullanılır. HTTP istek işleme sırasında oluşan istisnaları ele alır, kullanıcıya uygun bir hata mesajı döndürür ve hataları kaydeder. Bu, uygulamanın güvenilirliğini artırır ve hata ayıklama sürecini kolaylaştırır.
Bu sınıflar, uygulamanın performansını, güvenilirliğini ve bakımını artırmak için kullanılmaktadır.

## Kullanılan Paketler

### MediatR İle İlgili Paketler
- **MediatR** (v12.2.0)

### Entity Framework Core İle İlgili Paketler
- **Microsoft.EntityFrameworkCore** (v8.0.5)
- **Microsoft.EntityFrameworkCore.SqlServer** (v8.0.5)
- **Microsoft.EntityFrameworkCore.Tools** (v8.0.5)

### Microsoft.Extensions İle İlgili Paketler
- **Microsoft.Extensions.Caching.Abstractions** (v8.0.0 - v2.2.0)
- **Microsoft.Extensions.Configuration.Abstractions** (v8.0.0)
- **Microsoft.Extensions.Configuration.Binder** (v8.0.1)
- **Microsoft.Extensions.DependencyInjection** (v8.0.0)
- **Microsoft.Extensions.DependencyInjection.Abstractions** (v8.0.0 - v6.0.0)
- **Microsoft.Extensions.Logging** (v8.0.0)
- **Microsoft.Extensions.Logging.Abstractions** (v8.0.0 - v2.2.0)

### Serilog İle İlgili Paketler
- **Serilog** (v3.1.1)
- **Serilog.Sinks.File** (v5.0.0)
