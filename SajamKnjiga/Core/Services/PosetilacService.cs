
using Core.Interfaces;
using Core.Models;

namespace Core.Services
{
    public class PosetilacService
    {
        private readonly IPosetilacDao _posetilacDao;
        private readonly IAdresaDao _adresaDao;
        private readonly IKupovinaDao _kupovinaDao;
        private readonly IKnjigaDao _knjigaDao;

        public PosetilacService(
            IPosetilacDao posetilacDao,
            IAdresaDao adresaDao,
            IKupovinaDao kupovinaDao,
            IKnjigaDao knjigaDao)
        {
            _posetilacDao = posetilacDao;
            _adresaDao = adresaDao;
            _kupovinaDao = kupovinaDao;
            _knjigaDao = knjigaDao;
        }

        public List<Posetilac> GetAll() => _posetilacDao.GetAll();

        public Posetilac? GetById(int id) => _posetilacDao.GetById(id);

        public List<Knjiga> GetAllBooks() => _knjigaDao.GetAll();

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

            List<Kupovina> kupovine = _kupovinaDao.GetAll()
                .Where(k => k.PosetilacId == id)
                .ToList();

            foreach (Kupovina kupovina in kupovine)
            {
                _kupovinaDao.Delete(kupovina.Id);
            }

            int adresaId = posetilac.AdresaId;
            bool deleted = _posetilacDao.Delete(id);

            if (deleted)
            {
                _adresaDao.Delete(adresaId);
            }

            return deleted;
        }

        public bool AddToWishList(int posetilacId, int knjigaId)
        {
            Posetilac? posetilac = _posetilacDao.GetById(posetilacId);
            Knjiga? knjiga = _knjigaDao.GetById(knjigaId);

            if (posetilac == null || knjiga == null)
                return false;

            if (posetilac.ListaZelja.Any(k => k.Id == knjigaId))
                return false;

            posetilac.ListaZelja.Add(knjiga);
            posetilac.ListaZeljaIds.Add(knjigaId);

            if (!knjiga.PosetiociSaListeZelja.Contains(posetilac))
                knjiga.PosetiociSaListeZelja.Add(posetilac);

            _posetilacDao.SaveWishLists();
            return true;
        }

        public bool RemoveFromWishList(int posetilacId, int knjigaId)
        {
            Posetilac? posetilac = _posetilacDao.GetById(posetilacId);

            if (posetilac == null)
                return false;

            Knjiga? knjiga = posetilac.ListaZelja
                .FirstOrDefault(k => k.Id == knjigaId);

            if (knjiga == null)
                return false;

            posetilac.ListaZelja.Remove(knjiga);
            posetilac.ListaZeljaIds.RemoveAll(id => id == knjigaId);
            knjiga.PosetiociSaListeZelja.Remove(posetilac);

            _posetilacDao.SaveWishLists();
            return true;
        }
    }
}
