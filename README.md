# NotJustBubbleSort

Used Git LFS to track large files in Assets

To install go to Git LFS website or use `brew install git-lfs`

Detailed installation instruction: https://docs.github.com/en/repositories/working-with-files/managing-large-files/installing-git-large-file-storage

To check currently tracked files: `git lfs track`

To add new file type to lfs track: `git lfs track "*.file_extension_you_want_to_track"`

Tracked file types are stored in **.gitattributes**


## Unity Editor Version: 2022.3.20f1
To open the project in Unity, git clone the project into a folder and click **Add -> Add project from disk** in Unity, the necessary libraries will be downloaded automatically


## SortingSlot and SortingItem
SortingItem is what the user is going to interact with during the game, and SortingSlot is the container of SortingItem. SortingSlot should be invisible to the users.

The user need to drag the SortingItem from its original SortingSlot to another one and release. If this is the correct one, the SortingItems within the 2 SortingSlots will be swapped.

There is a very simple demo in Test scene.