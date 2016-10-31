using System;
using System.Windows.Forms;

namespace SpeedyList.Sample {
	public partial class Form1 : Form {
		public Form1() {
			InitializeComponent();
		}

		private void buttonTest_Click(object sender, EventArgs e) {
			IntTester intTester = new IntTester();
			string result = intTester.Start();

			//StringTester stringTester = new StringTester();
			//result += stringTester.Start();

			textBox.Text = result;
		}
	}
}
