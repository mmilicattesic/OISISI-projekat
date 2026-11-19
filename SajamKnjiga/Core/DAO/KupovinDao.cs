using Core.Interfaces;
using Core.Models;
using Core.Storage;

namespace Core.DAO
{
    public class KupovinaDao : IKupovinaDao
    {
        private readonly FileStorage<Kupovina> _storage = new FileStorage<Kupovina>("kupovine.csv");
        private readonly List<Kupovina> _kupovine;
        private readonly IPosetilacDao _posetilacDao;
        private readonly IKnjigaDao _knjigaDao;

        public KupovinaDao(IPosetilacDao posetilacDao, IKnjigaDao knjigaDao)
        {
            _posetilacDao = posetilacDao;
            _knjigaDao = knjigaDao;
            _kupovine = _storage.Load();

            foreach (Kupovina kupovina in _kupovine)
            {
                kupovina.Posetilac = _posetilacDao.GetById(kupovina.PosetilacId);
                kupovina.Knjiga = _knjigaDao.GetById(kupovina.KnjigaId);
                kupovina.Posetilac?.KupljeneKnjige.Add(kupovina);
            }
        }

        public List<Kupovina> GetAll() => _kupovine;

        public Kupovina? GetById(int id) => _kupovine.FirstOrDefault(k => k.Id == id);

        public void Add(Kupovina kupovina)
        {
            kupovina.Id = _kupovine.Count == 0 ? 1 : _kupovine.Max(k => k.Id) + 1;
            kupovina.Posetilac = _posetilacDao.GetById(kupovina.PosetilacId);
            kupovina.Knjiga = _knjigaDao.GetById(kupovina.KnjigaId);
            _kupovine.Add(kupovina);
            kupovina.Posetilac?.KupljeneKnjige.Add(kupovina);
            _storage.Save(_kupovine);
        }

        public bool Update(Kupovina kupovina)
        {
            Kupovina? old = GetById(kupovina.Id);
            if (old == null)
                return false;

            old.Posetilac?.KupljeneKnjige.Remove(old);

            old.PosetilacId = kupovina.PosetilacId;
            old.KnjigaId = kupovina.KnjigaId;
            old.Posetilac = _posetilacDao.GetById(kupovina.PosetilacId);
            old.Knjiga = _knjigaDao.GetById(kupovina.KnjigaId);
            old.DatumKupovine = kupovina.DatumKupovine;
            old.Ocena = kupovina.Ocena;
            old.Komentar = kupovina.Komentar;

            old.Posetilac?.KupljeneKnjige.Add(old);
            _storage.Save(_kupovine);
            return true;
        }
       
        public bool Delete(int id)
        {
            Kupovina? kupovina = GetById(id);
            if (kupovina == null)
                return false;

            kupovina.Posetilac?.KupljeneKnjige.Remove(kupovina);
            _kupovine.Remove(kupovina);
            _storage.Save(_kupovine);
            return true;
        }
    }
}








