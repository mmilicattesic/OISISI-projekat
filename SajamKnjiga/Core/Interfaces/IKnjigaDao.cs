using Core.Models;

namespace Core.Interfaces
{
    public interface IKnjigaDao
    {
        List<Knjiga> GetAll();
        Knjiga? GetById(int id);
        Knjiga? GetByIsbn(string isbn);
        void Add(Knjiga knjiga);
        bool Update(Knjiga knjiga);
        bool Delete(int id);
    }
}