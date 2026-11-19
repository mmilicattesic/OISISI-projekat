using Core.Interfaces;
using Core.Models;

namespace Core.Services
{
    // Business logic for authors. The DAO only reads and writes data,
    // this class takes care of the rules (for example what happens on delete).
    public class AutorService
    {
        private readonly IAutorDao _autorDao;
        private readonly IAdresaDao _adresaDao;
        private readonly IKnjigaDao _knjigaDao;
        private readonly IIzdavacDao _izdavacDao;

        public AutorService(IAutorDao autorDao, IAdresaDao adresaDao, IKnjigaDao knjigaDao, IIzdavacDao izdavacDao)
        {
            _autorDao = autorDao;
            _adresaDao = adresaDao;
            _knjigaDao = knjigaDao;
            _izdavacDao = izdavacDao;
        }

        public List<Autor> GetAll()
        {
            return _autorDao.GetAll();
        }

        public Autor? GetById(int id)
        {
            return _autorDao.GetById(id);
        }

        // Saves the address first, then the author that points to it
        public Autor Add(Autor autor, Adresa adresa)
        {
            _adresaDao.Add(adresa);
            autor.AdresaId = adresa.Id;
            _autorDao.Add(autor);
            return autor;
        }

        public bool Update(Autor autor, Adresa adresa)
        {
            adresa.Id = autor.AdresaId;
            _adresaDao.Update(adresa);
            return _autorDao.Update(autor);
        }

        public bool Delete(int id)
        {
            Autor? autor = _autorDao.GetById(id);
            if (autor == null)
                return false;

            // 1. Remove the author from all of their books
            foreach (Knjiga knjiga in autor.Knjige.ToList())
            {
                knjiga.AutorIds.Remove(autor.Id);
                _knjigaDao.Update(knjiga);
            }

            // 2. Remove the author as head of any publisher
            foreach (Izdavac izdavac in _izdavacDao.GetAll().Where(i => i.SefIzdavacaId == id).ToList())
            {
                izdavac.SefIzdavacaId = null;
                _izdavacDao.Update(izdavac);
            }

            // 3. Delete the author
            int adresaId = autor.AdresaId;
            _autorDao.Delete(id);

            // 4. Delete the address if nobody else uses it
            if (!_autorDao.GetAll().Any(a => a.AdresaId == adresaId))
                _adresaDao.Delete(adresaId);

            return true;
        }
    }
}