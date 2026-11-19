
using Core.Models;
using Core.Services;

namespace ConsoleClient.Controllers
{
    public class KnjigaConsoleController
    {
        private readonly KnjigaService _service;
        private readonly AutorService _autorService;
        private readonly IzdavacService _izdavacService;

        public KnjigaConsoleController(
            KnjigaService service,
            AutorService autorService,
            IzdavacService izdavacService)
        {
            _service = service;
            _autorService = autorService;
            _izdavacService = izdavacService;
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
                    default:
                        Console.WriteLine("Unknown option.");
                        break;
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

            foreach (Knjiga knjiga in knjige)
            {
                Console.WriteLine(knjiga);

                string authors = string.Join(", ",
                    knjiga.Autori.Select(a =>
                        $"{a.Ime} {a.Prezime}"));

                Console.WriteLine($"     Authors: {authors}");

                string publisher = "None";

                if (knjiga.IzdavacId.HasValue)
                {
                    Izdavac? izdavac =
                        _izdavacService.GetById(
                            knjiga.IzdavacId.Value);

                    publisher = izdavac?.Naziv
                        ?? "Publisher not found";
                }

                Console.WriteLine($"     Publisher: {publisher}");
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
            knjiga.IzdavacId = ReadPublisherId(null);

            string? error = _service.Add(knjiga);

            Console.WriteLine(error ?? "Book added.");
        }

        private void Edit()
        {
            Knjiga? old = AskForBook();

            if (old == null)
                return;

            Console.WriteLine(
                "Press Enter to keep the current value.");

            Knjiga changed = ReadBook(old);
            changed.Id = old.Id;

            List<int>? authorIds =
                ReadAuthorIds(old.AutorIds);

            if (authorIds == null)
                return;

            changed.AutorIds = authorIds;
            changed.IzdavacId =
                ReadPublisherId(old.IzdavacId);

            string? error = _service.Update(changed);

            Console.WriteLine(error ?? "Book updated.");
        }

        private void Delete()
        {
            Knjiga? knjiga = AskForBook();

            if (knjiga == null)
                return;

            if (!ConsoleInput.Confirm(
                $"Delete \"{knjiga.Naziv}\"?"))
            {
                Console.WriteLine("Cancelled.");
                return;
            }

            Console.WriteLine(
                _service.Delete(knjiga.Id)
                    ? "Book deleted."
                    : "Delete failed.");
        }

        private Knjiga? AskForBook()
        {
            if (!ShowAll())
                return null;

            int id = ConsoleInput.ReadInt(
                "Book Id", 1, int.MaxValue);

            Knjiga? knjiga = _service.GetById(id);

            if (knjiga == null)
            {
                Console.WriteLine(
                    "There is no book with that Id.");
            }

            return knjiga;
        }

        private static Knjiga ReadBook(Knjiga? current)
        {
            Knjiga knjiga = new Knjiga();

            knjiga.Isbn = ConsoleInput.ReadText(
                "ISBN", current?.Isbn);

            knjiga.Naziv = ConsoleInput.ReadText(
                "Title", current?.Naziv);

            knjiga.Zanr = ConsoleInput.ReadText(
                "Genre", current?.Zanr);

            knjiga.GodinaIzdanja = ConsoleInput.ReadInt(
                "Year of publication",
                1000,
                DateTime.Now.Year,
                current?.GodinaIzdanja);

            knjiga.Cena = ConsoleInput.ReadDecimal(
                "Price", 0, 1000000, current?.Cena);

            knjiga.BrojStrana = ConsoleInput.ReadInt(
                "Number of pages",
                1,
                10000,
                current?.BrojStrana);

            return knjiga;
        }

        private List<int>? ReadAuthorIds(List<int>? current)
        {
            List<Autor> authors = _autorService.GetAll();

            if (authors.Count == 0)
            {
                Console.WriteLine(
                    "There are no authors. Add an author first.");
                return null;
            }

            Console.WriteLine("Available authors:");

            foreach (Autor autor in authors)
            {
                Console.WriteLine(
                    $"  [{autor.Id}] {autor.Ime} {autor.Prezime}");
            }

            while (true)
            {
                string shown = current == null
                    ? ""
                    : $" [{string.Join(",", current)}]";

                Console.Write(
                    $"Author Ids separated by commas{shown}: ");

                string input = Console.ReadLine() ?? "";

                if (string.IsNullOrWhiteSpace(input)
                    && current != null)
                {
                    return new List<int>(current);
                }

                string[] parts = input.Split(
                    ',',
                    StringSplitOptions.RemoveEmptyEntries |
                    StringSplitOptions.TrimEntries);

                List<int> ids = new List<int>();
                bool valid = parts.Length > 0;

                foreach (string part in parts)
                {
                    if (int.TryParse(part, out int id)
                        && authors.Any(a => a.Id == id))
                    {
                        if (!ids.Contains(id))
                            ids.Add(id);
                    }
                    else
                    {
                        valid = false;
                    }
                }

                if (valid)
                    return ids;

                Console.WriteLine(
                    "Enter Ids of existing authors, separated by commas.");
            }
        }

        private int? ReadPublisherId(int? current)
        {
            List<Izdavac> publishers =
                _izdavacService.GetAll();

            if (publishers.Count == 0)
            {
                Console.WriteLine(
                    "No publishers available. Publisher set to none.");
                return null;
            }

            Console.WriteLine("Available publishers:");

            foreach (Izdavac izdavac in publishers)
            {
                Console.WriteLine(
                    $"  [{izdavac.Id}] {izdavac.Naziv}");
            }

            Console.WriteLine("Enter 0 for no publisher.");

            while (true)
            {
                string shown = current.HasValue
                    ? $" [{current.Value}]"
                    : "";

                Console.Write($"Publisher Id{shown}: ");

                string input = Console.ReadLine() ?? "";

                if (string.IsNullOrWhiteSpace(input)
                    && current.HasValue)
                {
                    return current;
                }

                if (int.TryParse(input, out int id))
                {
                    if (id == 0)
                        return null;

                    if (_izdavacService.GetById(id) != null)
                        return id;
                }

                Console.WriteLine(
                    "Enter an existing publisher Id or 0.");
            }
        }
    }
}
