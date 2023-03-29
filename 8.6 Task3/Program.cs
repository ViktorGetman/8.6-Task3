using Microsoft.VisualBasic.FileIO;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
class DirectoryCleener
{
    public static void Main()
    {
        Console.WriteLine("Укажите путь к папке");
        string folderPath = Console.ReadLine();
        double catalogSize = 0;
        double cleanCatalogSize = 0;
        catalogSize = sizeOfFolder(folderPath, ref catalogSize);
        if (catalogSize != 0)
        {
            Console.WriteLine("Исходный размер папки {0} составляет {1} байт", folderPath, catalogSize);
        }
        else
        {
            Console.WriteLine("Папка {0} пуста", folderPath);
        }
        Console.ReadLine();
        if (!Directory.Exists(folderPath))
        {
            Console.WriteLine("По указаному пути пака не существует");

        }
        else
        {
            DirectoryCleen(folderPath);
        }
        cleanCatalogSize = sizeOfFolder(folderPath, ref cleanCatalogSize);
        if (cleanCatalogSize != 0)
        {
            Console.WriteLine("Текущий размер папки {0} составляет {1} байт", folderPath, cleanCatalogSize);
        }
        else
        {
            Console.WriteLine("Папка {0} пуста", folderPath);
        }
        double released = catalogSize - cleanCatalogSize;
        Console.WriteLine("Освобождено {0} байт", released);

    }
    public static bool DirectoryCleen(string parentDirectory)
    {
        try
        {
            bool parentDirectoryEmpty = FilesCleen(parentDirectory);

            string[] dirs = Directory.GetDirectories(parentDirectory);
            for (int i = 0; i < dirs.Length; i++)
            {
                bool chaildDirectoryEmpty = DirectoryCleen(dirs[i]);

                var modTime = Directory.GetLastWriteTime(dirs[i]);


                if (DateTime.Now - modTime > TimeSpan.FromMinutes(30) && chaildDirectoryEmpty)
                {
                    try
                    {
                        Directory.Delete(dirs[i], true);
                    }
                    catch (Exception Ex)
                    {
                        parentDirectoryEmpty = false;
                        Console.WriteLine($"Невозможно удалить папку {dirs[i]}");
                    }
                }
                else
                {
                    parentDirectoryEmpty = false;
                }

            }
            return parentDirectoryEmpty;
        }
        catch
        {
            Console.WriteLine($"Невозможно очистить папку {parentDirectory}");
            return false;
        }

    }
    public static bool FilesCleen(string parentDirectory)
    {
        bool allFilesDelete = true;
        string[] files;
        files = Directory.GetFiles(parentDirectory);

        for (int i = 0; i < files.Length; i++)
        {

            var modTime = File.GetLastWriteTime(files[i]);


            if (DateTime.Now - modTime > TimeSpan.FromMinutes(30))
            {
                try
                {
                    File.Delete(files[i]);
                }
                catch (Exception Ex)
                {
                    Console.WriteLine($"Невозможно удалить файл {files[i]}");
                    allFilesDelete = false;
                }
            }
            else
            {
                allFilesDelete = false;
            }
        }
        return allFilesDelete;
    }
    static double sizeOfFolder(string folder, ref double catalogSize)
    {
        try
        {
            DirectoryInfo di = new DirectoryInfo(folder);
            DirectoryInfo[] diA = di.GetDirectories();
            FileInfo[] fi = di.GetFiles();
            foreach (FileInfo f in fi)
            {
                catalogSize = catalogSize + f.Length;
            }
            foreach (DirectoryInfo df in diA)
            {
                sizeOfFolder(df.FullName, ref catalogSize);
            }
            return Math.Round((double)(catalogSize));
        }
        catch (DirectoryNotFoundException ex)
        {
            Console.WriteLine("Папка не найдена. Ошибка: " + ex.Message);
            return 0;
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine("Отсутствует доступ. Ошибка: " + ex.Message);
            return 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Произошла ошибка. Обратитесь к администратору. Ошибка: " + ex.Message);
            return 0;
        }

    }
}
