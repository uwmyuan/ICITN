﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.NetworkAnalyst;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;

namespace ICITN
{
    public partial class Form1 : Form
    {
        private INAContext m_naContext;
        private INAContext m_NAContext;
        double[,] ODM = new double[25, 22];
        #region Main Form Constructor and Setup
        public Form1()
        {
          InitializeComponent();
          txtCutOff.Text = "5";
          lbOutput.Items.Clear();
          cbCostAttribute.Items.Clear();
          ckbShowLines.Checked = false;
          ckbUseRestriction.Checked = false;
          axMapControl.ClearLayers();
          
          //txtWorkspacePath.Text = Application.StartupPath + @"\..\..\..\..\..\Data\SanFrancisco\SanFrancisco.gdb";
          txtWorkspacePath.Text = "C:\\Users\\Yuanyun\\Desktop\\Instance\\SanFrancisco\\SanFrancisco.gdb";
          txtNetworkDataset.Text = "Streets_ND";
          txtFeatureDataset.Text = "Transportation";
          txtInputFacilities.Text = "Hospitals";
          gbServiceAreaSolver.Enabled = false;
        }  

        #endregion

        #region Button Clicks

        private void btnSolve_Click(object sender, EventArgs e)
		{
            this.toolStripStatusLabel1.Text = "Working...";
			this.Cursor = Cursors.WaitCursor;
			lbOutput.Items.Clear();

			ConfigureSolverSettings();

			try
			{
				IGPMessages gpMessages = new GPMessagesClass();
				if (m_naContext.Solver.Solve(m_naContext, gpMessages, null))
					LoadListboxAfterPartialSolve(gpMessages);
				else
					LoadListboxAfterSuccessfulSolve();
			}
			catch (Exception ex)
			{
				lbOutput.Items.Add("Solve Failed: " + ex.Message);
			}

			UpdateMapDisplayAfterSolve();

			this.Cursor = Cursors.Default;
            this.toolStripStatusLabel1.Text = "Done!";
		}

		private void btnLoadMap_Click(object sender, EventArgs e)
		{
			this.Cursor = Cursors.WaitCursor;

			gbServiceAreaSolver.Enabled = false;
			lbOutput.Items.Clear();

			IWorkspace workspace = OpenWorkspace(txtWorkspacePath.Text);
			if (workspace == null)
			{
				this.Cursor = Cursors.Default;
				return;
			}

			INetworkDataset networkDataset = OpenNetworkDataset(workspace, txtFeatureDataset.Text, txtNetworkDataset.Text);
			IFeatureWorkspace featureWorkspace = workspace as IFeatureWorkspace;
			CreateContextAndSolver(networkDataset);

			if (m_naContext == null)
			{
				this.Cursor = Cursors.Default;
				return;
			}

			LoadCostAttributes(networkDataset);

			if (!LoadLocations(featureWorkspace))
			{
				this.Cursor = Cursors.Default;
				return;
			}

			AddNetworkDatasetLayerToMap(networkDataset);
			AddNetworkAnalysisLayerToMap();

			// work around a transparency issue
			IGeoDataset geoDataset = networkDataset as IGeoDataset;
			axMapControl.Extent = axMapControl.FullExtent;
			axMapControl.Extent = geoDataset.Extent;

			if (m_naContext != null) gbServiceAreaSolver.Enabled = true;

			this.Cursor = Cursors.Default;
		}

		#endregion

		#region Set up Context and Solver

		//*********************************************************************************
		// Geodatabase functions
		// ********************************************************************************
		public IWorkspace OpenWorkspace(string strGDBName)
		{
			// As Workspace Factories are Singleton objects, they must be instantiated with the Activator
			var workspaceFactory = System.Activator.CreateInstance(System.Type.GetTypeFromProgID("esriDataSourcesGDB.FileGDBWorkspaceFactory")) as ESRI.ArcGIS.Geodatabase.IWorkspaceFactory;

			if (!System.IO.Directory.Exists(txtWorkspacePath.Text))
			{
				MessageBox.Show("The workspace: " + txtWorkspacePath.Text + " does not exist", "Workspace Error");
				return null;
			}

			IWorkspace workspace = null;
			try
			{
				workspace = workspaceFactory.OpenFromFile(txtWorkspacePath.Text, 0);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Opening workspace failed: " + ex.Message, "Workspace Error");
			}

			return workspace;
		}

