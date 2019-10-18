using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CECSAssignment3.Model
{
   public class Member:ObservableObject
    {
        private ObservableCollection<Member> membersList;
        private string firstname;
        private string lastname;
        private string email;
        public int text_limit = 25; 
        public Member(string firstname, string lastname, string emailid)
        {
            //members = new ObservableCollection<Member>();
            //database = new MemberDB(members);
            this.firstname = firstname;
            this.lastname = lastname;
            this.email = emailid;
        }
        public string Firstname
        {
            get
            {
                return firstname;
            }
            set
            {
                if(value.Length>text_limit)
                {
                    throw new ArgumentException("Too Long");
                }
                if(value.Length==0)
                {
                    throw new NullReferenceException("Invalid Entry");
                }
                firstname = value;
            }
        }
        public string Lastname
        {
            get { return lastname; }
            set
            {
                if(value.Length>text_limit)
                {
                    throw new ArgumentException("Too Long");
                }
                if(value.Length==0)
                {
                    throw new NullReferenceException("Invalid entry");
                }
                lastname = value;
            }
        }
        public string Email
        {
            get { return email; }
            set
            {
                if(value.Length>text_limit)
                {
                    throw new ArgumentException("Too Long");
                }
                if(value.Length==0)
                {
                    throw new NullReferenceException("Invalid Entry");
                }
                if(value.IndexOf('@')==-1||value.IndexOf('.')==-1)
                {
                    throw new FormatException();
                }
                email = value;
            }
        }
        public ObservableCollection<Member> MembersList
        {
            get
            {
                return membersList;
            }

            set
            {
                membersList = value;
                RaisePropertyChanged("Members");
            }
        }
        /// <summary>
        /// Display Method for Displaying it in ListBox
        /// </summary>
       public string Display
        {
            get
            {
                return Firstname + " " + Lastname + " " + Email;
            } 
        }
    }
}
