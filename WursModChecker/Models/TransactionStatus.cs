using System.ComponentModel;

namespace WursModChecker.Models
{
    public class TransactionStatus : INotifyPropertyChanged
    {
        private double progressBarPercentage;
        private string currentProcessedFile = string.Empty;
        private string statusMessage = string.Empty;
        private bool isProgressBarIndeterminate;
        public double ProgressBarPercentage
        {
            get
            {
                return progressBarPercentage;
            }
            set
            {
                progressBarPercentage = value;
                OnPropertyChanged(nameof(ProgressBarPercentage));
            }
        }
        public bool IsProgressBarIndeterminate
        {
            get
            {
                return isProgressBarIndeterminate;
            }
            set
            {
                isProgressBarIndeterminate = value;
                OnPropertyChanged(nameof(IsProgressBarIndeterminate));
            }
        }
        public string CurrentProcessedFile
        {
            get
            {
                return currentProcessedFile;
            }
            set
            {
                currentProcessedFile = value;
                OnPropertyChanged(nameof(CurrentProcessedFile));
            }
        }
        public string StatusMessage
        {
            get
            {
                return statusMessage;
            }
            set
            {
                statusMessage = value;
                OnPropertyChanged(nameof(statusMessage));
            }
        }
        #region INotifyPropertyChanged Members  

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
