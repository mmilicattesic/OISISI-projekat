using Core.Models;
using Core.Services;

namespace ConsoleClient.Controllers
{
    public class IzdavacConsoleController
    {
        private readonly IzdavacService _service;

        public IzdavacConsoleController(IzdavacService service)
        {
            _service = service;
        }

        public void Run()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("=== Publishers ===");
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
            List<Izdavac> izdavaci = _service.GetAll();
            if (izdavaci.Count == 0)
            {
                Console.WriteLine("There are no publishers.");
                return false;
            }

            foreach (Izdavac i in izdavaci)
            {
                Console.WriteLine(i);
            }
            return true;
        }

        private void Add()
        {
            Console.WriteLine("--- New publisher ---");
            Izdavac izdavac = ReadPublisher(null);
            Console.WriteLine(_service.Add(izdavac) ? "Publisher added." : "Invalid Head Author Id.");
        }

        private void Edit()
        {
            Izdavac? old = AskForPublisher();
            if (old == null)
                return;

            Console.WriteLine("Press Enter to keep the current value.");
            Izdavac changed = ReadPublisher(old);
            changed.Id = old.Id;

            Console.WriteLine(_service.Update(changed) ? "Publisher updated." : "Update failed.");
        }

        private void Delete()
        {
            Izdavac? izdavac = AskForPublisher();
            if (izdavac == null)
                return;

            if (!ConsoleInput.Confirm($"Delete publisher {izdavac.Naziv}?"))
            {
                Console.WriteLine("Cancelled.");
                return;
            }

            Console.WriteLine(_service.Delete(izdavac.Id) ? "Publisher deleted." : "Delete failed.");
        }

        private Izdavac? AskForPublisher()
        {
            if (!ShowAll())
                return null;

            int id = ConsoleInput.ReadInt("Publisher Id", 1, int.MaxValue);
            Izdavac? izdavac = _service.GetById(id);
            if (izdavac == null)
                Console.WriteLine("There is no publisher with that Id.");
            return izdavac;
        }

        private Izdavac ReadPublisher(Izdavac? current)
        {
            Izdavac i = new Izdavac();
            i.Sifra = ConsoleInput.ReadText("Code", current?.Sifra);
            i.Naziv = ConsoleInput.ReadText("Name", current?.Naziv);

            List<Autor> autori = _service.GetAllAuthors();
            if (autori.Count > 0)
            {
                Console.WriteLine("Available authors (enter 0 for none):");
                foreach (Autor a in autori)
                    Console.WriteLine($"  [{a.Id}] {a.Ime} {a.Prezime}");

                int defaultId = current?.SefIzdavacaId ?? 0;
                int sefId = ConsoleInput.ReadInt("Head Author Id (0 for none)", 0, int.MaxValue, current != null ? defaultId : null);
                i.SefIzdavacaId = sefId == 0 ? null : sefId;
            }
            else
            {
                Console.WriteLine("No authors in the system yet — Head Author set to none.");
                i.SefIzdavacaId = null;
            }

            return i;
        }
    }
}