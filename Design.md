# Game Summary
You are the AI inside of Google Maps. Your goal is to navigate a car around a city as efficiently as possible while accomplishing various goals in order to earn money. You will need to work around traffic, weather conditions, natural disasters, advanced technology, Alaskan Bull Worms, and more in order to optimize your route. Some obstacles you will encounter could be used to your advantage if used correctly, or can impede your progress otherwise. Many obstacles, such as traffic and weather, will change over time, so plan accordingly.

# Learning Objectives
* Planning and optimization

# Core Gameplay
* Draw path on grid for route to take
* After starting moving along path, step through each space
* Each space takes set amount of time, based on obstacle/state of the space
* Some spaces do special things, like moving you far across the map
* At each turn, player can reroute to change their path
* While routing, changing conditions do not evolve, they are shown as they are at the time of routing
* Time evolves based on time cost of all spaces traveled through
* Level ends after certain amount of time elapses

# Features
* At the end of the (day?, level?) your score will be compared with all other players who have played the level, so the player can see how close they came to the optimal route
* Routes will be saved at the end of the level so they can later be played back
* Can zoom in and out with mouse wheel
* Sidebar 

# Aesthetics
* More details when zoomed in
* Pixel art?

# Tone
* Not realistic
* Extreme, ridiculous
* Going to extreme lengths to accomplish mundane tasks

# Miscellaneous Ideas
* Giant worms eat you at head and you come out at end
* Teleporters teleport you to exit, but disintegrate certain things in car

# Technical Notes
* No RNG. Nothing should be random. All levels should play out exactly the same given the same route

# Concerns
* Evolving conditions are not known the first time a level is played, but on later playthroughs a player is at an advantage because they already know what will happen and can plan for it in initial routing
    * One solution to this would be to abandon the idea of evolving conditions and keeping the map static throughout the level, but the issue with this is there's no reason to reroute, which reduces gameplay to initial routing
    * Another solution would be to introduce RNG, but then there's no way to perfectly optimize your route, which makes comparing your score with others less meaningful (since maybe they just got better RNG)
    * Is this actually bad? This isn't so much an issue with the game being unbalanced or bad, and more of a replay value issue that subsequent "optimization" playthroughs of levels will be boring since they are not surprising anymore
    * Another way to look at this is that the rerouting system is more of a beginners crutch than a real mechanic. If we make all evolving obstacles behave according to predictable, consistent patterns, then the player could learn to anticipate the state of the map changing over time and can plan accordingly during their initial routing. The game then becomes more about the optimization of the initial route for advanced players, while beginner play is more based around figuring out how to deal with the changing conditions. This could get annoying having to do this in your head for advanced players though
        * Could fix this by giving players a "Predictive GPS" eventually, which shows the evolving map as you do your routing, but then after the player gets this the entire game takes place in initial routing
    * We want to solve the problem of playing a level once and memorizing it, and then all subsequent runs being all about optimizing the initial route which could be boring
