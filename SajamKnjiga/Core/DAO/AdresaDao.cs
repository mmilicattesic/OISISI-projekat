using Core.Interfaces;
using Core.Models;
using Core.Storage;

namespace Core.DAO
{
    // Based on CRUDExample (VehiclesDAO, course exercise materials).
    // Changes: the DAO has an interface, the Id is the biggest existing Id + 1.
    // Data access for addresses: reads and writes them, no business logic.
  
    public class AdresaDao : IAdresaDao
    {
        private readonly FileStorage<Adresa> _storage = new FileStorage<Adresa>("adrese.csv");
        private readonly List<Adresa> _adrese;

        public AdresaDao()
        {
            _adrese = _storage.Load();
        }

        public List<Adresa> GetAll()
        {
            return _adrese;
        }

        public Adresa? GetById(int id)
        {
            return _adrese.FirstOrDefault(a => a.Id == id);
        }

        public void Add(Adresa adresa)
        {
            // New Id = biggest existing Id + 1
            adresa.Id = _adrese.Count == 0 ? 1 : _adrese.Max(a => a.Id) + 1;
            _adrese.Add(adresa);
            _storage.Save(_adrese);
        }

        public bool Update(Adresa adresa)
        {
            Adresa? old = GetById(adresa.Id);
            if (old == null)
                return false;

         
            old.Ulica = adresa.Ulica;
            old.Broj = adresa.Broj;
            old.Grad = adresa.Grad;
            old.Drzava = adresa.Drzava;

            _storage.Save(_adrese);
            return true;
        }

        public bool Delete(int id)
        {
            Adresa? adresa = GetById(id);
            if (adresa == null)
                return false;

            _adrese.Remove(adresa);
            _storage.Save(_adrese);
            return true;
        }
    }
}