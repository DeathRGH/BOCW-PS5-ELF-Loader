namespace BOCW_PS5
{
    internal class Addresses
    {
        public static ulong baseAddr = 0;

        public static readonly ulong version_01_26_check = 0x0000000001F3B4CE;

        public static readonly ulong Cbuf_AddText_Buffer = 0x0000000007E438C0;
        public static readonly ulong Cbuf_AddText_Trigger = Cbuf_AddText_Buffer + 0x1F404;

        public static readonly ulong DrawClientInfo = 0x00000000002108E0;

        public static readonly ulong ElfLoaderBufferAddr = DrawClientInfo;

        // we leave 1 byte for a return when elf is not running
        public static readonly ulong ElfLoaderElfFileBufferAddr = DrawClientInfo + 1;
    }
}
