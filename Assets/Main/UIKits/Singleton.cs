internal class Singleton<T> where T : new()
{
    protected Singleton() { }

    protected static T _inst = new T();
    public static T Instance
    {
        get
        {
            if (null == _inst)
                _inst = new T();
            return _inst;
        }
    }
}