		public INetworkDataset OpenNetworkDataset(IWorkspace workspace, string featureDatasetName, string strNDSName)
		{
			// Obtain the dataset container from the workspace
			var featureWorkspace = workspace as IFeatureWorkspace;
			ESRI.ArcGIS.Geodatabase.IFeatureDataset featureDataset = featureWorkspace.OpenFeatureDataset(featureDatasetName);
			var featureDatasetExtensionContainer = featureDataset as ESRI.ArcGIS.Geodatabase.IFeatureDatasetExtensionContainer;
			ESRI.ArcGIS.Geodatabase.IFeatureDatasetExtension featureDatasetExtension = featureDatasetExtensionContainer.FindExtension(ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTNetworkDataset);
			var datasetContainer3 = featureDatasetExtension as ESRI.ArcGIS.Geodatabase.IDatasetContainer3;

			// Use the container to open the network dataset.
			ESRI.ArcGIS.Geodatabase.IDataset dataset = datasetContainer3.get_DatasetByName(ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTNetworkDataset, strNDSName);
			return dataset as ESRI.ArcGIS.Geodatabase.INetworkDataset;
		}

		private void CreateContextAndSolver(INetworkDataset networkDataset)
		{
			if (networkDataset == null) return;

			IDatasetComponent datasetComponent = networkDataset as IDatasetComponent;
			IDENetworkDataset deNetworkDataset = datasetComponent.DataElement as IDENetworkDataset;

			INASolver naSolver = new NAServiceAreaSolverClass();
			m_naContext = naSolver.CreateContext(deNetworkDataset, "ServiceArea");
			INAContextEdit naContextEdit = m_naContext as INAContextEdit;
			naContextEdit.Bind(networkDataset, new GPMessagesClass());
		}

		#endregion

		#region Load Form Controls

		private void LoadCostAttributes(INetworkDataset networkDataset)
		{
			cbCostAttribute.Items.Clear();

			int attrCount = networkDataset.AttributeCount;
			for (int attrIndex = 0; attrIndex < attrCount; attrIndex++)
			{
				INetworkAttribute networkAttribute = networkDataset.get_Attribute(attrIndex);
				if (networkAttribute.UsageType == esriNetworkAttributeUsageType.esriNAUTCost)
					cbCostAttribute.Items.Add(networkAttribute.Name);
			}

			if (cbCostAttribute.Items.Count > 0)
				cbCostAttribute.SelectedIndex = 0;
		}

		private bool LoadLocations(IFeatureWorkspace featureWorkspace)
		{
			IFeatureClass inputFeatureClass = null;
			try
			{
				inputFeatureClass = featureWorkspace.OpenFeatureClass(txtInputFacilities.Text);
			}
			catch (Exception)
			{
				MessageBox.Show("Specified input feature class does not exist");
				return false;
			}

			INamedSet classes = m_naContext.NAClasses;
			INAClass naClass = classes.get_ItemByName("Facilities") as INAClass;

			// delete existing locations, except barriers
			naClass.DeleteAllRows();

			// Create a NAClassLoader and set the snap tolerance (meters unit)
			INAClassLoader naClassLoader = new NAClassLoaderClass();
			naClassLoader.Locator = m_naContext.Locator;
			naClassLoader.Locator.SnapTolerance = 100;
			naClassLoader.NAClass = naClass;

			// Create field map to automatically map fields from input class to NAClass
			INAClassFieldMap naClassFieldMap = new NAClassFieldMapClass();
			naClassFieldMap.CreateMapping(naClass.ClassDefinition, inputFeatureClass.Fields);
			naClassLoader.FieldMap = naClassFieldMap;

			// Avoid loading network locations onto non-traversable portions of elements
			INALocator3 locator = m_naContext.Locator as INALocator3;
			locator.ExcludeRestrictedElements = true;
			locator.CacheRestrictedElements(m_naContext);

			// load network locations
			int rowsIn = 0;
			int rowsLocated = 0;
			naClassLoader.Load(inputFeatureClass.Search(null, true) as ICursor, null, ref rowsIn, ref rowsLocated);

			if (rowsLocated <= 0)
			{
				MessageBox.Show("Facilities were not loaded from input feature class");
				return false;
			}

			// Message all of the network analysis agents that the analysis context has changed
			INAContextEdit naContextEdit = m_naContext as INAContextEdit;
			naContextEdit.ContextChanged();

			return true;
		}

		private void AddNetworkAnalysisLayerToMap()
		{
			ILayer layer = m_naContext.Solver.CreateLayer(m_naContext) as ILayer;
			layer.Name = m_naContext.Solver.DisplayName;
			axMapControl.AddLayer(layer);
		}

		private void AddNetworkDatasetLayerToMap(INetworkDataset networkDataset)
		{
			INetworkLayer networkLayer = new NetworkLayerClass();
			networkLayer.NetworkDataset = networkDataset;
			ILayer layer = networkLayer as ILayer;
			layer.Name = "Network Dataset";
			axMapControl.AddLayer(layer);
		}

		#endregion

		#region Solver Settings

		private void ConfigureSolverSettings()
		{
			ConfigureSettingsSpecificToServiceAreaSolver();

			ConfigureGenericSolverSettings();

			UpdateContextAfterChangingSettings();
		}

