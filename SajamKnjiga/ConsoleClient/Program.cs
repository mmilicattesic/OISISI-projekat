using Core.DAO;
using Core.Models;

AdresaDao adresaDao = new AdresaDao();
AutorDao autorDao = new AutorDao(adresaDao);
KnjigaDao knjigaDao = new KnjigaDao(autorDao);

if (adresaDao.GetAll().Count == 0)
    adresaDao.Add(new Adresa(0, "Gogoljeva", "5", "Novi Sad", "Srbija"));

if (autorDao.GetAll().Count == 0)
    autorDao.Add(new Autor(0, "Petar", "Petrovic", new DateOnly(1980, 5, 12),
        adresaDao.GetAll()[0].Id, "021123456", "petar@mail.com", "123456789", 10));

if (knjigaDao.GetAll().Count == 0)
{
    Knjiga knjiga = new Knjiga(0, "978-86-1234-5", "Prva knjiga", "Drama", 2020, 999m, 250);
    knjiga.AutorIds = new List<int> { autorDao.GetAll()[0].Id };
    knjigaDao.Add(knjiga);
}

foreach (Knjiga k in knjigaDao.GetAll())
{
    Console.WriteLine(k);
    foreach (Autor a in k.Autori)
        Console.WriteLine("   Author: " + a);
}

foreach (Autor a in autorDao.GetAll())
    Console.WriteLine($"{a.Ime} {a.Prezime} has {a.Knjige.Count} book(s)");