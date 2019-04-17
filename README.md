Visual Studio Version 15.9.6

MAIN PAGE
The app starts of on the main page displaying the name of the name, a label, an entry box and a button. The background
are layers of grey bricks. The name of the game is called Pong. The entry box is used to allow the player to enter in
their name which is then stored in a file with their highest score. This page also includes a button, that on start up 
is not clickable. It only becomes clickable when the player has entered in some text into the entry box provided.
Once the player clicks on the button, they will be brought to the game page where all functionality takes place.
In the contructor of this page, the SetDefaultSettings method is called.

SetDefaultSettings
This method sets a Boolean variable to false which makes sure the button is not clickable. The Observable Collection
in the App.xaml.cs page then stored all the players from the file. This is done by calling the ReadPlayersFromFile
method in the Utils class.

Name_TextChanged
This is a TextChanged event that is on the entry box. It checks if the text in the entry box is blank. If so, keep the
button disabled. If there is text in the entry box, then set the button to be clickable and save the name in a variable
in the App.xaml.cs page. This allows the name to be used on the game page as well.

GamePage_Clicked
This is the name of the name that is called when the player clicks the "PLAY" button. It calls the CheckIfPlayerExists
method and then navigates to the Game Page.

CheckIfPlayerExists
This method loops through all the players in the Observable Collection to see if that name exists. If it does, then it
breaks out of the loop as nothing else needs to be done. If it does not exist, then a new player is created and added
to the Observable Collection which is then saved to the file with a default score of 0.

GAME PAGE
In the constructor of the game page there is a timer that allows the canvas i have displayed to keep redrawing itself
throughout the game. 

CanvasView_PaintSurface
This is the method that is redrawn to display the canvas. It first creates the background image. The score is output to the
screen in the top left-hand corner. The width and the height of the canvas are stored into variables which can be used later
for finding collisions. When the game starts up, _beginGame variable is true. This allows the next if statement to only be 
entered once for setting up the placement of the objects. The paddle x and y co-ordinates are set depending on the width and
height of the screen. The ball starting position is got from getting a random number between 0 and the width of the window.
_beginGame is then set to false which makes sure that this statement is not entered again. The next thing done is the creating
of the ball, both the inner all and the outside of it. After this the MoveBallVertically and MoveBallHorizontally methods are 
called to move the ball. The paddle is from the shared library and the image is used to create a bitmap that can be drawn onto
the canvas. CheckForCollision is then called to see if a collision has occurred between the ball and the paddle. If the score is
greater than 10, then the speed of the ball is increased to make the game more difficult. At the end of this method the 
CheckForGameOver method is called to see if the player has lost the game.

MoveBallVertically
This method is what moves the ball upwards and downwards. I have decided that the way to move the ball would be by using 
Boolean variables to check which direction the ball is going. If it has not hit any boundaries, then it will keep going that 
direction until it does. This method first checks if the ball is moving down. If it is then it then checks if the ball has 
touched the bottom of the screen. If it has, the balls' y position decreases by the speed which begins the movement of the 
ball upwards. The moveDown variable is then set to false and moveUp is set to true. This way the next time this method is 
checked it does not come into this if statement which prevents the ball from going in the wrong direction. If the ball has 
not touched the bottom of the screen, then the speed is added onto the balls' y position which keeps the ball moving downwards.
This method then checks if the ball is moving upwards. If is it, then it checks whether the ball is at the top of the 
screen. If it is, the y position of the ball increases and moveUp is set to false and moveDown is set to true. Else the ball
is kept moving upwards.

MoveBallHorizontally
This method works the same as the MoveBallVertically method. Instead of checking moving up and moving down, it checks moving left
and moving right. If the ball touches either the left or the right side of the screen, then the x position of the ball is changed
depending on what direction it is going. If it has not touched either side, then it will keep moving in that direction until it 
does.

CheckForCollision
This method checks to see if there is a collision between the ball and the paddle. This is done by checking is the y position of the
ball matches the co-ordinates of where the paddle is sitting and if the x position of the ball is within the range of where the 
beginning of the paddle is to the end of the paddle (width of the paddle). If this is true, a collision has occurred. A sound plays
that was embedded from the shared library to say that a bounce has occurred, the score increased. The moveUp variable is set to true 
and moveDown is set to false. This makes sure that the right statement is gone into for the ball to move the right direction again.

CheckForGameOver
If the y co-ordinate of the ball, is greater than the position of the paddle, then the game is over because the player did not get
to the ball in time. When this has happened, the movement balls become hidden so the game over buttons can appear in their place.
The speed of the ball is set to 0 which stops the movement of the ball. The name that was given on the main page is used here when 
outputting what the players score is.

MoveLeftBtn_Clicked
When the "Left" button is clicked by the player, this method is called which allows the paddle to move towards the left. I had some 
problems with getting the collision to occur because the x and y co-ordinates of the ball would not match with the x and y position
of the paddle. The best way for me to fix this was to subtract 62 onto the variable that holds the x position of the paddle. This allowed
the collision to keep working instead of just in certain areas.

MoveRightBtn_Clicked
This method is the same as the MoveLeftBtn_Clicked method. It is called when the user clicks the "Right" button. Instead of subtracting
62 from the x position of the paddle it adds it on instead.

PlayAgainBtn_Clicked
When the game is over, this is one of the two buttons that appear on the screen in place of the movement buttons for the paddle. This button
allows the player to have another go at the game. The y position of the ball is set back to 0 which leaves it sitting at the top of the screen
again. The speed is also reset back to its original speed and the score is back at 0. The movement buttons are made visible again, making the
game over option buttons hidden. The paddle is also replaced back to the middle of the scree, where is started off.

UpdatePlayerDetails
This method loops through all the players that are stored in an Observable Collection in the App.xaml.cs. When the player with that name has
been found, their score is updated if the old score is lower than their new score. It calls the SavePlayerToFile method in the Utils class
and passes in the players.

UTILS
This class deals with any work done on the file.

ReadPlayersFromFile
This method returns an Observable Collection of type Player. If the file cannot be found it uses the default file. 

SavePlayerToFile
This method saves the players details back to the file.