using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace BLEConnect
{
    public partial class ServicesForm : Form
    {
        public ServicesForm()
        {
            InitializeComponent();
        }

        public void SetServices(IReadOnlyList<GattDeviceService> services)
        {
            foreach (var service in services)
            {
                listBox1.Items.Add(service.Uuid.ToString());
            }
        }
    }
}
