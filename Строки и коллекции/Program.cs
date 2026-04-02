using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;

class Program
{
  static void Main()
  {
    Dictionary<string, string> errorWords = new Dictionary<string, string>()
        {
            { "привет-привет-пирвет", "привет" },
            { "пирвет", "привет" },
            { "преписи", "пунктуации" },
            { "репку", "репозиторий" },
            { "коммент", "комментарий" },
            { "пульт-реквест", "pull request" },
            { "гигабайсе", "GitHub" }
        };

    string directoryPath = @"C:\Users\Администратор\Desktop\Test";

    if (!Directory.Exists(directoryPath))
    {
      Console.WriteLine("Папка не найдена: " + directoryPath);
      Console.WriteLine("Создайте папку и положите туда текстовые файлы");
      return;
    }

    string[] files;
    files = Directory.GetFiles(directoryPath, "*.txt");

    if (files.Length == 0)
    {
      Console.WriteLine("Нет текстовых файлов в папке");
      return;
    }

    string content;

    foreach (string filePath in files)
    {
      Console.WriteLine("\nОбработка: " + Path.GetFileName(filePath));
      content = File.ReadAllText(filePath);

      foreach (KeyValuePair<string, string> pair in errorWords)
      {
        if (content.Contains(pair.Key))
        {
          content = content.Replace(pair.Key, pair.Value);
          Console.WriteLine("  Исправлено: " + pair.Key + " -> " + pair.Value);
        }
      }

      string replacement;
      string pattern;
      pattern = @"\((\d{3})\)\s*(\d{3})-(\d{2})-(\d{2})";
      replacement = "+380 $1 $2 $3 $4";

      if (Regex.IsMatch(content, pattern))
      {
        content = Regex.Replace(content, pattern, replacement);
        Console.WriteLine("  Найдены и исправлены номера телефонов");
      }

      File.WriteAllText(filePath, content);
      Console.WriteLine("  Сохранено");
    }

    Console.WriteLine("\nГотово! Все файлы обработаны.");
    Console.ReadKey();
  }
}