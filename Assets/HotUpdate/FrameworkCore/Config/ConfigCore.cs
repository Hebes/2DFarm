namespace Core
{
    public class ConfigCore
    {
#if UNITY_ANDROID
		public const string YooAseetPackage = "Android";
#elif UNITY_IOS
		public const string YooAseetPackage = "IOS";
#elif UNITY_STANDALONE_WIN
        public const string YooAseetPackage = "PC";
#elif UNITY_STANDALONE_OSX
		public const string YooAseetPackage = "PC";
#else
		public const string YooAseetPackage = "PC";
#endif
        //public const string YooAseetPackage = "Android";
    }
}
