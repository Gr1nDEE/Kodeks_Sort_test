using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kodeks_Sort_test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Вы хотите сгенерировать файлы перед запуском программы? (да/нет):");
            string userInput = Console.ReadLine()?.Trim().ToLower();
            bool IsYesResponse(string input)
            {
                // Список возможных вариантов ответа "да"
                var yesResponses = new HashSet<string> { "да", "д", "yes", "y", "ага", "конечно", "ок", "окей" };
                return yesResponses.Contains(input);
            }

            if (IsYesResponse(userInput))
            {
                Console.WriteLine("Введите путь для генерации тестовых данных:");
                string testDataPath = Console.ReadLine();
                DataGenerator.GenerateTestData(testDataPath, 5, 100, 1000); // 5 файлов, от 100 до 1000 чисел
            }
            

            Console.WriteLine("Введите путь к каталогу с текстовыми файлами:");
            string directoryPath = Console.ReadLine();

            if (!Directory.Exists(directoryPath))
            {
                Console.WriteLine("Указанный каталог не существует.");
                return;
            }

            try
            {
                // Список для хранения уникальных чисел
                HashSet<int> uniqueNumbers = new HashSet<int>();

                // Чтение всех файлов с расширением .txt
                string[] files = Directory.GetFiles(directoryPath, "*.txt");
                foreach (var file in files)
                {
                    string[] lines = File.ReadAllLines(file);

                    foreach (var line in lines)
                    {
                        if (int.TryParse(line, out int number))
                        {
                            // Проверяем остаток от деления на 4
                            if (number % 4 == 3)
                            {
                                uniqueNumbers.Add(number);
                            }
                        }
                    }
                }

                // Сортируем числа в порядке убывания
                List<int> sortedNumbers = uniqueNumbers.OrderByDescending(x => x).ToList();

                // Записываем результат в файл result.txt
                string resultFilePath = Path.Combine(directoryPath, "result.txt");
                File.WriteAllLines(resultFilePath, sortedNumbers.Select(x => x.ToString()));

                Console.WriteLine($"Результат записан в файл: {resultFilePath}");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
                Console.ReadKey();
            }
        }
    }
    class DataGenerator
    {
        public static void GenerateTestData(string directoryPath, int fileCount, int minNumbers, int maxNumbers)
        {
            Random random = new Random();

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            for (int i = 0; i < fileCount; i++)
            {
                string filePath = Path.Combine(directoryPath, $"file{i + 1}.txt");
                int numberCount = random.Next(minNumbers, maxNumbers + 1);

                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    for (int j = 0; j < numberCount; j++)
                    {
                        writer.WriteLine(random.Next(-1000, 1000)); // Генерация чисел от -1000 до 1000
                    }
                }

                Console.WriteLine($"Сгенерирован файл: {filePath} с {numberCount} числами.");
                
            }
        }
    }
}
