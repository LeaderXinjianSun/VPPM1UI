using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.ViewModel
{
    public class MachineStateViewModel : NotificationObject
    {
        private double daiLiao;

        public double DaiLiao
        {
            get { return daiLiao; }
            set
            {
                daiLiao = value;
                this.RaisePropertyChanged("DaiLiao");
            }
        }
        private double huanMo;

        public double HuanMo
        {
            get { return huanMo; }
            set
            {
                huanMo = value;
                this.RaisePropertyChanged("HuanMo");
            }
        }
        private double yangBen;

        public double YangBen
        {
            get { return yangBen; }
            set
            {
                yangBen = value;
                this.RaisePropertyChanged("YangBen");
            }
        }
        private double testerAlarm;

        public double TesterAlarm
        {
            get { return testerAlarm; }
            set
            {
                testerAlarm = value;
                this.RaisePropertyChanged("TesterAlarm");
            }
        }
        private double down;

        public double Down
        {
            get { return down; }
            set
            {
                down = value;
                this.RaisePropertyChanged("Down");
            }
        }
        private double uploaderAlarm;

        public double UploaderAlarm
        {
            get { return uploaderAlarm; }
            set
            {
                uploaderAlarm = value;
                this.RaisePropertyChanged("UploaderAlarm");
            }
        }
        private double run;

        public double Run
        {
            get { return run; }
            set
            {
                run = value;
                this.RaisePropertyChanged("Run");
            }
        }
        public void Clean()
        {
            DaiLiao = 0;
            HuanMo = 0;
            YangBen = 0;
            TesterAlarm = 0;
            Down = 0;
            UploaderAlarm = 0;
            Run = 0;
        }
    }
}
