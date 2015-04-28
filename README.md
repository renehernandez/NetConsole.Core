# NetConsole.Core

NetConsole.Core is a library who aims to serve as a basis for developing projects that uses command-like syntax.

## Basic Usage
For the simplest usage, go and use *CommandManager* class. It could load commands from raw string or a file using one of the following methods defined in it and returns an output from the command execution.

#### Reading Commands

Reading from string:
```csharp
var importer = new CommandManager(); // Loads all commands automatically from assemblies in current domain
var output = importer.GetOutputFromString("echo Hello World"); // Reading command from string
```
Or from file:
```csharp
var importer = new CommandManager();
var output = importer.GetOutputFromFile("C:\\Users\\Aegis\\Documents\\hello.txt"); // Reading command from a file
```

#### Working with output
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
By default  command classes with *NotRegistrable* class attribute will not be registered in the *CommandFactory* and *CommandManager* types.

**Notice:** You can always add a command instance to *CommandFactory*, even if it is marked with *NotRegistrable* attribute.

## Advanced Usage
See *CommandExtractor* and *CommandFactory* types to have a fine-grained control over the registered commands and their usage.

## Manual Installation
Install the latest version from [NuGet] (https://www.nuget.org/packages/NetConsole.Core/).

## Contributing
In lieu of a formal style guide, please take care to follow the existing code style. Add unit tests for any new or changed functionality. Use FAKE and paket to handle automation and dependencies management.

## Release History
Checks [Release Notes](https://github.com/renehernandez/NetConsole.Core/blob/develop/RELEASE_NOTES.md) to see the changes history
