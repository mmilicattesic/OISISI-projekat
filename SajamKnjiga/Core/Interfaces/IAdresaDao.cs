using Core.Models;

namespace Core.Interfaces
{
    public interface IAdresaDao
    {
        List<Adresa> GetAll();
        Adresa? GetById(int id);
        void Add(Adresa adresa);
        bool Update(Adresa adresa);
        bool Delete(int id);
    }
}