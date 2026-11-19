using Core.Interfaces;
using Core.Models;
using Core.Storage;

namespace Core.DAO
{
    // Based on CRUDExample (VehiclesDAO, course exercise materials).
    public class AutorDao : IAutorDao
    {
        private readonly FileStorage<Autor> _storage = new FileStorage<Autor>("autori.csv");
        private readonly List<Autor> _autori;
        private readonly IAdresaDao _adresaDao;

        public AutorDao(IAdresaDao adresaDao)
        {
            _adresaDao = adresaDao;
            _autori = _storage.Load();

            // The file only holds AdresaId, so connect each author with the real Adresa object
            foreach (Autor autor in _autori)
            {
                autor.Adresa = _adresaDao.GetById(autor.AdresaId);
            }
        }

        public List<Autor> GetAll()
        {
            return _autori;
        }

        public Autor? GetById(int id)
        {
            return _autori.FirstOrDefault(a => a.Id == id);
        }

        public void Add(Autor autor)
        {
            // New Id = biggest existing Id + 1
            autor.Id = _autori.Count == 0 ? 1 : _autori.Max(a => a.Id) + 1;
            autor.Adresa = _adresaDao.GetById(autor.AdresaId);
            _autori.Add(autor);
            _storage.Save(_autori);
        }

        public bool Update(Autor autor)
        {
            Autor? old = GetById(autor.Id);
            if (old == null)
                return false;

            // Copy the fields into the existing object so that
            // other objects that point to it stay valid
            old.Ime = autor.Ime;
            old.Prezime = autor.Prezime;
            old.DatumRodjenja = autor.DatumRodjenja;
            old.AdresaId = autor.AdresaId;
            old.Adresa = _adresaDao.GetById(autor.AdresaId);
            old.Telefon = autor.Telefon;
            old.Email = autor.Email;
            old.BrojLicneKarte = autor.BrojLicneKarte;
            old.GodineIskustva = autor.GodineIskustva;

            _storage.Save(_autori);
            return true;
        }

        public bool Delete(int id)
        {
            Autor? autor = GetById(id);
            if (autor == null)
                return false;

            _autori.Remove(autor);
            _storage.Save(_autori);
            return true;
        }
    }
}