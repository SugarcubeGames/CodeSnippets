#The seed will be defined at the start of the game.  A letter will be shown when the player
#starts a new runthrough.  This letter is the same very time.  By clicking on the letters
#they will highlight purple, indicating that they have been selected as part of the seed.
#When the player closes the letter the selection will be converted into a seed value for
#map generation.  This will allow players to share their maps if they want, or redo the same
#map over and over again to try to reach the church.

#As the player moves from point to point they are almost always moving towards the Church.
#Generally this movement is fairly slow, though ocassionaly the player will move much
#closer at once.  As it gets later into the day the likelihood of negative events increases.

#There are a handful of map point types:
#    Dense woods - Little can be seen, the volume of the whispers indicates potential dangers
#    Camp - A firepit with fire burning.  These allow the player to rest and recover a health
#        point or two.  They may also have food that is *probably* safe to eat.
#    Hut - Not sure if I'll implement these.  These are places where the player can get food
#        or tools.
#    Bell - Very rarely the player will be able to clearly hear the bells of the tower, or
#        maybe even see the bell towers themselves.  This represents a massive movement towards
#        the Church.
#    Clearing - This is a break in the trees.  Mainly this just gives the player a "Free space"
#        where nothiing bad will happen.  They may also rarely find food or items.

#Negative events:
#    Fog - Obscures the path and makes it so you can't see what is on the trail.  The player
#        will have to rely on sound to choose where you're going.
#    Frigid Air - The air is so cold that it becomes dangerous to the player and drops
#        their temp.
#    Chilly Fog - Cold fog that obscures vision and drops body temp.
#    Tree Attack - The trees attack you.  This starts as you tripping on roots during the day,
#        and by night the trees are actively attacking you.

#One-Time Locations
#    Rose Garden
#    Mausoleum
#    Graveyard
#    Well