		private void ConfigureSettingsSpecificToServiceAreaSolver()
		{
			INAServiceAreaSolver naSASolver = m_naContext.Solver as INAServiceAreaSolver;

			naSASolver.DefaultBreaks = ParseBreaks(txtCutOff.Text);

			naSASolver.MergeSimilarPolygonRanges = false;
			naSASolver.OutputPolygons = esriNAOutputPolygonType.esriNAOutputPolygonSimplified;
			naSASolver.OverlapLines = true;
			naSASolver.SplitLinesAtBreaks = false;
			naSASolver.TravelDirection = esriNATravelDirection.esriNATravelDirectionFromFacility;

			if (ckbShowLines.Checked)
				naSASolver.OutputLines = esriNAOutputLineType.esriNAOutputLineTrueShape;
			else
				naSASolver.OutputLines = esriNAOutputLineType.esriNAOutputLineNone;
		}

		private void ConfigureGenericSolverSettings()
		{
			INASolverSettings naSolverSettings = m_naContext.Solver as INASolverSettings;
			naSolverSettings.ImpedanceAttributeName = cbCostAttribute.Text;

			// set the oneway restriction, if necessary
			IStringArray restrictions = naSolverSettings.RestrictionAttributeNames;
			restrictions.RemoveAll();
			if (ckbUseRestriction.Checked)
				restrictions.Add("Oneway");
			naSolverSettings.RestrictionAttributeNames = restrictions;
			//naSolverSettings.RestrictUTurns = esriNetworkForwardStarBacktrack.esriNFSBNoBacktrack;
		}

		private void UpdateContextAfterChangingSettings()
		{
			IDatasetComponent datasetComponent = m_naContext.NetworkDataset as IDatasetComponent;
			IDENetworkDataset deNetworkDataset = datasetComponent.DataElement as IDENetworkDataset;
			m_naContext.Solver.UpdateContext(m_naContext, deNetworkDataset, new GPMessagesClass());
		}

		private IDoubleArray ParseBreaks(string p)
		{
			String[] breaks = p.Split(' ');
			IDoubleArray pBrks = new DoubleArrayClass();
			int firstIndex = breaks.GetLowerBound(0);
			int lastIndex = breaks.GetUpperBound(0);
			for (int splitIndex = firstIndex; splitIndex <= lastIndex; splitIndex++)
			{
				try
				{
					pBrks.Add(Convert.ToDouble(breaks[splitIndex]));
				}
				catch (FormatException)
				{
					MessageBox.Show("Breaks are not properly formatted.  Use only digits separated by spaces");
					pBrks.RemoveAll();
					return pBrks;
				}
			}

			return pBrks;
		}

		#endregion

		#region Post-Solve

		private void LoadListboxAfterPartialSolve(IGPMessages gpMessages)
		{
			lbOutput.Items.Add("Partial Solve Generated.");
			for (int msgIndex = 0; msgIndex < gpMessages.Messages.Count; msgIndex++)
			{
				string errorText = "";
				switch (gpMessages.GetMessage(msgIndex).Type)
				{
					case esriGPMessageType.esriGPMessageTypeError:
						errorText = "Error " + gpMessages.GetMessage(msgIndex).ErrorCode.ToString() + " " + gpMessages.GetMessage(msgIndex).Description;
						break;
					case esriGPMessageType.esriGPMessageTypeWarning:
						errorText = "Warning " + gpMessages.GetMessage(msgIndex).ErrorCode.ToString() + " " + gpMessages.GetMessage(msgIndex).Description;
						break;
					default:
						errorText = "Information " + gpMessages.GetMessage(msgIndex).Description;
						break;
				}
				lbOutput.Items.Add(errorText);
			}
		}

		private void LoadListboxAfterSuccessfulSolve()
		{
			ITable table = m_naContext.NAClasses.get_ItemByName("SAPolygons") as ITable;
			if (table.RowCount(null) > 0)
			{
				IGPMessage gpMessage = new GPMessageClass();
				lbOutput.Items.Add("FacilityID \t FromBreak \t ToBreak");
				ICursor cursor = table.Search(null, true);
				IRow row = cursor.NextRow();
				while (row != null)
				{
					int facilityID = (int)row.get_Value(table.FindField("FacilityID"));
					double fromBreak = (double)row.get_Value(table.FindField("FromBreak"));
					double toBreak = (double)row.get_Value(table.FindField("ToBreak"));
                    lbOutput.Items.Add(facilityID.ToString() + "\t " + fromBreak.ToString("#####0.00") + "\t " + toBreak.ToString("#####0.00"));
					row = cursor.NextRow();
				}
			}
		}

