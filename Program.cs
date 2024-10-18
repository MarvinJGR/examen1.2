using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace examen1._2
{
    internal class Program
    {
        private static string connectionString = "server=localhost;user=root;database=CarDB;port=3306;password=;";

        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Welcome to the Car Management App");
                Console.WriteLine("1. View All Cars");
                Console.WriteLine("2. Add a New Car");
                Console.WriteLine("3. Update an Existing Car");
                Console.WriteLine("4. Exit");
                Console.Write("Please select an option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ViewAllCars();  // Mostrar todos los carros
                        break;
                    case "2":
                        AddNewCar();    // Agregar un nuevo carro
                        break;
                    case "3":
                        UpdateCar();    // Actualizar un carro existente
                        break;
                    case "4":
                        Console.WriteLine("Exiting...");
                        // Pausar antes de salir
                        Console.WriteLine("Press any key to exit...");
                        Console.ReadKey();
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        // Método para ver todos los carros
        private static void ViewAllCars()
        {
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Car";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    Console.WriteLine("\n--- List of Cars ---");
                    while (reader.Read())
                    {
                        Console.WriteLine($"CarID: {reader["CarID"]}, Make: {reader["Make"]}, Model: {reader["Model"]}, Year: {reader["Year"]}, Price: {reader["Price"]}, DateAdded: {reader["DateAdded"]}");
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            // Pausar para que el usuario vea los resultados
            Console.WriteLine("\nPress any key to return to the main menu.");
            Console.ReadKey();
        }

        // Método para agregar un nuevo carro
        private static void AddNewCar()
        {
            try
            {
                Console.Write("Enter the make: ");
                string make = Console.ReadLine();

                Console.Write("Enter the model: ");
                string model = Console.ReadLine();

                Console.Write("Enter the year: ");
                int year = int.Parse(Console.ReadLine());

                Console.Write("Enter the price: ");
                decimal price = decimal.Parse(Console.ReadLine());

                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Car (Make, Model, Year, Price, DateAdded) VALUES (@make, @model, @year, @price, @dateAdded)";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@make", make);
                    cmd.Parameters.AddWithValue("@model", model);
                    cmd.Parameters.AddWithValue("@year", year);
                    cmd.Parameters.AddWithValue("@price", price);
                    cmd.Parameters.AddWithValue("@dateAdded", DateTime.Now);

                    cmd.ExecuteNonQuery();
                }

                Console.WriteLine("New car added successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            // Pausar para que el usuario vea el mensaje
            Console.WriteLine("Press any key to return to the main menu.");
            Console.ReadKey();
        }

        // Método para actualizar un carro existente
        private static void UpdateCar()
        {
            try
            {
                Console.Write("Enter the CarID of the car to update: ");
                int carID = int.Parse(Console.ReadLine());

                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Car WHERE CarID = @carID";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@carID", carID);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        Console.WriteLine($"Current data: Make: {reader["Make"]}, Model: {reader["Model"]}, Year: {reader["Year"]}, Price: {reader["Price"]}, DateAdded: {reader["DateAdded"]}");
                        reader.Close();

                        Console.Write("Enter the new make (leave blank to keep current): ");
                        string newMake = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(newMake)) newMake = (string)reader["Make"];

                        Console.Write("Enter the new model (leave blank to keep current): ");
                        string newModel = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(newModel)) newModel = (string)reader["Model"];

                        Console.Write("Enter the new year (leave blank to keep current): ");
                        string newYearStr = Console.ReadLine();
                        int newYear = string.IsNullOrWhiteSpace(newYearStr) ? (int)reader["Year"] : int.Parse(newYearStr);

                        Console.Write("Enter the new price (leave blank to keep current): ");
                        string newPriceStr = Console.ReadLine();
                        decimal newPrice = string.IsNullOrWhiteSpace(newPriceStr) ? (decimal)reader["Price"] : decimal.Parse(newPriceStr);

                        string updateQuery = "UPDATE Car SET Make = @make, Model = @model, Year = @year, Price = @price WHERE CarID = @carID";
                        MySqlCommand updateCmd = new MySqlCommand(updateQuery, connection);
                        updateCmd.Parameters.AddWithValue("@make", newMake);
                        updateCmd.Parameters.AddWithValue("@model", newModel);
                        updateCmd.Parameters.AddWithValue("@year", newYear);
                        updateCmd.Parameters.AddWithValue("@price", newPrice);
                        updateCmd.Parameters.AddWithValue("@carID", carID);

                        updateCmd.ExecuteNonQuery();

                        Console.WriteLine("Car updated successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Car not found.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            // Pausar para que el usuario vea los resultados
            Console.WriteLine("Press any key to return to the main menu.");
            Console.ReadKey();
        }
    }
}
