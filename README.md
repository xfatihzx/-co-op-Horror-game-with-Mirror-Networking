# -co-op-Horror-game-with-Mirror-Networking

Ünity ile Mirror Networking kullanılarak geliştirilen korku oyunudur. Proje üzerinde 9 ay boyunça çalışıldı. Oyunda playerController ile karakter kontrolünü sağlarken objelerle iletişimi geçebiliyoruz. Etkileşime geçtiğimiz objeleri inceleyebilir ve bulmacaları çözebiliriz. 

Ayrıca oyunda bizi korkutmak için bir adet enemy var. Bu enemy bizi belli algoritmalarda takip edip jumpScare ve SoundJumpScare komutlarıyla korkutmaya ce germeye çalışmaktadır. Enemy bir çok parametreye sahiptir örn: kızgınlık. Playerlar da bir çok parametreye sahiptir. örn: akıl sağlığı ve korku. Player enemy ilişkisi bu parametrelerle yönetiliyor.

Oyunda Mirror Networking kullanılıyor ve multiplayer özelliklerini NetworkBehaviour üzerinden kontrol ediyoruz. 

Oyun şu aşamada V0,3 dür. Şu anda geliştirmesi duraklatıldı ve gerekli şartlar sağlandığında geliştirme süreci devam edecektir.


Not: Oyundaki kodlar Mirror Networking ile co-op'a uygun olarak düzenlendiği için single player kodları -co-op-Horror-game-with-Mirror-Networking/ReflectionOfDarkness/Assets/Scripts/Deprecateds/ yolunda bulunmaktadır. Yazılan tüm Scriptler'e oradan ulaşabilir ve inceleyebilirsiniz. Oyun geliştirilmeye devam edildiği için detaylara ve senaryosuna giriş yapmıyorum.

İstanbul Aydın Üniversitesi GameLab'da yaptığımız çalışmalardan küçük bir kare:

https://user-images.githubusercontent.com/90801002/218258374-5692ffdd-5d47-42d0-a706-ca9b762ca7f9.mp4




