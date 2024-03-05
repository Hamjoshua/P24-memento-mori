using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace app
{
    public class Operations
    {
        public static string[] ProgramOperations = new string[]
        {
            "Работа с файлом",
            "Индексация файлов"
        };

        public static string[] TextEditorOperations = new string[]
        {
            "Сохранить в битовый файл",
            "Прочитать из битового файла",
            "Сохранить в Xml файл",
            "Прочитать из Xml файла",
            "Изменить содержимое файла",
            "Сохранить",
            "Отменить",
            "Информация о текущем файле"
        };
    }


    class Program
    {
        static int ChooseOperation(string header, string[] operations)
        {
            while (true)
            {
                Console.WriteLine(header);
                for (int operationIndex = 0; operationIndex < operations.Length; ++operationIndex)
                {
                    string operation = operations[operationIndex];
                    Console.WriteLine($"{operationIndex + 1}: {operation}");
                }
                Console.WriteLine("\n0: Назад");
                try
                {
                    int operationNumber = int.Parse(Console.ReadLine());

                    if (operationNumber < 0 || operationNumber > operations.Length)
                    {
                        Console.WriteLine("\aЧисло выходит за рамки доступных операций!");
                        continue;
                    }

                    return operationNumber;
                }
                catch
                {
                    Console.WriteLine("\aНе число!");
                }
            }
        }

        static string GetRightPath(string rightExtension = "", bool isNecessary = true)
        {
            while (true)
            {
                Console.WriteLine("Введите полный путь:");
                if (!isNecessary)
                {
                    Console.WriteLine("(Если нужно сохранить файл в папке с программой, нажмите Enter для продолжения)");
                }
                string path = Console.ReadLine();
                if(!isNecessary && path == "")
                {
                    return path;
                }

                bool directoryNotExists = !Directory.Exists(path);
                bool fileNotExists = !File.Exists(path);

                if (directoryNotExists && fileNotExists)
                {
                    Console.WriteLine("\aТакого пути не существует!");
                    continue;
                }

                string extension = path.Substring(path.IndexOf("."));
                if (extension != rightExtension)
                {
                    Console.WriteLine("\aНеправильно расширение файла!");
                    continue;
                }

                return path;
            }

        }

        static TextFile InitTextFile()
        {
            string fileName = "";
            string fileData = "";

            Console.WriteLine("Введите имя файла с расширением:");

            while (!fileName.Contains("."))
            {
                fileName = Console.ReadLine();
                if (!fileName.Contains("."))
                {
                    Console.WriteLine("\aНе содержит расширения файла!");
                }
            }

            Console.WriteLine("Введите любой текст:");
            fileData = Console.ReadLine();

            return new TextFile(fileName, fileData);
        }

        static void WorkWithTextFile()
        {
            Console.Clear();
            Console.WriteLine("Добро пожаловать в текстовый редактор имени Виталия Наливкина!");

            TextFile currentTextFile = InitTextFile();
            CareTaker careTaker = new CareTaker();
            careTaker.SaveState(currentTextFile);
            bool textEditorIsRunning = true;

            while (textEditorIsRunning)
            {
                int textEditorOperation = ChooseOperation("Выберите действие:", Operations.TextEditorOperations);

                switch (textEditorOperation)
                {
                    case 1:
                        string saveBinnaryPath = GetRightPath(isNecessary: false);
                        currentTextFile.SerializeToBinary(saveBinnaryPath);

                        Console.WriteLine($"Файл сохранен: {saveBinnaryPath}\\{currentTextFile.FileName}.dat");
                        break;
                    case 2:
                        string binnaryFilePath = GetRightPath(".dat");
                        currentTextFile = TextFile.DeserealizeFromBinary(binnaryFilePath);
                        break;
                    case 3:
                        string saveXmlPath = GetRightPath(isNecessary: false);
                        currentTextFile.SerializeToXml(saveXmlPath);

                        Console.WriteLine($"Файл сохранен: {saveXmlPath}\\{currentTextFile.FileName}.xml");
                        break;
                    case 4:
                        string xmlFilePath = GetRightPath(".xml");
                        currentTextFile = TextFile.DeserealizeFromXml(xmlFilePath);
                        break;
                    case 5:
                        Console.WriteLine("Введите любой текст:");
                        currentTextFile.Data = Console.ReadLine();
                        break;
                    case 6:
                        careTaker.SaveState(currentTextFile);
                        Console.WriteLine("Данные сохранены");
                        break;
                    case 7:
                        careTaker.RestoreState(currentTextFile);
                        Console.WriteLine("Данные восстановлены");
                        break;
                    case 8:
                        Console.WriteLine($"--Имя файла: {currentTextFile.FileName};\n--Расширение: {currentTextFile.Extension};\n--Данные:\n{currentTextFile.Data}\n");
                        break;
                    case 0:
                        textEditorIsRunning = false;
                        break;
                }
                Console.WriteLine();
            }
        }

        static void Main(string[] args)
        {
            bool isRunning = true;
            while (isRunning)
            {
                int programOperation = ChooseOperation("Добро пожаловать в программу имени Боки и Жоки!", Operations.ProgramOperations);

                switch (programOperation)
                {
                    case 0:
                        isRunning = false;
                        break;
                    case 1:
                        WorkWithTextFile();
                        break;
                    case 2:
                        WorkWithDirectoryIndexation();
                        break;
                }
            }

        }

        private static void WorkWithDirectoryIndexation()
        {
            throw new NotImplementedException();
        }
    }
}
