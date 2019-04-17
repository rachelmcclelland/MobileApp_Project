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
