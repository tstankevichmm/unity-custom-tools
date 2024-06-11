# Changelog

All notable changes to this package will be documented in this file.

## [1.0.8]
Add JoinToString extension method to ListExtensions

## [1.0.7]
A new constructor has been added to the BaseModifier class, allowing it to take an IModifierValue and a source as parameters. The new constructor defaults to a ModType of "Flat".

## [1.0.6]
Fixed an issue with IntWithMod not returning correct total value.

## [1.0.5]
Adding Modifiable Values.

The new ValueWithMod class in the ModifiableValue namespace adds functionality for creating values that can be altered by Modifiers. Alongside this, the BaseModifier class has been implemented to provide the Modifier objects used in the ValueWithMod class.

## [1.0.4]
Added Observer class.<br><br>
A new Observer namespace was introduced along with ObserverValue class, providing functionality to observe variable changes in Unity.

## [1.0.3]
Added Unity Extensions

## [1.0.2]
Updated Unity Version to use 2022.3

## [1.0.1]
Added Service Locator files

## [1.0.0]
Initial Release