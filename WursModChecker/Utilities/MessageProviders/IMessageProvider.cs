using WursModChecker.Resources.Enums;

namespace WursModChecker.Utilities.MessageProviders
{
    public interface IMessageProvider
    {
        void InformMessage(string name, ResourceType resourceType = ResourceType.Generic);
    }
}
