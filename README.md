# AIEquipmentUseFix

**This has been fixed in game version 1.3.3.**

A library mod for [Phantom Brigade](https://braceyourselfgames.com/phantom-brigade/) to patch a bug where attack actions of enemy units will overlap dash actions or enemy units will not wait for the entire duration for weapons tagged with `aihint_stopmovementtofire`.

It is compatible with game release version **1.3**. It works with both the Steam and Epic installs of the game. All library mods are fragile and susceptible to breakage whenever a new version is released.

There have been reports with screenshots showing enemy units planning out actions where an attack overlaps with a dash. Other reports suggested that the enemy units were not waiting as long as they should for weapons that are tagged with `aihint_stopmovementtofire` which is self-evidently meant to stop AI units from moving while firing when using that weapon.

The bug was caused by a change to how equipment use durations were reported to the AI when it was scheduling the actions for the units it controlled. This mod fixes that bug.
