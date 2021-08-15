# Unity-Easy-Packages
Packages made to ease generic tasks such as user settings, dialogs, chatboxes, etc

## TheMineWay Kernel
A component required as a dependency on every component. It cares about communicating between components. Just leave it there.
Includes:
- *TMW_Kernel.cs* (the Kernel itself).
- *TMW_Conf.cs* (the Configuration Kernel).
- *TMW_Logs.cs* (used in replacement of the Unity's default logs system).
- *SceneManager.cs* (required on every Unity scene. Its recommended to create a new GameObject on the scene and attach teh script to it).

## Easy Configuration UI
It allows you to include settings into your game without caring about the logic behind it. You only need to create a GameObject and attach to it the *EasyConfigurationUI.cs* script. Then, create a Canvas and create there the UI components (Dropdowns, Toggles, Sliders, etc) and attach them to the fields of the EasyConfigurationUI component.
On every audio source, you have to attach the *AudioController.cs* in order to apply the audio settings.

## Easy Achievements
A component that allows you to add achievements easily to your game.
In the *AchievementsController.cs* you MUST specify on the achievementsId array the ids of all achievements. Then, you must attach this script to a gameobject on every scene where you are going to work with achievements (read or write them).
The *Achievement.cs* script is used when you want to display the status of an achievement. You attach the cript to a scene GameObject (on a Canvas) and fill the fields.

## Easy Stats
It contains some scripts used to mesure statistics.
Includes:
- FPS counter.

## Easy Dialogs
It allows you to store game dialogs in JSON files. It supports multilanguage and multigender games. You can create a dialog file from the Unity editor (**Window > Easy Dialogs > Dialog creator**) and/or modify one existing dialog (**Window > Easy Dialogs > Dialogs manager**).
Every dialog file contains a set of dialogs organized as a *key value pair*.
