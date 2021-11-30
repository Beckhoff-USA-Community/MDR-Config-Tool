using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCatSysManagerLib;
using EnvDTE;
using System.IO;
using System.Diagnostics;
using System.Collections;
using System.Runtime.InteropServices;
using System.Xml;
using System.Xml.Linq;

using TwinCAT.Ads;
using System.Windows.Forms;
using System.Data;
using System.Text.RegularExpressions;

namespace MDRConfigTool
{
    public class SolutionHandler
    {
        private  ITcSysManager11 sysMan;
        private  EnvDTE.DTE dte;
        private  EnvDTE.Project pro;
        private  EnvDTE.Solution sol;

        private  TcAdsClient adsClient;
        private  string declarations;

        private  string BASEFOLDER;
        private  string TcTemplatePath = @"C:\TwinCAT\3.1\Components\Base\PrjTemplate\TwinCAT Project.tsproj";
        private string _solutionName;
        private string _tcProjectName;
        private int _nDriveCount;
        private  string ProgID = "TcXaeShell.DTE.15.0";


        //This contructor is meant to be used for testing individual methods
        //It should never be called in a live use 
        public SolutionHandler()
        {
            dte = attachToExistingDte(@"C:\Users\NathanM\Desktop\DELETE ME\Garth Gaddy\TestDirectory\Test4.sln",ProgID);
            sol = dte.Solution;
            pro = sol.Projects.Item(1);
            sysMan = (ITcSysManager11)pro.Object;
            adsClient = new TcAdsClient();
            ITcSmTreeItem drive = sysMan.LookupTreeItem(@"TIID^Device_1^InfeedWest0");

            LinkIoToPlc(drive);

            

        }
        public SolutionHandler(string filePath, string FileName)
        {
            this.BASEFOLDER = filePath;
            this._solutionName = _tcProjectName = FileName;
            this.adsClient = new TcAdsClient();
            MessageFilter.Register();

            /* -----------------------------------------------------------------
             * Creates new solution based off of TwinCAT's XAE and PLC templates.
             * -----------------------------------------------------------------*/ 
            if (dte == null) CreateNewProject();

            pro = sol.Projects.Item(1);
            sysMan = (ITcSysManager11)pro.Object;
           
            //System.Threading.Thread.Sleep(5000);
            ITcSmTreeItem plc = sysMan.LookupTreeItem("TIPC");
            plc.CreateChild("Untitled1", 0, "", "Standard PLC Template");
            //---Navigate to References node and cast to Library Manager interface
            ITcSmTreeItem references = sysMan.LookupTreeItem("TIPC^Untitled1^Untitled1 Project^References");
            ITcPlcLibraryManager libraryManager = (ITcPlcLibraryManager)this.RetrieveLibMan();

            /* ------------------------------------------------------
             * Check to see if library already exists, if it does it will delete before adding to avoid any exceptions.
             * ------------------------------------------------------ */

            foreach (ITcPlcLibRef libraryReference in libraryManager.References)
            {
                if (libraryReference is ITcPlcLibrary)
                {
                    ITcPlcLibrary library = (ITcPlcLibrary)libraryReference;
                    if (library.Name == "MDR_Control")
                    {
                        DeleteLibrary(ref libraryManager);
                    }
                }
            }
            /* ------------------------------------------------------
             * Add our MDR library to the project References.
             * ------------------------------------------------------ */
            AddLibrary(ref libraryManager);

            MessageFilter.Revoke();
        }//Constructor()
  

        public void CreateNewProject()
        {


            /* ------------------------------------------------------
             * Create Visual Studio instance and make VS window visible
             * ------------------------------------------------------ */
            Type t = System.Type.GetTypeFromProgID(ProgID);
            dte = (EnvDTE.DTE)System.Activator.CreateInstance(t);
            dte.SuppressUI = false;
            dte.UserControl = true; // true = leaves VS window open after code execution
            dte.MainWindow.Visible = true;

            /* ------------------------------------------------------
             * Create directories for new Visual Studio solution
             * ------------------------------------------------------ */
            //Helper.DeleteDirectory(BASEFOLDER);
            Directory.CreateDirectory(BASEFOLDER);
            Directory.CreateDirectory(BASEFOLDER);

            /* ------------------------------------------------------
             * Create and save new solution
             * ------------------------------------------------------ */
            sol = dte.Solution;
            sol.Create(BASEFOLDER, _solutionName + ".sln");
            sol.SaveAs(BASEFOLDER + "\\" + _solutionName);

            /* ------------------------------------------------------
             * Create new TwinCAT project, based on TwinCAT Project file (delivered with TwinCAT XAE)
             * ------------------------------------------------------ */
            pro = sol.AddFromTemplate(TcTemplatePath, BASEFOLDER + "\\" + _tcProjectName, _tcProjectName);
           

        }


