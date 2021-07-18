using TrueLayer.Model;

namespace TrueLayer.Service
{
    public class YodaTranslationService: BaseTranslationService
    {
        public YodaTranslationService(IHttpService<TranslateResponse> httpService) : base(httpService)
        {
        }

        public override string SupportedTranslationType => "yoda";
        public override string Url => "https://api.funtranslations.com/translate/Yoda.json";
    }
}
