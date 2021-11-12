using System;
using System.Collections.Generic;
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

using System.Windows.Threading;

namespace FlappyBird
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        DispatcherTimer GameTimer = new DispatcherTimer();

        double Score;
        int Gravity = 8;
        bool GameOver;
        Rect FlappyBirdHitBox;

        public MainWindow()
        {
            InitializeComponent();

            GameCanvas.Focus();

            GameTimer.Tick += GameEngine;
            GameTimer.Interval = TimeSpan.FromMilliseconds(20);
            GameTimer.Start();


            //StartGame();
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                flappyBird.RenderTransform = new RotateTransform(-20, flappyBird.Width / 2, flappyBird.Height / 2);
                Gravity = -8;
            }
            if (e.Key == Key.R && GameOver == true)
            {
                StartGame();
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            flappyBird.RenderTransform = new RotateTransform(5, flappyBird.Width / 2, flappyBird.Height / 2);
            Gravity = 8;
        }

        private void StartGame()
        {
            Score = 0;

            Canvas.SetTop(flappyBird, 100);

            GameOver = false;

            Canvas.SetTop(flappyBird, 190);

            foreach (var x in GameCanvas.Children.OfType<Image>())
            {
                if (x is Image && (string)x.Tag == "obs1")
                {
                    Canvas.SetLeft(x, 500);
                }
                if (x is Image && (string)x.Tag == "obs2")
                {
                    Canvas.SetLeft(x, 800);
                }
                if (x is Image && (string)x.Tag == "obs3")
                {
                    Canvas.SetLeft(x, 1100);
                }
            }
                  
           // GameTimer.Start();

        }

        private void GameEngine(object sender, EventArgs e)
        {
            txtScore.Content = "Score: " + Score;

            FlappyBirdHitBox = new Rect(Canvas.GetLeft(flappyBird), Canvas.GetTop(flappyBird), flappyBird.Width - 20, flappyBird.Height - 20);

            Canvas.SetTop(flappyBird, Canvas.GetTop(flappyBird) + Gravity);

            if (Canvas.GetTop(flappyBird) < -10  || Canvas.GetTop(flappyBird) > 425)
            {
                EndGame();
            }

            foreach (var x in GameCanvas.Children.OfType<Image>())
            {
                if ((string)x.Tag == "obs1" || (string)x.Tag == "obs2" || (string)x.Tag == "obs3")
                {
                    Canvas.SetLeft(x, Canvas.GetLeft(x) - 5);

                    if(Canvas.GetLeft(x) < -100)
                    {
                        Canvas.SetLeft(x, 800);
                        Score += .5;
                    }

                    Rect PipeHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                    if(FlappyBirdHitBox.IntersectsWith(PipeHitBox))
                    {
                        EndGame();
                    }
                }
            }
        }

        private void EndGame()
        {
            GameTimer.Stop();
            GameOver = true;
            txtScore.Content += " Game Over !! Press R to try again";
        }

    }
}
