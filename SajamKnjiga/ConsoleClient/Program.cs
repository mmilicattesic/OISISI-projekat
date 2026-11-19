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
PosetilacDao posetilacDao = new PosetilacDao(adresaDao);
KupovinaDao kupovinaDao = new KupovinaDao(posetilacDao, knjigaDao);
IzdavacDao izdavacDao = new IzdavacDao(autorDao);


AutorService autorService = new AutorService(autorDao, adresaDao, knjigaDao);
PosetilacService posetilacService = new PosetilacService(posetilacDao, adresaDao, kupovinaDao); 
KupovinaService kupovinaService = new KupovinaService(kupovinaDao, posetilacDao, knjigaDao);   
IzdavacService izdavacService = new IzdavacService(izdavacDao, autorDao);
AdresaService adresaService = new AdresaService(adresaDao);


AutorConsoleController autorController = new AutorConsoleController(autorService);
PosetilacConsoleController posetilacController = new PosetilacConsoleController(posetilacService); 
KupovinaConsoleController kupovinaController = new KupovinaConsoleController(kupovinaService);     
IzdavacConsoleController izdavacController = new IzdavacConsoleController(izdavacService);
AdresaConsoleController adresaController = new AdresaConsoleController(adresaService);

KnjigaService knjigaService = new KnjigaService(knjigaDao, autorDao);
KnjigaConsoleController knjigaController = new KnjigaConsoleController(knjigaService, autorService);


while (true)
{
    Console.WriteLine();
    Console.WriteLine("=== Book Fair ===");
    Console.WriteLine("1. Authors");
    Console.WriteLine("2. Visitors");  
    Console.WriteLine("3. Purchases"); 
    Console.WriteLine("4. Publishers");
    Console.WriteLine("6. Addresses");
    Console.WriteLine("0. Exit");
    Console.Write("Choice: ");

    string? choice = Console.ReadLine();
    if (choice == "0")
        break;

    switch (choice)
    {
        case "1": autorController.Run(); break;
        case "2": posetilacController.Run(); break; 
        case "3": kupovinaController.Run(); break;  
        case "4": izdavacController.Run(); break;
        case "5": knjigaController.Run(); break;
        case "6": adresaController.Run(); break;
        default: Console.WriteLine("Unknown option."); break;
    }
}