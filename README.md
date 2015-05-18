# NetConsole.Core

NetConsole.Core is a library who aims to serve as a basis for developing projects that uses command-like syntax.

## Definitions

#### Defining commands
To establish a convention, a **command** is user-defined type that implements *ICommand* interface in NetConsole.Core library.

Now, a **command** can have several actions to be executed and the pattern usage would be like this:
```
commandName:actionName parameters --options (= value)
```
So, suppose it is defined an **echo** command for the application,  NetConsole.Core provides one implementation within the tests suite (see it [here](https://github.com/renehernandez/NetConsole.Core/blob/master/src/tests/NetConsole.Core.Tests/EchoCommand.cs)), the usage would be:
```csharp
echo:echoed Hello World
````
Usage with help option:
```
echo:echoed --help
```
For more information about the syntax check the [grammar file](https://github.com/renehernandez/NetConsole.Core/blob/master/src/app/NetConsole.Core/Grammar/CommandGrammar.g4)

##### Using options
Options are a way to modify the behavior of an action within the current execution, or even the entire command.
There a two ways of declaring options in a command sentence:
```
--optionName
```
This is way for specifying declarative options, used mostly when the option implementation is about *true* or *false* values.
```
--optionName = value
```
This is when the option required a value to be pass in its declaration within the command sentence.

#### Defining actions
An action would be by definition, any public instance method in the user-defined type that implements *ICommand* interface.
This lets room to define another types of methods (static, private, etc.) and hiding them from being used as actions for the command.

It is also possible to set an action as default.
Just apply the *DefaultAction* method attribute to the selected action and then if only the command is written the default action will be invoked.
Following with the previous example, we could rewrite it like:
```csharp
echo Hello World
```
**Notice:**
* This example will work because echoed action has *DefaultAction* attribute applied on it.
* It is possible to set one default action at most per command.

## Defining scripts
A script is a type that implements [IScript] interface, and it is in charge to specify a set of actions to be invoked.

The main point about scripts is they allow a sort of automation of several actions explicited within the script definition.

So far, a script is meant to be used from code behind and it is not exposed to be matched directly from the the grammar usage. However, it could be possible to generate a command that parse a given name and invoked the correspondant script.

## Basic Usage
For the simplest usage, go and use *CommandManager* class. It could load commands from raw string or a file using one of the following methods defined in it and returns an output from the command execution.

Or, it is possible to work with *scripts* by using the *ScriptManager* class. It could execute a script by receiving its name as input.

*Notice:* It is always possible to use any of this command or script types as normal types in any application.

#### Reading Commands

Reading from string:
```csharp
var manager = new CommandManager(); // Loads all commands automatically from assemblies in current domain
var output = manager.GetOutputFromString("echo Hello World"); // Reading command from string
```
Or from file:
```csharp
var manager = new CommandManager();
var output = manager.GetOutputFromFile("C:\\Users\\Aegis\\Documents\\hello.txt"); // Reading command from a file
```

#### Working with the output
The output is an array of ReturnInfo type which contains relative data to each command executed.
It provides data as property like:
 * Status:
   - int type
   - Provides the status of the command execution. 0 means ok, anything else means something went wrong.
 * Output:
   - object type
   - The result from the command execution.
 * Type:
   - string type
   - Represents the runtime type of the output property.

#### Excluding commands from be registered
By default, command classes with *NotRegistrable* class attribute will not be registered in the *CommandFactory*, *CommandCache* and *CommandManager* types.

**Notice:** You can always add a command instance to *CommandFactory*, even if it is marked with *NotRegistrable* attribute.

## Advanced Usage
See *CommandExtractor*, *CommandFactory* and *CommandCache* types to have a fine-grained control over the registered commands and their usage.

## Manual Installation
Install the latest version from [NuGet] (https://www.nuget.org/packages/NetConsole.Core/).

## Contributing
In lieu of a formal style guide, please take care to follow the existing code style. Add unit tests for any new or changed functionality. Use FAKE and paket to handle automation and dependencies management.

## Release History
Checks [Release Notes](https://github.com/renehernandez/NetConsole.Core/blob/master/RELEASE_NOTES.md) to see the changes history
