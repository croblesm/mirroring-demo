using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        // Check if the correct number of command-line arguments is provided
        if (args.Length != 2 || !int.TryParse(args[0], out int numberOfThreads) || !int.TryParse(args[1], out int executionsPerThread))
        {
            Console.WriteLine("Usage: SqlQueryApp <NumberOfThreads> <ExecutionsPerThread>");
            return;
        }

        // Load environment variables from .env file
        LoadEnvVariables();

        // Retrieve connection string from environment variables
        string connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

        // Ensure the connection string is not null or empty
        if (string.IsNullOrEmpty(connectionString))
        {
            Console.WriteLine("Connection string not found. Please check your .env file.");
            return;
        }

        // Run the stored procedure in multiple threads, each executing it multiple times
        Parallel.For(0, numberOfThreads, i =>
        {
            for (int j = 0; j < executionsPerThread; j++)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("[dbo].[SelectRandomData]", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        // You can add parameters if your stored procedure requires them
                        // command.Parameters.AddWithValue("@ParameterName", parameterValue);

                        string result = command.ExecuteScalar()?.ToString();
                        Console.WriteLine($"Thread {i + 1}, Execution {j + 1}: Result from stored procedure: {result}");
                    }
                }
            }
        });
    }

    static void LoadEnvVariables()
    {
        string envFilePath = ".env";
        if (File.Exists(envFilePath))
        {
            foreach (var line in File.ReadLines(envFilePath))
            {
                var parts = line.Split('=', 2);
                if (parts.Length == 2)
                {
                    var key = parts[0].Trim();
                    var value = parts[1].Trim();
                    Environment.SetEnvironmentVariable(key, value);
                }
            }
        }
    }
}
