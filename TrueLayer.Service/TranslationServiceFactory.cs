using System.Collections.Generic;
using System.Linq;

namespace TrueLayer.Service
{
    public class TranslationServiceFactory: ITranslationServiceFactory
    {
        private readonly IEnumerable<ITranslationService> _translationServices;

        public TranslationServiceFactory(IEnumerable<ITranslationService> translationServices)
        {
            _translationServices = translationServices;
        }
        public ITranslationService Create(string translationType)
        {
            return _translationServices.Single(x => x.CanTranslate(translationType));
        }
    }

    public interface ITranslationServiceFactory
    {
        ITranslationService Create(string translationType);
    }
}