        public void PLCdeclarations(DataTable dt)
        {

            string mainVar = @"VAR_GLOBAL" + Environment.NewLine;
            string endMain = Environment.NewLine + @"          
END_VAR";
            string constDeclStart = @"VAR_GLOBAL CONSTANT" + Environment.NewLine;
            string constDeclEnd = Environment.NewLine + @"          
END_VAR";
            createDriveStruct();
            System.Threading.Thread.Sleep(3000);
            //Add function blocks to MAIN declaration
            ITcSmTreeItem main = sysMan.LookupTreeItem("TIPC^Untitled1^Untitled1 Project^GVLs");
            ITcSmTreeItem GVL = main.CreateChild("MDR_Control_FBs", 615, "", 0);
            ITcSmTreeItem StructGVL = main.CreateChild("EP7402_List", 615, "", 0);
            //Cast to specific interface for declaration and implementation area
            ITcPlcDeclaration mainDecl = (ITcPlcDeclaration)GVL;
            ITcPlcDeclaration structDecl = (ITcPlcDeclaration)StructGVL;



            //Get current declaration and implementation area content
            string strMainDecl = mainDecl.DeclarationText;
            string strStructDecl = structDecl.DeclarationText;
            //string strMainImpl = mainImpl.ImplementationText;
            _nDriveCount = dt.Rows.Count;
            string[] boxName = new string[_nDriveCount];
            int j = 0;
            int i = 0;

            string structCount = string.Format(" nDriveCount : UINT := {0};" ,_nDriveCount);
            for (i = 0; i < _nDriveCount - 1; i++)
            {
                boxName[i] = dt.Rows[j].ItemArray[0].ToString() + j.ToString();
                //Console.WriteLine(boxName[i]);
                j++;
            }
            
            
            declarations = string.Join(": FB_MDR_Control;" + Environment.NewLine, boxName);
            mainDecl.DeclarationText = mainVar + declarations + endMain;
            
            for (i = 0; i < _nDriveCount - 1;i++)
            {
                boxName[i] = boxName[i].Insert(0,"st");
            }
            

            declarations = string.Join(": ST_EP7402;" +Environment.NewLine, boxName);
            structDecl.DeclarationText = mainVar + declarations + endMain + Environment.NewLine + constDeclStart + structCount + constDeclEnd;
            createFBCalls(boxName);

            System.Threading.Thread.Sleep(2000);
            var settings = dte.GetObject("TcAutomationSettings");
            settings.SilentMode = true;
            dte.Solution.SolutionBuild.Build(true); // compile the plc project
            settings.SilentMode = false;
        }

        public  void AddLibrary(ref ITcPlcLibraryManager libraryManager)
        {
           

            string libPath = buildPathString("MDR_Control.library");

            //---Install library into new repository
            libraryManager.InstallLibrary("System", libPath, true); //--library must exists in TwinCAT folder.
            //--- libpath needs to be added to basefolder in this install command (needs full path to folder + MDR_Control.library

            //---Add library from repository
            libraryManager.AddLibrary("MDR_Control", "1.0", "BAUS");
        }

        public void DeleteLibrary(ref ITcPlcLibraryManager libraryManager)
        {
            //---Create new repo path
            //string newRepoPath = @"C:\TwinCAT\3.1\Components\Plc\Managed Libraries\BAUS";

            //---Remove Library from references 
            libraryManager.RemoveReference("MDR_Control");

            //---Uninstall library from repository
            libraryManager.UninstallLibrary("System", "MDR_Control", "1.0", "BAUS");

            //---Remove repository from system    
          
        }

