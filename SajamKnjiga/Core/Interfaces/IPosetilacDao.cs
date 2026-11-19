using Core.Models;

namespace Core.Interfaces
{
    public interface IPosetilacDao
    {
        List<Posetilac> GetAll();
        Posetilac? GetById(int id);
        void Add(Posetilac posetilac);
        bool Update(Posetilac posetilac);
        bool Delete(int id);
        void SaveWishLists();
    }
}



