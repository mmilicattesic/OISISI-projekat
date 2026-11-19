using Core.Interfaces;
using Core.Models;
using Core.Storage;

namespace Core.DAO
{
    public class PosetilacDao : IPosetilacDao
    {
        private readonly FileStorage<Posetilac> _storage = new FileStorage<Posetilac>("posetioci.csv");
        private readonly List<Posetilac> _posetioci;
        private readonly IAdresaDao _adresaDao;

        public PosetilacDao(IAdresaDao adresaDao)
        {
            _adresaDao = adresaDao;
            _posetioci = _storage.Load();

            foreach (Posetilac posetilac in _posetioci)
            {
                posetilac.Adresa = _adresaDao.GetById(posetilac.AdresaId);
            }
        }

        public List<Posetilac> GetAll() => _posetioci;

        public Posetilac? GetById(int id) => _posetioci.FirstOrDefault(p => p.Id == id);

        public void Add(Posetilac posetilac)
        {
            posetilac.Id = _posetioci.Count == 0 ? 1 : _posetioci.Max(p => p.Id) + 1;
            posetilac.Adresa = _adresaDao.GetById(posetilac.AdresaId);
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
            old.TrenutnaGodinaClanstva = posetilac.TrenutnaGodinaClanstva;
            old.Status = posetilac.Status;
            old.ProsecnaOcenaRecenzija = posetilac.ProsecnaOcenaRecenzija;

            _storage.Save(_posetioci);
            return true;
        }

       

        public bool Delete(int id)
        {
            Posetilac? posetilac = GetById(id);
            if (posetilac == null)
                return false;

            _posetioci.Remove(posetilac);
            _storage.Save(_posetioci);
            return true;
        }
    }
}
