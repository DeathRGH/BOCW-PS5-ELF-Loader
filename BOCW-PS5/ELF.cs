using libdebug;

namespace BOCW_PS5
{
    internal class ELF
    {
        // elf loader currently supports 0x5F0E bytes in size (including data section)
        // to expand the size if needed patch the call to 00000000002167F0 LobbyDebug_DrawInGame
        // 0000000000F1D569   call LobbyDebug_DrawInGame
        // this would free up another 0xAB6 bytes
        // can be further expanded by patching calls to the next few functions down in memory

        // to save path for reloading
        public static string lastElfPath = "";

        private static byte[] GetElfAsm(string path, ref bool failedWithMsg)
        {
            byte[] fileBytes = File.ReadAllBytes(path);

            // this array marks the end of the text and static data section
            // /lib64/ld-linux-x86-64
            int[] offsetFoundList = fileBytes.Locate(new byte[] { 0x2F, 0x6C, 0x69, 0x62, 0x36, 0x34, 0x2F, 0x6C, 0x64, 0x2D, 0x6C, 0x69, 0x6E, 0x75, 0x78, 0x2D, 0x78, 0x38, 0x36, 0x2D, 0x36, 0x34 });

            if (offsetFoundList.Length <= 0)
            {
                failedWithMsg = true;
                MessageBox.Show("Invalid file selected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new byte[0];
            }

            int start = BitConverter.ToInt32(fileBytes, 0x48); // 0x48 = elf code section start

            if (start <= 0)
            {
                failedWithMsg = true;
                MessageBox.Show("Invalid file selected, could not find code section!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new byte[0];
            }

            int size = offsetFoundList[0] - start;

            byte[] finalInjectArray = new byte[size];
            Array.Copy(fileBytes, start, finalInjectArray, 0, size);

            return finalInjectArray;
        }

        public static void LoadELF(PS5DBG ps5, int pid, string elfPath)
        {
            // unload first to be safe
            UnloadELF(ps5, pid);

            try
            {
                // write actual elf file to buffer
                bool failedWithMsg =  false;
                byte[] elfBytes = GetElfAsm(elfPath, ref failedWithMsg);
                if (elfBytes == null || elfBytes.Length <= 0)
                {
                    if (!failedWithMsg) // only display message box if we haven't displayed one in the "GetElfAsm" function
                    {
                        MessageBox.Show("Invalid file selected, file is null or empty!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    return;
                }

                ps5.WriteMemory(pid, Addresses.baseAddr + Addresses.ElfLoaderElfFileBufferAddr, elfBytes);

                // the elf starts with a jump to our main function:
                // jmp _main
                // we want to replace the jump with a call so it returns to the rip automatically after each loop
                // jmp _main -> call _main
                ps5.WriteByte(pid, Addresses.baseAddr + Addresses.ElfLoaderElfFileBufferAddr, 0xE8); // E9 -> E8

                // to return to the function that DrawClientInfo was called from we insert a return after the call to main
                // we add 5 to our elf file entry address to skip the call _main which is always 5 bytes
                ps5.WriteByte(pid, Addresses.baseAddr + Addresses.ElfLoaderElfFileBufferAddr + 5, 0xC3);

                // to have access to the base address we use the api to grab it and write it to the elf memory
                // in the example below it is qword_26E1 which is 8 nops
                // to get our relative address we take the int from the call/jmp which is located at +1
                // see actual elf project for reference
                //
                // .text:; int __cdecl main(int argc, const char** argv, const char** envp)
                // .text:                 public _main
                // .text: _main proc near
                // .text:   endbr64
                // .text:   lea rax, qword_26E1
                // .text:   jmp qword ptr cs:sub_26E9
                // .text: _main           endp
                // .text:
                // .text: qword_26E1 dq 9090909090909090h
                // .text:
                // .text: sub_26E9 proc near
                // .text:   mov rax, [rax]
                // .text:   mov     cs:baseAddress, rax
                // .text:   retn
                // .text: sub_26E9 endp
                //
                int callOffset = ps5.ReadInt32(pid, Addresses.baseAddr + Addresses.ElfLoaderElfFileBufferAddr + 1);

                // on top of the offset we add the rip with is at +5
                // this will take us to the _main
                // now we have to skip another 0x11 bytes to reach qword_26E1 which will be used for the base address to be written to
                int baseWriteOffset = callOffset + 5 + 0x11;
                // now we write the process base address to the qword from the example
                ps5.WriteUInt64(pid, Addresses.baseAddr + Addresses.ElfLoaderElfFileBufferAddr + (ulong)baseWriteOffset, Addresses.baseAddr);

                // now we replace the fake jump in our elf (jmp [rip + 8]) with a relative one
                // the jump is at 0x0B in the _main so we can re-use our calculation but with 0x0B instead of 0x11 this time
                int jmpWriteOffset = callOffset + 5 + 0x0B;
                // now we write the jump
                ps5.WriteMemory(pid, Addresses.baseAddr + Addresses.ElfLoaderElfFileBufferAddr + (ulong)jmpWriteOffset, new byte[] { 0xEB, 0x0C });

                // finally we overwrite the return (C3) at the start of the original function with a simple nop
                // this will start executing the loop in our elf
                ps5.WriteByte(pid, Addresses.baseAddr + Addresses.ElfLoaderBufferAddr, 0x90);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Load ELF Error:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void UnloadELF(PS5DBG ps5, int pid)
        {
            try
            {
                // write return to original function to ignore any injected code
                ps5.WriteByte(pid, Addresses.baseAddr + Addresses.ElfLoaderBufferAddr, 0xC3);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unload ELF Error:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void ReloadELF(PS5DBG ps5, int pid, string elfPath, int waitTime = 100)
        {
            UnloadELF(ps5, pid);
            Thread.Sleep(waitTime);
            LoadELF(ps5, pid, elfPath);
        }
    }
}
