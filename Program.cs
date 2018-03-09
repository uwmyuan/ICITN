// Copyright 2010 ESRI
// 
// All rights reserved under the copyright laws of the United States
// and applicable international laws, treaties, and conventions.
// 
// You may freely redistribute and use this sample code, with or
// without modification, provided you include the original copyright
// notice and use restrictions.
// 
// See the use restrictions at &lt;your ArcGIS install location&gt;/DeveloperKit10.0/userestrictions.txt.
// 

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ESRI.ArcGIS.esriSystem;

/*
*    '*************************************************************************
*    '       Network Analyst - Service Area Solver sample
*    '
*    '   This code shows how to :
*    '    1) Open a workspace and open a Network Dataset
*    '    2) Create a NAContext and its NASolver
*    '    3) Load Facilities from a Feature Class and create Network Locations
*    '    4) Set the Solver parameters
*    '    5) Solve a Service Area problem
*    '    6) Display SAPolygons output
*    '
*    '*************************************************************************
*/

namespace ICITN
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			if (!ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.Engine))
			{
				if (!ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.Desktop))
				{
					System.Windows.Forms.MessageBox.Show("This application could not load the correct version of ArcGIS.");
					return;
				}
			}

			LicenseInitializer aoLicenseInitializer = new LicenseInitializer();
			if (!aoLicenseInitializer.InitializeApplication(new esriLicenseProductCode[] { esriLicenseProductCode.esriLicenseProductCodeEngine, esriLicenseProductCode.esriLicenseProductCodeArcView, esriLicenseProductCode.esriLicenseProductCodeArcEditor, esriLicenseProductCode.esriLicenseProductCodeArcInfo },
			new esriLicenseExtensionCode[] { esriLicenseExtensionCode.esriLicenseExtensionCodeNetwork }))
			{
				System.Windows.Forms.MessageBox.Show("This application could not initialize with the correct ArcGIS license and will shutdown. LicenseMessage: " + aoLicenseInitializer.LicenseMessage());
				aoLicenseInitializer.ShutdownApplication();
				return;
			}

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Form1 mainForm = new Form1();

			// Check that the form was not already disposed of during initialization before running it.
			if (mainForm != null && !mainForm.IsDisposed)
				Application.Run(mainForm);

			//ESRI License Initializer generated code.
			//Do not make any call to ArcObjects after ShutDownApplication()
			aoLicenseInitializer.ShutdownApplication();
		}
	}
}