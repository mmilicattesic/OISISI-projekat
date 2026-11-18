using System.Text;
using Core.Serialization;

namespace Core.Storage
{
    // Based on CRUDExample (course exercise materials).
    // Changes: '|' inside text is replaced with '/', empty lines are skipped.
    public class Serializer<T> where T : ISerializable, new()
    {
        private const char Delimiter = '|';

        public string ToCSV(List<T> objects)
        {
            StringBuilder sb = new StringBuilder();

            foreach (T obj in objects)
            {
                // The delimiter is replaced inside the text, so a user cannot break the file
                string[] values = obj.ToCSV()
                    .Select(v => (v ?? string.Empty).Replace(Delimiter, '/'))
                    .ToArray();

                sb.AppendLine(string.Join(Delimiter, values));
            }

            return sb.ToString();
        }

        public List<T> FromCSV(IEnumerable<string> lines)
        {
            List<T> objects = new List<T>();

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                T obj = new T();
                obj.FromCSV(line.Split(Delimiter));
                objects.Add(obj);
            }

            return objects;
        }
    }
}