using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TrueLayer.Service
{
    public class HttpService: IHttpService
    {
        public async Task<T> Get<T>(string mewtwo)
        {
            throw new NotImplementedException();
        }
    }

    public interface IHttpService
    {
        Task<T> Get<T>(string mewtwo);
    }
}
