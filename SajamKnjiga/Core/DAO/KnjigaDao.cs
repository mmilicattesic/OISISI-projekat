using Core.Interfaces;
using Core.Models;
using Core.Storage;

namespace Core.DAO
{
    public class KnjigaDao : IKnjigaDao
    {
        private readonly FileStorage<Knjiga> _storage = new FileStorage<Knjiga>("knjige.csv");
        private readonly List<Knjiga> _knjige;
        private readonly IAutorDao _autorDao;

        public KnjigaDao(IAutorDao autorDao)
        {
            _autorDao = autorDao;
            _knjige = _storage.Load();

            // The file only holds author IDs, so connect them with the real Autor objects
            foreach (Knjiga knjiga in _knjige)
            {
                LinkAuthors(knjiga);
            }
        }

        public List<Knjiga> GetAll()
        {
            return _knjige;
        }

        public Knjiga? GetById(int id)
        {
            return _knjige.FirstOrDefault(k => k.Id == id);
        }

        public Knjiga? GetByIsbn(string isbn)
        {
            return _knjige.FirstOrDefault(k => k.Isbn == isbn);
        }

        public void Add(Knjiga knjiga)
        {
            // New Id = biggest existing Id + 1
            knjiga.Id = _knjige.Count == 0 ? 1 : _knjige.Max(k => k.Id) + 1;
            LinkAuthors(knjiga);
            _knjige.Add(knjiga);
            _storage.Save(_knjige);
        }

        public bool Update(Knjiga knjiga)
        {
            Knjiga? old = GetById(knjiga.Id);
            if (old == null)
                return false;

            // Copy the fields into the existing object so that
            // other objects that point to it stay valid
            UnlinkAuthors(old);
            old.Isbn = knjiga.Isbn;
            old.Naziv = knjiga.Naziv;
            old.Zanr = knjiga.Zanr;
            old.GodinaIzdanja = knjiga.GodinaIzdanja;
            old.Cena = knjiga.Cena;
            old.BrojStrana = knjiga.BrojStrana;
            old.IzdavacId = knjiga.IzdavacId;
            old.AutorIds = new List<int>(knjiga.AutorIds);
            LinkAuthors(old);

            _storage.Save(_knjige);
            return true;
        }

        public bool Delete(int id)
        {
            Knjiga? knjiga = GetById(id);
            if (knjiga == null)
                return false;

            UnlinkAuthors(knjiga);
            _knjige.Remove(knjiga);
            _storage.Save(_knjige);
            return true;
        }

        // Fills Knjiga.Autori from AutorIds and adds the book to each author's list
        private void LinkAuthors(Knjiga knjiga)
        {
            knjiga.Autori.Clear();
            foreach (int autorId in knjiga.AutorIds)
            {
                Autor? autor = _autorDao.GetById(autorId);
                if (autor == null)
                    continue;

                knjiga.Autori.Add(autor);
                if (!autor.Knjige.Contains(knjiga))
                    autor.Knjige.Add(knjiga);
            }
        }

        // Removes the book from every author's list
        private void UnlinkAuthors(Knjiga knjiga)
        {
            foreach (Autor autor in knjiga.Autori)
            {
                autor.Knjige.Remove(knjiga);
            }
            knjiga.Autori.Clear();
        }
    }
}