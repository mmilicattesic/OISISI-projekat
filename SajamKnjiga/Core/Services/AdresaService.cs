
using Core.DAO;
using Core.Models;

namespace Core.Services
{
    public class AdresaService
    {
        private readonly AdresaDao _adresaDao;

        public AdresaService(AdresaDao adresaDao)
        {
            _adresaDao = adresaDao;
        }

        public List<Adresa> GetAll()
        {
            return _adresaDao.GetAll();
        }

        public Adresa? GetById(int id)
        {
            return _adresaDao.GetById(id);
        }

        public void Add(Adresa adresa)
        {
            _adresaDao.Add(adresa);
        }

        public bool Update(Adresa adresa)
        {
            return _adresaDao.Update(adresa);
        }

        public bool Delete(int id)
        {
            return _adresaDao.Delete(id);
        }
    }
}
