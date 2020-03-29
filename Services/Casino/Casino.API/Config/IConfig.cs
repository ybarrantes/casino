using System;

namespace Casino.API.Config
{
    interface IConfig
    {
        public bool ApplyConfiguration()
        {
            try
            {
                this.SetConfiguration();
                return true;
            }
            catch (Exception)
            {
            }

            return false;
        }

        protected void SetConfiguration();
    }
}