        public void linkVariables(DataTable dt)
        {
            string[] boxName = new string[dt.Rows.Count];
            int j = 0;
            int i = 0;
            string plcInputsPath = "TIPC^Untitled1^Untitled1 Instance^PlcTask Inputs";
            string plcOutputsPath = "TIPC^Untitled1^Untitled1 Instance^PlcTask Outputs";

            for (i = 0; i < dt.Rows.Count - 1; i++)
            {
                boxName[i] = dt.Rows[j].ItemArray[0].ToString() + j.ToString();
                if (i >= 1)
                {
                    this.sysMan.LinkVariables(plcInputsPath + "^MDR_Control_FBs." + boxName[i - 1] + ".stInput^SlugMode_From_Upstream_Zone", plcOutputsPath + "^MDR_Control_FBs." + boxName[i] + ".stOutput^SlugMode_To_Downstream_Zone");
                    this.sysMan.LinkVariables(plcInputsPath + "^MDR_Control_FBs." + boxName[i - 1] + ".stInput^Wakeup_From_Upstream_Zone", plcOutputsPath + "^MDR_Control_FBs." + boxName[i] + ".stOutput^Wakeup_To_Downstream_Zone");
                    this.sysMan.LinkVariables(plcInputsPath + "^MDR_Control_FBs." + boxName[i] + ".stInput^RTR_From_Downstream_Zone", plcOutputsPath + "^MDR_Control_FBs." + boxName[i - 1] + ".stOutput^RTR_To_Upstream_Zone");
                    this.sysMan.LinkVariables(plcInputsPath + "^MDR_Control_FBs." + boxName[i] + ".stInput^Occupied_From_Downstream_Zone", plcOutputsPath + "^MDR_Control_FBs." + boxName[i - 1] + ".stOutput^Occupied_To_Upstream_Zone");
                }

                j++;
            }

            



        }

