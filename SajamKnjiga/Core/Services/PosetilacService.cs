using Core.Interfaces;
using Core.Models;

namespace Core.Services
{
    public class PosetilacService
    {
        private readonly IPosetilacDao _posetilacDao;
        private readonly IAdresaDao _adresaDao;
        private readonly IKupovinaDao _kupovinaDao;

        public PosetilacService(IPosetilacDao posetilacDao, IAdresaDao adresaDao, IKupovinaDao kupovinaDao)
        {
            _posetilacDao = posetilacDao;
            _adresaDao = adresaDao;
            _kupovinaDao = kupovinaDao;
        }

        public List<Posetilac> GetAll() => _posetilacDao.GetAll();

        public Posetilac? GetById(int id) => _posetilacDao.GetById(id);

        public void Add(Posetilac posetilac, Adresa adresa)
        {
            _adresaDao.Add(adresa);
            posetilac.AdresaId = adresa.Id;
            _posetilacDao.Add(posetilac);
        }

        public bool Update(Posetilac posetilac, Adresa adresa)
        {
            Posetilac? existing = _posetilacDao.GetById(posetilac.Id);
            if (existing == null)
                return false;

            if (existing.Adresa != null)
            {
                adresa.Id = existing.AdresaId;
                _adresaDao.Update(adresa);
            }
            else
            {
                _adresaDao.Add(adresa);
                posetilac.AdresaId = adresa.Id;
            }

            return _posetilacDao.Update(posetilac);
        }

        public bool Delete(int id)
        {
            Posetilac? posetilac = _posetilacDao.GetById(id);
            if (posetilac == null)
                return false;

            // Delete associated purchases first
            List<Kupovina> kupovine = _kupovinaDao.GetAll().Where(k => k.PosetilacId == id).ToList();
            foreach (Kupovina k in kupovine)
            {
                _kupovinaDao.Delete(k.Id);
            }

            int adresaId = posetilac.AdresaId;
            bool deleted = _posetilacDao.Delete(id);
            if (deleted)
            {
                _adresaDao.Delete(adresaId);
            }
            return deleted;
        }
    }
}
