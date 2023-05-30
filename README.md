### Naturally this requres Andraste Modding Framework https://turboduck.net/files/file/544-andraste-modding-framework-development-build/ https://andrasteframework.github.io/content/1.0.0/index.html


With this plugin enabled mod packs can skip Bnk1.map modding for loading additional bnks

Extract the zip and put the directory under Andraste's mod directory, it should look as follow once installed
 
```
TestDriveUnlimited.exe
Launcher.exe
...
mods/05_magicmap/
├── mod.json
└── tdu_andraste_magicmap.dll
...
```

Note that this plugin does not patch bnk size limit so right now for huge bnks one would still need project paradise `-bigbnk`

Patching methodology can be found at https://github.com/Kethen/tdu_andraste_magicmap/blob/main/tdu_andraste_magicmap/MyClass.cs
