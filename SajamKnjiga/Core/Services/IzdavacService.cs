
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
            if (!IsValidHeadAuthor(izdavac.SefIzdavacaId))
                return false;

            _izdavacDao.Add(izdavac);
            return true;
        }

        public bool Update(Izdavac izdavac)
        {
            if (!IsValidHeadAuthor(izdavac.SefIzdavacaId))
                return false;

            return _izdavacDao.Update(izdavac);
        }

        public bool Delete(int id) => _izdavacDao.Delete(id);

        private bool IsValidHeadAuthor(int? autorId)
        {
            // Publisher can temporarily have no head author.
            if (!autorId.HasValue)
                return true;

            Autor? autor = _autorDao.GetById(autorId.Value);

            if (autor == null)
                return false;

            return autor.GodineIskustva >= 5;
        }
    }
}
