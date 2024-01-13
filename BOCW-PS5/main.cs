using BOCW_PS5.Properties;
using libdebug;

namespace BOCW_PS5
{
    public partial class main : Form
    {
        public static PS5DBG ps5;
        public static int processId = -1;
        public static uint section0Length = 0;

        Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

        void UpgradeUserSettings()
        {
            if (Settings.Default.UPDATE_REQUIRED)
            {
                Settings.Default.Upgrade();
                Settings.Default.UPDATE_REQUIRED = false;
                Settings.Default.Save();
            }
        }

        bool IsIPAddress(string check)
        {
            if (System.Net.IPAddress.TryParse(check, out _))
                return true;
            else
                return false;
        }

        void ChangeIPAdress(string newIp)
        {
            if (IsIPAddress(newIp))
            {
                Settings.Default.IP = newIp;
                Settings.Default.Save();

                ps5 = new PS5DBG(newIp);
            }
        }

        void SetLog(string text)
        {
            Invoke((MethodInvoker)delegate
            {
                label_log.Text = text + "\n";
                label_log.Update();
            });
        }

        void AppendLog(string appendText)
        {
            Invoke((MethodInvoker)delegate
            {
                label_log.Text += appendText + "\n";
                label_log.Update();
            });
        }

        void ResetCursorToDefault()
        {
            Invoke((MethodInvoker)delegate
            {
                Cursor = Cursors.Default;
            });
        }

        public main()
        {
            InitializeComponent();

            // updated settings if we had a previous release of the tool
            UpgradeUserSettings();
        }

        private void main_Load(object sender, EventArgs e)
        {
            this.Text += $" [{version}]";
            box_ps5_ip.Text = Settings.Default.IP;
        }

        private void box_ps5_ip_TextChanged(object sender, EventArgs e)
        {
            ChangeIPAdress(box_ps5_ip.Text);
        }

        void ShowFailedToGetProcessError()
        {
            AppendLog("Unable to find the game process, make sure it is running!");
            MessageBox.Show("Unable to find the game process, make sure it is running!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        void ShowFailedToGetEntryError()
        {
            AppendLog("Unable to find entry point!");
            MessageBox.Show("Unable to find entry point!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btn_connect_Click(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                Invoke((MethodInvoker)delegate
                {
                    Cursor = Cursors.WaitCursor;
                    btn_loadElf.Enabled = false;
                    btn_unloadElf.Enabled = false;
                    btn_reloadElf.Enabled = false;

                    // check if we have a valid ip
                    if (string.IsNullOrEmpty(Settings.Default.IP))
                    {
                        ResetCursorToDefault();
                        AppendLog("No IP found in settings!");
                        MessageBox.Show("No IP found in settings!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    SetLog($"Connecting to {Settings.Default.IP}...");
                });

                ps5 = new PS5DBG(Settings.Default.IP);

                // connect to console
                try
                {
                    ps5.Connect();
                    SetLog($"Connected to {Settings.Default.IP}!\n");
                }
                catch
                {
                    ResetCursorToDefault();
                    AppendLog($"Unable to connect to PS5 at {Settings.Default.IP}!");
                    MessageBox.Show($"Unable to connect to PS5 at {Settings.Default.IP}!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // get process id
                try
                {
                    AppendLog("Searching for game process...");
                    ProcessList pl = ps5.GetProcessList();
                    libdebug.Process p = pl.FindProcess("eboot.bin");
                    if (p == null)
                    {
                        ResetCursorToDefault();
                        ShowFailedToGetProcessError();
                        return;
                    }
                    processId = p.pid;
                    AppendLog($"Successfully found game process eboot.bin with pid {p.pid}!\n");
                }
                catch
                {
                    ResetCursorToDefault();
                    ShowFailedToGetProcessError();
                    return;
                }

                // get the process base address
                try
                {
                    AppendLog("Searching for entry point...");
                    ProcessMap pmap = ps5.GetProcessMaps(processId);
                    MemoryEntry me = pmap.FindEntry("executable");

                    if (me == null)
                    {
                        ResetCursorToDefault();
                        ShowFailedToGetEntryError();
                        return;
                    }
                    else if (me.prot >= 4) // 5 ?
                    {
                        Addresses.baseAddr = me.start;
                        section0Length = (uint)(me.end - me.start);
                        AppendLog($"Successfully found entry point 0x{Addresses.baseAddr:X}!\n");
                    }
                    else
                    {
                        ResetCursorToDefault();
                        ShowFailedToGetEntryError();
                        return;
                    }
                }
                catch
                {
                    ResetCursorToDefault();
                    ShowFailedToGetEntryError();
                    return;
                }

                // protect the first section with prot 7 so the elf can write to it
                try
                {
                    AppendLog("Setting up memory protection...");
                    ps5.ChangeProtection(processId, Addresses.baseAddr, section0Length, PS5DBG.VM_PROTECTIONS.VM_PROT_ALL);
                    AppendLog("Successfully set up memory protections!\n");
                }
                catch
                {
                    ResetCursorToDefault();
                    AppendLog("Unable to find the game process, make sure it is running!");
                    MessageBox.Show("Unable to find the game process, make sure it is running!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try
                {
                    AppendLog("Checking application for correct version...");
                    if (ps5.ReadString(processId, Addresses.baseAddr + Addresses.version_01_26_check, 0x100).Contains("Multiplayer"))
                    {
                        AppendLog("Verified version number 1.26, you are good to go!\n\nAll Done :)");
                    }
                    else
                    {
                        ResetCursorToDefault();
                        AppendLog("Wrong game or update version detected!\nOnly 1.26 is supported right now.");
                        MessageBox.Show("Wrong game or update version detected!\nOnly 1.26 is supported right now.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                catch
                {
                    ResetCursorToDefault();
                    AppendLog("Unable to check update version!");
                    MessageBox.Show("Unable to check update version!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Invoke((MethodInvoker)delegate
                {
                    btn_loadElf.Enabled = true;
                    btn_unloadElf.Enabled = true;
                    Cursor = Cursors.Default;
                });
            });
        }

        void Cbuf_AddText(string text)
        {
            ps5.WriteString(processId, Addresses.baseAddr + Addresses.Cbuf_AddText_Buffer, text + '\0');
            ps5.WriteInt32(processId, Addresses.baseAddr + Addresses.Cbuf_AddText_Trigger, text.Length + 1);
        }

        private void btn_cbuf_Click(object sender, EventArgs e)
        {
            Cbuf_AddText(box_cbuf.Text);
        }

        private void btn_loadElf_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            ELF.lastElfPath = ofd.FileName;
            ELF.LoadELF(ps5, processId, ofd.FileName);
            btn_reloadElf.Enabled = true;
        }

        private void btn_unloadElf_Click(object sender, EventArgs e)
        {
            btn_reloadElf.Enabled = false;
            ELF.UnloadELF(ps5, processId);
        }

        private void btn_reloadElf_Click(object sender, EventArgs e)
        {
            if (!File.Exists(ELF.lastElfPath))
            {
                MessageBox.Show("The last used elf file was deleted or moved!\nBrowse for it again by using \"Load ELF...\".", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            ELF.ReloadELF(ps5, processId, ELF.lastElfPath);
        }
    }
}
