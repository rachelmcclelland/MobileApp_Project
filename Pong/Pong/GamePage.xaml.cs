using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using SkiaSharp;
using SkiaSharp.Views.Forms;

using System.Reflection;
using System.IO;
using MyUtility;

namespace Pong
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GamePage : ContentPage
    {

        private int windowWidth = (int) Application.Current.MainPage.Width;
        private int windowHeight = (int)Application.Current.MainPage.Height;
        private bool _beginGame = true;

        //boolean variables for detecting the movement of the ball
        Boolean moveLeft = false;
        Boolean moveRight = true;
        Boolean moveUp = false;
        Boolean moveDown = true;

        //starting position of the ball
        int x;
        int y;

        public GamePage()
        {
            InitializeComponent();
            
            //timer to get the canvas to keep redrawing itself
            Device.StartTimer(TimeSpan.FromSeconds(1f/60), () =>
            {
                canvasView.InvalidateSurface();

                return true;
            });
        }

        //speed the ball is going
        int speedX = 2;
        int speedY = 2;

        // create a new instance of SKBitmap to create the paddle
        private SKBitmap paddle;

        SKMatrix matrix = SKMatrix.MakeIdentity();

        // variables for x and y position of the paddle
        private float paddleX;
        private float paddleY;

        //the initial score
        int score = 0;

        private void CanvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            // Games background image
            BackgroundImage = "Assets/Images/background.jpg";

            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            //clear the canvas
            canvas.Clear();

            //output the score at the top of the game
            string str = "Score: " + score;

            // Create an SKPaint object to display the text
            SKPaint textPaint = new SKPaint
            {
                Color = SKColors.White
            };

            textPaint.TextSize = 35;

            // And draw the text onto the window
            canvas.DrawText(str, 50, 50, textPaint);

            //width and height of the window
            int width = e.Info.Width;
            int height = e.Info.Height;

            //if its the start of the game
            if(_beginGame)
            {
                Random random = new Random();

                //set the paddles x and y co ords
                 paddleX = windowWidth / 2;
                //paddleX = 0;
                paddleY = height - 50;

                // set the starting position of the ball to be random
                // between 0 and the width of the canvas
                x = random.Next(0, windowWidth);

                // set _beginGame to false so that this is function is not entered again
                _beginGame = false;
            }

            #region code repainting and movement of the ball
            //CODE FOR BALL
            // Create an SKPaint object to make outer ball
            SKPaint paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = Color.Black.ToSKColor(),
                StrokeWidth = 8
            };

            // draw the circle onto the canvas with a radius of 12 on given x, y co ords
            canvas.Save();
            canvas.DrawCircle(x, y, 12, paint);

            //create the inner ball and draw it onto the canvas
            paint.Style = SKPaintStyle.Fill;
            paint.Color = SKColors.Blue;
            canvas.DrawCircle(x, y, 12, paint);

            // movement of the ball
            MoveBallVertically(windowHeight);
            MoveBallHorizontally(windowWidth);

            // restore the state of the canvas
            canvas.Restore();

            #endregion

            //CODE FOR PADDLE
            // get the paddles image from the shared library
            string resourceID = "Pong.Assets.Images.paddle.png";
            Assembly assembly = GetType().GetTypeInfo().Assembly;

            using (Stream stream = assembly.GetManifestResourceStream(resourceID))
            {
                paddle = SKBitmap.Decode(stream);
            }

            canvas.SetMatrix(matrix);

            // add the paddle to the canvas
            canvas.DrawBitmap(paddle, paddleX, paddleY);

            // check if collision has occured
            CheckForCollision(height);

            // check what the score is, if greater than 10 make the ball move faster
            if(score >= 10)
            {
                speedX = 3;
                speedY = 3;
            }

            //check if the ball misses the paddle
            CheckForGameOver(height);
            
        }// CanvasView_PaintSurface

        private void MoveBallVertically(int height)
        {
            //if the ball is moving down
            if (moveDown)
            {
                // if it hits the bottom of the window, move back upwards
                if (y >= height)
                {
                    y -= speedY;
                    moveDown = false;
                    moveUp = true;
                }
                else // keep moving down
                {
                    y += speedY;
                }
            }

            // if the ball is moving upwards
            if (moveUp)
            {
                // if y hits the top of the window, bounce the ball back down
                if (y <= 0)
                {
                    y += speedY;
                    moveUp = false;
                    moveDown = true;
                }
                else // keep the ball moving upwards
                {
                    y -= speedY;
                }
            }
        } //MoveBallVertically

        private void MoveBallHorizontally(int width)
        {
            // if the ball if moving left
            if (moveLeft)
            {
                // if the x co ord of the ball hits the left side of the window, 
                // bounce the ball to the right
                if (x <= 0)
                {
                    x += speedX;
                    moveLeft = false;
                    moveRight = true;
                }
                else // keep moving left
                {
                    x -= speedX;
                }
            }

            // if the ball is moving right
            if (moveRight)
            {
                // if the ball hits the right side of the window, bounce the ball left
                if (x >= width)
                {
                    x -= speedX;
                    moveRight = false;
                    moveLeft = true;
                }
                else // else keep moving right
                {
                    x += speedX;
                }
            }
        }// MoveBallHorizontally

        private void CheckForCollision(int height)
        {
            //if the ball is moving down, check for a collision
            if (moveDown)
            {
                // check is the ball collides with the paddle and if so,bounce back & increment score by 1
                if (y >= (height - 60) && x >= paddleX && x <= paddleX + paddle.Width)
                //if(y >= paddleY && x >= paddleX && x <= paddleX + 170)
                {

                    // if a collision has occurred, find the sound in the shared library
                    var assembly2 = IntrospectionExtensions.GetTypeInfo(typeof(MainPage)).Assembly;
                    Stream audioStream = assembly2.GetManifestResourceStream("Pong.Assets.Sounds.bounce.wav");

                    // and load it in so thats it is ready to be played
                    var bounce = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
                    bounce.Load(audioStream);

                    // increase the score by 1
                    score++;
                    
                    // set moveUp to true so when the canvas goes around again, it know to send 
                    // the ball upwards and not downwards
                    moveUp = true;
                    moveDown = false;

                    // subtract the speed from the ball to start it moving upwards again
                    y -= speedY;

                    try
                    {
                        //play the sound
                        bounce.Play();
                    }
                    catch
                    {

                    }
                }
            }
            
        }// CheckForCollision

        private void CheckForGameOver(int height)
        {
            // if the y co ordinate is greater than where the paddle is sitting,
            // then game is over
            if(y > height - 50)
            {   
                // hide the move buttons
                moveLeftBtn.IsVisible = false;
                moveRightBtn.IsVisible = false;

                // stop the movement of the ball
                speedX = 0;
                speedY = 0;

                // show the buttons that allow the user to pick if they want to play
                // again or exit
                playAgainBtn.IsVisible = true;
                exitGameBtn.IsVisible = true;

                detailsLbl.IsVisible = true;
                detailsLbl.Text = App.pName + ", your score was: " + score;
                }       
        }// CheckForGameOver

        private void MoveLeftBtn_Clicked(object sender, EventArgs e)
        {
            // when the left button is clicked, decrease the x position of the paddle
            // so that it moves left
            matrix.TransX -= 20;
            paddleX -= 62;
        }// MoveLeftBtn_Clicked

        private void MoveRightBtn_Clicked(object sender, EventArgs e)
        {
            // if the right button is clicked, increase the x position of the paddle,
            // which moves the ball right
            matrix.TransX += 20;
            paddleX += 62;
        }// MoveRightBtn_Clicked

        private void PlayAgainBtn_Clicked(object sender, EventArgs e)
        {
            // when the user clicks that they want to play again, this method will be called

            // set y back to 0, so ball is at top of the window
            y = 0;

            // reset the speed of the ball to make is move again
            speedX = 2;
            speedY = 2;

            // reset score back to 0
            score = 0;

            // make the movement buttons visible again
            moveLeftBtn.IsVisible = true;
            moveRightBtn.IsVisible = true;

            // and the options button hidden
            playAgainBtn.IsVisible = false;
            exitGameBtn.IsVisible = false;

            detailsLbl.IsVisible = false;

            //paddle is set back to the middle of the screen
            paddleX = windowWidth / 2;

        }

        private void ExitGameBtn_Clicked(object sender, EventArgs e)
        {
            // if the user decides they want to exit the game,
            // their details are saved to the file and the game is exited
            UpdatePlayerDetails();
            Environment.Exit(0);
            //Process.GetCurrentProcess().Kill(); // another way to end the game
        }// ExitGameBtn_Clicked

        private void UpdatePlayerDetails()
        {
            // loop through the playerDetails file to find the name of the
            // player thats just after exiting the game
            foreach(var player in App.players)
            {
                if(player.Name == App.pName)
                {
                    // when the player is found, update that players score and 
                    // save the player to the file

                    //update only if new score is higher than old score
                    if(player.Score < score)
                    {
                        player.Score = score;
                        Utils.SavePlayerToFile(App.players);
                    }                  
                    break;
                }
            }
        } // UpdatePlayerDetails

        #region commented out code
        // Touch information
        //long touchId = -1;
        //SKPoint previousPoint;
        //SKRect rect;

        //private void CanvasView_Touch(object sender, SKTouchEventArgs e)
        //{
        //    SKPoint point =
        //        new SKPoint((float)(canvasView.CanvasSize.Width * e.Location.X / canvasView.Width),
        //                    (float)(canvasView.CanvasSize.Height * e.Location.Y / canvasView.Height));
        //    switch(e.ActionType)
        //    {
        //        case SKTouchAction.Pressed:

        //            rect = new SKRect(paddleX, paddleY, paddleX + paddle.Width, paddleY + paddle.Height);
        //            rect = matrix.MapRect(rect);

        //            // Determine if the touch was within that rectangle
        //            if ((e.Location.X > rect.Left && e.Location.X < rect.Right)
        //                && (e.Location.Y > rect.Top && e.Location.Y < rect.Bottom))
        //            {
        //                touchId = e.Id;
        //                previousPoint = point;
        //            }                   
        //            break;
        //        case SKTouchAction.Moved:
        //            if (touchId == e.Id)
        //            {
        //                // Adjust the matrix for the new position
        //                matrix.TransX += point.X - previousPoint.X;
        //                previousPoint = point;
        //               // paddleX = matrix.TransX;
        //                //paddleY = matrix.TransY;
        //            }
        //            break;

        //        case SKTouchAction.Released:
        //        case SKTouchAction.Cancelled:
        //            touchId = -1;
        //            break;
        //    }// switch statement
        //}
        #endregion

    }
}