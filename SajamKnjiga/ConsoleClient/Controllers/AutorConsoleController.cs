using Core.Models;
using Core.Services;

namespace ConsoleClient.Controllers
{
    // Console menu for authors: show, add, edit, delete.
    public class AutorConsoleController
    {
        private readonly AutorService _service;

        public AutorConsoleController(AutorService service)
        {
            _service = service;
        }

        public void Run()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("=== Authors ===");
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

        private bool ShowAll()
        {
            List<Autor> autori = _service.GetAll();
            if (autori.Count == 0)
            {
                Console.WriteLine("There are no authors.");
                return false;
            }

            foreach (Autor a in autori)
            {
                Console.WriteLine(a);
                Console.WriteLine($"     Address: {a.Adresa}");
                Console.WriteLine($"     Books: {a.Knjige.Count}");
            }
            return true;
        }

        private void Add()
        {
            Console.WriteLine("--- New author ---");
            Autor autor = ReadAuthor(null);
            Adresa adresa = ReadAddress(null);

            _service.Add(autor, adresa);
            Console.WriteLine("Author added.");
        }

        private void Edit()
        {
            Autor? old = AskForAuthor();
            if (old == null)
                return;

            Console.WriteLine("Press Enter to keep the current value.");
            Autor changed = ReadAuthor(old);
            changed.Id = old.Id;
            changed.AdresaId = old.AdresaId;
            Adresa adresa = ReadAddress(old.Adresa);

            Console.WriteLine(_service.Update(changed, adresa) ? "Author updated." : "Update failed.");
        }

        private void Delete()
        {
            Autor? autor = AskForAuthor();
            if (autor == null)
                return;

            if (!ConsoleInput.Confirm($"Delete {autor.Ime} {autor.Prezime}?"))
            {
                Console.WriteLine("Cancelled.");
                return;
            }

            Console.WriteLine(_service.Delete(autor.Id) ? "Author deleted." : "Delete failed.");
        }

        // Shows all authors and asks for the Id of one of them
        private Autor? AskForAuthor()
        {
            if (!ShowAll())
                return null;

            int id = ConsoleInput.ReadInt("Author Id", 1, int.MaxValue);
            Autor? autor = _service.GetById(id);
            if (autor == null)
                Console.WriteLine("There is no author with that Id.");
            return autor;
        }

        private static Autor ReadAuthor(Autor? current)
        {
            Autor autor = new Autor();
            autor.Ime = ConsoleInput.ReadText("First name", current?.Ime);
            autor.Prezime = ConsoleInput.ReadText("Last name", current?.Prezime);
            autor.DatumRodjenja = ConsoleInput.ReadDate("Date of birth", current?.DatumRodjenja);
            autor.Telefon = ConsoleInput.ReadPhone("Phone", current?.Telefon);
            autor.Email = ConsoleInput.ReadEmail("Email", current?.Email);
            autor.BrojLicneKarte = ConsoleInput.ReadText("ID card number", current?.BrojLicneKarte);
            autor.GodineIskustva = ConsoleInput.ReadInt("Years of experience", 0, 80, current?.GodineIskustva);
            return autor;
        }

        private static Adresa ReadAddress(Adresa? current)
        {
            Adresa adresa = new Adresa();
            adresa.Ulica = ConsoleInput.ReadText("Street", current?.Ulica);
            adresa.Broj = ConsoleInput.ReadText("Number", current?.Broj);
            adresa.Grad = ConsoleInput.ReadText("City", current?.Grad);
            adresa.Drzava = ConsoleInput.ReadText("Country", current?.Drzava);
            return adresa;
        }
    }
}