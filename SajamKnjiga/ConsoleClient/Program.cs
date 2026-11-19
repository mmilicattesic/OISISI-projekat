using System.Text;
using ConsoleClient.Controllers;
using Core.DAO;
using Core.Services;

// So that the letters č, ć, š, ž, đ are shown and typed correctly
Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

AdresaDao adresaDao = new AdresaDao();
AutorDao autorDao = new AutorDao(adresaDao);
KnjigaDao knjigaDao = new KnjigaDao(autorDao);

AutorService autorService = new AutorService(autorDao, adresaDao, knjigaDao);
AutorConsoleController autorController = new AutorConsoleController(autorService);

while (true)
{
    Console.WriteLine();
    Console.WriteLine("=== Book Fair ===");
    Console.WriteLine("1. Authors");
    Console.WriteLine("0. Exit");
    Console.Write("Choice: ");

    string? choice = Console.ReadLine();
    if (choice == "0")
        break;

    switch (choice)
    {
        case "1": autorController.Run(); break;
        default: Console.WriteLine("Unknown option."); break;
    }
}