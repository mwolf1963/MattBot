using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Newtonsoft.Json;

namespace MattBot
{
    public class InfoModule : ModuleBase<SocketCommandContext>
    {
        private String unixHelp;

        [Command("Reminder", RunMode = RunMode.Async)]
        [Summary("Send a Reminder")]
        [RequireContext(ContextType.Guild)]
        public static async Task SendReminder()
        {
            //await Context.C

        }

        [Command("Ping", RunMode = RunMode.Async)]
        [Summary("A simple test")]
        [RequireContext(ContextType.Guild)]
        public async Task Ping()
        {
            await Context.Channel.SendMessageAsync(Context.Channel.ToString());
            await Context.Channel.SendMessageAsync("pong");
        }

        [Command("Vihelp", RunMode = RunMode.Async)]
        [Summary("vi commands")]
        [RequireContext(ContextType.Guild)]
        public async Task ViHelp()
        {
            await Context.Channel.SendMessageAsync(@"vi commands:
i   -insert at the cursor
I   -insert at the beginning of the line
a   -append after the cursor
A   -append at the end of the line
o   -open a line below cursor
O   -open a line above cursor
dd  -cut entire current line
yy   -copy entire current line
p   -paste from dd or yy below current line
P   -paste from dd or yy above current line
D   -cut/delete from cursor to end of line
dw -delete current word  (position cursor on first character
x    -delete one character under cursor
3x  - delete 3 character under cursor
5yy  - copy 5 lines beginning with current line
4dd   -cut 4 lines beginning with current line

ESC :W will write/save current document and remain in vi
ESC :wq will write/save current document then quit to shell
ESC :q! will quit to shell without saving current changes");
        }

        [Command("UnixHelp", RunMode = RunMode.Async)]
        [Summary("unix commands")]
        [RequireContext(ContextType.Guild)]
        public async Task UnixHelp()
        {
            
            await Context.Channel.SendMessageAsync(@"# List all files in a long listing (detailed) format
    ls -al
# Display the present working directory
    pwd
# Create a directory
    mkdir directory
# Remove (delete) file
    rm file
# Remove the directory and its contents recursively
    rm -r directory
# Force removal of file without prompting for confirmation
    rm -f file
# Forcefully remove directory recursively
    rm -rf directory
# Copy file1 to file2
    cp file1 file2
# Copy source_directory recursively to destination. If destination exists, copy source_directory into destination, otherwise create destination with the contents of source_directory.
    cp -r source_directory destination
# Rename or move file1 to file2. If file2 is an existing directory, move file1 into directory file2
    mv file1 file2
# Create symbolic link to linkname
    ln -s /path/to/file linkname
# Create an empty file or update the access and modification times of file.
    touch file
# View the contents of file
    cat file
# Browse through a text file
    less file
# Display the first 10 lines of file
    head file
# Display the last 10 lines of file
    tail file
# Display the last 10 lines of file and ""follow"" the file as it grows.
    tail -f file");
        }

        [Command("MattBot", RunMode = RunMode.Async)]
        [Summary("list bot commands")]
        [RequireContext(ContextType.Guild)]
        public async Task MattBot()
        {
            await Context.Channel.SendMessageAsync(@"List of command:
!unixhelp       -list unix command line commands
!githelp        -list common git commands
!vihelp         -list common vi commands");
        }

        [Command("GitHelp", RunMode = RunMode.Async)]
        [Summary("Git Help")]
        [RequireContext(ContextType.Guild)]
        public async Task GitHelp()
        {
            Console.WriteLine("In git help");
            await Context.Channel.SendMessageAsync(@"Commit changes along with a custom message:
        git commit -m ""(message)""
Commit and add all changes to staging:
    git commit -am ""(message)""
Switch to a commit in the current branch:
    git checkout <commit>
Show metadata and content changes of a commit:
    git show <commit>
Discard all changes to a commit:
    git reset –hard <commit>
Discard all local changes in the directory:
    git reset –hard Head
Show the history of changes:
    git log
Stash all modified files:
    git stash
Retrieve stashed files:
    git stash pop
Empty stash:
    git stash drop
Define a tag:
    git tag (tag_name)
Push changes to origin:
    git push
            ");
        }
    }
}