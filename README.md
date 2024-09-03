
# BadUsbInterpreter

## Introduction

The `BadUsbInterpreter` is a C# utility designed to simulate keyboard inputs programmatically. It processes command-like input lines to simulate keystrokes, key combinations, delays, and other keyboard interactions on a Windows system. This tool is particularly useful for scripting automated input sequences, such as those used in penetration testing scenarios where a BadUSB device might be involved.

## Table of Contents

- [Introduction](#introduction)
- [Features](#features)
- [Installation](#installation)
- [Usage](#usage)
- [Configuration](#configuration)
- [Dependencies](#dependencies)
- [Examples](#examples)
- [Troubleshooting](#troubleshooting)
- [Contributors](#contributors)
- [License](#license)
- [Version limitation](#versionlimitation)

## Features

- **Command-based Input Simulation**: Execute commands like `STRING`, `DELAY`, `GUI`, `CTRL`, `SHIFT`, `ALT`, and more to simulate keyboard actions.
- **Support for Delays**: Set default or specific delays between commands for more controlled input simulation.
- **Command Repetition**: Repeat the last executed command a specified number of times using the `REPEAT` command.
- **Key Combinations**: Simulate key combinations involving control keys like `CTRL`, `ALT`, `SHIFT`, etc.
- **Customizable Input**: Easily modify the script to add support for additional keys or commands.

## Installation

To use the `BadUsbInterpreter`, follow these steps:

1. **Clone the Repository**:  
   ```bash
   git clone https://github.com/blod22/BadUsbDeveloperAndTester.git
   ```

2. **Build the Project**:  
   Open the project in Visual Studio or use the .NET CLI to build:
   ```bash
   dotnet build
   ```

3. **Run the Application**:  
   Execute the application with your script file as input.

## Usage

You can use `BadUsbInterpreter` by running a script that contains lines of commands. Each command will be executed sequentially to simulate keyboard inputs.

### Example Command Script:
```bash
DEFAULT_DELAY 100
STRING Hello, World!
ENTER
DELAY 500
CTRL C
REPEAT 2
```

### Command Reference:
- **DEFAULT_DELAY `<ms>`**: Sets the default delay between commands.
- **STRING `<text>`**: Types the provided text.
- **DELAY `<ms>`**: Pauses for the specified number of milliseconds.
- **GUI `<key>`**: Simulates the Windows key press with the specified key.
- **CTRL `<key>`**: Simulates a CTRL + `<key>` combination.
- **SHIFT `<key>`**: Simulates a SHIFT + `<key>` combination.
- **ALT `<key>`**: Simulates an ALT + `<key>` combination.
- **REPEAT `<count>`**: Repeats the last command `<count>` times.

## Configuration

You can configure the interpreter by modifying the default delay and other parameters directly in the source code. The `defaultDelay` variable controls the pause between commands, and other configurations can be adjusted according to your needs.

## Dependencies

The project depends on the following libraries:

- **[WindowsInput](https://github.com/michaelnoonan/inputsimulator)**: A library for simulating keyboard and mouse input in .NET applications.

## Examples

### Example 1: Simple Text Input
```bash
STRING Hello, this is a test.
ENTER
```

### Example 2: Using Delays and Repeats
```bash
DEFAULT_DELAY 200
STRING First line.
ENTER
REPEAT 3
STRING This will repeat 3 times.
ENTER
```

## Troubleshooting

- **Invalid Command**: Ensure that each command is spelled correctly and matches the expected syntax.
- **Unhandled Key**: If the script attempts to use a key that is not supported, you'll need to add support for that key in the source code or modify the script.

## Contributors

- [blod22](https://github.com/blod22) - Initial work

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Version Limitation

Current version doesnćt support ALTCHAR, ALTSTRING and ALTCODE commands

