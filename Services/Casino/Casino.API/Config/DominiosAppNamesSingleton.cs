
namespace Casino.API.Config
{
    public class DominiosAppNamesSingleton
    {
        // singleton pattern
        private static DominiosAppNamesSingleton _instance = null;
        public static DominiosAppNamesSingleton GetInstance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new DominiosAppNamesSingleton();
                }

                return _instance;
            }
        }

        // constantes de nombres de dominios
        public string ROULETTES_STATES => "ROULETTES_STATES";
        public string ROULETTES_TYPES => "ROULETTES_TYPES";
    }
}
