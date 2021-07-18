using TrueLayer.Model;

namespace TrueLayer.Service
{
    public class ShakespeareTranslationService: BaseTranslationService
    {
        public ShakespeareTranslationService(IHttpService<TranslateResponse> httpService) : base(httpService)
        {
        }

        public override string SupportedTranslationType => "shakespeare";
        public override string Url => "https://api.funtranslations.com/translate/shakespeare";
    }
}
