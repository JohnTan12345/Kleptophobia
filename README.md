# Kelptophobia
A security guard simulator

## Description
Kleptophobia is a guard simulator game revolving around playing as the role of a security guard. The security guard’s job is to find the shoplifters while overseeing a supermarket.

## Gameplay
The player’s job is to find all the shoplifters before the supermarket closes for the day.

### Player
The player can do the following:
- Look through the CCTV using the monitor and control it
- Arrest NPCs

### NPCs
There are currently 4 types of NPCs
- Innocent Customer (-2 points if caught): Your regular customer that buy stuff
- Careless Shoplifter (1 point if caught): Mimics a normal customer without paying for stuff
- Scared Shoplifter (2 points if caught): Will run off fast after stealing an item, will run even faster after being followed by the player
- Careful Shoplifter (5 points if caught): Will immediately leave after stealing something

## Features

### Implemented
1. Player viewing the cameras through a monitor
2. Player arresting NPCs
3. Shoplifters stealing an item
4. Innocent customers lining up at the cash register
5. Scared shoplifter speeding up after stealing an item
6. Scared shoplifter running straight out of the supermarket if the player is
nearby for too long and it stole an item
7. Careful shoplifter leaving immediately after stealing an item
8. NPCs spawning and despawning
9. A title screen to enter the game
10. Points system that tracks how many shoplifters caught, escaped, or how
many that is arrested wrongfully
11. Stamina bar
12. A store timer
13. Rotating door for the security door

### Planned Features
1. A day system where areas of the supermarket slowly open up
2. Random events that happen on certain days
3. Supermarket upgrades
4. Unique items
5. A questioning system for NPCs
6. Innocent NPCs taking a cart or a basket
7. Multiplayer
8. Save/Load file system
9. A regional leaderboard

### Known Issues
1. The Careful Shoplifter may have a small chance to stop moving for some reason. No known cause of the issue since it happened twice throughout the development and playtesting combined
2. The items that the NPCs take are based on the original size of the prefab, meaning that they might appear overly huge in their hands

# Project Roles
- John Tan - Lead Programmer and UX Designer for the game
- Lucas Tan - VFX Artist and 3D Artist
- Xander Foong - 3D Artist
- Rayner Chua - Team Leader and UI/UX Designer for mobile app

# Credits
- Arrest/Handcuffs SFX: https://freesound.org/people/Fissile/sounds/764388/
- Cash register SFX: https://freesound.org/people/Zott820/sounds/209578/
- Camera switch SFX: https://freesound.org/people/kwahmah_02/sounds/261592/
- BGM: https://pixabay.com/music/upbeat-grocery-spot-268719/

All names and brands used in this project are purely fictional, any names and/or brands that exists in real life is purely coincidental.