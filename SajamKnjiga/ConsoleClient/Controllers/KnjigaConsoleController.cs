using Core.Models;
using Core.Services;

namespace ConsoleClient.Controllers
{
    // Console menu for books: show, add, edit, delete.
    public class KnjigaConsoleController
    {
        private readonly KnjigaService _service;
        private readonly AutorService _autorService;

        public KnjigaConsoleController(KnjigaService service, AutorService autorService)
        {
            _service = service;
            _autorService = autorService;
        }

        public void Run()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("=== Books ===");
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
            List<Knjiga> knjige = _service.GetAll();
            if (knjige.Count == 0)
            {
                Console.WriteLine("There are no books.");
                return false;
            }

            foreach (Knjiga k in knjige)
            {
                Console.WriteLine(k);
                string authors = string.Join(", ", k.Autori.Select(a => $"{a.Ime} {a.Prezime}"));
                Console.WriteLine($"     Authors: {authors}");
            }
            return true;
        }

        private void Add()
        {
            Console.WriteLine("--- New book ---");
            List<int>? authorIds = ReadAuthorIds(null);
            if (authorIds == null)
                return;

            Knjiga knjiga = ReadBook(null);
            knjiga.AutorIds = authorIds;

            string? error = _service.Add(knjiga);
            Console.WriteLine(error ?? "Book added.");
        }

        private void Edit()
        {
            Knjiga? old = AskForBook();
            if (old == null)
                return;

            Console.WriteLine("Press Enter to keep the current value.");
            Knjiga changed = ReadBook(old);
            changed.Id = old.Id;
            changed.IzdavacId = old.IzdavacId;

            List<int>? authorIds = ReadAuthorIds(old.AutorIds);
            if (authorIds == null)
                return;
            changed.AutorIds = authorIds;

            string? error = _service.Update(changed);
            Console.WriteLine(error ?? "Book updated.");
        }

        private void Delete()
        {
            Knjiga? knjiga = AskForBook();
            if (knjiga == null)
                return;

            if (!ConsoleInput.Confirm($"Delete \"{knjiga.Naziv}\"?"))
            {
                Console.WriteLine("Cancelled.");
                return;
            }

            Console.WriteLine(_service.Delete(knjiga.Id) ? "Book deleted." : "Delete failed.");
        }

        // Shows all books and asks for the Id of one of them
        private Knjiga? AskForBook()
        {
            if (!ShowAll())
                return null;

            int id = ConsoleInput.ReadInt("Book Id", 1, int.MaxValue);
            Knjiga? knjiga = _service.GetById(id);
            if (knjiga == null)
                Console.WriteLine("There is no book with that Id.");
            return knjiga;
        }

        private static Knjiga ReadBook(Knjiga? current)
        {
            Knjiga knjiga = new Knjiga();
            knjiga.Isbn = ConsoleInput.ReadText("ISBN", current?.Isbn);
            knjiga.Naziv = ConsoleInput.ReadText("Title", current?.Naziv);
            knjiga.Zanr = ConsoleInput.ReadText("Genre", current?.Zanr);
            knjiga.GodinaIzdanja = ConsoleInput.ReadInt("Year of publication", 1000, DateTime.Now.Year, current?.GodinaIzdanja);
            knjiga.Cena = ConsoleInput.ReadDecimal("Price", 0, 1000000, current?.Cena);
            knjiga.BrojStrana = ConsoleInput.ReadInt("Number of pages", 1, 10000, current?.BrojStrana);
            return knjiga;
        }

        // Shows the authors and asks for their Ids, for example: 1,3
        // Returns null if there are no authors yet.
        private List<int>? ReadAuthorIds(List<int>? current)
        {
            List<Autor> authors = _autorService.GetAll();
            if (authors.Count == 0)
            {
                Console.WriteLine("There are no authors. Add an author first.");
                return null;
            }

            Console.WriteLine("Available authors:");
            foreach (Autor a in authors)
                Console.WriteLine($"  [{a.Id}] {a.Ime} {a.Prezime}");

            while (true)
            {
                string shown = current == null ? "" : $" [{string.Join(",", current)}]";
                Console.Write($"Author Ids separated by commas (example: 1,3){shown}: ");
                string input = Console.ReadLine() ?? "";

                if (string.IsNullOrWhiteSpace(input) && current != null)
                    return new List<int>(current);

                string[] parts = input.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                List<int> ids = new List<int>();
                bool ok = parts.Length > 0;

                foreach (string part in parts)
                {
                    if (int.TryParse(part, out int id) && authors.Any(a => a.Id == id))
                    {
                        if (!ids.Contains(id))
                            ids.Add(id);
                    }
                    else
                    {
                        ok = false;
                    }
                }

                if (ok)
                    return ids;

                Console.WriteLine("Enter Ids of existing authors, separated by commas.");
            }
        }
    }
}