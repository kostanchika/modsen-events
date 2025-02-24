using Microsoft.EntityFrameworkCore;
using EventsAPI.Models;

namespace EventsAPI.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) 
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.BirthDateTime)
                    .IsRequired();

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Role)
                    .IsRequired();

                entity.HasIndex(e => e.Login)
                    .IsUnique();

                entity.HasIndex(e => e.Email)
                    .IsUnique();
            });
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Login = "admin",
                    PasswordHash = "$2a$11$fR8mRh4qNDDrAeao7y7RQezOoFsfG3h7nudLj/Gxqt.faTukK4bIu",//admin123
                    Name = "Администратор",
                    LastName = "Главный",
                    BirthDateTime = new DateTime(2005, 3, 1, 3, 0, 0).ToUniversalTime(),
                    Email = "admin@gmail.com",
                    Events = [],
                    RefreshToken = "_",
                    RefreshTokenExpiresAt = new DateTime(2025, 2, 24, 23, 0, 0).ToUniversalTime(),
                    Role = "Admin"
                }
            );

            modelBuilder.Entity<Event>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Description)
                    .HasMaxLength(500);

                entity.Property(e => e.EventDateTime)
                    .IsRequired();

                entity.Property(e => e.Location)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Category)
                    .IsRequired();

                entity.Property(e => e.MaximumParticipants)
                    .IsRequired();

                entity.Property(e => e.ImagePath)
                    .HasDefaultValue("/images/default.png");

                entity.HasMany(e => e.Participants)
                    .WithMany(u => u.Events)
                    .UsingEntity(j => j.ToTable("EventsParticipants"));
            });
            modelBuilder.Entity<Event>().HasData(
                new Event
                {
                    Id = 1,
                    Name = "Концерт группы \"Мумий Тролль\" в Минске",
                    Description = "Вечер живой музыки с любимыми хитами и новыми песнями.",
                    EventDateTime = new DateTime(2025, 3, 15, 19, 0, 0).ToUniversalTime(),
                    Location = "Дворец Республики",
                    Category = EventCategories.Music,
                    MaximumParticipants = 500,
                    ImagePath = "/images/mumiy_troll.jpg"
                },
                new Event
                {
                    Id = 2,
                    Name = "Спортивное мероприятие \"Минский марафон\"",
                    Description = "Забег на 5, 10 и 21 километр для всех желающих.",
                    EventDateTime = new DateTime(2025, 4, 7, 9, 0, 0).ToUniversalTime(),
                    Location = "Центральный парк",
                    Category = EventCategories.Sports,
                    MaximumParticipants = 3000,
                    ImagePath = "/images/marathon.jpg"
                },
                new Event
                {
                    Id = 3,
                    Name = "Лекция \"История искусства эпохи Возрождения\"",
                    Description = "Увлекательная лекция о художественных достижениях Возрождения.",
                    EventDateTime = new DateTime(2025, 3, 12, 18, 30, 0).ToUniversalTime(),
                    Location = "Национальный музей",
                    Category = EventCategories.Education,
                    MaximumParticipants = 100,
                    ImagePath = "/images/renaissance_art.jpg"
                },
                new Event
                {
                    Id = 4,
                    Name = "Семинар \"Здоровое питание: мифы и реальность\"",
                    Description = "Экспертное мнение о правильном питании и здоровье.",
                    EventDateTime = new DateTime(2025, 3, 20, 17, 0, 0).ToUniversalTime(),
                    Location = "Конференц-зал гостиницы \"Минск\"",
                    Category = EventCategories.Health,
                    MaximumParticipants = 200,
                    ImagePath = "/images/healthy_food.jpg"
                },
                new Event
                {
                    Id = 5,
                    Name = "Выставка современного искусства",
                    Description = "Экспозиция работ современных художников.",
                    EventDateTime = new DateTime(2025, 3, 25, 10, 0, 0).ToUniversalTime(),
                    Location = "Галерея \"ArtBel\"",
                    Category = EventCategories.Art,
                    MaximumParticipants = 150,
                    ImagePath = "/images/modern_art.jpg"
                },
                new Event
                {
                    Id = 6,
                    Name = "Кинофестиваль \"Минское кино\"",
                    Description = "Показ лучших фильмов от режиссёров Беларуси и мира.",
                    EventDateTime = new DateTime(2025, 3, 22, 16, 0, 0).ToUniversalTime(),
                    Location = "Кинотеатр \"Октябрь\"",
                    Category = EventCategories.Film,
                    MaximumParticipants = 300,
                    ImagePath = "/images/film_festival.jpg"
                },
                new Event
                {
                    Id = 7,
                    Name = "Благотворительный концерт \"Помоги детям\"",
                    Description = "Музыкальный вечер в поддержку детей из детских домов.",
                    EventDateTime = new DateTime(2025, 3, 18, 19, 30, 0).ToUniversalTime(),
                    Location = "Дом культуры \"Минск\"",
                    Category = EventCategories.Music,
                    MaximumParticipants = 400,
                    ImagePath = "/images/charity_concert.jpg"
                },
                new Event
                {
                    Id = 8,
                    Name = "Семинар по личностному росту \"Стань лучшей версией себя\"",
                    Description = "Практические занятия и тренинги по саморазвитию.",
                    EventDateTime = new DateTime(2025, 3, 30, 10, 0, 0).ToUniversalTime(),
                    Location = "Конференц-зал \"Минск\"",
                    Category = EventCategories.Education,
                    MaximumParticipants = 150,
                    ImagePath = "/images/personal_growth.jpg"
                },
                new Event
                {
                    Id = 9,
                    Name = "Театральное представление \"Ромео и Джульетта\"",
                    Description = "Классическая постановка шекспировской трагедии.",
                    EventDateTime = new DateTime(2025, 3, 27, 18, 0, 0).ToUniversalTime(),
                    Location = "Национальный театр",
                    Category = EventCategories.Theatre,
                    MaximumParticipants = 500,
                    ImagePath = "/images/romeo_juliet.jpg"
                },
                new Event
                {
                    Id = 10,
                    Name = "Конференция \"Технологии будущего\"",
                    Description = "Обсуждение последних инноваций и трендов в IT-индустрии.",
                    EventDateTime = new DateTime(2025, 4, 10, 9, 0, 0).ToUniversalTime(),
                    Location = "Конференц-центр \"Минск\"",
                    Category = EventCategories.Science,
                    MaximumParticipants = 600,
                    ImagePath = "/images/tech_conference.jpg"
                },
                new Event
                {
                    Id = 11,
                    Name = "Мастер-класс по живописи",
                    Description = "Практические уроки по техникам живописи.",
                    EventDateTime = new DateTime(2025, 3, 21, 14, 0, 0).ToUniversalTime(),
                    Location = "Арт-студия \"Минск\"",
                    Category = EventCategories.Art,
                    MaximumParticipants = 50,
                    ImagePath = "/images/painting_workshop.jpg"
                },
                new Event
                {
                    Id = 12,
                    Name = "Кулинарный мастер-класс \"Готовим вместе\"",
                    Description = "Обучение приготовлению блюд национальной кухни.",
                    EventDateTime = new DateTime(2025, 4, 5, 11, 0, 0).ToUniversalTime(),
                    Location = "Кулинарная студия \"Минск\"",
                    Category = EventCategories.Food,
                    MaximumParticipants = 30,
                    ImagePath = "/images/cooking_workshop.jpg"
                },
                new Event
                {
                    Id = 13,
                    Name = "Экскурсия по историческим местам Минска",
                    Description = "Увлекательное путешествие по историческим достопримечательностям города.",
                    EventDateTime = new DateTime(2025, 3, 28, 10, 0, 0).ToUniversalTime(),
                    Location = "Центральная площадь",
                    Category = EventCategories.Education,
                    MaximumParticipants = 40,
                    ImagePath = "/images/city_tour.jpg"
                },
                new Event
                {
                    Id = 14,
                    Name = "Вечер поэзии",
                    Description = "Чтение стихов местных поэтов и любителей поэзии.",
                    EventDateTime = new DateTime(2025, 3, 26, 18, 0, 0).ToUniversalTime(),
                    Location = "Литературный клуб \"Минск\"",
                    Category = EventCategories.Literature,
                    MaximumParticipants = 100,
                    ImagePath = "/images/poetry_evening.jpg"
                },
                new Event
                {
                    Id = 15,
                    Name = "Выступление стендап-комиков",
                    Description = "Вечер юмора с участием популярных комиков.",
                    EventDateTime = new DateTime(2025, 4, 1, 20, 0, 0).ToUniversalTime(),
                    Location = "Клуб \"Смех\"",
                    Category = EventCategories.Theatre,
                    MaximumParticipants = 150,
                    ImagePath = "/images/standup_comedy.jpg"
                }
            );
        }
    }
}
