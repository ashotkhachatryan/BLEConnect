using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Enumeration;

namespace BLEConnect
{
    public partial class BLEConnect : Form
    {
        readonly string aqsAllBLEDevices = "(System.Devices.Aep.ProtocolId:=\"{bb7bb05e-5972-42b5-94fc-76eaa7084d49}\")";
        readonly string[] requestedBLEProperties = { "System.Devices.Aep.DeviceAddress", "System.Devices.Aep.IsConnected", "System.Devices.Aep.Bluetooth.Le.IsConnectable" };
        List<DeviceInformation> devices = new List<DeviceInformation>();

        DeviceWatcher watcher = null;

        public BLEConnect()
        {
            InitializeComponent();
            connectButton.Enabled = false;

            watcher = DeviceInformation.CreateWatcher(aqsAllBLEDevices, requestedBLEProperties, DeviceInformationKind.AssociationEndpoint);

            watcher.Added += Watcher_Added;
            //watcher.Removed += Watcher_Removed;
            //watcher.Stopped += Watcher_Stopped;
            //watcher.Updated += Watcher_Updated;
            //watcher.EnumerationCompleted += Watcher_EnumerationCompleted;

            watcher.Start();
        }

        private void Watcher_EnumerationCompleted(DeviceWatcher sender, object args)
        {
        }

        private void Watcher_Updated(DeviceWatcher sender, DeviceInformationUpdate args)
        {
            int a = 0;
        }

        private void Watcher_Stopped(DeviceWatcher sender, object args)
        {
        }

        private void Watcher_Removed(DeviceWatcher sender, DeviceInformationUpdate args)
        {
        }

        void Watcher_Added(DeviceWatcher sender, DeviceInformation deviceInfo)
        {
            if (string.IsNullOrEmpty(deviceInfo.Name))
                return;
            
            if (devices.Find(x => x.Id == deviceInfo.Id) == null)
            {
                devices.Add(deviceInfo);
                BLEDeviceInfo devInfo = new BLEDeviceInfo(deviceInfo.Id, deviceInfo.Name);
                this.Invoke((MethodInvoker)(() => listBox1.Items.Add(devInfo)));
            }
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            connectButton.Enabled = false;
            BLEDeviceInfo info = (BLEDeviceInfo)listBox1.SelectedItem;
            ConnectDevice(info.Id);
        }

        async void ConnectDevice(string devId)
        {
            // Note: BluetoothLEDevice.FromIdAsync must be called from a UI thread because it may prompt for consent.
            BluetoothLEDevice bluetoothLeDevice = await BluetoothLEDevice.FromIdAsync(devId);
            if (bluetoothLeDevice != null)
            {
                GattDeviceServicesResult result = await bluetoothLeDevice.GetGattServicesAsync();

                if (result.Status == GattCommunicationStatus.Success)
                {
                    var services = result.Services;
                    ServicesForm servicesForm = new ServicesForm();
                    servicesForm.SetServices(services);
                    servicesForm.ShowDialog();
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
                connectButton.Enabled = true;
        }
    }

    public class BLEDeviceInfo
    {
        public BLEDeviceInfo(string id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public string Id { get; set; }
        public string Name { get; set; }

        override public string ToString()
        {
            return Name;
        }
    }
}
