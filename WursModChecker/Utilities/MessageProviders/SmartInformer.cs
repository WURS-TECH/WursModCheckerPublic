using System;
using WursModChecker.Models;
using WursModChecker.Resources.Enums;

namespace WursModChecker.Utilities.MessageProviders
{
    public class SmartInformer : IMessageProvider
    {
        private readonly string defaultNameSpace = "WursModChecker.Resources.ResourceFiles";
        private readonly string defaultSufix = "Messages";
        private TransactionStatus _transactionStatus;
        public SmartInformer(TransactionStatus transactionStatus)
        {
            _transactionStatus = transactionStatus;
        }
        public void InformMessage(string name, ResourceType resourceType)
        {
            string dynamicNamespace = $"{defaultNameSpace}.{Enum.GetName(resourceType)}{defaultSufix}";


            var propertyValue = (string)
                Type.GetType(dynamicNamespace)?
                .GetProperty(name)?
                .GetValue(null, null)!;

            _transactionStatus.StatusMessage = propertyValue ?? "";
        }
    }
}
