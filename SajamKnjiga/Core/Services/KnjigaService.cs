using Core.Interfaces;
using Core.Models;

namespace Core.Services
{
    // Business logic for books: checks the rules before the DAO saves anything.
    // Methods that can fail return an error message (null means success).
    public class KnjigaService
    {
        private readonly IKnjigaDao _knjigaDao;
        private readonly IAutorDao _autorDao;

        public KnjigaService(IKnjigaDao knjigaDao, IAutorDao autorDao)
        {
            _knjigaDao = knjigaDao;
            _autorDao = autorDao;
        }

        public List<Knjiga> GetAll()
        {
            return _knjigaDao.GetAll();
        }

        public Knjiga? GetById(int id)
        {
            return _knjigaDao.GetById(id);
        }

        public string? Add(Knjiga knjiga)
        {
            string? error = Validate(knjiga);
            if (error != null)
                return error;

            _knjigaDao.Add(knjiga);
            return null;
        }

        public string? Update(Knjiga knjiga)
        {
            string? error = Validate(knjiga);
            if (error != null)
                return error;

            return _knjigaDao.Update(knjiga) ? null : "The book was not found.";
        }

        public bool Delete(int id)
        {
            // The DAO removes the book from its authors.
            // TODO: also remove it from purchases and wish lists (after merging with the visitors' part).
            return _knjigaDao.Delete(id);
        }

        private string? Validate(Knjiga knjiga)
        {
            // ISBN must be unique
            if (_knjigaDao.GetAll().Any(k => k.Isbn == knjiga.Isbn && k.Id != knjiga.Id))
                return "A book with this ISBN already exists.";

            if (knjiga.AutorIds.Count == 0)
                return "A book must have at least one author.";

            foreach (int autorId in knjiga.AutorIds)
            {
                if (_autorDao.GetById(autorId) == null)
                    return $"There is no author with Id {autorId}.";
            }

            return null;
        }
    }
}