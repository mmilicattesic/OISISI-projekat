
using Core.Models;
using Core.Services;

namespace ConsoleClient.Controllers
{
    public class AdresaConsoleController
    {
        private readonly AdresaService _service;

        public AdresaConsoleController(AdresaService service)
        {
            _service = service;
        }


        public void Run()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("=== Addresses ===");
                Console.WriteLine("1. Show all");
                Console.WriteLine("2. Add");
                Console.WriteLine("3. Edit");
                Console.WriteLine("4. Delete");
                Console.WriteLine("0. Back");
                Console.Write("Choice: ");

                switch (Console.ReadLine())
                {
                    case "1": ShowAll(); break;
                    case "2": Add(); break;
                    case "3": Edit(); break;
                    case "4": Delete(); break;
                    case "0": return;
                    default: Console.WriteLine("Unknown option."); break;
                }
            }
        }


        private void ShowAll()
        {
            var adrese = _service.GetAll();

            if (adrese.Count == 0)
            {
                Console.WriteLine("There are no addresses.");
                return;
            }

            foreach (var adresa in adrese)
            {
                Console.WriteLine(adresa);
            }
        }

        private void Add()
        {
            Console.WriteLine("--- New address ---");

            Adresa adresa = new Adresa();

            adresa.Ulica = ConsoleInput.ReadText("Street");
            adresa.Broj = ConsoleInput.ReadText("Number");
            adresa.Grad = ConsoleInput.ReadText("City");
            adresa.Drzava = ConsoleInput.ReadText("Country");

            _service.Add(adresa);

            Console.WriteLine("Address added.");
        }

        private void Edit()
        {
            Console.Write("Enter address ID: ");

            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            Adresa? adresa = _service.GetById(id);

            if (adresa == null)
            {
                Console.WriteLine("Address not found.");
                return;
            }

            Console.WriteLine("Enter new address information:");

            adresa.Ulica = ConsoleInput.ReadText("Street");
            adresa.Broj = ConsoleInput.ReadText("Number");
            adresa.Grad = ConsoleInput.ReadText("City");
            adresa.Drzava = ConsoleInput.ReadText("Country");

            _service.Update(adresa);

            Console.WriteLine("Address updated.");
        }

        private void Delete()
        {
            Console.Write("Enter address ID: ");

            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            Adresa? adresa = _service.GetById(id);

            if (adresa == null)
            {
                Console.WriteLine("Address not found.");
                return;
            }

            Console.Write("Are you sure you want to delete this address? (y/n): ");
            string? confirmation = Console.ReadLine();

            if (confirmation?.ToLower() != "y")
            {
                Console.WriteLine("Deletion cancelled.");
                return;
            }

            if (_service.Delete(id))
                Console.WriteLine("Address deleted.");
            else
                Console.WriteLine("Address could not be deleted (it is used by an author or a visitor).");
        }



    }
}
