using Core.Models;
using Core.Services;

namespace ConsoleClient.Controllers
{
    public class KupovinaConsoleController
    {
        private readonly KupovinaService _service;

        public KupovinaConsoleController(KupovinaService service)
        {
            _service = service;
        }

        public void Run()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("=== Purchases ===");
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
            List<Kupovina> kupovine = _service.GetAll();
            if (kupovine.Count == 0)
            {
                Console.WriteLine("There are no purchases.");
                return false;
            }

            foreach (Kupovina k in kupovine)
            {
                Console.WriteLine(k);
                string visitorName = k.Posetilac != null ? $"{k.Posetilac.Ime} {k.Posetilac.Prezime}" : "Unknown";
                string bookTitle = k.Knjiga != null ? k.Knjiga.Naziv : "Unknown";
                Console.WriteLine($"     Visitor: {visitorName} | Book: {bookTitle}");
            }
            return true;
        }

        private void Add()
        {
            List<Posetilac> visitors = _service.GetAllVisitors();
            List<Knjiga> books = _service.GetAllBooks();

            if (visitors.Count == 0 || books.Count == 0)
            {
                Console.WriteLine("You need at least one visitor and one book before adding a purchase.");
                return;
            }

            Console.WriteLine("--- Available visitors ---");
            foreach (Posetilac v in visitors)
                Console.WriteLine($"  [{v.Id}] {v.Ime} {v.Prezime} ({v.BrojClanskeKarte})");

            Console.WriteLine("--- Available books ---");
            foreach (Knjiga b in books)
                Console.WriteLine($"  [{b.Id}] {b.Naziv}");

            Kupovina kupovina = ReadPurchase(null);
            Console.WriteLine(_service.Add(kupovina) ? "Purchase added." : "Invalid Visitor Id or Book Id.");
        }

        private void Edit()
        {
            Kupovina? old = AskForPurchase();
            if (old == null)
                return;

            Console.WriteLine("Press Enter to keep the current value.");
            Kupovina changed = ReadPurchase(old);
            changed.Id = old.Id;

            Console.WriteLine(_service.Update(changed) ? "Purchase updated." : "Update failed (check Visitor/Book Id).");
        }

        private void Delete()
        {
            Kupovina? kupovina = AskForPurchase();
            if (kupovina == null)
                return;

            if (!ConsoleInput.Confirm($"Delete purchase #{kupovina.Id}?"))
            {
                Console.WriteLine("Cancelled.");
                return;
            }

            Console.WriteLine(_service.Delete(kupovina.Id) ? "Purchase deleted." : "Delete failed.");
        }

        private Kupovina? AskForPurchase()
        {
            if (!ShowAll())
                return null;

            int id = ConsoleInput.ReadInt("Purchase Id", 1, int.MaxValue);
            Kupovina? kupovina = _service.GetById(id);
            if (kupovina == null)
                Console.WriteLine("There is no purchase with that Id.");
            return kupovina;
        }

        private static Kupovina ReadPurchase(Kupovina? current)
        {
            Kupovina k = new Kupovina();
            k.PosetilacId = ConsoleInput.ReadInt("Visitor Id", 1, int.MaxValue, current?.PosetilacId);
            k.KnjigaId = ConsoleInput.ReadInt("Book Id", 1, int.MaxValue, current?.KnjigaId);
            k.DatumKupovine = ConsoleInput.ReadDate("Purchase date", current?.DatumKupovine);
            k.Ocena = ConsoleInput.ReadInt("Rating (1-5)", 1, 5, current?.Ocena);
            k.Komentar = ConsoleInput.ReadText("Comment", current?.Komentar);
            return k;
        }
    }
}
