# Tanks-Multiplayer
Android 2D top down tanks arena multiplayer game using Unity
# Project Status
This project is currently in development. the basic functions is all implemented like moving tank, aimming with turret, shooting, android controls, speical abilities, notification system, scoreboard, network lobby, match settings, select player wapons and customize tanks, in addition to a testing map and field of view.
## Screen shots
![alt text](https://imgur.com/3jZBVxo.png)
![alt text](https://imgur.com/b1cRBFD.png)
![alt text](https://imgur.com/1huHZp9.png)
![alt text](https://imgur.com/9DvFOhD.png)
# Reflection
This is a personal project to practise using `Unity` and `OpenGL` Shaders.

The idea came to me when we couldn't find a good fps game for a local network party -without internet connection- , so I wanted to make something simple and fun at the same time for a group of people.

# Implementation
The game starts with the network screen allowing player to host local network games, a dedicated server and edit the match settings, or join a game lobby which another player already created, the main room was based on `Network Lobby`package from unity store but alot of modifications were made to allow match settings, map preview and player tank customizations.
After hosting or joining a game the player is directed to the lobby screen where he can edit his in-game name, color, turret, hull , ability and paint, the connection between players is done with `Unity Networking` (`Rpc` and `Command` methods in additional to shared variables and components).
In game plahyer can move using a `Fixed joystick` on android, also aim and shoot using taps or another joystick, the field of view is implemented using custom `Shader`, `Ray Cast` and `Mesh` to allow fast and easy customizations later, most of the coding is done with `C#` as `Unity` scripts.
Weapons have more than 20 customizable paramters to help them varry in look, feel and game play, Some weapons (sprayable weapons like fire and ice) use special `Particle system` and particle collision, also hulls have different paramters like speed , draft , health and power which allow for a complex tank physics system to allow for fun modes in teh future and all the collision and damage is done one the server to prevent lagging and cheating.
the tank graphics and `Tile Sets` where done using `Photoshop` and `Illustrator`.
