using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NotesDZ
{
    public class Notes
    {
        public string Text { get; set; }
        public int numLines = 0;
        public string Date { get; set; }
        public int Id { get; private set; }
        private static int globalId = 0;

        public Notes()
        {
            Id = ++globalId;
        }
    }
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindableLayout.SetItemsSource(Col1, Instance.marks1);
            BindableLayout.SetItemsSource(Col2, Instance.marks2);
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            button.IsEnabled = false;
            Instance.currentId = 0;

            var form = new EditPage();
            Navigation.PushAsync(form);
            form.Disappearing += (send, ev) =>
            {
                button.IsEnabled = true;

                //ребилд сцены

                Instance.l1 = 0;
                Instance.l2 = 0;
                Instance.marks1.Clear();
                Instance.marks2.Clear();
                for (int i = 0; i < Instance.pool.Count; i++)
                {
                    if (Instance.l1 <= Instance.l2)
                    {
                        Instance.marks1.Add(Instance.pool[i]);
                        Instance.l1 += Instance.pool[i].numLines + 2;
                    }
                    else
                    {
                        Instance.marks2.Add(Instance.pool[i]);
                        Instance.l2 += Instance.pool[i].numLines + 2;
                    }
                }
            };
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var smth = sender as Frame;
            Instance.currentId = int.Parse(smth.AutomationId.ToString());

            var form = new EditPage();
            Navigation.PushAsync(form);

            form.Disappearing += (send, ev) =>
            {
                //ребилд сцены

                Instance.l1 = 0;
                Instance.l2 = 0;
                Instance.marks1.Clear();
                Instance.marks2.Clear();
                for (int i = 0; i < Instance.pool.Count; i++)
                {
                    if (Instance.l1 <= Instance.l2)
                    {
                        Instance.marks1.Add(Instance.pool[i]);
                        Instance.l1 += Instance.pool[i].numLines + 2;
                    }
                    else
                    {
                        Instance.marks2.Add(Instance.pool[i]);
                        Instance.l2 += Instance.pool[i].numLines + 2;
                    }
                }
            };
        }

        private async void PanGestureRecognizer_PanUpdatedLeft(object sender, PanUpdatedEventArgs e)
        {

            var thisFrame = sender as Frame;
            bool flag = true;

            switch (e.StatusType)
            {
                case GestureStatus.Running:
                    {
                        thisFrame.TranslationX = -Math.Abs(e.TotalX);
                        //thisFrame.TranslationX = Math.Min(0, e.TotalX * 1.2);
                        //Math.Max(Math.Min(0, e.TotalX), -Math.Abs(Content.Width - 700));
                        if (Math.Abs(thisFrame.TranslationX / (MainStack.Width / 6)) > 0.8)
                        {                         
                            if (flag && await DisplayAlert("Delete", "Are you sure?", "Yes", "Cancel"))
                            {
                                flag = false;
                                Instance.pool.Remove(Instance.pool.Find(x => x.Id == int.Parse(thisFrame.AutomationId)));

                                //ребилд сцены

                                Instance.l1 = 0;
                                Instance.l2 = 0;
                                Instance.marks1.Clear();
                                Instance.marks2.Clear();
                                for (int i = 0; i < Instance.pool.Count; i++)
                                {
                                    if (Instance.l1 <= Instance.l2)
                                    {
                                        Instance.marks1.Add(Instance.pool[i]);
                                        Instance.l1 += Instance.pool[i].numLines + 2;
                                    }
                                    else
                                    {
                                        Instance.marks2.Add(Instance.pool[i]);
                                        Instance.l2 += Instance.pool[i].numLines + 2;
                                    }
                                }
                            }
                            else
                            {
                                thisFrame.TranslationX = 0;
                                flag = false;
                            }
                        }
                        else
                        {
                            thisFrame.TranslationX = 0;
                        }

                        break;
                    }


                    /*case GestureStatus.Completed:
                    {

                        
                        break;
                    }*/
            }


        }

        private async void PanGestureRecognizer_PanUpdatedRight(object sender, PanUpdatedEventArgs e)
        {

            var thisFrame = sender as Frame;
            bool flag = true;

            switch (e.StatusType)
            {
                case GestureStatus.Running:
                    {
                        thisFrame.TranslationX = Math.Abs(e.TotalX);
                        //thisFrame.TranslationX = Math.Max(0, e.TotalX * 1.2);
                        //Math.Max(Math.Min(0, e.TotalX), -Math.Abs(Content.Width - 700));
                        if (Math.Abs(thisFrame.TranslationX / (MainStack.Width / 6)) > 0.8)
                        {                        
                            if (flag && await DisplayAlert("Delete", "Are you sure?", "Yes", "Cancel"))
                            {
                                flag = false;
                                Instance.pool.Remove(Instance.pool.Find(x => x.Id == int.Parse(thisFrame.AutomationId)));

                                //ребилд сцены

                                Instance.l1 = 0;
                                Instance.l2 = 0;
                                Instance.marks1.Clear();
                                Instance.marks2.Clear();
                                for (int i = 0; i < Instance.pool.Count; i++)
                                {
                                    if (Instance.l1 <= Instance.l2)
                                    {
                                        Instance.marks1.Add(Instance.pool[i]);
                                        Instance.l1 += Instance.pool[i].numLines + 2;
                                    }
                                    else
                                    {
                                        Instance.marks2.Add(Instance.pool[i]);
                                        Instance.l2 += Instance.pool[i].numLines + 2;
                                    }
                                }
                            }
                            else
                            {
                                thisFrame.TranslationX = 0;
                                flag = false;
                            }
                        }
                        else
                        {
                            thisFrame.TranslationX = 0;
                        }

                        break;
                    }


                    /*case GestureStatus.Completed:
                    {

                        
                        break;
                    }*/
            }
        }
    }
}
