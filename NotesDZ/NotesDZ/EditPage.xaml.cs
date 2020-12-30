using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NotesDZ
{
    static class Instance
    {
        static public ObservableCollection<Notes> marks1 = new ObservableCollection<Notes>();
        static public ObservableCollection<Notes> marks2 = new ObservableCollection<Notes>();
        static public List<Notes> pool = new List<Notes>();
        static public double l1 = 0, l2 = 0;
        static public int currentId = 0;
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditPage : ContentPage
    {
        public EditPage()
        {
            InitializeComponent();

            //подгружаю текст с уже существующей заметки
            if(Instance.currentId != 0)
                TextBox.Text = Instance.pool.Find(x => x.Id == Instance.currentId).Text;
        }
        private void TextBox_Completed(object sender, EventArgs e)
        {
            var text = TextBox.Text;
            text.Normalize();

            if (text != null && text.Length != 0)
            {
                if (Instance.currentId == 0)
                {
                    // создаю новый элемент

                    Notes newNote = new Notes { Text = text, numLines = text.Split('\n').Length, Date = DateTime.Now.ToString() };
                    Instance.currentId = newNote.Id;
                    Instance.pool.Add(newNote);
                }
                else
                {
                    // редачу новый или уже существующий
                    Instance.pool.Find(x => x.Id == Instance.currentId).Text = text;
                    Instance.pool.Find(x => x.Id == Instance.currentId).numLines = text.Split('\n').Length;
                    Instance.pool.Find(x => x.Id == Instance.currentId).Date = DateTime.Now.ToString();
                }

            }
        }
    }
}