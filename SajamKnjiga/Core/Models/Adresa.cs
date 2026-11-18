using Core.Serialization;

namespace Core.Models
{
    public class Adresa : ISerializable
    {
        public int Id { get; set; }
        public string Ulica { get; set; } = string.Empty;
        public string Broj { get; set; } = string.Empty;
        public string Grad { get; set; } = string.Empty;
        public string Drzava { get; set; } = string.Empty;

        public Adresa() { }

        public Adresa(int id, string ulica, string broj, string grad, string drzava)
        {
            Id = id;
            Ulica = ulica;
            Broj = broj;
            Grad = grad;
            Drzava = drzava;
        }

        public string[] ToCSV()
        {
            return new string[]
            {
                Id.ToString(),
                Ulica,
                Broj,
                Grad,
                Drzava
            };
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Ulica = values[1];
            Broj = values[2];
            Grad = values[3];
            Drzava = values[4];
        }

        public override string ToString()
        {
            return $"[{Id}] {Ulica} {Broj}, {Grad}, {Drzava}";
        }
    }
}