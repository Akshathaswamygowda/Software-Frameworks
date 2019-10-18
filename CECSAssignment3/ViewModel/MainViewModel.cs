using CECSAssignment3.Model;
using CECSAssignment3.View;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System;
using System.Windows;
using System.Windows.Data;
using GalaSoft.MvvmLight.Messaging;
using CECSAssignment3.Message;
using System.Windows.Controls;

namespace CECSAssignment3.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    /// 
    public class MainViewModel : ViewModelBase
    {
        private ObservableCollection<Member> members;
        private MemberDB database;
        private Member currentMember;
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
            members = new ObservableCollection<Member>();
            database = new MemberDB(Members);
            AddCommand = new RelayCommand(AddMethod);
            ExitCommand = new RelayCommand<ICloseable>(this.ExitMethod);
            DeleteCommand = new RelayCommand(DeleteMethod);
            SelectedItemCommand = new RelayCommand(SelectItemMethod);
            SelectMethod();
            Messenger.Default.Register<MessageMember>(this, OnMessageReceived);
            
        }

       /// <summary>
       /// Property for Selected Item
       /// </summary>
        public Member CurrentMember
        {
            get
            {
                return currentMember;
            }
            set
            {
                currentMember = value;
                RaisePropertyChanged("CurrentMember");
                
            }
        }
        /// <summary>
        /// Method for operation performed on the listbox
        /// </summary>
        /// <param name="obj"></param>
       private void OnMessageReceived(MessageMember obj)
        {
           
            if (obj.MessageType == ((int)MessageMember.MessageTypeList.AddList))
            {
                database += obj.member;
            }
            else if (obj.MessageType == ((int)MessageMember.MessageTypeList.UpdateList))
            {
                if (Members.Contains(obj.memberN))
                {
                    int position = Members.IndexOf(obj.memberN);
                    Members.Remove(obj.memberN);
                    Members.Insert(position, obj.member);
                }
            }
            else if (obj.MessageType == ((int)MessageMember.MessageTypeList.DeleteList))
            {
                database -= obj.memberN;
            }
           
                SaveMethod();
        }
        /// <summary>
        /// Method For saving in File
        /// </summary>
        public void SaveMethod()
        {
            database.SaveMembership(Members);
        }

        /// <summary>
        /// Method For displaying update window
        /// </summary>
        private void SelectItemMethod()
        {
            ChangeMember window = new ChangeMember(currentMember);
            window.Show();
        }
        /// <summary>
        /// Method for displaying members in Listbox
        /// </summary>
        public void SelectMethod()
        {
            Members = database.GetMembership();
            
        }

        public ObservableCollection<Member> Members
        {
            get
            {
                return members;
            }
            set
            {
                members = value;
                RaisePropertyChanged("Members");
            }
        }
        /// <summary>
        /// Method for Delete Operation
        /// </summary>
        public void DeleteMethod()
        {
            MessageMember message = new MessageMember();
            message.memberN = CurrentMember;
            message.MessageType = (int)MessageMember.MessageTypeList.DeleteList;
            Messenger.Default.Send<MessageMember>(message);
        }
        /// <summary>
        /// Method for Exit Operation
        /// </summary>
        /// <param name="window"></param>
        private void ExitMethod(ICloseable window)
        {
            
            if(window!=null)
            {
                window.Close();
            }
        }
        public ICommand SelectedItemCommand
        {
            get;
            private set;
        }
        public ICommand AddCommand
        {
            get;
            private set;
        }
        public ICommand ExitCommand
        {
            get;
            private set;
        }
        public ICommand DeleteCommand
        {
            get;
            private set;
        }
        /// <summary>
        /// Add Window Method
        /// </summary>
        public void AddMethod()
        {
            
            AddMember add = new AddMember();
            add.Show();
            
        }
       
        

    }

}