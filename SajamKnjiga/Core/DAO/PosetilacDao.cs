
using Core.Interfaces;
using Core.Models;
using Core.Storage;

namespace Core.DAO
{
    public class PosetilacDao : IPosetilacDao
    {
        private readonly FileStorage<Posetilac> _storage =
            new FileStorage<Posetilac>("posetioci.csv");

        private readonly List<Posetilac> _posetioci;
        private readonly IAdresaDao _adresaDao;
        private IKnjigaDao? _knjigaDao;

        public PosetilacDao(IAdresaDao adresaDao)
        {
            _adresaDao = adresaDao;
            _posetioci = _storage.Load();

            foreach (Posetilac posetilac in _posetioci)
            {
                posetilac.Adresa =
                    _adresaDao.GetById(posetilac.AdresaId);
            }
        }

        // Called after both PosetilacDao and KnjigaDao are created.
        public void ConnectWishLists(IKnjigaDao knjigaDao)
        {
            _knjigaDao = knjigaDao;

            foreach (Knjiga knjiga in _knjigaDao.GetAll())
            {
                knjiga.PosetiociSaListeZelja.Clear();
            }

            foreach (Posetilac posetilac in _posetioci)
            {
                LinkWishList(posetilac);
            }
        }

        public List<Posetilac> GetAll() => _posetioci;

        public Posetilac? GetById(int id) =>
            _posetioci.FirstOrDefault(p => p.Id == id);

        public void Add(Posetilac posetilac)
        {
            posetilac.Id = _posetioci.Count == 0
                ? 1
                : _posetioci.Max(p => p.Id) + 1;

            posetilac.Adresa =
                _adresaDao.GetById(posetilac.AdresaId);

            LinkWishList(posetilac);

            _posetioci.Add(posetilac);
            _storage.Save(_posetioci);
        }

        public bool Update(Posetilac posetilac)
        {
            Posetilac? old = GetById(posetilac.Id);

            if (old == null)
                return false;

            old.Ime = posetilac.Ime;
            old.Prezime = posetilac.Prezime;
            old.DatumRodjenja = posetilac.DatumRodjenja;
            old.AdresaId = posetilac.AdresaId;
            old.Adresa = _adresaDao.GetById(posetilac.AdresaId);
            old.KontaktTelefon = posetilac.KontaktTelefon;
            old.Email = posetilac.Email;
            old.BrojClanskeKarte = posetilac.BrojClanskeKarte;
            old.TrenutnaGodinaClanstva =
                posetilac.TrenutnaGodinaClanstva;
            old.Status = posetilac.Status;
            old.ProsecnaOcenaRecenzija =
                posetilac.ProsecnaOcenaRecenzija;

            // Keep the existing wish list when editing personal data.
            _storage.Save(_posetioci);
            return true;
        }

        public bool Delete(int id)
        {
            Posetilac? posetilac = GetById(id);

            if (posetilac == null)
                return false;

            foreach (Knjiga knjiga in posetilac.ListaZelja)
            {
                knjiga.PosetiociSaListeZelja.Remove(posetilac);
            }

            posetilac.ListaZelja.Clear();
            posetilac.ListaZeljaIds.Clear();

            _posetioci.Remove(posetilac);
            _storage.Save(_posetioci);
            return true;
        }

        // Saves all wish lists after they have been changed.
        public void SaveWishLists()
        {
            foreach (Posetilac posetilac in _posetioci)
            {
                posetilac.ListaZeljaIds = posetilac.ListaZelja
                    .Select(k => k.Id)
                    .Distinct()
                    .ToList();
            }

            _storage.Save(_posetioci);
        }

        private void LinkWishList(Posetilac posetilac)
        {
            posetilac.ListaZelja.Clear();

            if (_knjigaDao == null)
                return;

            foreach (int knjigaId in posetilac.ListaZeljaIds)
            {
                Knjiga? knjiga = _knjigaDao.GetById(knjigaId);

                if (knjiga == null)
                    continue;

                if (!posetilac.ListaZelja.Contains(knjiga))
                    posetilac.ListaZelja.Add(knjiga);

                if (!knjiga.PosetiociSaListeZelja.Contains(posetilac))
                    knjiga.PosetiociSaListeZelja.Add(posetilac);
            }
        }
    }
}
