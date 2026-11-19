using Core.Models;

namespace Core.Interfaces
{
    public interface IIzdavacDao
    {
        List<Izdavac> GetAll();
        Izdavac? GetById(int id);
        void Add(Izdavac izdavac);
        bool Update(Izdavac izdavac);
        bool Delete(int id);
    }
    
}