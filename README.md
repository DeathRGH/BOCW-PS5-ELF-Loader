# Black Ops Cold War PS5 ELF Loader

This is a custom elf loader built for Black Ops Cold War on PlayStation5.
Currently supporting update version 1.26 of the game which runs on a PS5 with firmware 4.03 or higher.


For a example project to load, take a look at:
https://github.com/DeathRGH/BOCW-1.26-ELF-PS5

## How It Works
As the current elf loader implementation in ps5debug simply doesn't work or turns your game into a potato I decided to port my elf loader from the really early ps4 days.
It doesn't work in the classic sense where you would hook game functions. It injects the raw bytes of the elf into a leftover debug drawing function.
The leftover function is executed once per frame on the render thread of the game.
This gives us access to write memory in the text section, render anything with engine rendering and even make a basic aimbot.

In order for the game to let us write to the text section we need to change the protection to Read-Write-Execute (7).
This is done with ps5debug which writes the protection level to the virtual memory map in the kernel data section.

After all the setup is done we can load and unload a elf as many times as we want without restarting the game.


### The tool does the following steps when you connect:
1. Connect to PS5.
2. Find the process "eboot.bin".
3. Get the virtual memory map of the process to find the base address.
4. Protect the text section of the eboot with prot 7.
5. Reading a unique string from the games text section to verify we are on the correct update version.


### The actual elf loading part:
1. We need to grab the bytes of the elfs text section that we want to load. We do this by searching for the string "/lib64/ld-linux-x86-64" which marks the end of all the data we need.
  The start can be found by simply reading the start address of the text section from the elf header.
2. To stop the original function from executing while we write our injected bytes we add a return at the start of the function.
3. After injecting the elf bytes we patch the injected elf to call the entry instead of jumping to it and a few other patches (see code for details).
4. As the last step we remove our return at the start of the function by replacing it with a nop. This will now execute our elf.

## Credits

- Sistro (ps5debug)
