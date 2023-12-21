using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

public class TableReservationApp
{
    static void Main(string[] args)
    {
        // Оновлено ім'я змінної для відображення призначення
        var reservationManager = new ReservationManager();
        reservationManager.AddRestaurant("A", 10);
        reservationManager.AddRestaurant("B", 5);

        // Використано нову змінну з оновленим ім'ям
        Console.WriteLine(reservationManager.BookTable("A", new DateTime(2023, 12, 25), 3)); // True
        Console.WriteLine(reservationManager.BookTable("A", new DateTime(2023, 12, 25), 3)); // False

        
        // Цей момент я трішечки не зміг розібрати
        //var reservationManagerFile = new ReservationManager();
        //reservationManager.LoadRestaurantsFromFile("C:\\Users\\Егор\\Desktop\\Новая папка (2)\\load.txt");
    }
}

public class ReservationManager
{
    // Оновлено назву властивості для полегшення читабельності та відображення сутності
    public List<Restaurant> Restaurants;

    public ReservationManager()
    {
        Restaurants = new List<Restaurant>();
    }

    // Оновлено назву методу для відображення призначення методу
    public void AddRestaurant(string name, int tableCount)
    {
        try
        {
            var restaurant = new Restaurant(name, tableCount);
            Restaurants.Add(restaurant);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка");
        }
    }

    public void LoadRestaurantsFromFile(string filePath)
    {
        try
        {
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                // Замінено назву змінних для полегшення читабельності
                var parts = line.Split(',');
                if (parts.Length == 2 && int.TryParse(parts[1], out int tableCount))
                {
                    // Використано оновлену назву методу
                    AddRestaurant(parts[0], tableCount);
                }
                else
                {
                    Console.WriteLine(line);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при загрузке ресторанов из файла: " + ex.Message);
        }
    }

    // Перейменовано параметри методу для відображення призначення
    public List<string> FindAllFreeTables(DateTime date)
    {
        try
        {
            List<string> freeTables = new List<string>();
            foreach (var restaurant in Restaurants)
            {
                for (int tableNumber = 0; tableNumber < restaurant.Tables.Count; tableNumber++)
                {
                    // Використано оновлені назви властивостей та методів
                    if (!restaurant.Tables[tableNumber].IsBooked(date))
                    {
                        freeTables.Add($"{restaurant.Name} - Столик {tableNumber + 1}");
                    }
                }
            }
            return freeTables;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при поиске свободных столов: " + ex.Message);
            return new List<string>();
        }
    }

    // Перейменовано параметри методу для відображення призначення
    public bool BookTable(string restaurantName, DateTime date, int tableNumber)
    {
        try
        {
            // Оновлено змінну для відображення призначення
            var restaurant = GetRestaurantByName(restaurantName);
            if (restaurant == null)
            {
                throw new Exception("Ресторан не найден");
            }

            // Додано додаткову перевірку на коректність номеру столика
            if (tableNumber < 0 || tableNumber >= restaurant.Tables.Count)
            {
                throw new Exception("Неверный номер столика");
            }

            // Використано новий метод для забронювання столика
            return restaurant.Tables[tableNumber].Book(date);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка: " + ex.Message);
            return false;
        }
    }

    // Додано новий метод для сортування ресторанів за доступністю столиків
    public void SortRestaurantsByAvailabilityForUsers(DateTime date)
    {
        try
        {
            // Замінено лямбда-вираз на виклик окремого методу
            Restaurants = Restaurants.OrderByDescending(r => CountAvailableTables(r, date)).ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка");
        }
    }

    // Винесено частину логіки в окремий метод
    private int CountAvailableTables(Restaurant restaurant, DateTime date)
    {
        try
        {
            return restaurant.Tables.Count(t => !t.IsBooked(date));
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка");
            return 0;
        }
    }

    // Перейменовано метод для відображення призначення
    private Restaurant GetRestaurantByName(string name)
    {
        return Restaurants.FirstOrDefault(r => r.Name == name);
    }
}

// Оновлено назву класу для відображення призначення класу
public class Restaurant
{
    // Перейменовано властивість для полегшення читабельності та відображення сутності
    public string Name { get; private set; }
    public List<RestaurantTable> Tables { get; private set; }

    public Restaurant(string name, int tableCount)
    {
        Name = name;
        Tables = Enumerable.Range(0, tableCount).Select(_ => new RestaurantTable()).ToList();
    }
}

public class RestaurantTable
{
    // Оновлено назву властивості для полегшення читабельності та відображення сутності вмісту
    private HashSet<DateTime> bookedDates; 

    public RestaurantTable()
    {
        // Ініціалізовано властивість з новим іменем
        bookedDates = new HashSet<DateTime>();
    }

    // Покращено метод бронювання столика
    public bool Book(DateTime date)
    {
        // Вилучено блок catch (Exception ex), оскільки він не здається корисним для HashSet
        try
        {
            // Перевірка, чи дата вже заброньована
            if (bookedDates.Contains(date))
            {
                return false;
            }
            // Додано дату до списку заброньованих
            bookedDates.Add(date);
            return true;
        }
        // Виведено повідомлення про помилку та повернуто false у випадку винятку
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка во время бронирования: {ex.Message}");
            return false;
        }
    }

    // Перейменовано параметри методу для відображення призначення
    public bool IsBooked(DateTime date)
    {
        return bookedDates.Contains(date);
    }
}

