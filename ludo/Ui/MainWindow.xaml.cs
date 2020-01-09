using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;

namespace Ui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
            
        public List<Ellipse> Fields = new List<Ellipse>();

        public MainWindow()
        {
            InitializeComponent();

            Grid DynamicGrid = new Grid();
            DynamicGrid.Width = this.Width;
            DynamicGrid.HorizontalAlignment = HorizontalAlignment.Left;
            DynamicGrid.VerticalAlignment = VerticalAlignment.Top;

            // Create Columns
            for (int i = 0; i < 12; i++)
            {
                ColumnDefinition gridCol = new ColumnDefinition();
                DynamicGrid.ColumnDefinitions.Add(gridCol);

            }

            for (int i = 0; i < 12; i++)
            {
                RowDefinition gridRow = new RowDefinition();
                gridRow.Height = new GridLength(this.Height / 12);
                DynamicGrid.RowDefinitions.Add(gridRow);
            }

            List<gridXY> XY = new List<gridXY>();

            XY.Add(new gridXY { x = 0, y = 4 });
            XY.Add(new gridXY { x = 1, y = 4 });
            XY.Add(new gridXY { x = 2, y = 4 });
            XY.Add(new gridXY { x = 3, y = 4 });
            XY.Add(new gridXY { x = 4, y = 4 });

            XY.Add(new gridXY { x = 4, y = 3 });
            XY.Add(new gridXY { x = 4, y = 2 });
            XY.Add(new gridXY { x = 4, y = 1 });
            XY.Add(new gridXY { x = 4, y = 0 });

            XY.Add(new gridXY { x = 5, y = 0 });

            XY.Add(new gridXY { x = 6, y = 0 });
            XY.Add(new gridXY { x = 6, y = 1 });
            XY.Add(new gridXY { x = 6, y = 2 });
            XY.Add(new gridXY { x = 6, y = 3 });
            XY.Add(new gridXY { x = 6, y = 4 });

            XY.Add(new gridXY { x = 6, y = 4 });
            XY.Add(new gridXY { x = 7, y = 4 });
            XY.Add(new gridXY { x = 8, y = 4 });
            XY.Add(new gridXY { x = 9, y = 4 });
            XY.Add(new gridXY { x = 10, y = 4 });

            XY.Add(new gridXY { x = 10, y = 5 });

            XY.Add(new gridXY { x = 10, y = 6 });
            XY.Add(new gridXY { x = 9, y = 6 });
            XY.Add(new gridXY { x = 8, y = 6 });
            XY.Add(new gridXY { x = 7, y = 6 });
            XY.Add(new gridXY { x = 6, y = 6 });

            XY.Add(new gridXY { x = 6, y = 7 });
            XY.Add(new gridXY { x = 6, y = 8 });
            XY.Add(new gridXY { x = 6, y = 9 });
            XY.Add(new gridXY { x = 6, y = 10 });

            XY.Add(new gridXY { x = 5, y = 10 });

            XY.Add(new gridXY { x = 4, y = 10 });
            XY.Add(new gridXY { x = 4, y = 9 });
            XY.Add(new gridXY { x = 4, y = 8 });
            XY.Add(new gridXY { x = 4, y = 7 });
            XY.Add(new gridXY { x = 4, y = 6 });

            XY.Add(new gridXY { x = 3, y = 6 });
            XY.Add(new gridXY { x = 2, y = 6 });
            XY.Add(new gridXY { x = 1, y = 6 });
            XY.Add(new gridXY { x = 0, y = 6 });

            XY.Add(new gridXY { x = 0, y = 5 });


            foreach (var VARIABLE in XY)
            {
                Canvas canvas = new Canvas();
                canvas.Height = this.Height / 12;
                canvas.Width = this.Width / 12;
                canvas.Background = System.Windows.Media.Brushes.Black;

                Ellipse ellipse = new Ellipse();
                ellipse.Height = this.Height / 12;
                ellipse.Width = this.Width / 12;
                ellipse.Stroke = System.Windows.Media.Brushes.Black;
                ellipse.Fill = System.Windows.Media.Brushes.DarkBlue;
                ellipse.HorizontalAlignment = HorizontalAlignment.Left;
                ellipse.VerticalAlignment = VerticalAlignment.Center;

                canvas.Children.Add(ellipse);
                Fields.Add(ellipse);

                Grid.SetColumn(canvas, VARIABLE.x);
                Grid.SetRow(canvas, VARIABLE.y);
                DynamicGrid.Children.Add(canvas);
                
            }


            this.Content = DynamicGrid;
            //start();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            start();
        }
        public async void start()
        {
            string path = System.IO.Path.GetTempPath() + "ludo";

            StreamReader sr = new StreamReader(path + @"\hist.json");
            string jsonString = sr.ReadToEnd();

            List<HistJson> Hists = JsonConvert.DeserializeObject<List<HistJson>>(jsonString);

            int times = Hists[0].Moves.Count;
            for (int i = 0; i < times; i++)
            {
                foreach (var hist in Hists)
                {
                    var isNumeric = int.TryParse(hist.Moves[i], out int n);
                    if (isNumeric)
                    {
                        //Fields[n].Fill
                        switch (hist.Color)
                        {
                            case "Red":
                                Fields[n].Fill = System.Windows.Media.Brushes.Red;
                                break;
                            case "Blue":
                                Fields[n].Fill = System.Windows.Media.Brushes.Blue;
                                break;
                            case "Green":
                                Fields[n].Fill = System.Windows.Media.Brushes.Green;
                                break;
                            case "Yellow":
                                Fields[n].Fill = System.Windows.Media.Brushes.Yellow;
                                break;
                        }
                    }
                    await Task.Run(() => System.Threading.Thread.Sleep(5l0));
                }
            }
        }
    }
}
