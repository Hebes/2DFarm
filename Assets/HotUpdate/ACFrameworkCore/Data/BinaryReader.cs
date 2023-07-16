namespace ACFrameworkCore
{
    public class BinaryReader : IRead,ICoreComponent
    {
        public void OnCroeComponentInit()
        {
        }

        public T LoadData<T>(string fileName) where T : class, new()
        {
            //var bf = new BinaryFormatter();
            //var data = bf.Deserialize(fs) as T;
            return null;
        }


        public void SaveData(object data, string fileName)
        {
        }
    }
}