		private void UpdateMapDisplayAfterSolve()
		{
			// Zoom to the extent of the service areas
			IGeoDataset geoDataset = m_naContext.NAClasses.get_ItemByName("SAPolygons") as IGeoDataset;
			IEnvelope envelope = geoDataset.Extent;
			if (!envelope.IsEmpty)
			{
				envelope.Expand(1.1, 1.1, true);
				axMapControl.Extent = envelope;

				// Call this to update the renderer for the service area polygons
				// based on the new breaks.
				m_naContext.Solver.UpdateLayer(axMapControl.get_Layer(0) as INALayer);
			}
			axMapControl.Refresh();
		}

		#endregion

        /// <summary>
        /// Initialize the solver by calling the network analyst functions.
        /// </summary>
        private void Initialize()
        {
            IFeatureWorkspace featureWorkspace = null;
            INetworkDataset networkDataset = null;
            try
            {
                // Open the Network Dataset
                //IWorkspace workspace = OpenWorkspace(Application.StartupPath + @"\..\..\..\..\..\Data\SanFrancisco\SanFrancisco.gdb");
                IWorkspace workspace = OpenWorkspace("C:\\Users\\Yuanyun\\Desktop\\Instance\\SanFrancisco\\SanFrancisco.gdb");
                networkDataset = OpenNetworkDataset(workspace, "Transportation", "Streets_ND");
                featureWorkspace = workspace as IFeatureWorkspace;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Unable to open dataset. Error Message: " + ex.Message);
                this.Close();
                return;
            }

            // Create NAContext and NASolver
            m_NAContext = CreateSolverContext(networkDataset);

            // Get available cost attributes from the network dataset
            INetworkAttribute networkAttribute;
            for (int i = 0; i < networkDataset.AttributeCount - 1; i++)
            {
                networkAttribute = networkDataset.get_Attribute(i);
                if (networkAttribute.UsageType == esriNetworkAttributeUsageType.esriNAUTCost)
                {
                    comboCostAttribute.Items.Add(networkAttribute.Name);
                }
            }
            comboCostAttribute.SelectedIndex = 0;
            textTargetFacility.Text = "";
            textCutoff.Text = "";

            // Load locations from feature class
            IFeatureClass inputFClass = featureWorkspace.OpenFeatureClass("Stores");
            LoadNANetworkLocations("Origins", inputFClass, 100);
            inputFClass = featureWorkspace.OpenFeatureClass("Hospitals");
            LoadNANetworkLocations("Destinations", inputFClass, 100);

            // Create layer for network dataset and add to ArcMap
            INetworkLayer networkLayer = new NetworkLayerClass();
            networkLayer.NetworkDataset = networkDataset;
            ILayer layer = networkLayer as ILayer;
            layer.Name = "Network Dataset";
            axMapControl.AddLayer(layer, 0);

            // Create a network analysis layer and add to ArcMap
            INALayer naLayer = m_NAContext.Solver.CreateLayer(m_NAContext);
            layer = naLayer as ILayer;
            layer.Name = m_NAContext.Solver.DisplayName;
            axMapControl.AddLayer(layer, 0);
        }

        /// <summary>
        /// Call the OD cost matrix solver and display the results
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event</param>
        private void cmdSolve_Click(object sender, EventArgs e)
        {
            this.toolStripStatusLabel1.Text = "Working...";
            Initialize();
            try
            {
                lbOutput.Items.Clear();
                cmdSolve.Text = "Solving...";

                SetSolverSettings();

                // Solve
                IGPMessages gpMessages = new GPMessagesClass();
                if (!m_NAContext.Solver.Solve(m_NAContext, gpMessages, null))
                {
                    // Get the ODLines output
                    GetODOutput();
                }
                else
                    lbOutput.Items.Add("Partial Result");

                // Display Error/Warning/Informative messages
                if (gpMessages != null)
                {
                    for (int i = 0; i < gpMessages.Count; i++)
                    {
                        switch (gpMessages.GetMessage(i).Type)
                        {
                            case esriGPMessageType.esriGPMessageTypeError:
                                lbOutput.Items.Add("Error " + gpMessages.GetMessage(i).ErrorCode.ToString() + " " + gpMessages.GetMessage(i).Description);
                                break;
                            case esriGPMessageType.esriGPMessageTypeWarning:
                                lbOutput.Items.Add("Warning " + gpMessages.GetMessage(i).Description);
                                break;
                            default:
                                lbOutput.Items.Add("Information " + gpMessages.GetMessage(i).Description);
                                break;
                        }
                    }
                }

                // Zoom to the extent of the route
                IGeoDataset gDataset = m_NAContext.NAClasses.get_ItemByName("ODLines") as IGeoDataset;
                IEnvelope envelope = gDataset.Extent;
                if (!envelope.IsEmpty)
                {
                    envelope.Expand(1.1, 1.1, true);
                    axMapControl.Extent = envelope;
                }

                axMapControl.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                cmdSolve.Text = "Find OD Cost Matrix";
            }
            this.toolStripStatusLabel1.Text = "Done!";
        }

