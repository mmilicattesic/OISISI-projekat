using Core.Interfaces;
using Core.Models;

namespace Core.Services
{
    public class KupovinaService
    {
        private readonly IKupovinaDao _kupovinaDao;
        private readonly IPosetilacDao _posetilacDao;
        private readonly IKnjigaDao _knjigaDao;

        public KupovinaService(IKupovinaDao kupovinaDao, IPosetilacDao posetilacDao, IKnjigaDao knjigaDao)
        {
            _kupovinaDao = kupovinaDao;
            _posetilacDao = posetilacDao;
            _knjigaDao = knjigaDao;
        }

        public List<Kupovina> GetAll() => _kupovinaDao.GetAll();

        public Kupovina? GetById(int id) => _kupovinaDao.GetById(id);

        public List<Posetilac> GetAllVisitors() => _posetilacDao.GetAll();

        public List<Knjiga> GetAllBooks() => _knjigaDao.GetAll();

        public bool Add(Kupovina kupovina)
        {
            if (_posetilacDao.GetById(kupovina.PosetilacId) == null || _knjigaDao.GetById(kupovina.KnjigaId) == null)
                return false;

            _kupovinaDao.Add(kupovina);
            RecalculateAverageRating(kupovina.PosetilacId);
            return true;
        }

        public bool Update(Kupovina kupovina)
        {
            Kupovina? old = _kupovinaDao.GetById(kupovina.Id);
            if (old == null)
                return false;

            if (_posetilacDao.GetById(kupovina.PosetilacId) == null || _knjigaDao.GetById(kupovina.KnjigaId) == null)
                return false;

            int oldPosetilacId = old.PosetilacId;
            bool updated = _kupovinaDao.Update(kupovina);
            if (updated)
            {
                RecalculateAverageRating(oldPosetilacId);
                if (oldPosetilacId != kupovina.PosetilacId)
                    RecalculateAverageRating(kupovina.PosetilacId);
            }
            return updated;
        }

        public bool Delete(int id)
        {
            Kupovina? kupovina = _kupovinaDao.GetById(id);
            if (kupovina == null)
                return false;

            int posetilacId = kupovina.PosetilacId;
            bool deleted = _kupovinaDao.Delete(id);
            if (deleted)
            {
                RecalculateAverageRating(posetilacId);
            }
            return deleted;
        }

        private void RecalculateAverageRating(int posetilacId)
        {
            Posetilac? posetilac = _posetilacDao.GetById(posetilacId);
            if (posetilac == null)
                return;

            List<Kupovina> njegoveKupovine = _kupovinaDao.GetAll()
                .Where(k => k.PosetilacId == posetilacId)
                .ToList();

            posetilac.ProsecnaOcenaRecenzija = njegoveKupovine.Count == 0
                ? 0.0
                : njegoveKupovine.Average(k => k.Ocena);

            _posetilacDao.Update(posetilac);
        }
    }
}