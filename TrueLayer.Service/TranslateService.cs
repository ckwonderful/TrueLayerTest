using System.Threading.Tasks;
using TrueLayer.Model;

namespace TrueLayer.Service
{
    public abstract class BaseTranslationService : ITranslationService
    {
        private readonly IHttpService<TranslateResponse> _httpService;

        public BaseTranslationService(IHttpService<TranslateResponse> httpService)
        {
            _httpService = httpService;
        }

        public async Task<string> Translate(string text)
        {
            var translate = await _httpService.Post<TranslateRequest>(Url, new TranslateRequest
            {
                text = text
            });
            return translate?.contents?.translated;
        }

        public bool CanTranslate(string translationType)
        {
            return translationType == SupportedTranslationType;
        }

        public abstract string SupportedTranslationType { get; }
        public abstract string Url { get; }
    }
    
    public interface ITranslationService
    {
        Task<string> Translate(string text);
        bool CanTranslate(string translationType);
    }
}