        /// <summary>
        /// Get the impedance cost from the ODLines class output
        /// </summary>
        public void GetODOutput()
        {
            ITable naTable = m_NAContext.NAClasses.get_ItemByName("ODLines") as ITable;
            if (naTable == null)
                lbOutput.Items.Add("Impossible to get the ODLines table");

            lbOutput.Items.Add("Number of destinations found: " + naTable.RowCount(null).ToString());
            lbOutput.Items.Add("");

            if (naTable.RowCount(null) > 0)
            {
                lbOutput.Items.Add("OriginID, DestinationID, DestinationRank, Total_" + comboCostAttribute.Text);
                double total_impedance;
                long OriginID;
                long DestinationID;
                long DestinationRank;

                ICursor naCursor = naTable.Search(null, false);
                IRow naRow = naCursor.NextRow();
                while (naRow != null)
                {
                    OriginID = long.Parse(naRow.get_Value(naTable.FindField("OriginID")).ToString());
                    DestinationID = long.Parse(naRow.get_Value(naTable.FindField("DestinationID")).ToString());
                    DestinationRank = long.Parse(naRow.get_Value(naTable.FindField("DestinationRank")).ToString());
                    total_impedance = double.Parse(naRow.get_Value(naTable.FindField("Total_" + comboCostAttribute.Text)).ToString());
                    lbOutput.Items.Add(OriginID.ToString() + "\t " + DestinationID.ToString() + "\t" +
                        DestinationRank.ToString() + "\t " + total_impedance.ToString("#0.00"));
										int i=int.Parse(naRow.get_Value(naTable.FindField("OriginID")).ToString());
										int j=int.Parse(naRow.get_Value(naTable.FindField("DestinationID")).ToString());
										double k=total_impedance;
										ODM[i-1,j-1]=k;					
                    naRow = naCursor.NextRow();
                }
            }

            lbOutput.Refresh();
        }

        #region Network analyst functions

        /// <summary>
        /// Create NASolver and NAContext
        /// </summary>
        /// <param name="networkDataset">Input network dataset</param>
        /// <returns>NAContext</returns>
        public INAContext CreateSolverContext(INetworkDataset networkDataset)
        {
            //Get the data element
            IDENetworkDataset deNDS = GetDENetworkDataset(networkDataset);
            INASolver naSolver = new NAODCostMatrixSolver();
            INAContextEdit contextEdit = naSolver.CreateContext(deNDS, naSolver.Name) as INAContextEdit;
            //Bind a context using the network dataset 
            contextEdit.Bind(networkDataset, new GPMessagesClass());

            return contextEdit as INAContext;
        }

        /// <summary>
        /// Set solver settings
        /// </summary>
        /// <param name="strNAClassName">NAClass name</param>
        /// <param name="inputFC">Input feature class</param>
        /// <param name="snapTolerance">Snap tolerance</param>
        public void LoadNANetworkLocations(string strNAClassName, IFeatureClass inputFC, double snapTolerance)
        {
            INamedSet classes = m_NAContext.NAClasses;
            INAClass naClass = classes.get_ItemByName(strNAClassName) as INAClass;

            // Delete existing locations from the specified NAClass
            naClass.DeleteAllRows();

            // Create a NAClassLoader and set the snap tolerance (meters unit)
            INAClassLoader loader = new NAClassLoader();
            loader.Locator = m_NAContext.Locator;
            if (snapTolerance > 0)
                loader.Locator.SnapTolerance = snapTolerance;
            loader.NAClass = naClass;

            // Create field map to automatically map fields from input class to NAClass
            INAClassFieldMap fieldMap = new NAClassFieldMapClass();
            fieldMap.CreateMapping(naClass.ClassDefinition, inputFC.Fields);
            loader.FieldMap = fieldMap;

            // Avoid loading network locations onto non-traversable portions of elements
            INALocator3 locator = m_NAContext.Locator as INALocator3;
            locator.ExcludeRestrictedElements = true;
            locator.CacheRestrictedElements(m_NAContext);

            // Load network locations
            int rowsIn = 0;
            int rowsLocated = 0;
            loader.Load((ICursor)inputFC.Search(null, true), null, ref rowsIn, ref rowsLocated);

            // Message all of the network analysis agents that the analysis context has changed.
            INAContextEdit naContextEdit = m_NAContext as INAContextEdit;
            naContextEdit.ContextChanged();
        }

