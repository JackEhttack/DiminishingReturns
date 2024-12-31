# Diminishing Returns

___
Tired of certain crewmates insisting that you should **ONLY** go to Titan and Artifice?
___

### So called """Reasons:"""
- "they have the most loot"
- "why are we going to embrion we are going to die for no reason"
- "rend and dine are stupid >:("

Well, this mod gives them reasons to explore the other 11 moons god-emporer Zeekers has lovingly handcrafted after the first quota!

___
## What the mod do:

Adds diminishing returns to scrap generation when visiting the same moon in too small of a time span.

### In addition:
- A flat discount for paid moons to make it more economical.
- Bonus Scrap Anomalies, in other words a bonus to scrap generation on a random moon some days. (_maybe you'll have a reason to go to embrion, maybe_)
- Fully configurable.

To see current levels of diminish and the current bonus moon, use the new terminal command: **TRACKER**
___
If you experience any bugs or incompatibilities, make a GitHub issue describing the problem. ([issues](https://github.com/JackEhttack/DiminishingReturns/issues))
___
### Special Thanks:
- [lethal.wiki](lethal.wiki) for showing me how to make this thing.
___
### Changelog:

### 1.0.9 (2024-12-30)

#### Bug Fixes
* Moon discounts are now properly synced between host and clients. (big thanks to [dopadream](https://thunderstore.io/c/lethal-company/p/dopadream/)!)

### 1.0.8 (2024-10-05)

#### Bug Fixes
* Liquidation can no longer be selected as the bonus moon.
* Visiting the company moon no longer replenishes diminished moons.

### 1.0.7 (2024-09-07)

#### Bug Fixes
* Fixed moon diminishment not properly being read when loading a save.

### 1.0.6 (2024-09-02)

#### Note: Updating to this version will (probably) reset diminishment from older saves!

#### Features
* Increased compatibility with other mods. (namely WeatherRegistry)
* Added config to choose how much a moon is diminished per visit. (100% of max diminishment by default)
* Removed MMHook requirement.

#### Bug Fixes
* Fixed moon diminishment not being reset when switching save files.

### 1.0.5 (2024-08-27)

#### Features
* Moons are no longer regenerated on the same day as they are diminished.

#### Bug Fixes
* Fixed softlock when landing on moon after host leaves and rehosts room.

### 1.0.4 (2024-08-23)

#### Features
* Host client synchronization for moon discounts.

#### Bug Fixes
* Fixed broken config option to change discount for moons.

### 1.0.3 (2024-08-21) ???

#### Oops
* Changed icon to correct spelling of "dimining" to "diminishing".
* No change to the mod.

### 1.0.2 (2024-08-21)

#### Features
* Host client synchronization for **tracker** command.

#### Bug Fixes
* Bonus moons no longer appear in the "reduced moons" category.

### 1.0.1 (2024-07-17)

#### Features
* Added config option for moon discount.

#### Bug Fixes
* Fixed bug that caused the moon discount to be applied repeatedly.
* Made the config actually generate.
* Fixed bug that caused the value of scrap to be a lot higher than it should.

### 1.0.0 (2024-07-17)

#### Features

* Mod released to public.

