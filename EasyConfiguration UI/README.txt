If you are using Easy Dialogs and in the configuration you enabled the gender selector, you MUST setup a Scene Manager
and a "Easy Dialogs_Scene Controller" because the gender names change depending on the selected language.
That why on the "Easy Configuration UI" the list of gender names is called "Gender Ids", you are not writing the names
of the genders, instead, you are specifying the id of each gender. Then, the dialogs engine will get the names from
the dialogs files that you set up on the "Easy Dialogs_Scene Controller".