using System;
using System.Threading.Tasks;

namespace ADE02Service.Helpers
{
    public static class RetryHelper
    {
        public static void RetryOnException(int timeout, Action metodo)
        {       
            do
            {
                TimeSpan pausaEntreFalhas = TimeSpan.FromSeconds(Math.Pow(2, timeout));

                try
                {
                    metodo();

                    break;
                }
                catch
                {
                    timeout++;

                    Task.Delay(pausaEntreFalhas).Wait();
                }
            } while (true);
        }
    }
}