        /// <summary>
        /// Set solver settings
        /// </summary>
        public void SetSolverSettings()
        {
            // Set OD solver specific settings
            INASolver solver = m_NAContext.Solver;
            INAODCostMatrixSolver odSolver = solver as INAODCostMatrixSolver;
            if (textCutoff.Text.Length > 0 && IsNumeric(textCutoff.Text.Trim()))
                odSolver.DefaultCutoff = textCutoff.Text;
            else
                odSolver.DefaultCutoff = null;

            if (textTargetFacility.Text.Length > 0 && IsNumeric(textTargetFacility.Text.Trim()))
                odSolver.DefaultTargetDestinationCount = textTargetFacility.Text;
            else
                odSolver.DefaultTargetDestinationCount = null;

            odSolver.OutputLines = esriNAOutputLineType.esriNAOutputLineStraight;

            // Set generic solver settings
            // Set the impedance attribute
            INASolverSettings solverSettings = solver as INASolverSettings;
            solverSettings.ImpedanceAttributeName = comboCostAttribute.Text;

            // Set the OneWay restriction if necessary
            IStringArray restrictions = solverSettings.RestrictionAttributeNames;
            restrictions.RemoveAll();
            if (checkUseRestriction.Checked)
                restrictions.Add("oneway");
            solverSettings.RestrictionAttributeNames = restrictions;

            // Restrict UTurns
            solverSettings.RestrictUTurns = esriNetworkForwardStarBacktrack.esriNFSBNoBacktrack;
            solverSettings.IgnoreInvalidLocations = true;

            // Set the hierarchy attribute
            solverSettings.UseHierarchy = checkUseHierarchy.Checked;
            if (solverSettings.UseHierarchy)
                solverSettings.HierarchyAttributeName = "hierarchy";

            // Do not forget to update the context after you set your impedance
            solver.UpdateContext(m_NAContext, GetDENetworkDataset(m_NAContext.NetworkDataset), new GPMessagesClass());
        }

        /// <summary>
        /// Geodatabase function: open work space
        /// </summary>
        /// <param name="strGDBName">Input file name</param>
        /// <returns>Workspace</returns>

        /// <summary>
        /// Geodatabase function: open network dataset
        /// </summary>
        /// <param name="workspace">Input workspace</param>
        /// <param name="strNDSName">Input network dataset name</param>
        /// <returns></returns>

        /// <summary>
        /// Geodatabase function: get network dataset
        /// </summary>
        /// <param name="networkDataset">Input network dataset</param>
        /// <returns>DE network dataset</returns>
        public IDENetworkDataset GetDENetworkDataset(INetworkDataset networkDataset)
        {
            // Cast from the network dataset to the DatasetComponent
            IDatasetComponent dsComponent = networkDataset as IDatasetComponent;

            // Get the data element
            return dsComponent.DataElement as IDENetworkDataset;
        }

        #endregion

