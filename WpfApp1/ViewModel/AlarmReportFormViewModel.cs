using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.ViewModel
{
    public class AlarmReportFormViewModel: NotificationObject
    {
        public string Code { set; get; }
        public string Content { set; get; }        
        private int count;

        public int Count
        {
            get { return count; }
            set
            {
                count = value;
                this.RaisePropertyChanged("Count");
            }
        }
        private TimeSpan timeSpan;

        public TimeSpan TimeSpan
        {
            get { return timeSpan; }
            set
            {
                timeSpan = value;
                this.RaisePropertyChanged("TimeSpan");
            }
        }
    }
}
