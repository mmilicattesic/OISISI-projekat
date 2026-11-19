using Core.Models;

namespace Core.Interfaces
{
    public interface IKupovinaDao
    {
        List<Kupovina> GetAll();
        Kupovina? GetById(int id);
        void Add(Kupovina kupovina);
        bool Update(Kupovina kupovina);
        bool  Delete(int id);
    }
}