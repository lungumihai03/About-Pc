using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.IO;
using System.Diagnostics;
using Microsoft.Win32;

namespace About_Pc
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string cpuInfo = GetCPUInfo();
            label5.Text = cpuInfo;

            string ramInfo = GetRAMInformation();
            label6.Text = ramInfo;

            string gpuInfo = GetGPUInfo();
            label7.Text = gpuInfo;

            string romInfo = GetROMInfo();
            label8.Text = romInfo;

            string osInfo = GetOSInfo();
            label11.Text = osInfo;

            string motherboardInfo = GetMotherboardInfo();
            label13.Text = motherboardInfo;
        }


        private string GetCPUInfo()
        {
            string cpuInfo = string.Empty;
            ManagementClass mc = new ManagementClass("win32_processor");
            ManagementObjectCollection moc = mc.GetInstances();

            foreach (ManagementObject mo in moc)
            {
                string name = mo.Properties["Name"].Value.ToString();
                int cores = Convert.ToInt32(mo.Properties["NumberOfCores"].Value);
                int threads = Convert.ToInt32(mo.Properties["NumberOfLogicalProcessors"].Value);
                cpuInfo += $"{name} (Cores: {cores}, Threads: {threads})";
                break;
            }

            return cpuInfo;
        }



        private string GetRAMInformation()
        {
            string ramInfo = string.Empty;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMemory");

            foreach (ManagementObject mo in searcher.Get())
            {
                string capacity = mo["Capacity"] != null ? BytesToGB(long.Parse(mo["Capacity"].ToString())) : "Unknown";
                string speed = mo["Speed"]?.ToString();
                string memoryType = mo["SMBIOSMemoryType"] != null ? GetSMBIOSMemoryTypeString(int.Parse(mo["SMBIOSMemoryType"].ToString())) : "Unknown";

                
                string formFactor = mo["FormFactor"] != null ? GetFormFactorString(int.Parse(mo["FormFactor"].ToString())) : "Unknown";
                string manufacturer = mo["Manufacturer"]?.ToString();
                string partNumber = mo["PartNumber"]?.ToString();

                ramInfo +=
                        $"{capacity} GB, " +
                        $"{memoryType}, " +
                        $"{speed} MHz, " +
                        $"{formFactor} " + $"{partNumber}\n";
            }
            return ramInfo.Trim();
        }

        private string BytesToGB(long bytes)
        {
            double gb = bytes / (1024.0 * 1024.0 * 1024.0);
            return gb.ToString("0.##");
        }
        private string BytesToMB(long bytes)
        {
            double gb = bytes / (1024.0 * 1024.0);
            return gb.ToString("0.##");
        }
        private string GetSMBIOSMemoryTypeString(int memoryType)
        {
            switch (memoryType)
            {
                case 24: return "DDR3";
                case 25: return "FBD2";
                case 26: return "DDR4";
                // adaugă alte cazuri după nevoie
                default: return "Unknown";
            }
        }


       

        private string GetFormFactorString(int formFactor)
        {
            switch (formFactor)
            {
                case 1:
                    return "Other";
                case 2:
                    return "SIP";
                case 3:
                    return "DIP";
                case 4:
                    return "ZIP";
                case 5:
                    return "SOJ";
                case 6:
                    return "Proprietary";
                case 7:
                    return "SIMM";
                case 8:
                    return "DIMM";
                case 9:
                    return "TSOP";
                case 10:
                    return "PGA";
                case 11:
                    return "RIMM";
                case 12:
                    return "SODIMM";
                case 13:
                    return "SRIMM";
                case 14:
                    return "SMD";
                case 15:
                    return "SSMP";
                case 16:
                    return "QFP";
                case 17:
                    return "TQFP";
                case 18:
                    return "SOIC";
                case 19:
                    return "LCC";
                case 20:
                    return "PLCC";
                case 21:
                    return "BGA";
                case 22:
                    return "FPBGA";
                case 23:
                    return "LGA";
                default:
                    return "Unknown";
            }
        }


        private string GetGPUInfo()
        {
            string gpuInfo = string.Empty;
            ManagementClass mc = new ManagementClass("win32_videocontroller");
            ManagementObjectCollection moc = mc.GetInstances();

            foreach (ManagementObject mo in moc)
            {
                string name = mo.Properties["Name"].Value.ToString();
                string vram = mo.Properties["AdapterRAM"].Value != null ? BytesToMB(long.Parse(mo.Properties["AdapterRAM"].Value.ToString())) : "Unknown";
                gpuInfo += $"{name} (VRAM: {vram} MB)" + Environment.NewLine;
            }

            return gpuInfo.Trim();
        }



        private string GetROMInfo()
        {
            string romInfo = string.Empty;
            ManagementClass mc = new ManagementClass("win32_logicaldisk");
            ManagementObjectCollection moc = mc.GetInstances();

            foreach (ManagementObject mo in moc)
            {
                string driveType = mo.Properties["DriveType"].Value.ToString();
                string driveLetter = mo.Properties["Name"].Value.ToString();
                string driveLabel = mo.Properties["VolumeName"].Value.ToString();
                string driveFileSystem = mo.Properties["FileSystem"].Value.ToString();
                ulong freeSpace = Convert.ToUInt64(mo.Properties["FreeSpace"].Value);
                string freeSpaceGB = (freeSpace / (1024 * 1024 * 1024)).ToString(); // Convert free space to GB

                if (driveType == "2" || driveType == "3") // DriveType 2 is for removable drives
                {
                    string driveTypeString = GetDriveTypeString(driveLetter);
                    romInfo += $"{driveLetter} ({driveLabel}) - Free Space: {freeSpaceGB} GB,  File System: {driveFileSystem}" + Environment.NewLine;
                }
            }

            return romInfo.Trim();
        }


        private string GetDriveTypeString(string driveLetter)
        {
            string driveTypeString = "Unknown";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");

            foreach (ManagementObject mo in searcher.Get())
            {
                string deviceId = mo["DeviceID"].ToString();
                string driveLetterId = deviceId.Substring(4, 1);

                if (driveLetter.Equals(driveLetterId, StringComparison.OrdinalIgnoreCase))
                {
                    foreach (ManagementObject partition in mo.GetRelated("Win32_DiskPartition"))
                    {
                        foreach (ManagementObject logicalDisk in partition.GetRelated("Win32_LogicalDisk"))
                        {
                            if (logicalDisk["DeviceID"].ToString().Equals(driveLetter, StringComparison.OrdinalIgnoreCase))
                            {
                                string mediaType = mo["MediaType"].ToString().ToLower();

                                if (mediaType.Contains("ssd"))
                                {
                                    driveTypeString = "SSD";
                                }
                                else if (mediaType.Contains("hdd"))
                                {
                                    driveTypeString = "HDD";
                                }
                                else
                                {
                                    driveTypeString = "Unknown";
                                }

                                break;
                            }
                        }
                    }
                    break;
                }
            }

            return driveTypeString;
        }








        private string GetOSInfo()
        {
            ManagementObjectSearcher mos = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
            ManagementObjectCollection moc = mos.Get();

            string osInfo = string.Empty;
            foreach (ManagementObject mo in moc)
            {
                osInfo = mo["Caption"].ToString() + " " + mo["Version"].ToString();
                break;
            }

            return osInfo;
        }
        private string GetMotherboardInfo()
        {
            string motherboardInfo = string.Empty;
            ManagementClass mc = new ManagementClass("win32_baseboard");
            ManagementObjectCollection moc = mc.GetInstances();

            foreach (ManagementObject mo in moc)
            {
                motherboardInfo = mo.Properties["Manufacturer"].Value.ToString() + " " +
                                   mo.Properties["Product"].Value.ToString();
                break;
            }

            return motherboardInfo;
        }

    }
}
