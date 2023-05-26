using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Panic
{
    public class Discipline : INotifyPropertyChanged
    {
        private string title;
        public string Title 
        {
            get 
            { 
                return title;
            } 
            set
            {  
                title = value; 
                OnPropertyChanged("Title"); 
            } 
        }
        private bool isPassed;
        public bool IsPassed 
        { 
            get 
            {  
                return isPassed; 
            } 
            set 
            {  
                isPassed = value;
                OnPropertyChanged("IsPassed");
            } 
        }
        
        public Discipline(string title, bool isPassed)
        {
            this.title = title;
            this.isPassed = isPassed;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
