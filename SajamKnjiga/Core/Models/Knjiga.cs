using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Core.Serialization;

namespace Core.Models
{
    public class Knjiga : ISerializable
    {
        public int Id { get; set; }
        public string Isbn { get; set; } = string.Empty;
        public string Naziv { get; set; } = string.Empty;
        public string Zanr { get; set; } = string.Empty;
        public int GodinaIzdanja { get; set; }
        public decimal Cena { get; set; }
        public int BrojStrana { get; set; }

        public int? IzdavacId { get; set; }
        public Izdavac? Izdavac { get; set; }

        // IDs of the authors as read from the file
        public List<int> AutorIds { get; set; } = new List<int>();

        // Filled in after loading, not saved in the book's file
        public List<Autor> Autori { get; set; } = new List<Autor>();
        public List<Posetilac> Kupci { get; set; } = new List<Posetilac>();
        public List<Posetilac> PosetiociSaListeZelja { get; set; } = new List<Posetilac>();

        public Knjiga() { }

        public Knjiga(int id, string isbn, string naziv, string zanr, int godinaIzdanja,
            decimal cena, int brojStrana, int? izdavacId = null)
        {
            Id = id;
            Isbn = isbn;
            Naziv = naziv;
            Zanr = zanr;
            GodinaIzdanja = godinaIzdanja;
            Cena = cena;
            BrojStrana = brojStrana;
            IzdavacId = izdavacId;
        }

        public string[] ToCSV()
        {
            return new string[]
            {
                Id.ToString(),
                Isbn,
                Naziv,
                Zanr,
                GodinaIzdanja.ToString(),
                Cena.ToString(CultureInfo.InvariantCulture),
                BrojStrana.ToString(),
                IzdavacId?.ToString() ?? "",
                string.Join(";", AutorIds)
            };
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Isbn = values[1];
            Naziv = values[2];
            Zanr = values[3];
            GodinaIzdanja = int.Parse(values[4]);
            Cena = decimal.Parse(values[5], CultureInfo.InvariantCulture);
            BrojStrana = int.Parse(values[6]);
            IzdavacId = string.IsNullOrEmpty(values[7]) ? null : int.Parse(values[7]);
            AutorIds = values[8]
                .Split(';', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToList();
        }

        public override string ToString()
        {
            return $"[{Id}] ISBN: {Isbn} | {Naziv} | Žanr: {Zanr} | {GodinaIzdanja}. | " +
                   $"Cena: {Cena:F2} | Strana: {BrojStrana}";
        }
    }
}