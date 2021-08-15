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
Every dialog file contains a set of dialogs organized as a *key value pair*. If you want your game to be displayed in multiple languages, you'll have to create a dialog file for each language.
Its not necessary to have all the dialogs in a unique file, you can split dialogs in multiple files. For example, a 2 scene Unity game should have 2 dialog files, one per each scene. If the game was available in English and Spanish, you'll have 2 english dialog files and 2 spanish dialog files. You can store them by pairs on different directories.
You need a *EasyDialogs_SceneController.cs* on every scene where you are going to use dialogs. Its recommended to attach this script on the same GameObject where the *SceneManager.cs* is. Also, the *SceneManager.cs* script asks you for a reference of the *EasyDialogs_SceneController.cs*.

## Easy Chatboxes
It allows you to display *EasyDialogs* dialogs as chatboxes that use UnityEvents to handle scenarios.
Be careful! It uses some Unity's Input behaviour. If you moved to the new Input System you'll need to open the *EasyChatbox.cs* file and read some comments.

## Easy Texts
It allows you to display dialogs without having to use custom scripts. You just need to attach the *TMW_TextObject.cs* file to a Text GameObject and specify the dialog Id.
If you enable the toggle *subscribe to changes*, when the **language** or **referal gender** changes, the text will be updated.
