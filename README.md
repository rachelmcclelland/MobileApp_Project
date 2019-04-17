Visual Studio Version 15.9.6

The app starts of on the main page displaying the name of the name, a label, an entry box and a button. The background
are layers of grey bricks. The name of the game is called Pong. The entry box is used to allow the player to enter in
their name which is then stored in a file with their highest score. This page also includes a button, that on start up 
is not clickable. It only becomes clickable when the player has entered in some text into the entry box provided.
Once the player clicks on the button, they will be brought to the game page where all functionality takes place.

In the constructor of the game page there is a timer that allows the canvas i have displayed to keep redrawing itself
throughout the game. In the canvas, the background image, which is the same as the main page, is displayed.
It is also where the score is being displayed all the time. From the canvas method, the ball is redrawn and the bitmap image
of the image is also created. Every time the canvas is run the MoveBallVertically and the MoveBallHorizontally functions are 
called. These functions allow the ball to bounce around the screen. The CheckForCollision method and the CheckForGameOver 
method are also called each time to find out if the ball collides with the paddle.

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