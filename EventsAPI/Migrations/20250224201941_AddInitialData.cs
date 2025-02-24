using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EventsAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Event",
                columns: new[] { "Id", "Category", "Description", "EventDateTime", "ImagePath", "Location", "MaximumParticipants", "Name" },
                values: new object[,]
                {
                    { 1, 1, "Вечер живой музыки с любимыми хитами и новыми песнями.", new DateTime(2025, 3, 15, 16, 0, 0, 0, DateTimeKind.Utc), "/images/mumiy_troll.jpg", "Дворец Республики", 500, "Концерт группы \"Мумий Тролль\" в Минске" },
                    { 2, 2, "Забег на 5, 10 и 21 километр для всех желающих.", new DateTime(2025, 4, 7, 6, 0, 0, 0, DateTimeKind.Utc), "/images/marathon.jpg", "Центральный парк", 3000, "Спортивное мероприятие \"Минский марафон\"" },
                    { 3, 3, "Увлекательная лекция о художественных достижениях Возрождения.", new DateTime(2025, 3, 12, 15, 30, 0, 0, DateTimeKind.Utc), "/images/renaissance_art.jpg", "Национальный музей", 100, "Лекция \"История искусства эпохи Возрождения\"" },
                    { 4, 4, "Экспертное мнение о правильном питании и здоровье.", new DateTime(2025, 3, 20, 14, 0, 0, 0, DateTimeKind.Utc), "/images/healthy_food.jpg", "Конференц-зал гостиницы \"Минск\"", 200, "Семинар \"Здоровое питание: мифы и реальность\"" },
                    { 5, 5, "Экспозиция работ современных художников.", new DateTime(2025, 3, 25, 7, 0, 0, 0, DateTimeKind.Utc), "/images/modern_art.jpg", "Галерея \"ArtBel\"", 150, "Выставка современного искусства" },
                    { 6, 9, "Показ лучших фильмов от режиссёров Беларуси и мира.", new DateTime(2025, 3, 22, 13, 0, 0, 0, DateTimeKind.Utc), "/images/film_festival.jpg", "Кинотеатр \"Октябрь\"", 300, "Кинофестиваль \"Минское кино\"" },
                    { 7, 1, "Музыкальный вечер в поддержку детей из детских домов.", new DateTime(2025, 3, 18, 16, 30, 0, 0, DateTimeKind.Utc), "/images/charity_concert.jpg", "Дом культуры \"Минск\"", 400, "Благотворительный концерт \"Помоги детям\"" },
                    { 8, 3, "Практические занятия и тренинги по саморазвитию.", new DateTime(2025, 3, 30, 7, 0, 0, 0, DateTimeKind.Utc), "/images/personal_growth.jpg", "Конференц-зал \"Минск\"", 150, "Семинар по личностному росту \"Стань лучшей версией себя\"" },
                    { 9, 10, "Классическая постановка шекспировской трагедии.", new DateTime(2025, 3, 27, 15, 0, 0, 0, DateTimeKind.Utc), "/images/romeo_juliet.jpg", "Национальный театр", 500, "Театральное представление \"Ромео и Джульетта\"" },
                    { 10, 12, "Обсуждение последних инноваций и трендов в IT-индустрии.", new DateTime(2025, 4, 10, 6, 0, 0, 0, DateTimeKind.Utc), "/images/tech_conference.jpg", "Конференц-центр \"Минск\"", 600, "Конференция \"Технологии будущего\"" },
                    { 11, 5, "Практические уроки по техникам живописи.", new DateTime(2025, 3, 21, 11, 0, 0, 0, DateTimeKind.Utc), "/images/painting_workshop.jpg", "Арт-студия \"Минск\"", 50, "Мастер-класс по живописи" },
                    { 12, 6, "Обучение приготовлению блюд национальной кухни.", new DateTime(2025, 4, 5, 8, 0, 0, 0, DateTimeKind.Utc), "/images/cooking_workshop.jpg", "Кулинарная студия \"Минск\"", 30, "Кулинарный мастер-класс \"Готовим вместе\"" },
                    { 13, 3, "Увлекательное путешествие по историческим достопримечательностям города.", new DateTime(2025, 3, 28, 7, 0, 0, 0, DateTimeKind.Utc), "/images/city_tour.jpg", "Центральная площадь", 40, "Экскурсия по историческим местам Минска" },
                    { 14, 8, "Чтение стихов местных поэтов и любителей поэзии.", new DateTime(2025, 3, 26, 15, 0, 0, 0, DateTimeKind.Utc), "/images/poetry_evening.jpg", "Литературный клуб \"Минск\"", 100, "Вечер поэзии" },
                    { 15, 10, "Вечер юмора с участием популярных комиков.", new DateTime(2025, 4, 1, 17, 0, 0, 0, DateTimeKind.Utc), "/images/standup_comedy.jpg", "Клуб \"Смех\"", 150, "Выступление стендап-комиков" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BirthDateTime", "Email", "LastName", "Login", "Name", "PasswordHash", "RefreshToken", "RefreshTokenExpiresAt", "Role" },
                values: new object[] { 1, new DateTime(2005, 3, 1, 1, 0, 0, 0, DateTimeKind.Utc), "admin@gmail.com", "Главный", "admin", "Администратор", "$2a$11$fR8mRh4qNDDrAeao7y7RQezOoFsfG3h7nudLj/Gxqt.faTukK4bIu", "_", new DateTime(2025, 2, 24, 20, 0, 0, 0, DateTimeKind.Utc), "Admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
