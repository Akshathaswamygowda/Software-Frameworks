using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CECSAssignment4
{
    public partial class BooksDatabase : Form
    {
        public BooksDatabase()
        {
            InitializeComponent();
        }

        private void BooksDatabase_Load(object sender, EventArgs e)
        {
            // Entity Framework DBContext
            BooksEntities dbcontext =
               new BooksEntities();

            // get authors and Title of each book
            var authorsAndTitles =
               from author in dbcontext.Authors
               from book in author.Titles
               orderby book.Title1
               select new { book.Title1, author.FirstName, author.LastName };

            outputTextBox.AppendText("Authors and Titles:");

            // display authors and Title in tabular format
            foreach (var element in authorsAndTitles)
            {
                outputTextBox.AppendText(
                   String.Format("\r\n\t{0,-10} {1,-10} {2,-10}",
                       element.Title1, element.FirstName, element.LastName));
            } // end foreach

            outputTextBox.AppendText("\r\n\r\nAuthors and titles with authors sorted for each title:");
            //get authors and titles sort by first and last name
            var authorgetbyname =
                 from authors in dbcontext.Authors
                 from books in authors.Titles
                 orderby books.Title1, authors.LastName, authors.FirstName
                 select new { books.Title1, authors.FirstName, authors.LastName };

            //display authors by Name
            foreach(var element in authorgetbyname)
           {
               outputTextBox.AppendText(
               string.Format("\r\n\t{0,-10} {1,-10} {2,-10}", 
               element.Title1,element.FirstName,element.LastName));
            }

            

            //Get author name based on title
            var authorsGroupname =
                from books in dbcontext.Titles
                orderby books.Title1
                select new
                {
                    Titles=books.Title1,
                    Author=
                    from authors in books.Authors
                    orderby authors.LastName, authors.FirstName
                    select new {authors.LastName,authors.FirstName}
                };

            outputTextBox.AppendText("\r\n\r\nTitles grouped by author:");

            // display titles written by each author, grouped by author
            foreach (var title in authorsGroupname)
            {
                // display author's name
                outputTextBox.AppendText("\r\n\t" + title.Titles + ":");

                // display titles written by that author
                foreach (var Name in title.Author)
                {
                    outputTextBox.AppendText("\r\n\t\t" + Name.FirstName+" "+Name.LastName);
                } // end inner foreach
            } // end outer foreach

            // get authors and titles of each book they co-authored
            /* var authorsAndTitles =
                from book in dbcontext.Titles
                from author in book.Authors
                orderby author.LastName, author.FirstName, book.Title1
                select new
                {
                    author.FirstName,
                    author.LastName,
                    book.Title1
                };

             outputTextBox.AppendText("\r\n\r\nAuthors and titles:");

             // display authors and titles in tabular format
             foreach (var element in authorsAndTitles)
             {
                 outputTextBox.AppendText(
                    String.Format("\r\n\t{0,-10} {1,-10} {2}",
                       element.FirstName, element.LastName, element.Title1));
             } // end foreach

             // get authors and titles of each book 
             // they co-authored; group by author
             var titlesByAuthor =
                from author in dbcontext.Authors
                orderby author.LastName, author.FirstName
                select new
                {
                    Name = author.FirstName + " " + author.LastName,
                    Titles =
                      from book in author.Titles
                      orderby book.Title1
                      select book.Title1
                };

             outputTextBox.AppendText("\r\n\r\nTitles grouped by author:");

             // display titles written by each author, grouped by author
             foreach (var author in titlesByAuthor)
             {
                 // display author's name
                 outputTextBox.AppendText("\r\n\t" + author.Name + ":");

                 // display titles written by that author
                 foreach (var title in author.Titles)
                 {
                     outputTextBox.AppendText("\r\n\t\t" + title);
                 } // end inner foreach
             } // end outer foreach

             */

        }
    }
}

