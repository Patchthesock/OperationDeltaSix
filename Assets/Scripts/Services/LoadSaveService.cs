using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Assets.Scripts.Services
{
    public class LoadSaveService
    {
        public LoadSaveService(BinaryFormatter binaryFormatter)
        {
            _binaryFormatter = binaryFormatter;
        }

        public void Save()
        {
            if (!Directory.Exists("Saves")) Directory.CreateDirectory("Saves");
            var dominosFile = File.Create("Saves/dominos.binary");
            //_binaryFormatter.Serialize(dominosFile, );
            dominosFile.Close();
        }

        public void Load()
        {
            if (!Directory.Exists("Saves")) return;
            if (!File.Exists("Saves/dominos.binary")) return;
            var dominosFile = File.Open("Saves/dominos.binary", FileMode.Open);
            //var dom = (ListObjectPositionSave)_binaryFormatter.Deserialize(dominosFile);
            dominosFile.Close();
        }

        private readonly BinaryFormatter _binaryFormatter;
    }
}
