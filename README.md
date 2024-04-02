# NotJustBubbleSort

## Install Git LFS first!!!
To install, use homebrew `brew install git-lfs`

For the first time, init lfs globally, `git lfs install`

To check currently tracked files: `git lfs track`

To add new file type to lfs track: `git lfs track *.file_extension_you_want_to_track`

Tracked file types are stored in **.gitattributes**

Documentation: https://docs.github.com/en/repositories/working-with-files/managing-large-files/installing-git-large-file-storage

## Unity Editor Version: **2022.3.20f1**
- Type `git clone https://github.com/CS5520FeinbergSpring2024/team-project-a6-group9.git`
- Click **Add -> Add project from disk** in Unity, the necessary libraries will be downloaded automatically

## Project UI
Buttons - Asset LazyDay


## For each scene/level - adjust following
    - Pause - Window - Content - Restart Button: adjust to current scene index
    - Game Over - Window - Content - Restart Button: adjust to current scene index
    - Game Over Time (if exist): same as above
    - You Win - Window - Content - Next Button: adjust to next scene index
