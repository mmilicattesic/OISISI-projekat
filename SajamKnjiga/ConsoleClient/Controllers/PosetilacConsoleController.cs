using Core.Models;
using Core.Services;

namespace ConsoleClient.Controllers
{
    public class PosetilacConsoleController
    {
        private readonly PosetilacService _service;

        public PosetilacConsoleController(PosetilacService service)
        {
            _service = service;
        }

        public void Run()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("=== Visitors ===");
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
            List<Posetilac> posetioci = _service.GetAll();
            if (posetioci.Count == 0)
            {
                Console.WriteLine("There are no visitors.");
                return false;
            }

            foreach (Posetilac p in posetioci)
            {
                Console.WriteLine(p);
                Console.WriteLine($"     Purchased books: {p.KupljeneKnjige.Count}");
            }
            return true;
        }

        private void Add()
        {
            Console.WriteLine("--- New visitor ---");
            Posetilac posetilac = ReadVisitor(null);
            Adresa adresa = ReadAddress(null);

            _service.Add(posetilac, adresa);
            Console.WriteLine("Visitor added.");
        }

        private void Edit()
        {
            Posetilac? old = AskForVisitor();
            if (old == null)
                return;

            Console.WriteLine("Press Enter to keep the current value.");
            Posetilac changed = ReadVisitor(old);
            changed.Id = old.Id;
            changed.AdresaId = old.AdresaId;
            changed.ProsecnaOcenaRecenzija = old.ProsecnaOcenaRecenzija;
            Adresa adresa = ReadAddress(old.Adresa);

            Console.WriteLine(_service.Update(changed, adresa) ? "Visitor updated." : "Update failed.");
        }

        private void Delete()
        {
            Posetilac? posetilac = AskForVisitor();
            if (posetilac == null)
                return;

            if (!ConsoleInput.Confirm($"Delete {posetilac.Ime} {posetilac.Prezime}?"))
            {
                Console.WriteLine("Cancelled.");
                return;
            }

            Console.WriteLine(_service.Delete(posetilac.Id) ? "Visitor deleted." : "Delete failed.");
        }

        private Posetilac? AskForVisitor()
        {
            if (!ShowAll())
                return null;

            int id = ConsoleInput.ReadInt("Visitor Id", 1, int.MaxValue);
            Posetilac? posetilac = _service.GetById(id);
            if (posetilac == null)
                Console.WriteLine("There is no visitor with that Id.");
            return posetilac;
        }

        private static Posetilac ReadVisitor(Posetilac? current)
        {
            Posetilac p = new Posetilac();
            p.Ime = ConsoleInput.ReadText("First name", current?.Ime);
            p.Prezime = ConsoleInput.ReadText("Last name", current?.Prezime);
            p.DatumRodjenja = ConsoleInput.ReadDate("Date of birth", current?.DatumRodjenja);
            p.KontaktTelefon = ConsoleInput.ReadPhone("Phone", current?.KontaktTelefon);
            p.Email = ConsoleInput.ReadEmail("Email", current?.Email);
            p.BrojClanskeKarte = ConsoleInput.ReadText("Membership card number", current?.BrojClanskeKarte);
            p.TrenutnaGodinaClanstva = ConsoleInput.ReadInt("Current membership year", 1, 100, current?.TrenutnaGodinaClanstva);

            int currentStatus = current != null ? (int)current.Status : 0;
            int statusChoice = ConsoleInput.ReadInt("Status (0 = Redovan, 1 = VIP)", 0, 1, current != null ? currentStatus : null);
            p.Status = (StatusPosetioca)statusChoice;
            return p;
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
