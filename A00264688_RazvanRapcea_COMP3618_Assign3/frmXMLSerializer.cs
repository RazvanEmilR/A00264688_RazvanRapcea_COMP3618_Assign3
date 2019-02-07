using A00264688_RazvanRapcea_COMP3618_Assign3.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace A00264688_RazvanRapcea_COMP3618_Assign3
{
    public partial class frmXMLSerializer : Form
    {
        public frmXMLSerializer()
        {
            InitializeComponent();
        }

        private void buttonSerialize_Click(object sender, EventArgs e)
        {
            var emp = new Employee();

            if (!string.IsNullOrEmpty(textBoxEmployeeID.Text))
                emp.EmployeeID = Int32.Parse(textBoxEmployeeID.Text);

            if (!string.IsNullOrEmpty(textBoxFirstName.Text))
                emp.FirstName = textBoxFirstName.Text;

            if (!string.IsNullOrEmpty(textBoxLastName.Text))
                emp.LastName = textBoxLastName.Text;

            if (!string.IsNullOrEmpty(textBoxHomePhone.Text))
                emp.HomePhone = textBoxHomePhone.Text;

            if (!string.IsNullOrEmpty(textBoxNotes.Text))
                emp.Notes = textBoxNotes.Text;

            SerializeToXml(emp);

            if (checkBoxDisplay.Checked)
            {
                DisplayXMLFileInBrowserWindow();
            }
                


        }

        private void SerializeToXml(Employee emp)
        {
            FileStream fs = new FileStream("Employee.xml", FileMode.Create);

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Employee));

            xmlSerializer.Serialize(fs, emp);

            fs.Close();
        }

        private void DisplayXMLFileInBrowserWindow()
        {
            System.Diagnostics.Process.Start("iexplore.exe", Path.GetFullPath("Employee.xml"));
        }

        private Employee DeserializeFromXml()
        {
            FileStream fs = new FileStream("Employee.xml", FileMode.Open);

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Employee));

            Employee result = (Employee) xmlSerializer.Deserialize(fs);

            fs.Close();

            return result;
        }

        private void buttonDeserialize_Click(object sender, EventArgs e)
        {
            try
            {
                ValidateXmlFile();

                Employee emp = DeserializeFromXml();

                textBoxEmployeeID.Text = emp.EmployeeID.ToString();
                textBoxFirstName.Text = emp.FirstName;
                textBoxLastName.Text = emp.LastName;
                textBoxHomePhone.Text = emp.HomePhone;
                textBoxNotes.Text = emp.Notes;
            }
            catch (Exception ex)
            {
                labelErrors.Visible = true;
                labelErrors.Text = ex.Message;
            }
            
        }

        private void ValidateXmlFile()
        {       
            var path = new Uri(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase)).LocalPath;
            XmlSchemaSet schema = new XmlSchemaSet();
            schema.Add("", path + "\\Employee.xsd");
            XmlReader rd = XmlReader.Create(path + "\\Employee.xml");
            XDocument doc = XDocument.Load(rd);
           
            doc.Validate(schema, ValidationEventHandler);
        }

        static void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            XmlSeverityType type = XmlSeverityType.Warning;
            if (Enum.TryParse<XmlSeverityType>("Error", out type))
            {
                if (type == XmlSeverityType.Error) throw new Exception(e.Message);
            }
        }

        private void frmXMLSerializer_Load(object sender, EventArgs e)
        {
            labelErrors.Visible = false;
        }
    }
}
