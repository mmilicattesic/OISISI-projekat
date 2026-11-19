using Core.Interfaces;
using Core.Models;

namespace Core.Services
{
    public class IzdavacService
    {
        private readonly IIzdavacDao _izdavacDao;
        private readonly IAutorDao _autorDao;

        public IzdavacService(IIzdavacDao izdavacDao, IAutorDao autorDao)
        {
            _izdavacDao = izdavacDao;
            _autorDao = autorDao;
        }

        public List<Izdavac> GetAll() => _izdavacDao.GetAll();

        public Izdavac? GetById(int id) => _izdavacDao.GetById(id);

        public List<Autor> GetAllAuthors() => _autorDao.GetAll();

        public bool Add(Izdavac izdavac)
        {
            if (izdavac.SefIzdavacaId.HasValue && _autorDao.GetById(izdavac.SefIzdavacaId.Value) == null)
                return false;

            _izdavacDao.Add(izdavac);
            return true;
        }

        public bool Update(Izdavac izdavac)
        {
            if (izdavac.SefIzdavacaId.HasValue && _autorDao.GetById(izdavac.SefIzdavacaId.Value) == null)
                return false;

            return _izdavacDao.Update(izdavac);
        }

        public bool Delete(int id) => _izdavacDao.Delete(id);
    }
}
