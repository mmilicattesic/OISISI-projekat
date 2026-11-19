
using Core.Interfaces;
using Core.Models;

namespace Core.Services
{
    public class KnjigaService
    {
        private readonly IKnjigaDao _knjigaDao;
        private readonly IAutorDao _autorDao;
        private readonly IKupovinaDao _kupovinaDao;
        private readonly IPosetilacDao _posetilacDao;

        public KnjigaService(
            IKnjigaDao knjigaDao,
            IAutorDao autorDao,
            IKupovinaDao kupovinaDao,
            IPosetilacDao posetilacDao)
        {
            _knjigaDao = knjigaDao;
            _autorDao = autorDao;
            _kupovinaDao = kupovinaDao;
            _posetilacDao = posetilacDao;
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

            return _knjigaDao.Update(knjiga)
                ? null
                : "The book was not found.";
        }

        public bool Delete(int id)
        {
            Knjiga? knjiga = _knjigaDao.GetById(id);

            if (knjiga == null)
                return false;

            // Remove purchases associated with this book.
            List<int> kupovinaIds = _kupovinaDao.GetAll()
                .Where(k => k.KnjigaId == id)
                .Select(k => k.Id)
                .ToList();

            foreach (int kupovinaId in kupovinaIds)
            {
                if (!_kupovinaDao.Delete(kupovinaId))
                    return false;
            }

            // Remove the book from visitors' wish lists.
            foreach (Posetilac posetilac in _posetilacDao.GetAll())
            {
                posetilac.ListaZelja.RemoveAll(k => k.Id == id);
            }

            knjiga.PosetiociSaListeZelja.Clear();
            knjiga.Kupci.Clear();

            // KnjigaDao also removes the book from its authors.
            return _knjigaDao.Delete(id);
        }

        private string? Validate(Knjiga knjiga)
        {
            if (_knjigaDao.GetAll().Any(k =>
                k.Isbn == knjiga.Isbn && k.Id != knjiga.Id))
            {
                return "A book with this ISBN already exists.";
            }

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
