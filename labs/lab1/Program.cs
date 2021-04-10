using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.Json;

namespace lab1
{
    class Game
    {
        private string name;
        private float price;
        public Game(string name, float price)
        {
            this.name = name;
            this.price = price;
        }
        public Game() { }
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
            }
        }
        public float Price
        {
            get { return price; }
            set
            {
                price = value;
            }
        }
    }

    class Program
    {
        static void PrintMenu()
        {
            Console.WriteLine("1) Open file");
            Console.WriteLine("2) Save file");
            Console.WriteLine("3) Rename file");
            Console.WriteLine("4) Move file");
            Console.WriteLine("5) Delete file");
            Console.WriteLine("6) File history");
            Console.WriteLine("9) Exit");
        }

        static string GetFileName()
        {
            while (true)
            {
                try
                {
                    Console.Write("Filename: ");
                    string filename = Console.ReadLine();
                    string ext = Path.GetExtension(filename);
                    if (ext != ".json" && ext != ".gz" && ext != ".bin")
                    {
                        throw new Exception("Allowed extensions: .json, .bin, .gz");
                    }
                    return filename;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Wrong input: " + e.Message);
                }
            }
        }

        static string GetFilePath()
        {
            string fileName;
            string filePath;
            while (true)
            {
                try
                {
                    fileName = GetFileName();
                    filePath = SelectFile(fileName);
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return filePath;
        }

        static string ReadJson(string filePath) 
        {
            string data = File.ReadAllText(filePath);
            return data;
        }

        static void WriteJson(string path, string data)
        {
            File.WriteAllText(Path.Join(Directory.GetCurrentDirectory(), path), data);
        }
        
        static string ReadBin(string filePath, List<Game> games)
        {
            string data = "";
            using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
            {
                while (reader.PeekChar() > -1)
                {
                    string name = reader.ReadString();
                    data += name + " ";
                    float price = reader.ReadSingle();
                    data += price.ToString() + "\n";
                    games.Add(new Game(name, price));
                }
            }
            return data;
        }
        
        static void WriteBin(string filePath, List<Game> games)        
        {

            using (BinaryWriter writer = new BinaryWriter(File.Open(filePath, FileMode.OpenOrCreate)))
            {
                foreach(Game game in games)
                {
                    writer.Write(game.Name);
                    writer.Write(game.Price);
                }                
            }
        }

        static string ReadGzip(string filePath) 
        {
            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                using (GZipStream dcmpStream = new GZipStream(fs, CompressionMode.Decompress))
                {
                    StreamReader reader = new StreamReader(dcmpStream);
                    string resString = reader.ReadToEnd();
                    return resString;
                }
            }
        }

        static void WriteGzip(string jsonData, string filePath) 
        {
            using (FileStream source = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                using (GZipStream cmpStream = new GZipStream(source, CompressionMode.Compress))
                {
                    new MemoryStream(Encoding.ASCII.GetBytes(jsonData)).CopyTo(cmpStream);
                }
            }
        }

        static string SelectFile(string fileName)
        {
            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory(), fileName, SearchOption.AllDirectories);
            if (files.Length == 0)
            {
                throw new Exception("File not found");
            }
            string selectedFile;
            int option;
            if (files.Length == 1)
            {
                selectedFile = files[0];
            }
            else
            {
                for (int i = 0; i < files.Length; i++ )
                {
                    Console.WriteLine((i+1).ToString() + ")" + files[i]);
                }
                while (true)
                {
                    try
                    {
                        option = int.Parse(Console.ReadLine());
                        selectedFile = files[option - 1];
                        break;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }            
            return selectedFile;
        }

        static void RenameFile(string oldName)
        {
            string newName;
            string[] badChars = new string[] { "/", "\\", ":", "*", "?", "\"", "<", ">", "|" };
            while(true)
            {
                try
                {
                    Console.Write("New name: ");
                    newName = Console.ReadLine();
                    foreach (string ch in badChars)
                    {
                        if (newName.Contains(ch))
                        {
                            throw new Exception("Not allowed symbol " + ch);
                        }
                    }
                    if (newName.Length > 64)
                    { 
                        throw new Exception("Max length = 64");                      
                    }
                    File.Move(oldName, newName);
                    break;
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message); ;
                }
            }

        }

        static void MoveFile(string oldPath)
        {
            Console.Write("New path: ");
            var newPath = Console.ReadLine();            
            try
            {
                if (Path.GetExtension(newPath) != ".json" && Path.GetExtension(newPath) != ".gz" 
                    && Path.GetExtension(newPath) != ".bin")
                {
                    throw new Exception("Allowed extensions: .json, .bin, .gz");
                }
                new FileInfo(Path.Join(Directory.GetCurrentDirectory(), newPath)).Directory.Create();
                File.Move(oldPath, Path.Join(Directory.GetCurrentDirectory(), newPath));
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static void Main()
        {
            try
            {
                Directory.SetCurrentDirectory(Directory.CreateDirectory(
                    Directory.GetCurrentDirectory() + "/../../../" + "/files/").FullName);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }
            CultureInfo.CurrentCulture = new CultureInfo("en-US", false);
            List<Game> games = new List<Game>(); 
            
            while (true)
            {
                Console.Clear();
                PrintMenu();
                ConsoleKeyInfo key = Console.ReadKey(false);
                if (key.Key == ConsoleKey.D1 || key.Key == ConsoleKey.NumPad1) //open
                {
                    Console.Clear();
                    string filePath = GetFilePath();
                    string data = "";
                    if (Path.GetExtension(filePath) == ".json")
                    {
                        data = ReadJson(filePath);
                        games = JsonSerializer.Deserialize<List<Game>>(data);
                    }
                    else if (Path.GetExtension(filePath) == ".bin")
                    {
                        data = ReadBin(filePath, games);
                    }
                    else
                    {
                        data = ReadGzip(filePath);
                        games = JsonSerializer.Deserialize<List<Game>>(data);
                    }
                    Console.WriteLine("Data : \n" + data);
                    Console.ReadKey(false);
                }
                else if (key.Key == ConsoleKey.D2 || key.Key == ConsoleKey.NumPad2) //save
                {
                    Console.Clear();
                    string fileName = GetFileName();
                    if (!Directory.Exists(fileName)) Directory.CreateDirectory(fileName + "/../");
                    if (Path.GetExtension(fileName) == ".json")
                    {
                        string jsonData = JsonSerializer.Serialize(games);
                        WriteJson(fileName, jsonData);
                    }
                    else if (Path.GetExtension(fileName) == ".bin")
                    {
                        WriteBin(fileName, games);
                    }
                    else if (Path.GetExtension(fileName) == ".gz")
                    {
                        string jsonData = JsonSerializer.Serialize(games);
                        WriteGzip(jsonData, fileName);
                    }
                }
                else if (key.Key == ConsoleKey.D3 || key.Key == ConsoleKey.NumPad3) //rename
                {
                    Console.Clear();
                    string filePath = GetFilePath();
                    RenameFile(filePath);
                    Console.ReadKey(false);
                }
                else if (key.Key == ConsoleKey.D4 || key.Key == ConsoleKey.NumPad4) //move
                {
                    Console.Clear();

                    string filePath = GetFilePath();
                    MoveFile(filePath);
                }
                else if (key.Key == ConsoleKey.D5 || key.Key == ConsoleKey.NumPad5) //delete
                {
                    Console.Clear();
                    string filePath = GetFilePath();
                    File.Delete(filePath);
                }
                else if (key.Key == ConsoleKey.D6 || key.Key == ConsoleKey.NumPad6) //history
                {
                    Console.Clear();
                    var filepath = GetFilePath();
                    Console.Write("Last write: ");
                    Console.WriteLine(File.GetLastWriteTime(filepath));
                    Console.ReadKey();
                }
                else if (key.Key == ConsoleKey.D9 || key.Key == ConsoleKey.NumPad9) //exit
                {
                    return;
                }
            }
        }
    }
}