        public void activateAndRunPLC()
        {
            
            adsClient.Connect(sysMan.GetTargetNetId(), 10000); //AMS net id of target system. TwinCAT system service port = 10000
            System.Threading.Thread.Sleep(10000); //waiting for TwinCAT to go into Run mode.
            sysMan.ActivateConfiguration();
            sysMan.StartRestartTwinCAT();
            try
            {
                bool rundo = true;
                do
                {
                    if (adsClient.ReadState().AdsState == AdsState.Run) //reads TwinCAT's system state, if it is in run mode then log into plc.
                    {
                        string xml = "<TreeItem><IECProjectDef><OnlineSettings><Commands><LoginCmd>true</LoginCmd><StartCmd>true</StartCmd></Commands></OnlineSettings></IECProjectDef></TreeItem>";
                        ITcSmTreeItem plcProject = sysMan.LookupTreeItem("TIPC^Untitled1^Untitled1 Project");
                        plcProject.ConsumeXml(xml);
                        rundo = false;
                    }
                } while (rundo);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public  EnvDTE.DTE attachToExistingDte(string solutionPath, string progId)
        {
            EnvDTE.DTE dte = null;
            try
            {

                Hashtable dteInstances = Helper.GetIDEInstances(false, progId);
                IDictionaryEnumerator hashtableEnumerator = dteInstances.GetEnumerator();

                while (hashtableEnumerator.MoveNext())
                {
                    EnvDTE.DTE dteTemp = (EnvDTE.DTE)hashtableEnumerator.Value;
                    if (dteTemp.Solution.FullName == solutionPath)
                    {
                        Console.WriteLine("Found solution in list of all open DTE objects. " + dteTemp.Name);
                        dte = dteTemp;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return dte;
        }


        public void ScanDevicesAndBoxes(DataTable dt)
        {
            int config;

            if ((config = checkForRunMode()) == 1)
            {
                ITcSmTreeItem ioDevicesItem = sysMan.LookupTreeItem("TIID"); // Get The IO Devices Node

                // Scan Devices (Be sure that the target system is in Config mode!)
                string scannedXml = ioDevicesItem.ProduceXml(false); // Produce Xml implicitly starts the ScanDevices on this node.

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(scannedXml); // Loads the Xml data into an XML Document
                XmlNodeList xmlDeviceList = xmlDoc.SelectNodes("TreeItem/DeviceGrpDef/FoundDevices/Device");
                List<ITcSmTreeItem> devices = new List<ITcSmTreeItem>();

                //local variables for our spreadsheet writing statements
                int deviceCount = 0;

                int i = 2;

                // Add all found devices to configuration
                foreach (XmlNode node in xmlDeviceList)
                {
                    // Add a selection or subrange of devices
                    int itemSubType = int.Parse(node.SelectSingleNode("ItemSubType").InnerText);
                    string typeName = node.SelectSingleNode("ItemSubTypeName").InnerText;
                    XmlNode xmlAddress = node.SelectSingleNode("AddressInfo");

                    ITcSmTreeItem device = ioDevicesItem.CreateChild(string.Format("Device_{0}", ++deviceCount), itemSubType, string.Empty, null);

                    string xml = string.Format("<TreeItem><DeviceDef>{0}</DeviceDef></TreeItem>", xmlAddress.OuterXml);
                    device.ConsumeXml(xml); // Consume Xml Parameters (here the Address of the Device)
                    devices.Add(device);
                }


                // Scan all added devices for attached boxes
                foreach (ITcSmTreeItem device in devices)
                {
                    string xml = "<TreeItem><DeviceDef><ScanBoxes>1</ScanBoxes></DeviceDef></TreeItem>"; // Using the "ScanBoxes XML-Method"
                    try
                    {
                        device.ConsumeXml(xml); // Consume starts the ScanBoxes and inserts every found box/terminal into the configuration
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Warning: {0}", ex.Message);
                    }
                    int j = 0;
                    foreach (ITcSmTreeItem Box in device)
                    {


                        if (Box.ItemSubTypeName.Contains("EP7402-0057"))
                        {
                            Box.Name = dt.Rows[j].ItemArray[0].ToString() + j.ToString();
                            this.EditParams(Box, dt.Rows[j].ItemArray[2].ToString());
                            j++;
                            LinkIoToPlc(Box);
                        }




                    }//for each Box
                } //end of foreach loops
            }
        }//ScanDevicesandBoxes()

        public void SetNetId(string NetId)
        {
            //finding the open form so we can get the textbox value
            if (NetId.Equals("127.0.0.1.1.1"))
            {
                NetId = AmsNetId.Local.ToString();
            }
            //sets the target to the textbox value.
            sysMan.SetTargetNetId(NetId);
            
        }//SetNetId()

        public void ActivateConfiguration()
        {
            //    adsClient = new TcAdsClient();
            //    adsClient.Connect(10000);
            //   adsClient.WriteControl(new StateInfo(AdsState.Start, adsClient.ReadState().DeviceState));
            sysMan.ActivateConfiguration();
            sysMan.StartRestartTwinCAT();
        }   //ActivateConfiguration()

        public void EditParams(ITcSmTreeItem drive, string filename)
        {
            //Create the filepath to the motor file using the path provided to load the excel spreadsheet.
            string file = filename;
            string path = buildPathString(file);

            //open the Motor file as an xml document,read its tags with InitCmds
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(path);
            XmlNode xNode = xDoc.SelectSingleNode("/EtherCATMailbox/CoE/InitCmds");

            string driveParams = drive.ProduceXml();
            XmlNodeList xmlInitCmds = xNode.ChildNodes;
            foreach (XmlNode Cmd in xmlInitCmds)
            {
                string newStartListParam = "<" + Cmd.Name + ">";
                foreach (XmlNode node in Cmd)
                {
                    newStartListParam += "<" + node.Name + ">";
                    newStartListParam += node.InnerText;
                    newStartListParam += "</" + node.Name + ">";
                }
                newStartListParam += "</" + Cmd.Name+ ">";
                int idx = driveParams.IndexOf("</InitCmd></InitCmds><CanOpenType>55</CanOpenType></CoE></Mailbox>");
                idx = idx + 10;
                driveParams = driveParams.Insert(idx, newStartListParam);
            }
            drive.ConsumeXml(driveParams);

        }//EditParams

        public string buildPathString(string file)
        {
            
            string path = BASEFOLDER;
            path += "\\" + file;

            return path;
        }


        public ITcSmTreeItem RetrieveLibMan()
        {
            ITcSmTreeItem references = sysMan.LookupTreeItem("TIPC^Untitled1^Untitled1 Project^References");
            return references;
        }

        private bool isLinkingCompatible(ITcSmTreeItem drive)
        {
            /*--------------------------------
             * Check the that the passed drive has a hardware version newer than the beta version
             * make sure that we can read the revision version of the drive through its xml file
             * find the index of the xml node for the CoE parameter for Hardwareversion 16#1009
             * Check if the value of that node is of Hardware version 01(?) or higher.
             * The last beta revision number is 1114169
             * -------------------------------*/

            ulong lastBeta = 1114169;
            string driveParams = drive.ProduceXml();
            int idxStart = driveParams.LastIndexOf("<RevisionNo>"), idxEnd = driveParams.IndexOf("</RevisionNo>");
            ulong revisionNo; ulong.TryParse(driveParams.Substring(idxStart, (idxEnd - idxStart)),out revisionNo);

            if(revisionNo > lastBeta)
            {
                return true;
            }
            else
            {
                return false;
            }
            

        }//isLinkingCompatible()



        private void CreatePLCVariable()
       {
            /*-------------------------------------------
             * This Method should be used to create thhesary PLC variable within the PLC project.
             * The process should simulate the act of opening the drive's node in TwinCAt, openning the PLC tab,
             * and then checking the create PLC DataTyp box.
             * For now, we should see if we can get the necessary xml nodes from the drive to use the create Datatype method
             * of the ITcTypeSystem interface.
             * ------------------------------------------*/
         //   ITcTypeSystem TS = sysMan.GetTypeSystem();
          //      TS.CreateType();
       }//CreatePLCVariable()

        private void LinkIoToPlc(ITcSmTreeItem drive)
        {
            /*------------------------------------------
             * This is a placehiolder method for the code needed to link our drives to the PLC.
             * When we say PLC we mean explicitly and exclusively the PLC, no NC task/configuration
             * can be used.
             * -----------------------------------------*/


            string plcInputsPath = "TIPC^Untitled1^Untitled1 Instance^PlcTask Inputs";
            string plcOutputsPath = "TIPC^Untitled1^Untitled1 Instance^PlcTask Outputs";
            string drivePath = drive.PathName;
            string boxName = drive.Name;

            // Create links between the current drive and the PLC structure

            //Link the inputs
            this.sysMan.LinkVariables(drivePath + "^DI Inputs^Input 1", plcInputsPath + "^EP7402_List.st" + boxName + ".stInputs^Input_1");
            this.sysMan.LinkVariables(drivePath + "^DI Inputs^Input 2", plcInputsPath + "^EP7402_List.st" + boxName + ".stInputs^Input_2");
            this.sysMan.LinkVariables(drivePath + "^DI Inputs^Input 3", plcInputsPath + "^EP7402_List.st" + boxName + ".stInputs^Input_3");
            this.sysMan.LinkVariables(drivePath + "^DI Inputs^Input 4", plcInputsPath + "^EP7402_List.st" + boxName + ".stInputs^Input_4");
            this.sysMan.LinkVariables(drivePath + "^DI Inputs^Control input 1", plcInputsPath + "^EP7402_List.st" + boxName + ".stInputs^Control_input_1");
            this.sysMan.LinkVariables(drivePath + "^DI Inputs^Control input 2", plcInputsPath + "^EP7402_List.st" + boxName + ".stInputs^Control_input_2");
            this.sysMan.LinkVariables(drivePath + "^DI Inputs^Control input 3", plcInputsPath + "^EP7402_List.st" + boxName + ".stInputs^Control_input_3");
            this.sysMan.LinkVariables(drivePath + "^DI Inputs^Control input 4", plcInputsPath + "^EP7402_List.st" + boxName + ".stInputs^Control_input_4");
            this.sysMan.LinkVariables(drivePath + "^STM Status Channel 1^Status", plcInputsPath + "^EP7402_List.st" + boxName + ".stInputs^Channel_1_Status");
            this.sysMan.LinkVariables(drivePath + "^STM Status Channel 2^Status", plcInputsPath + "^EP7402_List.st" + boxName + ".stInputs^Channel_2_Status");
            this.sysMan.LinkVariables(drivePath + "^STM Synchron info data Channel 1^Info data 1", plcInputsPath + "^EP7402_List.st" + boxName + ".stInputs^Channel_1_info_data_1");
            this.sysMan.LinkVariables(drivePath + "^STM Synchron info data Channel 1^Info data 2", plcInputsPath + "^EP7402_List.st" + boxName + ".stInputs^Channel_1_info_data_2");
            this.sysMan.LinkVariables(drivePath + "^STM Synchron info data Channel 1^Info data 3", plcInputsPath + "^EP7402_List.st" + boxName + ".stInputs^Channel_1_info_data_3");
            this.sysMan.LinkVariables(drivePath + "^STM Synchron info data Channel 2^Info data 1", plcInputsPath + "^EP7402_List.st" + boxName + ".stInputs^Channel_2_info_data_1");
            this.sysMan.LinkVariables(drivePath + "^STM Synchron info data Channel 2^Info data 2", plcInputsPath + "^EP7402_List.st" + boxName + ".stInputs^Channel_2_info_data_2");
            this.sysMan.LinkVariables(drivePath + "^STM Synchron info data Channel 2^Info data 3", plcInputsPath + "^EP7402_List.st" + boxName + ".stInputs^Channel_2_info_data_3");
            this.sysMan.LinkVariables(drivePath + "^STM Inputs Device^Device Diag", plcInputsPath + "^EP7402_List.st" + boxName + ".stInputs^Device_Diag");

            //Link the Outputs
            this.sysMan.LinkVariables(drivePath + "^DO Outputs^Output 1", plcOutputsPath + "^EP7402_List.st" + boxName + ".stOutputs^Output_1");
            this.sysMan.LinkVariables(drivePath + "^DO Outputs^Output 2", plcOutputsPath + "^EP7402_List.st" + boxName + ".stOutputs^Output_2");
            this.sysMan.LinkVariables(drivePath + "^DO Outputs^Output 3", plcOutputsPath + "^EP7402_List.st" + boxName + ".stOutputs^Output_3");
            this.sysMan.LinkVariables(drivePath + "^DO Outputs^Output 4", plcOutputsPath + "^EP7402_List.st" + boxName + ".stOutputs^Output_4");
            this.sysMan.LinkVariables(drivePath + "^DO Outputs^Control output 1", plcOutputsPath + "^EP7402_List.st" + boxName + ".stOutputs^Control_output_1");
            this.sysMan.LinkVariables(drivePath + "^DO Outputs^Control output 2", plcOutputsPath + "^EP7402_List.st" + boxName + ".stOutputs^Control_output_2");
            this.sysMan.LinkVariables(drivePath + "^DO Outputs^Control output 3", plcOutputsPath + "^EP7402_List.st" + boxName + ".stOutputs^Control_output_3");
            this.sysMan.LinkVariables(drivePath + "^DO Outputs^Control output 4", plcOutputsPath + "^EP7402_List.st" + boxName + ".stOutputs^Control_output_4");
            this.sysMan.LinkVariables(drivePath + "^STM Control Channel 1^Control", plcOutputsPath + "^EP7402_List.st" + boxName + ".stOutputs^Channel_1_Control");
            this.sysMan.LinkVariables(drivePath + "^STM Control Channel 2^Control", plcOutputsPath + "^EP7402_List.st" + boxName + ".stOutputs^Channel_2_Control");
            this.sysMan.LinkVariables(drivePath + "^STM Target Velocity Channel 1^Velocity", plcOutputsPath + "^EP7402_List.st" + boxName + ".stOutputs^Channel_1_Velocity");
            this.sysMan.LinkVariables(drivePath + "^STM Target Velocity Channel 1^Accelleration", plcOutputsPath + "^EP7402_List.st" + boxName + ".stOutputs^Channel_1_Accelleration");
            this.sysMan.LinkVariables(drivePath + "^STM Target Velocity Channel 1^Decceleration", plcOutputsPath + "^EP7402_List.st" + boxName + ".stOutputs^Channel_1_Decceleration");
            this.sysMan.LinkVariables(drivePath + "^STM Target Velocity Channel 2^Velocity", plcOutputsPath + "^EP7402_List.st" + boxName + ".stOutputs^Channel_2_Velocity");
            this.sysMan.LinkVariables(drivePath + "^STM Target Velocity Channel 2^Accelleration", plcOutputsPath + "^EP7402_List.st" + boxName + ".stOutputs^Channel_2_Accelleration");
            this.sysMan.LinkVariables(drivePath + "^STM Target Velocity Channel 2^Decceleration", plcOutputsPath + "^EP7402_List.st" + boxName + ".stOutputs^Channel_2_Decceleration");
            

        }//LinkIoToPlc()


        private void createDriveStruct()
        {
            //create strings for the beginning and end of the struct declaration
            string structStart = "TYPE ST_EP7402:" + Environment.NewLine + "STRUCT" + Environment.NewLine;
            string structEnd = "END_STRUCT" + Environment.NewLine + "END_TYPE" + Environment.NewLine;

            //create the DUT and assign it to a container
            ITcSmTreeItem DUT = sysMan.LookupTreeItem("TIPC^Untitled1^Untitled1 Project^DUTs");
            ITcSmTreeItem driveStruct = DUT.CreateChild("ST_EP7402", 606, "");
            ITcPlcDeclaration structDecl = (ITcPlcDeclaration)driveStruct;

            string sdeclarationText;

            //create a string for generating the member declaration of the struct.
            string structMembers = @"stInputs   AT %I*  :   EP7402_Inputs;" + Environment.NewLine +
                                   "stOutputs   AT %Q*  :   EP7402_Outputs;" + Environment.NewLine;

            //combine the string into a single string to create the declaration text
            sdeclarationText = structStart + structMembers + structEnd;
            structDecl.DeclarationText = sdeclarationText;

        }

        private void createFBCalls( string[] boxName)
        {
            //acuire project instance of MAIN Pou
            //then tell TwinCAT to create 3 new Actions for MAIN
            ITcSmTreeItem mainPOU = sysMan.LookupTreeItem("TIPC^Untitled1^Untitled1 Project^POUs^MAIN");
            ITcSmTreeItem fbBlockAct = mainPOU.CreateChild("A_Block",608,"");
            ITcSmTreeItem fbInitAct = mainPOU.CreateChild("A_Init", 608, "");

            ITcPlcPou pouMain = (ITcPlcPou)mainPOU;
                       

            ITcPlcImplementation actBlock = (ITcPlcImplementation)fbBlockAct;
            ITcPlcImplementation actInit = (ITcPlcImplementation)fbInitAct;
           
            ITcPlcImplementation pouMainImpl = (ITcPlcImplementation)pouMain;

            ITcPlcDeclaration pouMainDecl = (ITcPlcDeclaration)pouMain;

            generateMainDecl(ref pouMainDecl, ref boxName);
            generateMainImpl(ref pouMainImpl);
            generateBlockAct(ref actBlock, ref boxName);
            generateInitAct(ref actInit, ref boxName);
            



        }

        private void generateMainDecl(ref ITcPlcDeclaration mainDecl, ref string[] boxName)
        {
            string sDeclStart = @"PROGRAM MAIN
VAR" + Environment.NewLine;

            string sDeclEnd = Environment.NewLine + "END_VAR" + Environment.NewLine;

            string sCommonVars = @"
    xEnable         : BOOL;
    xInitFlag		: BOOL;
	xRun			: BOOL;
	xStart			: BOOL;
	xStop			: BOOL;
    xEmergencyStop	: BOOL;
	xClearError		: BOOL;
	iIndex			: INT;
	lrTempReal		: LREAL;
	pTempBool		: POINTER TO BOOL;
	pTempDint		: POINTER TO DINT;
	xVisibility		: BOOL;
	artrigEvent		: ARRAY [1..5] OF r_trig;
	iAnimationIndex	: INT;
	xEmpty			: BOOL;
	xSimulate		: BOOL;
    uiIndex			: UINT;
	uiModeCycle		: UINT;
	xZoneTriggered	: BOOL;
	uiZoneActive	: UINT;
	iSetVelocity	: INT	:= 1000;
	xFwdRev			: BOOL;
	iCmdVelocity	: INT;
    xConfigInit			: BOOL := FALSE;
	tmrWebStartDelay	: TON;
	xTemp: BOOL;";

            //Create formats for declaring new variables for each drive in the list
            string sUniqueVars = "";
            string feedsFormat = "{0}Feed			: ARRAY [1..2] OF BOOL;" + Environment.NewLine;
            string zpaSettingsFormat = "a{0}ZpaSettings     : ARRAY[1..2] OF ST_ZpaSettings;" + Environment.NewLine;
            string zoneFormat = "a{0}Out		: ARRAY [1..2] OF ST_ZoneOut;" + Environment.NewLine;
            string mdrSettingsFormat = "{0}MdrSettings      : ST_Mdr_Settings;" + Environment.NewLine;

            //generate the variables needed for each function block 
            for (int i = 0; i < _nDriveCount- 1; i++)
            {
                sUniqueVars += string.Format(feedsFormat, boxName[i]);
                sUniqueVars += string.Format(zpaSettingsFormat, boxName[i]);
                sUniqueVars += string.Format(zoneFormat, boxName[i]);
                sUniqueVars += string.Format(mdrSettingsFormat, boxName[i]);
                sUniqueVars += Environment.NewLine;
            }

            string sDeclarationText = sDeclStart + sCommonVars + sUniqueVars + sDeclEnd;

            mainDecl.DeclarationText = sDeclarationText;



        }//GenerateMainDecl()

        private void generateMainImpl(ref ITcPlcImplementation mainImpl)
        {
            string sImplemenetationText = @"
IF NOT xInitFlag THEN
xInitFlag := TRUE;
A_Init();
ELSE
A_Block();
END_IF

IF xStart THEN
    xRun := TRUE;
ELSIF xStop THEN
    xRun := FALSE;
END_IF";
            mainImpl.ImplementationText = sImplemenetationText;
        }

        private void generateInitAct(ref ITcPlcImplementation actImpl,ref string[] boxName)
        {
            string sImplementationTextFormat = @"
    {0}MdrSettings.xDI_Invert_Input_1:= FALSE;// invert zone 1 PE
    {0}MdrSettings.xDI_Invert_Input_3:= FALSE;// invert zone 2 PE
    {0}MdrSettings.xReverse_Motor_1:= FALSE;
    {0}MdrSettings.xReverse_Motor_2:= false;

    a{0}ZpaSettings[1].xEnable:= TRUE;
    a{0}ZpaSettings[1].iPeOffDelay:= 10;
    a{0}ZpaSettings[1].iPeOnDelay:= 10;
    a{0}ZpaSettings[1].iReleaseDelay:= 0;
    a{0}ZpaSettings[1].iZoneTraverseTime:= 1000;
    a{0}ZpaSettings[1].iPowerSaveTimeout:= 300;
    a{0}ZpaSettings[1].iMinimumVelocity:= 400;
    a{0}ZpaSettings[1].iReleaseJamTime:= 5000;
    a{0}ZpaSettings[1].iReleaseVelocity:= 1200;
    a{0}ZpaSettings[1].iReceiveVelocity:= 1000;
    a{0}ZpaSettings[1].uiAccel:= 1000;
    a{0}ZpaSettings[1].uiDecel:= 5000;
    a{0}ZpaSettings[1].eZpaMode:= e_ZpaMode.eSlugMode;

    a{0}ZpaSettings[2].xEnable:= TRUE;
    a{0}ZpaSettings[2].iPeOffDelay:= 10;
    a{0}ZpaSettings[2].iPeOnDelay:= 10;
    a{0}ZpaSettings[2].iReleaseDelay:= 0;
    a{0}ZpaSettings[2].iZoneTraverseTime:= 1000;
    a{0}ZpaSettings[2].iPowerSaveTimeout:= 300;
    a{0}ZpaSettings[2].iMinimumVelocity:= 400;
    a{0}ZpaSettings[2].iReleaseJamTime:= 5000;
    a{0}ZpaSettings[2].iReleaseVelocity:= 1200;
    a{0}ZpaSettings[2].iReceiveVelocity:= 1000;
    a{0}ZpaSettings[2].uiAccel:= 1000;
    a{0}ZpaSettings[2].uiDecel:= 5000;
    a{0}ZpaSettings[2].eZpaMode:= e_ZpaMode.eSingulationMode;"+ Environment.NewLine;

            string sImplementationText = "";
            for(int i = 0;i < _nDriveCount-1; i++)
            {
                sImplementationText += string.Format(sImplementationTextFormat, boxName[i]);
            }

            actImpl.ImplementationText = sImplementationText;

        }//GenerateInitAct()
        

        private void generateBlockAct(ref ITcPlcImplementation actImpl, ref string[] boxName)
        {
            string sImplementationTextFormat = @"{1}(
                    i_xEnable:= xEnable,
                    i_xStartCmd:= xRun,
                    i_xClearError:= xClearError,
                    i_axWakeUp:= {0}Feed,
                    i_astZpaSettings:= a{0}ZpaSettings,
                    i_stMdrSettings:= {0}MdrSettings,
                    i_pstEP7402:= ADR({0}));" + Environment.NewLine;

            string sImplementationText = "";
            for (int i = 0; i < _nDriveCount - 1; i++)
            {
                sImplementationText += string.Format(sImplementationTextFormat, boxName[i], boxName[i].Remove(0,2));
            }//for

            actImpl.ImplementationText = sImplementationText;


        }//generateBlockAct()

        private int checkForRunMode()
        {
            int result = 1;
            try
            {
                
                adsClient.Connect(sysMan.GetTargetNetId(), 10000);
                AdsErrorCode adsError;
                StateInfo state;
                AdsState adsState;
                if ((adsError = adsClient.TryReadState(out state)) == 0)
                {
                    if(( adsState = state.AdsState)== AdsState.Run)
                    {
                        state.AdsState = AdsState.Config;
                        adsClient.WriteControl(state);
                    }

                    
                }
                else
                {
                    throw new Exception("Could not read Ads State of Target, AdsError code: " + adsError);
                }

            }catch(Exception ex)
            {
                result = -1;
                MessageBox.Show(ex.Message);
            }
            adsClient.Disconnect();
            return result;
        }
       

    }//class

   

  

}//namespace
