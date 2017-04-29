using System;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;

namespace PresetPedalForms.Models
{
    public class User
    {
        public ObservableCollectionEx<Preset> Presets { get; set; }
        public ObservableCollectionEx<Song> Songs { get; set; }

        public Profile mainProfile { get; set; }

        public Guid UserGuid { get; set; }

        public string Email { get; set; }

        public string PageName { get; set; }

        public DateTime DateCreated { get; set; }

        public User()
        {
            Presets = new ObservableCollectionEx<Preset>();
            Songs = new ObservableCollectionEx<Song>();
            mainProfile = new Profile();

            //((INotifyPropertyChanged)Presets).PropertyChanged += new PropertyChangedEventHandler(HandlePropertyChangedEventHandler);
        }

        //void HandlePropertyChangedEventHandler(object sender, PropertyChangedEventArgs e)
        //{
        //    Debug.WriteLine("HIT");
        //}
    }

    //public class GroupedSongModel : ObservableCollectionEx<Song>
    //{
    //    public string LongName { get; set; }
    //    public string ShortName { get; set; }
    //}
}
