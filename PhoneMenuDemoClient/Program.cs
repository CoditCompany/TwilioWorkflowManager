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


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Workflow.Client;
using Microsoft.Workflow.Samples.Common;
using System.Collections.ObjectModel;
using Microsoft.Activities;
using System.Activities;
using Microsoft.ServiceBus;
using System.Configuration;
using Microsoft.ServiceBus.Messaging;

namespace PhoneMenuDemoClient
{
    class Program
    {
        // Change these variables to your settings
        static string workflowName = "FlightMenuStateMachine";
        static string machineName = "wfmgrlab";
        static string scopeName = "PhoneStateDemo";


        static void Main(string[] args)
        {
            // Check test settings - you can deploy to the test host to attach visual studio to it.  
            bool test = false;
            Console.WriteLine("Press t to deploy to test host");
            test = Console.ReadLine().ToLower() == "t";
            string baseAddress = string.Format("https://{0}:12290/", machineName);
            if (test)
                baseAddress = string.Format("http://{0}:12292/", machineName);


            // Create scope
            Console.Write("Setting up scope...");
            WorkflowManagementClient client = WorkflowUtils.CreateForSample(baseAddress, scopeName);
            WorkflowUtils.PrintDone();

            // Deploy all activities
            Console.Write("Publishing Phone activities...");
            client.PublishActivity("ReceiveCallerInput", @"..\..\..\PhoneMenuWorkflows\ReceiveCallerInput.xaml");
            client.PublishActivity("RetrieveUserFlights", @"..\..\..\PhoneMenuWorkflows\RetrieveUserFlights.xaml");
            client.PublishActivity("SetNextOptions", @"..\..\..\PhoneMenuWorkflows\SetNextOptions.xaml");
            client.PublishActivity("SayGoodbye", @"..\..\..\PhoneMenuWorkflows\SayGoodbye.xaml");

            WorkflowUtils.PrintDone();

            // Deploy actual workflow, by specifying the external variables
            Console.Write("Publishing Workflow...");
            Collection<ExternalVariable> externalVariables = new Collection<ExternalVariable>
            {
                new ExternalVariable<string> { Name = "UserOptions", Default = "", Modifiers = VariableModifiers.Mapped},
                new ExternalVariable<string> { Name = "PhoneNr", Default = "", Modifiers = VariableModifiers.Mapped},
                new ExternalVariable<string> { Name = "GoodbyeStatement", Default = "", Modifiers = VariableModifiers.Mapped}                
            };
            client.PublishWorkflow(workflowName, @"..\..\..\PhoneMenuWorkflows\FlightMenuStateMachine.xaml", externalVariables);

            WorkflowUtils.PrintDone();

        }
    }
}
