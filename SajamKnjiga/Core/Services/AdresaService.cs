using Core.Interfaces;
using Core.Models;

namespace Core.Services
{
    public class AdresaService
    {
        private readonly IAdresaDao _adresaDao;
        private readonly IAutorDao _autorDao;
        private readonly IPosetilacDao _posetilacDao;

        public AdresaService(IAdresaDao adresaDao, IAutorDao autorDao, IPosetilacDao posetilacDao)
        {
            _adresaDao = adresaDao;
            _autorDao = autorDao;
            _posetilacDao = posetilacDao;
        }

        public List<Adresa> GetAll() => _adresaDao.GetAll();

        public Adresa? GetById(int id) => _adresaDao.GetById(id);

        public void Add(Adresa adresa) => _adresaDao.Add(adresa);

        public bool Update(Adresa adresa) => _adresaDao.Update(adresa);

        // An address that is used by an author or a visitor cannot be deleted
        public bool Delete(int id)
        {
            if (_autorDao.GetAll().Any(a => a.AdresaId == id))
                return false;
            if (_posetilacDao.GetAll().Any(p => p.AdresaId == id))
                return false;

            return _adresaDao.Delete(id);
        }
    }
}