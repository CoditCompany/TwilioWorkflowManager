//======================================================================================= 
// Codit - http://blog.codit.eu 
// 
// This sample contains the code of the Workflow Manager project that was demonstrated during 
// the BizTalk Summit on March 4, 2014, in London
// Please refer to my blog post at http://blog.codit.eu/post/2014/03/07/Using-a-State-Machine-in-Workflow-manager-to-instrument-a-Twilio-call.aspx/.  
//  
// Author: Sam Vanhoutte - @SamVanhoutte
//======================================================================================= 
// Copyright © 2014 Codit. All rights reserved. 
//  
// THIS CODE AND INFORMATION IS PROVIDED 'AS IS' WITHOUT WARRANTY OF ANY KIND, EITHER  
// EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES OF  
// MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE. YOU BEAR THE RISK OF USING IT. 
//======================================================================================= 


using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Markup;


// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("PhoneMenuWorkflows")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("PhoneMenuWorkflows")]
[assembly: AssemblyCopyright("Copyright ©  2014")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]


// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("71c5c2f3-b4fd-40a3-8adb-f23b226b9e12")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

[assembly: XmlnsDefinition("wf://workflow.windows.net/$Current/$Activities", "PhoneMenuWorkflows")]
