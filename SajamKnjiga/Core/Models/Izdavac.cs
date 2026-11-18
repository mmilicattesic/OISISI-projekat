using System.Collections.Generic;
using Core.Serialization;

namespace Core.Models
{
    public class Izdavac : ISerializable
    {
        public int Id { get; set; }
        public string Sifra { get; set; } = string.Empty;
        public string Naziv { get; set; } = string.Empty;
        public int? SefIzdavacaId { get; set; }
        public Autor? SefIzdavaca { get; set; }

        public List<Autor> SpisakAutora { get; set; } = new List<Autor>();
        public List<Knjiga> SpisakKnjiga { get; set; } = new List<Knjiga>();

        public Izdavac() { }

        public Izdavac(int id, string sifra, string naziv, int? sefIzdavacaId = null)
        {
            Id = id;
            Sifra = sifra;
            Naziv = naziv;
            SefIzdavacaId = sefIzdavacaId;
        }

        public string[] ToCSV()
        {
            return new string[]
            {
                Id.ToString(),
                Sifra,
                Naziv,
                SefIzdavacaId?.ToString() ?? ""
            };
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Sifra = values[1];
            Naziv = values[2];
            SefIzdavacaId = string.IsNullOrEmpty(values[3]) ? null : int.Parse(values[3]);
        }

        public override string ToString()
        {
            string sef = SefIzdavaca != null ? $"{SefIzdavaca.Ime} {SefIzdavaca.Prezime}" : (SefIzdavacaId?.ToString() ?? "Nema");
            return $"[{Id}] Šifra: {Sifra} | Naziv: {Naziv} | Šef izdavača: {sef}";
        }
    }
}