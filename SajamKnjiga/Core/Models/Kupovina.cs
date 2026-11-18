using System;
using System.Globalization;
using Core.Serialization;

namespace Core.Models
{
    public class Kupovina : ISerializable
    {
        public int Id { get; set; }
        public int PosetilacId { get; set; }
        public Posetilac? Posetilac { get; set; }
        public int KnjigaId { get; set; }
        public Knjiga? Knjiga { get; set; }
        public DateOnly DatumKupovine { get; set; }
        public int Ocena { get; set; }
        public string Komentar { get; set; } = string.Empty;

        public Kupovina() { }

        public Kupovina(int id, int posetilacId, int knjigaId, DateOnly datumKupovine, int ocena, string komentar)
        {
            Id = id;
            PosetilacId = posetilacId;
            KnjigaId = knjigaId;
            DatumKupovine = datumKupovine;
            Ocena = ocena;
            Komentar = komentar;
        }

        public string[] ToCSV()
        {
            return new string[]
            {
                Id.ToString(),
                PosetilacId.ToString(),
                KnjigaId.ToString(),
                DatumKupovine.ToString("dd.MM.yyyy.", CultureInfo.InvariantCulture),
                Ocena.ToString(),
                Komentar
            };
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            PosetilacId = int.Parse(values[1]);
            KnjigaId = int.Parse(values[2]);
            DatumKupovine = DateOnly.ParseExact(values[3], "dd.MM.yyyy.", CultureInfo.InvariantCulture);
            Ocena = int.Parse(values[4]);
            Komentar = values[5];
        }

        public override string ToString()
        {
            return $"[{Id}] PosetilacId: {PosetilacId} | KnjigaId: {KnjigaId} | " +
                   $"Datum: {DatumKupovine:dd.MM.yyyy.} | Ocena: {Ocena}/5 | Komentar: {Komentar}";
        }
    }
}