        /// <summary>
        /// Check whether a string represents a double value.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private bool IsNumeric(string str)
        {
            try
            {
                double.Parse(str.Trim());
            }
            catch (Exception) { return false; }

            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.toolStripStatusLabel1.Text = "Import ODCost Matrix Successfully!";
            int m=25,p=22;
            string s=null;
            lbOutput.Items.Clear();
            lbOutput.Items.Add("ODCost Matrix:");
            for (int i = 0; i < m; i++)
            {
                
                for (int j = 0; j < p; j++)
                    s+=ODM[i, j].ToString() + " ";
                lbOutput.Items.Add(s);
                s = null;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.toolStripStatusLabel1.Text = "Calculating...";
            const int m = 25,p = 22, R = 2;
            double[] hi = new double[m] { 15, 10, 12, 18, 5, 24, 11, 16, 13, 22, 19, 20, 15, 10, 12, 18, 5, 24, 11, 16, 13, 22, 19, 20 ,10};
            double[,] dij = ODM;//初始ODCost矩阵，自由流成本矩阵
            double[,] net_free = new double[m, p]
            {
                {0,15,0,0,24,0,18,0,0,0,0,0,0,15,0,0,24,0,18,0,0,0},
                {15,0,22,0,0,0,0,0,0,0,0,0,15,0,22,0,0,0,0,0,0,0},
                {0,22,0,18,16,0,0,0,20,0,0,0,0,22,0,18,16,0,0,0,20,0},
                {0,0,18,0,0,24,0,0,0,0,0,0,0,0,18,0,0,24,0,0,0,0},
                {24,0,16,0,0,0,25,12,24,0,0,0,24,0,16,0,0,0,25,12,24,0},
                {0,0,0,24,0,0,0,0,12,0,0,22,0,0,0,24,0,0,0,0,12,0},
                {18,0,0,0,25,12,0,15,0,22,0,0,18,0,0,0,25,12,0,15,0,22},
                {0,0,0,0,0,0,15,0,30,0,15,0,0,0,0,0,0,0,15,0,30,0},
                {0,0,20,0,24,12,0,30,0,0,19,19,0,0,20,0,24,12,0,30,0,0},
                {0,0,0,0,0,0,22,0,0,0,19,0,0,0,0,0,0,0,22,0,0,0},
                {0,0,0,0,0,0,0,15,19,19,0,21,0,0,0,0,0,0,0,15,19,19},
                {0,0,0,0,0,22,0,0,19,0,21,0,0,0,0,0,0,22,0,0,19,0},
                {0,15,0,0,24,0,18,0,0,0,0,0,0,15,0,0,24,0,18,0,0,0},
                {15,0,22,0,0,0,0,0,0,0,0,0,15,0,22,0,0,0,0,0,0,0},
                {0,22,0,18,16,0,0,0,20,0,0,0,0,22,0,18,16,0,0,0,20,0},
                {0,0,18,0,0,24,0,0,0,0,0,0,0,0,18,0,0,24,0,0,0,0},
                {24,0,16,0,0,0,25,12,24,0,0,0,24,0,16,0,0,0,25,12,24,0},
                {0,0,0,24,0,0,0,0,12,0,0,22,0,0,0,24,0,0,0,0,12,0},
                {18,0,0,0,25,12,0,15,0,22,0,0,18,0,0,0,25,12,0,15,0,22},
                {0,0,0,0,0,0,15,0,30,0,15,0,0,0,0,0,0,0,15,0,30,0},
                {0,0,20,0,24,12,0,30,0,0,19,19,0,0,20,0,24,12,0,30,0,0},
                {0,0,0,0,0,0,22,0,0,0,19,0,0,0,0,0,0,0,22,0,0,0},
                {0,0,0,0,0,0,0,15,19,19,0,21,0,0,0,0,0,0,0,15,19,19},
                {0,0,0,0,0,22,0,0,19,0,21,0,0,0,0,0,0,22,0,0,19,0},
                {0,0,0,0,0,22,0,0,19,0,21,0,0,0,0,0,0,22,0,0,19,0},
            };//自由流邻接矩阵
            //int[] x = { 1, 0, 0, 0, 0, 1, 1, 0, 1, 1, 0, 0 };//已经选定的设施点的位置
            int[] r = new int[p];//被中断设施点的位置
            double[,] OD = new double[m, p];//由于服务关系产生的OD
            double[,] NET = new double[m, p];//用于分配的行程时间矩阵
            double[,] ODCost = new double[m, p];//用于选址的行程时间矩阵
            double[,] capacity = new double[m, p];//路网容量
            //得到已经选定的设施点的位置标号
            //int[] index_x = new int[p];
            //for (int i = 0, j = 0; i < m; i++)
            //    if (x[i] == 1) { index_x[j] = i; j++; }
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < p; j++)
                {
                    if (i != j) capacity[i, j] = 30;
                }
            }
            //记录程序开始的时间
            DateTime start_time = new DateTime();
            DateTime final_time = new DateTime();
            start_time = System.DateTime.Now;
            System.Array.Copy(net_free, NET, net_free.Length);
            System.Array.Copy(dij, ODCost, dij.Length);
            //Console.WriteLine();
            //Console.WriteLine("initial OD cost:");
            //for (int i = 0; i < m; i++)
            //{
            //    Console.WriteLine();
            //    for (int j = 0; j < m; j++)
            //        Console.Write(dij[i, j] + " ");
            //}
            //初始化OD
            for (int i = 0; i < m; i++)
            {
                double temp = double.MaxValue;
                int index = 0;
                for (int j = 0; j < p; j++)
                {
                    if (ODCost[i, j] < temp && i != j && r[j] == 0)
                    {
                        temp = ODCost[i, j];
                        index = j;
                    }
                }
                OD[i, index] = hi[i];                
            }
            //Console.WriteLine();
            //Console.WriteLine("initial OD:");
            //for (int i = 0; i < m; i++)
            //{
            //    Console.WriteLine();
            //    for (int j = 0; j < m; j++)
            //        Console.Write(OD[i, j] + " ");
            //}
            Parameter par = new Parameter(hi, ODCost, p, R, m);
            GA g = new GA(par);
            g.MutationRate = 0.6;
            g.CrossRate = 0.6;
            g.optimal();
            r = g.output();
            //Console.WriteLine();
            //for (int i = 0; i < p; i++) Console.Write(r[i] + " ");
            //Console.Read();
            SUE sue = new SUE(NET, capacity, OD);
            ODCost = sue.traffic_assignment();
            bool flag = false;
            int flag2 = 0;
            while (!flag)
            {
                par = new Parameter(hi, ODCost, p, R, m);
                g = new GA(par);
                g.optimal();
                bool flag1 = false;
                for (int i = 0; i < p; i++)
                {
                    if (r[i] != g.output()[i]) flag1 = true;
                }
                if (!flag1)
                    flag2++;
                else r = g.output();
                if (flag2 == 3) flag = true;
                //Console.WriteLine();
                //for (int i = 0; i < p; i++) Console.Write(r[i] + " ");
                //Console.Read();
                //中断后的OD
                OD = new double[m, m];
                for (int i = 0; i < m; i++)
                {
                    double temp = double.MaxValue;
                    int index = 0;
                    for (int j = 0; j < p; j++)
                    {
                        if (ODCost[i, j] < temp && i != j && r[j] == 0)
                        {
                            temp = ODCost[i, j];
                            index = j;
                        }
                    }
                    OD[i, index] = hi[i];
                }

                sue = new SUE(NET, capacity, OD);
                ODCost = sue.traffic_assignment();
            }
            //输出结果
            //Console.WriteLine("R-interdiction");
            //for (int i = 0; i < p; i++)
            //    Console.Write(r[i] + " ");
            lbOutput.Items.Clear();
            lbOutput.Items.Add("R-interdiction");
            string output = null;
            for (int i = 0; i < p; i++)
                output+=(r[i].ToString() + " ");
            lbOutput.Items.Add(output);
            //输出程序所花费的时间
            final_time = System.DateTime.Now;
            //Console.WriteLine();
            //Console.WriteLine("Used Time:" + (final_time - start_time).ToString());
            //Console.Read();
            lbOutput.Items.Add("Used Time:" + (final_time - start_time).ToString());
            lbOutput.Refresh();
            IFeatureWorkspace featureWorkspace = null;
            try
            {
                // Open the Network Dataset
                //IWorkspace workspace = OpenWorkspace(Application.StartupPath + @"\..\..\..\..\..\Data\SanFrancisco\SanFrancisco.gdb");
                IWorkspace workspace = OpenWorkspace("C:\\Users\\Yuanyun\\Desktop\\Instance\\SanFrancisco\\SanFrancisco.gdb");
                featureWorkspace = workspace as IFeatureWorkspace;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Unable to open dataset. Error Message: " + ex.Message);
                this.Close();
                return;
            }
            GetFacilityInterdicted(featureWorkspace, r);
            this.toolStripStatusLabel1.Text = "Done!";
        }

