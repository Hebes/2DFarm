namespace ACFrameworkCore
{
    public class BinaryReader : IRead,ICore
    {
        public void ICroeInit()
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
