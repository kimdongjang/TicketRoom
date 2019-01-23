using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.Shop
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WriteReviewPage : ContentPage
    {
        List<Grid> Grade_Grid = new List<Grid>();
        Queue<Grid> ColorChange_Queue = new Queue<Grid>();
        int gradeScore = 0;

        public WriteReviewPage()
        {
            InitializeComponent();
            Init();

        }

        private void Init()
        {
            Grade_Grid.Add(OneGrade);
            Grade_Grid.Add(TwoGrade);
            Grade_Grid.Add(ThreeGrade);
            Grade_Grid.Add(FourGrade);
            Grade_Grid.Add(FiveGrade);

            for (int i = 0; i < Grade_Grid.Count; i++)
            {
                Grid tempGrid = Grade_Grid[i];
                tempGrid.GestureRecognizers.Add(new TapGestureRecognizer()
                {
                    Command = new Command(() =>
                    {
                        if (tempGrid == OneGrade)
                        {
                            gradeScore = 1;
                        }
                        else if (tempGrid == TwoGrade)
                        {
                            gradeScore = 2;
                        }
                        else if (tempGrid == ThreeGrade)
                        {
                            gradeScore = 3;
                        }
                        else if (tempGrid == FourGrade)
                        {
                            gradeScore = 4;
                        }
                        else if (tempGrid == FiveGrade)
                        {
                            gradeScore = 5;
                        }

                        if (ColorChange_Queue.Count < 2)
                        {
                            if (ColorChange_Queue.Count != 0)
                            {
                                Grid temp = ColorChange_Queue.Dequeue();
                                temp.BackgroundColor = Color.Gray;
                            }
                            tempGrid.BackgroundColor = Color.Black;
                            ColorChange_Queue.Enqueue(tempGrid);
                        }
                    })
                });
            }


        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {

        }

        private void Tab_Changed(object sender, EventArgs e)
        {

        }
    }
}