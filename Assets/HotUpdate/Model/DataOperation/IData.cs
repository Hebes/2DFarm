namespace ACFrameworkCore
{
    public interface IData
    {
        public abstract void Save(object obj, string fileName);

        public abstract T Load<T>(string fileName) where T : class;
    }
}
