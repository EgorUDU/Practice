using System;
using System.IO;
using Xunit;

public class ReservationManagerTests
{
    [Fact]
    public void AddRestaurant_ValidInput_Success()
    {
        var reservationManager = new ReservationManager();
        reservationManager.AddRestaurant("A", 10);

        Assert.Single(reservationManager.Restaurants);
        Assert.Equal("A", reservationManager.Restaurants[0].Name);
        Assert.Equal(10, reservationManager.Restaurants[0].Tables.Count);
    }

    [Fact]
    public void BookTable_ValidInput_Success()
    {
        var reservationManager = new ReservationManager();
        reservationManager.AddRestaurant("A", 10);

        var result = reservationManager.BookTable("A", new DateTime(2023, 12, 25), 3);

        Assert.True(result);
    }

    [Fact]
    public void BookTable_InvalidRestaurantName_ReturnsFalse()
    {
        var reservationManager = new ReservationManager();

        var result = reservationManager.BookTable("NonExistent", DateTime.Now, 0);

        Assert.False(result);
    }

    [Fact]
    public void BookTable_InvalidTableNumber_ReturnsFalse()
    {
        var reservationManager = new ReservationManager();
        reservationManager.AddRestaurant("A", 5);

        var result = reservationManager.BookTable("A", DateTime.Now, 10);

        Assert.False(result);
    }

    [Fact]
    public void SortRestaurantsByAvailabilityForUsers_ValidInput_Success()
    {
        var reservationManager = new ReservationManager();
        reservationManager.AddRestaurant("A", 5);
        reservationManager.AddRestaurant("B", 10);

        // Перевірка сортування за доступністю столиків
        reservationManager.SortRestaurantsByAvailabilityForUsers(DateTime.Now);

        Assert.Equal("B", reservationManager.Restaurants[0].Name);
        Assert.Equal("A", reservationManager.Restaurants[1].Name);
    }

    [Fact]
    public void FindAllFreeTables_ValidInput_Success()
    {
        var reservationManager = new ReservationManager();
        reservationManager.AddRestaurant("A", 2);
        reservationManager.AddRestaurant("B", 3);

        var date = new DateTime(2023, 12, 25);

        // Забронювати один столик, щоб перевірити пошук вільних столиків
        reservationManager.BookTable("A", date, 0);

        var freeTables = reservationManager.FindAllFreeTables(date);

        Assert.Equal(4, freeTables.Count); // 2 столики в A та 3 столики в B
    }

    [Fact]
    public void LoadRestaurantsFromFile_ValidInput_Success()
    {
        var reservationManager = new ReservationManager();

        // Створення тимчасового файлу для тестування
        var filePath = Path.GetTempFileName();
        File.WriteAllText(filePath, "A,5\nB,3");

        // Завантаження ресторанів з файлу
        reservationManager.LoadRestaurantsFromFile(filePath);

        Assert.Equal(2, reservationManager.Restaurants.Count);
        Assert.Equal("A", reservationManager.Restaurants[0].Name);
        Assert.Equal(5, reservationManager.Restaurants[0].Tables.Count);
        Assert.Equal("B", reservationManager.Restaurants[1].Name);
        Assert.Equal(3, reservationManager.Restaurants[1].Tables.Count);

        // Видалення тимчасового файлу
        File.Delete(filePath);
    }
}

public class RestaurantTests
{
    [Fact]
    public void Constructor_ValidInput_Success()
    {
        var restaurant = new Restaurant("A", 3);

        Assert.Equal("A", restaurant.Name);
        Assert.Equal(3, restaurant.Tables.Count);
    }
}

public class RestaurantTableTests
{
    [Fact]
    public void Book_ValidInput_Success()
    {
        var restaurantTable = new RestaurantTable();
        var date = new DateTime(2023, 12, 25);

        var result = restaurantTable.Book(date);

        Assert.True(result);
        Assert.True(restaurantTable.IsBooked(date));
    }

    [Fact]
    public void Book_AlreadyBooked_ReturnsFalse()
    {
        var restaurantTable = new RestaurantTable();
        var date = new DateTime(2023, 12, 25);

        restaurantTable.Book(date);

        var result = restaurantTable.Book(date);

        Assert.False(result);
        Assert.True(restaurantTable.IsBooked(date));
    }

    [Fact]
    public void IsBooked_BookedDate_ReturnsTrue()
    {
        var restaurantTable = new RestaurantTable();
        var date = new DateTime(2023, 12, 25);

        restaurantTable.Book(date);

        var result = restaurantTable.IsBooked(date);

        Assert.True(result);
    }

    [Fact]
    public void IsBooked_NotBookedDate_ReturnsFalse()
    {
        var restaurantTable = new RestaurantTable();
        var date = new DateTime(2023, 12, 25);

        var result = restaurantTable.IsBooked(date);

        Assert.False(result);
    }
}
