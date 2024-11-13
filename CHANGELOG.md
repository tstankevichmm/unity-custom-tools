# Changelog

All notable changes to this package will be documented in this file.

## [1.0.48]
Added a callGlobal parameter to local event buses.

## [1.0.47]
Fixed some UnityEditor errors with building.

## [1.0.46]
Added an ID to the float modifier.

Also added functions to search and remove by this ID.

## [1.0.45]
Removing the float and int classes and replaced the system with a better ModifiableFloat class

## [1.0.44]
Adding a ModifierObservableValue class with base float and int classes.

This class is an extension of the Observable value class that incorporates the ability to add modifiers onto the value.

## [1.0.43]
Added Animation Event System.

Introduced an animation event system based on Unity's AnimationStateBehaviours

## [1.0.42]
Added another overload to the DestroyChildren function that will allow for ignoring transforms.

## [1.0.41]
Added the ability to register/deregister globally directly from an EventBinding

## [1.0.40]
Add Timer and CountdownTimer classes

Introduce a base Timer class with start, stop, pause, and resume functionality. Added CountdownTimer subclass implementing a countdown mechanism, including a reset function. These utilities are part of the custom tools for Unity

## [1.0.39]
Change Debug.LogError to Debug.LogWarning for non-critical issues

This update changes `Debug.LogError` to `Debug.LogWarning` in `ServiceManager.cs` and `ServiceLocator.cs` for scenarios where the issues are non-critical and do not require immediate error attention. This will make the log output clearer and categorize the warnings appropriately without disrupting the error logs.

## [1.0.38]
Refactor service registration functions.

Removed redundant generic overloads for service registration across ServiceLocator, ServiceManager, and ServiceLocatorHelperGlobal classes. This streamlines the API, reducing potential confusion and maintaining consistency in method signatures.

## [1.0.37]
Log error and change exception to return self

Replaced the throw of ArgumentException with a Debug.LogError call to provide better debugging information. Additionally, modified the method to return the instance instead of throwing an exception when a service is not registered, improving resilience

## [1.0.36]
Fix log message errors in ServiceManager.cs

Corrected improper usage of nameof operator in log strings to ensure accurate type names are printed. This change improves debugging clarity by correctly displaying the full type names in log messages

## [1.0.35]
Fix debug logs in ServiceLocator register and deregister methods

Corrected debug log statements to properly display type names. This change enhances the clarity and accuracy of log outputs for better debugging and monitoring

## [1.0.34]
Add debug logs for service registration and deregistration

Included debug statements to log detailed information when services are registered or deregistered in both ServiceManager and ServiceLocator classes. This will help in better tracking of service lifecycle events for debugging purposes.

## [1.0.33]
Remove default value from Register method's override parameter

The `Register` methods in `ServiceLocator` no longer have a default value for the `overrideCurrent` parameter. This change ensures that the caller must explicitly specify whether existing services should be overridden, reducing potential errors from unintended behavior

## [1.0.32]
Refactor ServiceManager registration logic

Simplify the service registration process by removing unnecessary error logging and ensuring services can be overridden if specified. This enhances readability and ensures the correct error message is displayed when a service fails to register

## [1.0.31]
Add override option to ServiceManager and ServiceLocator registration

Modified the `Register` methods in `ServiceManager` and `ServiceLocator` to include an `overrideCurrent` parameter. This allows previously registered services to be replaced if the override flag is set to true, enhancing the flexibility and control over service management

## [1.0.30]
Add DeRegister method to ServiceManager and ServiceLocator

The DeRegister method allows services to be removed from the ServiceManager and ServiceLocator. This addition includes debug logs for error handling when deregistering non-existent or mismatched services.

## [1.0.29]
Add BasicMonoBehaviourEvents class

A new class, BasicMonoBehaviourEvents, has been added to handle common Unity events. This class provides UnityEvents for standard MonoBehaviour callbacks such as Awake, Start, Enable, Disable, and Destroy. To provide better organization, the class is located within the CustomTools.UnityEvents namespace.

## [1.0.28]
Add Database System script and metadata files

The new files implement a database system that stores and processes data records. The DatabaseSO class is generic, allows for handling different types of records, and provides methods for record updates, cleaning, and ID duplication checks. The other files are related to Unity's system for asset identification and tracking changes during runtime

## [1.0.27]
Update HasCommandLineArgument method in CommandLine class

The HasCommandLineArgument method in the CommandLine class has been updated to also return the following argument data if it exists. Instead of only checking if the argument exists, the method now loops through the arguments, and when it finds a match, it assigns the following argument to a data parameter and returns true.

## [1.0.26]
This new utility, named CommandLine, has been added under the SystemTools. The utility includes a method to check if a certain command line argument exists. It simplifies handling of command line arguments and increases code readability

## [1.0.25]
Wrap VersionProcessor in UNITY_EDITOR directive

The main change in this commit involves wrapping the VersionProcessor class found in VersionSystem directory in a UNITY_EDITOR directive. This ensures that the VersionProcessor class is only compiled and included in the build when testing within the Unity Editor, thereby optimizing the performance and size of the final build.

