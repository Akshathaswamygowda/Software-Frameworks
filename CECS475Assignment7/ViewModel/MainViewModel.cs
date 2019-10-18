using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using System;
using System.Windows;
using System.Collections.ObjectModel;
using CECS475Assignment7.Model;
using System.Windows.Controls;
using System.Xml.Linq;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CECS475Assignment7.ViewModel
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
    public class MainViewModel : ViewModelBase
    {
        //Private Key For Flicker Website
        private const string KEY = "72028fcc658df12c4a4d73cb6cb68a48";
        //Webclient to comunicate withini web browser
        WebClient flickrClient = new WebClient();
        Task<string> flickrTask;
        //Image Box Variable
        public Image pictureBox;

        private string title;
        private string url;
        //Collection Object for storing title and Url
        private ObservableCollection<FlickerImage> flickerList = new ObservableCollection<FlickerImage>();
        private List<string> titleName = new List<string>();
        FlickerImage fi = new FlickerImage();
        private FlickerImage selectedItem;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        /// 
        public MainViewModel(Image image)
        {
            pictureBox = image;
            SearchEngine = new RelayCommand(SeachTag);
            SelectedItemCommand = new RelayCommand(SelectedPhoto);
        }
        /// <summary>
        /// Async Method to display selected photo
        /// </summary>
        private async void SelectedPhoto()
        {
            if (SelectedItem != null)
            {

                //retrieve url of the selected photo
                var selectedURL =
                   ((FlickerImage)SelectedItem).Url;
                WebClient client = new WebClient();
                Task<byte[]> downloadTask = client.DownloadDataTaskAsync(new Uri(selectedURL));
                byte[] imageBytes = await downloadTask;
                MemoryStream stream = new MemoryStream(imageBytes);
                var imagesource = new BitmapImage();
                imagesource.BeginInit();
                imagesource.StreamSource = stream;
                imagesource.EndInit();
                pictureBox.Source = imagesource;
            }
        }

        private async void DisplayPicture(string selectedURL)
        {
            if (selectedURL != null)
            {
                try
                {
                    WebClient client = new WebClient();
                    Task<byte[]> downloadTask = client.DownloadDataTaskAsync(new Uri(selectedURL));
                    byte[] imageBytes = await downloadTask;
                    MemoryStream stream = new MemoryStream(imageBytes);
                    var imagesource = new BitmapImage();
                    imagesource.BeginInit();
                    imagesource.StreamSource = stream;
                    imagesource.EndInit();
                    pictureBox.Source = imagesource;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "No Result Found!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("No Result Found!");
                pictureBox.Source = null;
            }
        }
        /// <summary>
        /// Async method for searching tags
        /// </summary>
        private async void SeachTag()
        {
            if (flickrTask != null &&
               flickrTask.Status != TaskStatus.RanToCompletion)
            {
                var result = MessageBox.Show("Cancel the Flicker Search?", "Are you sure?",
                             MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.No)
                    return;
                else
                {
                    flickrClient.CancelAsync();
                }
            }
            var flickrUrl = string.Format("https://api.flickr.com/services/rest/?method=flickr.photos.search&tags={0}&api_key={1}&privacy_filter=1", Title.Replace(" ", ","), KEY);
            FlickerList.Clear();
            try
            {
                //Reterives related photos based on search tags
                flickrTask = flickrClient.DownloadStringTaskAsync(new Uri(flickrUrl));
                XDocument doc = XDocument.Parse(await flickrTask);
                var flickrPhotos =
                       from photo in doc.Descendants("photo")
                       let id = photo.Attribute("id").Value.ToString()
                       let title = photo.Attribute("title").Value.ToString()
                       let secret = photo.Attribute("secret").Value.ToString()
                       let server = photo.Attribute("server").Value.ToString()
                       let farm = photo.Attribute("farm").Value.ToString()
                       select new FlickerImage
                       {
                           Title = title,
                           Url = string.Format("https://farm{0}.staticflickr.com/{1}/{2}_{3}.jpg", farm, server, id, secret)
                       };
                FlickerList.Clear();

                if (flickrPhotos.Any())
                {
                    foreach (var list in flickrPhotos)
                    {
                        FlickerList.Add(new FlickerImage() { Title = list.Title, Url = list.Url });
                    }

                }
                else
                {
                    fi.Title = "No Match Found!";
                    FlickerList.Add(fi);
                }
            }
            catch (WebException)
            {
                if (flickrTask.Status == TaskStatus.Faulted)
                    MessageBox.Show("Unable to get results from Flickr",
                       "Flickr Error", MessageBoxButton.OK,
                       MessageBoxImage.Error);
                FlickerList.Clear();
                fi.Title = "Error Occured";
                FlickerList.Add(fi);
            }

            var selectedURL =
                  ((FlickerImage)FlickerList[0]).Url;
            DisplayPicture(selectedURL);

        }

        public ICommand SearchEngine { get; private set; }
        public ICommand SelectedItemCommand { get; private set; }

        /// <summary>
        /// Property for Title
        /// </summary>
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
                RaisePropertyChanged("Title");
            }
        }

        /// <summary>
        /// Property for URL
        /// </summary>
        public string Url
        {
            get
            {
                return url;
            }
            set
            {
                url = value;
                RaisePropertyChanged("Url");
            }
        }


        /// <summary>
        /// Property of FlickerList Collection
        /// </summary>
        public ObservableCollection<FlickerImage> FlickerList
        {
            get
            {
                return flickerList;
            }
            set
            {
                flickerList = value;
                RaisePropertyChanged("FlickerList");
            }
        }
        /// <summary>
        /// Property of SelecedItem
        /// </summary>
        public FlickerImage SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                selectedItem = value;
                RaisePropertyChanged("SelectedItem");
            }
        }

    }
}