        private void button3_Click(object sender, EventArgs e)
        {
			IFeatureWorkspace featureWorkspace = null;
            try
            {
                // Open the Network Dataset
                //IWorkspace workspace = OpenWorkspace(Application.StartupPath + @"\..\..\..\..\..\Data\SanFrancisco\SanFrancisco.gdb");
                IWorkspace workspace = OpenWorkspace("C:\\Users\\Yuanyun\\Desktop\\Instance\\SanFrancisco\\SanFrancisco.gdb");
                featureWorkspace = workspace as IFeatureWorkspace;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Unable to open dataset. Error Message: " + ex.Message);
                this.Close();
                return;
            }
            
        }
        private void GetFacilityInterdicted(IFeatureWorkspace featureWorkspace,int[] r)
        {
            int p=22;
        	IFeatureClass featureClass = featureWorkspace.OpenFeatureClass("Hospitals");
        	IFeature pFeature;
        	for(int i=1;i<=p;i++)
        	{
        		pFeature = featureClass.GetFeature(i) as IFeature;
                IFeatureBuffer pFeatureBuffer = pFeature as IFeatureBuffer;    
        		pFeatureBuffer.set_Value(pFeature.Fields.FindField("interdicted"),r[i-1]);
        	}
            IMap pMap=this.axMapControl.Map;
            IGraphicsContainer pGraphicsContainerXL = pMap as IGraphicsContainer;
            //add interdicted points
            for (int i = 0; i < p; i++)
            {
                if (r[i] == 1)
                {
                    pFeature = featureClass.GetFeature(i) as IFeature;
                    IPoint pPoint = pFeature.Shape as IPoint;
                    ITextElement pTextEle = new TextElementClass();
                    IElement pElement = pTextEle as IElement;
                    //text
                    pTextEle.Text = "interdicted";
                    pTextEle.ScaleText = true;
                    pElement.Geometry = pPoint;
                    //BalloonCallout
                    IBalloonCallout pBalloonCallout = new BalloonCalloutClass();
                    pBalloonCallout.Style = esriBalloonCalloutStyle.esriBCSRoundedRectangle;
                    pBalloonCallout.AnchorPoint = pPoint;
                    IFormattedTextSymbol pTextSymbol = new TextSymbolClass();
                    pTextSymbol.Background = pBalloonCallout as ITextBackground;
                    pTextSymbol.Direction = esriTextDirection.esriTDAngle;
                    pTextSymbol.Angle = 15;
                    pTextSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
                    pTextSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVATop;
                    pTextEle.Symbol = pTextSymbol;
                    pGraphicsContainerXL.AddElement(pTextEle as IElement, 0);
                }
            }
            axMapControl.Refresh();
        }
    }
}
