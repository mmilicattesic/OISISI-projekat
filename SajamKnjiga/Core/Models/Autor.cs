using System;
using System.Collections.Generic;
using System.Globalization;
using Core.Serialization;

namespace Core.Models
{
    public class Autor : ISerializable
    {
        public int Id { get; set; }
        public string Ime { get; set; } = string.Empty;
        public string Prezime { get; set; } = string.Empty;
        public DateOnly DatumRodjenja { get; set; }
        public int AdresaId { get; set; }
        public Adresa? Adresa { get; set; }
        public string Telefon { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string BrojLicneKarte { get; set; } = string.Empty;
        public int GodineIskustva { get; set; }

        // Not saved in the author's file. It is filled in after loading,
        // by looking at which books contain this author.
        public List<Knjiga> Knjige { get; set; } = new List<Knjiga>();

        public Autor() { }

        public Autor(int id, string ime, string prezime, DateOnly datumRodjenja, int adresaId,
            string telefon, string email, string brojLicneKarte, int godineIskustva)
        {
            Id = id;
            Ime = ime;
            Prezime = prezime;
            DatumRodjenja = datumRodjenja;
            AdresaId = adresaId;
            Telefon = telefon;
            Email = email;
            BrojLicneKarte = brojLicneKarte;
            GodineIskustva = godineIskustva;
        }

        // Turns the object into a list of text values (one line in the file)
        public string[] ToCSV()
        {
            return new string[]
            {
                Id.ToString(),
                Ime,
                Prezime,
                DatumRodjenja.ToString("dd.MM.yyyy.", CultureInfo.InvariantCulture),
                AdresaId.ToString(),
                Telefon,
                Email,
                BrojLicneKarte,
                GodineIskustva.ToString()
            };
        }

        // Turns the text values from one line of the file back into an object
        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Ime = values[1];
            Prezime = values[2];
            DatumRodjenja = DateOnly.ParseExact(values[3], "dd.MM.yyyy.", CultureInfo.InvariantCulture);
            AdresaId = int.Parse(values[4]);
            Telefon = values[5];
            Email = values[6];
            BrojLicneKarte = values[7];
            GodineIskustva = int.Parse(values[8]);
        }

        public override string ToString()
        {
            return $"[{Id}] {Ime} {Prezime} | LK: {BrojLicneKarte} | Rođen: {DatumRodjenja:dd.MM.yyyy.} | " +
                   $"Tel: {Telefon} | Email: {Email} | Iskustvo: {GodineIskustva} god.";
        }
    }
}