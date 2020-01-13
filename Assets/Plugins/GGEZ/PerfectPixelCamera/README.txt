

Pixel Perfect Camera
====================

Hey there, thanks for buying my code! :)


Getting Started
---------------

To get an idea for what PerfectPixelCamera can do, play the scene "Demo - Perfect Pixel Camera"


To set this up on your own camera:

 1. Add the PerfectPixelCamera component to the same game object as your Camera
 2. Set the new component's "Texture Pixels Per World Unit" property in the inspector to the PPU value you're using on your sprites. This is almost always 8, 16 or 32.
 3. You're done!
 
To control your camera, use whatever code you normally use to set the Transform's "position" and Camera's "orthographicSize" properties. This script works behind-the-scenes so you don't have to switch all your code to using it. Camera functions like ScreenToWorldPoint will continue to work normally.

Keep in mind that this is built to work using the default 2D orientation, so keep the camera facing parallel to the Z axis.


Support
-------

Feel free to send me an email if you need help!

    - Karl

    support@mail.ggez.org

