# Red Faction: Armageddon Mod Manager

### Info

![alt text](https://i.imgur.com/3rNPPr2.png "First release picture")

This tool helps you install, create and share mods easily. 3 mods have already been provided in the download to show you how the new mod manager system works compared to the tool for Red Faction: Guerrila (Re-Mars-tered).

The mod manager similarly creates a backup of the VPPs it edits and renames table.vpp_pc for you to prevent any problems from occuring!



### Creating Mods

Creating mods is now more simpler in this mod manager, the steps go as follows. (This may be a bit hard to follow, you can skip this if you already know how to unpack and edit xtbl files.)

* Download the Saints Row 3 modding tools by Gibbed (https://cdn.discordapp.com/attachments/524727117339164692/591856625401200650/gibbedvolition-rev96.zip), place the folder wherever you like.

* Navigate to your game directory (Red Faction: Armageddon) and go to the build folder, pc, then cache. Find the misc.vpp_pc file and drag it into the Gibbed.SaintsRow3.UnpackVPP exe file from the folder you just downloaded & extracted.

* Navigate back to the cache folder where the misc.vpp_pc file has been unpacked, it should be named "misc".

* Look for a file that you would like to edit, in this case we will recreate the Godmode mod, so we will be editing difficulty.xtbl. Open it up in a file editing program (Notepad++, Sublime Text, etc.)

* You can then find a value you would like to change, lets take "MENU_PLAYER_DAMAGE_TAKEN", about a quarter way down the file.

* Change all the values to 0 (Easy, Medium, Hard and Insane). Save the file.

* Go to your "mods" folder located in the game directory, you can either copy an existing mod folder and just edit it (easier) or make your own (harder).

* Copy the difficulty.xtbl file (or the one you edited) into the "misc.vpp_pc" folder inside your mod folder.

* Open up the modinfo.xml inside your mod folder to edit the info (name, author and description). Save it.

* Now you can share the mod and upload it! Right click on your mod folder and make it a ZIP file (if you have 7ZIP you can do 7-Zip > Add to (foldername).zip).



### Downloading & Installing Mods

To download user-made mod manager compatible mods, you can visit this site here:
* https://www.factionfiles.com/ff.php?action=files&file_category=28

After downloading a mod, you can extract it into the "mods" folder located in your game directory. After doing so, you can then open the mod manager, select the mod on the left side of the program where the icons are and click on Activate Mod.
