using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Casino.API.Services
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
        public string ESTADOS_RULETAS => "ESTADOS_RULETAS";
        public string TIPOS_RULETAS => "TIPOS_RULETAS";
    }
}
