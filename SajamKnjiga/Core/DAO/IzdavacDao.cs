using Core.Interfaces;
using Core.Models;
using Core.Storage;

namespace Core.DAO
{
    public class IzdavacDao : IIzdavacDao
    {
        private readonly FileStorage<Izdavac> _storage = new FileStorage<Izdavac>("izdavaci.csv");
        private readonly List<Izdavac> _izdavaci;
        private readonly IAutorDao _autorDao;

        public IzdavacDao(IAutorDao autorDao)
        {
            _autorDao = autorDao;
            _izdavaci = _storage.Load();

            foreach (Izdavac izdavac in _izdavaci)
            {
                if (izdavac.SefIzdavacaId.HasValue)
                {
                    izdavac.SefIzdavaca = _autorDao.GetById(izdavac.SefIzdavacaId.Value);
                }
            }
        }

        public List<Izdavac> GetAll() => _izdavaci;

        public Izdavac? GetById(int id) => _izdavaci.FirstOrDefault(i => i.Id == id);

        public void Add(Izdavac izdavac)
        {
            izdavac.Id = _izdavaci.Count == 0 ? 1 : _izdavaci.Max(i => i.Id) + 1;
            izdavac.SefIzdavaca = izdavac.SefIzdavacaId.HasValue ? _autorDao.GetById(izdavac.SefIzdavacaId.Value) : null;
            _izdavaci.Add(izdavac);
            _storage.Save(_izdavaci);
        }

        public bool Update(Izdavac izdavac)
        {
            Izdavac? old = GetById(izdavac.Id);
            if (old == null)
                return false;

            old.Sifra = izdavac.Sifra;
            old.Naziv = izdavac.Naziv;
            old.SefIzdavacaId = izdavac.SefIzdavacaId;
            old.SefIzdavaca = izdavac.SefIzdavacaId.HasValue ? _autorDao.GetById(izdavac.SefIzdavacaId.Value) : null;

            _storage.Save(_izdavaci);
            return true;
        }

        public bool Delete(int id)
        {
            Izdavac? izdavac = GetById(id);
            if (izdavac == null)
                return false;

            _izdavaci.Remove(izdavac);
            _storage.Save(_izdavaci);
            return true;
        }
    }
}


