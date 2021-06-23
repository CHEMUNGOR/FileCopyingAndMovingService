using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Xml;
using System.Data.SqlClient;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.IO;

/// <summary>
/// Namespace for File Moving Service- Created 2018.
/// </summary>
namespace FileCopyingService
{
    public partial class FileCopyingService : ServiceBase
    {
        int TInterval2;
        System.Timers.Timer oTimer = null;
        string LogTransactions, CopyFrom, CopyTo, MoveTo;

        public FileCopyingService()
        {
            InitializeComponent();
            //Create an event log to write to
            if (!System.Diagnostics.EventLog.SourceExists("FileCopyingService"))
            {
                System.Diagnostics.EventLog.CreateEventSource("FileCopyingService", "FileCopyingService");
            }

            eventLog1.Source = "FileCopyingService";
            eventLog1.Log = "FileCopyingService";
        }
        protected override void OnStart(string[] args)
        {

            String ParXMLFile;
            String ParFound = "X";

            // test = ConfigurationManager.ConnectionStrings["Connectionstring"].ToString();
            //Read the parameters XML document.
            //Parameters are stored in an XML document.
            ParXMLFile = @"" + System.AppDomain.CurrentDomain.BaseDirectory + "Parameters.xml";
            XmlReaderSettings mySettings = new XmlReaderSettings();
            try
            {
                using (XmlReader myXmlReader = XmlReader.Create(ParXMLFile, mySettings))
                {
                    while (myXmlReader.Read())
                    {
                        if (myXmlReader.NodeType == XmlNodeType.Element)
                        {
                            ParFound = myXmlReader.Name;

                        }
                        else if (myXmlReader.NodeType == XmlNodeType.Text & !string.IsNullOrEmpty(ParFound))
                        {
                            ParFound = ParFound.ToString();
                            switch (ParFound)
                            {
                                case "ParTInterval2":
                                    TInterval2 = Convert.ToInt32(myXmlReader.Value);
                                    break;
                                case "LogTransactions":
                                    LogTransactions = Convert.ToString(myXmlReader.Value);
                                    break;
                                case "CopyFrom":
                                    CopyFrom = Convert.ToString(myXmlReader.Value);
                                    break;
                                case "CopyTo":
                                    CopyTo = Convert.ToString(myXmlReader.Value);
                                    break;
                                case "MoveTo":
                                    MoveTo = Convert.ToString(myXmlReader.Value);
                                    break;
                            }
                            ParFound = "";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                eventLog1.WriteEntry("FileCopyingService: " + ex.Message, EventLogEntryType.Error, 3);
            }
            //Pass details of the function to run.
            this.oTimer = new System.Timers.Timer();
            this.oTimer.Elapsed += new System.Timers.ElapsedEventHandler(AfterTimeInterval);
            this.oTimer.Enabled = true;
            this.oTimer.Interval = this.TInterval2;

        }
        // This method is called by the timer delegate. 
        public void AfterTimeInterval(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                CheckFolder();
            }
            catch (Exception ex)
            {
                if (LogTransactions == "YES")
                {
                    eventLog1.WriteEntry("FileCopyingService: " + ex.Message, EventLogEntryType.Error, 3);
                }
            }
        }
        public void CheckFolder()
        {
            string[] Files, Sections;
            string fileName, NameF="";

            try
            {

                Files = Directory.GetFiles(CopyFrom);
                if (Files.GetLength(0) > 0)
                {
                    foreach (string myfile in Files)
                    {
                        fileName = myfile;

                        //Copying
                        Sections = fileName.Split('\\');
                        NameF = Sections.GetValue(Sections.GetLength(0) - 1).ToString();
                        if (NameF != ".dropbox" || NameF != "desktop.ini")
                        {
                            if (!File.Exists(CopyTo + '\\' + NameF))
                            {
                                File.Copy(CopyFrom + '\\' + NameF, CopyTo + '\\' + NameF);
                            }

                            //Moving
                            if (!File.Exists(MoveTo + '\\' + NameF))
                            {
                                Directory.Move(CopyFrom + '\\' + NameF, MoveTo + '\\' + NameF);
                            }
                            else
                            {
                                if (File.Exists(MoveTo + '\\' + NameF))
                                {
                                    File.Delete(CopyFrom + '\\' + NameF);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (LogTransactions == "YES")
                {
                    eventLog1.WriteEntry("FileCopyingService: " + ex.Message, EventLogEntryType.Error, 3);
                }
            }
        }
    }
}
