# PC Information Viewer

## Overview
This application is a Windows Forms-based tool developed in C# that retrieves and displays detailed hardware and system information about a Windows PC. It uses the `System.Management` namespace to query system components via WMI (Windows Management Instrumentation). The tool is designed for users who need a quick overview of their system's specifications, such as CPU, RAM, GPU, storage, operating system, and motherboard details, displayed in a simple graphical interface.

## Features
- **CPU Information**: Displays the processor name, number of cores, and threads.
- **RAM Information**: Lists details for each RAM module, including capacity (in GB), memory type (e.g., DDR3, DDR4), speed (MHz), form factor (e.g., DIMM, SODIMM), and part number.
- **GPU Information**: Shows the graphics card name and VRAM capacity (in MB).
- **Storage Information**: Provides details for logical disks (e.g., HDD, SSD), including drive letter, label, free space (in GB), and file system type.
- **Operating System Information**: Displays the OS name and version.
- **Motherboard Information**: Shows the motherboard manufacturer and product name.
- **Automatic Data Retrieval**: Populates all system information on application startup.

## Usage
1. **Run the Application**: Launch the application in a Windows environment with the .NET Framework installed.
2. **View System Information**: Upon startup, the application automatically queries and displays the following in labeled fields:
   - **CPU**: Processor details (name, cores, threads).
   - **RAM**: Memory module details (capacity, type, speed, form factor, part number).
   - **GPU**: Graphics card details (name, VRAM).
   - **ROM**: Storage details (drive letter, label, free space, file system).
   - **OS**: Operating system name and version.
   - **Motherboard**: Manufacturer and product name.
3. **Review Output**: All information is displayed in pre-configured labels on the form. No user input is required.
4. **Error Handling**: The application handles missing or unavailable data gracefully, displaying "Unknown" for any properties that cannot be retrieved.