## [1.0.24]
Wrapped editor scripts in UNITY_EDITOR directives

This commit wraps the ExtendedButton_Inspector.cs and GameSceneSOEditor.cs scripts in UNITY_EDITOR preprocessor directives. This is done to prevent errors when building the application, as certain functions are meant to work strictly in the editor environment

## [1.0.23]
Add UNITY_EDITOR directive to ScriptableObjectExtensions

The UNITY_EDITOR directive was added to the ScriptableObjectExtensions class. This ensures that the class and its methods are only active in the Unity editor, preventing potential issues in the final build.

## [1.0.22]
Add Compressor class for string compression

The commit introduces a new Compressor class in the Compression folder. Compressor provides static methods for compressing and decompressing strings, utilizing UTF8 encoding and GZipStream for the actual compression and decompression process

## [1.0.21]
Add WebTools for custom web requests

This commit adds a new namespace, CustomTools.WebTools, to handle web requests. The new module provides a comprehensive set of functionalities including GET, POST, and DELETE methods for making requests, and parsing responses. The ErrorResponse and WebResponse classes have been added to handle the responses

## [1.0.20]
Add VersionProcessor to Preprocess Build stage

This commit introduces a new VersionProcessor class to the VersionSystem in Unity editor tools. It is designed to preprocess the build, find and validate the current version, and then update it. The automatic handling of versions simplifies the update process and ensures a proper version format before each build

## [1.0.19]
Add scene management scripts and metadata

This commit introduces a set of scripts and metadata files for scene management in Unity. It includes 'GameSceneSO', a scriptable object class for game scenes, and 'GameSceneSOEditor', a custom editor class for modifying scene attributes. Metafiles associated with these classes and their directories were also added

## [1.0.18]
This commit introduces a new SingletonMonoBehaviour class in the Unity custom tools runtime. This class creates a singleton of MonoBehaviour that survives scene changes in Unity Engine. The SingletonMonoBehaviour ensures that only one instance of a MonoBehaviour exists, and this instance is reusable across the entire project.

## [1.0.17]
Added UI extensions and editor features

This commit includes the addition of new files related to Unity UI extensions and various editor features. The changes particularly include 'ExtendedButton.cs' and 'ExtendedButton_Inspector.cs' scripts, along with their respective metadata. Moreover, the 'com.metamonkey.customtools.Editor.asmdef' file was created, detailing assembly definition for the editor's tools.

## [1.0.16]
Add ExtendedButton class in UIExtensions

A new ExtendedButton class has been added to the UIExtensions namespace. This class extends Unity's standard Button with additional functionality including support for right and middle click events. It also includes methods to execute specific code when these events occur

## [1.0.15]
Add SetActive method to GameObjectExtensions

The newly added SetActive method allows an array of GameObjects to be simultaneously set to active or inactive. If isActive is true, it will show all GameObjects within the array, but if it is false, it will hide them.

## [1.0.14]
Add IComponent interface to unity-custom-tools.

This commit introduces a new interface, IComponent, to the unity-custom-tools project. The interface, residing under the HelperInterfaces directory, defines several essential properties and methods like gameObject, transform, and GetComponent, which are staples of many components.

## [1.0.13]
A new class, ScriptableObjectExtensions, has been added to the UnityExtensions namespace.

This includes a new LoadScriptableObject function, which can load ScriptableObject instances using their name. This functionality can aid in retrieving specific ScriptableObjects programmatically within Unity.

## [1.0.12]
Added auto global registration for EventBinding

## [1.0.11]
Introduced changes to the EventBus system to process pending events more efficiently.

## [1.0.10]
The EventBus system was modified to handle actions specific to each event type.

The 'callback' field in the 'EventData' struct and the 'Raise' methods in the 'EventBus' and 'LocalEventBus' classes now take an 'Action<T>', where 'T' is the type of the event, instead of a simple 'Action'.

This allows the callback invoked during event raising to have information about the event that was raised.

## [1.0.9]
Added ToString method in BaseModifier class

## [1.0.8]
Added JoinToString extension method to ListExtensions

## [1.0.7]
A new constructor has been added to the BaseModifier class, allowing it to take an IModifierValue and a source as parameters. The new constructor defaults to a ModType of "Flat".

## [1.0.6]
Fixed an issue with IntWithMod not returning correct total value.

## [1.0.5]
Adding Modifiable Values.

The new ValueWithMod class in the ModifiableValue namespace adds functionality for creating values that can be altered by Modifiers. Alongside this, the BaseModifier class has been implemented to provide the Modifier objects used in the ValueWithMod class.

## [1.0.4]
Added Observer class.

A new Observer namespace was introduced along with ObserverValue class, providing functionality to observe variable changes in Unity.

## [1.0.3]
Added Unity Extensions

## [1.0.2]
Updated Unity Version to use 2022.3

## [1.0.1]
Added Service Locator files

## [1.0.0]
Initial Release