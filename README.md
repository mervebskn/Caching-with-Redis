## 1. Redis

- Distributed Caching
- Replication
- Redis Sentinel

### Caching ?
Yazılım sürecinde veriye daha hızlı erişim sağlamak için, veriyi veritabanı yerine ön bellekten almayı sağlayan sistem.
### In-Memory Caching
Veriyi, uygulamanın çalıştığı bilgisayarın RAM'inde caching yapmayı sağlayan yaklaşım.
```
AbsoluteTime => Cache'deki datanın ne kadar süre tutulanacağını dair net ömrü tanımlar, süre bitince cache temizlenir.
Slidingtime => Cache'lenmiş datanın memory'de belirtilen süre zarfında tutulmasını belirler.
Örnek : Bir veriye 1 aylık absolutetime değeri verelim ve slidingtime 2 gün belirleyelim. 2 gün bu veride işlem yapılmazsa cache silinir.
```
### Distributed Caching
Veriyi tek bir ortamda tutmak yerine, birden fazla fiziksel makinede cache'leyen yaklaşım. Büyük veri setleri için, veriler bölünerek makinelere dağıtılır.
### Redis
```
-Caching işlemi için kullanılan veritabanı
-Message Broker olarak da kullanılmaktadır
-Yapısal olarak key-value yapısında çalışır
-NoSql veritabanıdır
```
### Replication - Managing Redis
Redis ile yapılan çalışmalarda, sunucuda saklanan verilerin güvencesini sağlamak adına, farklı bir sunucuda çoğaltabiliriz.
**_Replication_** , bir redis sunucusundaki datayı farklı bir sunucuda çoğaltma/replice işlemidir. **_Master_** olarak belirlenen sunucuda hata olduğunda **_Slave_** olarak belirlenen sunucu görevi devralır. _Slave_, readonly, veriyi sadece okur.
