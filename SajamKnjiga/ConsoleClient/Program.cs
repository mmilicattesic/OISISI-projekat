using Core.DAO;
using Core.Models;

AdresaDao adresaDao = new AdresaDao();
AutorDao autorDao = new AutorDao(adresaDao);

if (adresaDao.GetAll().Count == 0)
    adresaDao.Add(new Adresa(0, "Gogoljeva", "5", "Novi Sad", "Srbija"));

int adresaId = adresaDao.GetAll()[0].Id;
autorDao.Add(new Autor(0, "Petar", "Petrovic", new DateOnly(1980, 5, 12), adresaId,
    "021123456", "petar@mail.com", "123456789", 10));

foreach (Autor a in autorDao.GetAll())
{
    Console.WriteLine(a);
    Console.WriteLine("   Address: " + a.Adresa);
}