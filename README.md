# IsoTool for Windows

IsoTool is an open source Videogame Tool that makes 2D isometric sprite sheets from 3D models (similar to sprites from the Diablo I and II games). It uses Blender under the hood to generate sprites in various orientations, and a custom texture packer to seal the sprites into a single, fast sprite sheet.  This project compiles and runs on Microsoft Windows.

Iso tool is fully customizable, allowing you to specify the number of model directions to rotate the sprite.  It also provides an API to let you load the sprite sheet into your application.  The tool lso allows you to batch process models for quicker sprite creation.

## Installation

To run IsoTool you need Blender 2.8 installed on your machine.  The tool runs a Blender Python script to rotate the model.  The sprite may not (probably won't) initially fit into the sprite box shown in the tool.  Adjusting the parameters in the Sprite dialog box will adjust the location of the rendered model output.  You can save these settings, then use them in future instances to batch process your models for your game.

## Note

Note here, this code may look ugly because this application was actually deleted a long time ago, and the code wasn't checked into Git (yikes).  The code that is here checked in was decompiled with an IL tool, and edited and modified.  Most of the code checked in here is a direct result of the decompiled code output.  Fortunately much of the decompiler output looks similar to the code that was originally written.  In the future, this application must be cleaned up.

There are many enhancements to be made to the Iso Tool as well, which will allow for a better workflow.


