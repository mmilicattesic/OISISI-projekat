
using Core.Serialization;

namespace Core.Storage
{
    // Based on CRUDExample (course exercise materials)
    // Reads and writes one file for one type of object.
    public class FileStorage<T> where T : ISerializable, new()
    {
        private readonly string _filePath;
        private readonly Serializer<T> _serializer = new Serializer<T>();

        public FileStorage(string fileName)
        {
            // Path.Combine puts the correct separator between folders.
            // From ConsoleClient\bin\Debug\net10.0 four steps up is the solution folder (SajamKnjiga).
            string folder = Path.GetFullPath(Path.Combine(
                AppContext.BaseDirectory, "..", "..", "..", "..", "Data"));
            Directory.CreateDirectory(folder);
            _filePath = Path.Combine(folder, fileName);
        }

        public List<T> Load()
        {
            // If the file does not exist yet, there is nothing to load
            if (!File.Exists(_filePath))
                return new List<T>();

            return _serializer.FromCSV(File.ReadAllLines(_filePath));
        }

        public void Save(List<T> objects)
        {
            File.WriteAllText(_filePath, _serializer.ToCSV(objects));
        }
    }
}