using IdGen;

namespace lcapis.Utils
{
    public sealed class SnowflakeGeneratorSingleton
    {
        private static IdGenerator instance = null;
        private static readonly object padlock = new object();

        SnowflakeGeneratorSingleton(){}

        public static IdGenerator Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new IdGenerator(0);
                    }
                    return instance;
                }
            }
        }
    }
}
