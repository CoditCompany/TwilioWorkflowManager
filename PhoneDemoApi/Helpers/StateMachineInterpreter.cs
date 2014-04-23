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


using Microsoft.Activities;
using Microsoft.Workflow.Client;
using Microsoft.Workflow.Samples.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace PhoneDemoApi.Controllers
{
    public class StateMachineInterpreter
    {
        private const string _workflowName = "FlightMenuStateMachine";
        private static WorkflowManagementClient _wfClient;
        private static Dictionary<string, string> _workflows = new Dictionary<string, string>();

        // The cached WorkflowManagementClient (static)
        // The client gets instantiated, when null and connects to the configured workflow manager and scope (web.config!)
        private static WorkflowManagementClient WFClient
        {
            get
            {
                if (_wfClient == null)
                {
                    string baseAddress = ConfigurationManager.AppSettings["WorkflowManagerBaseAddress"];
                    string workflowScope = ConfigurationManager.AppSettings["WorkflowFlightScope"];
                    _wfClient = new WorkflowManagementClient(new Uri(baseAddress + workflowScope));
                }
                return _wfClient;
            }
        }


        public static bool HasActiveWorkflow(string callerId)
        {
            if (_workflows.ContainsKey(callerId))
            {
                // We check if a workflow exists for that phone nr
                // Then we verify if the workflow is started
                // If there is no workflow , or it is a completed one, we remove it from our caching

                string existingWorkflowId = _workflows[callerId];
                var existingWorkflow = WFClient.Instances.Get(_workflowName, existingWorkflowId);
                if (existingWorkflow != null)
                {
                    // Only very recent (1 minute old) workflows or still active workflows can be reused
                    if (existingWorkflow.WorkflowStatus == WorkflowInstanceStatus.Started
                        || existingWorkflow.LastModified.AddMinutes(1) > DateTime.Now)
                    {
                        return true;
                    }
                }
                // Since there's no active workflow, we remove the workflow from our cached dictionary
                _workflows.Remove(callerId);
            }
            return false;
        }

        public static void StartNewWorkflow(string CallerId)
        {
            // We start a new workflow, passing in the CallerId
            WorkflowStartParameters startParameters = new WorkflowStartParameters
            {
            };
            startParameters.Content.Add("CallerID", CallerId);
            var instanceId = WFClient.Workflows.Start(_workflowName, startParameters);
            // We store the workflow instance id in our cached dictionary
            _workflows.Add(CallerId, instanceId);
        }

        public static void SendInputToExistingWorkflow(string CallerId, string Digit)
        {
            // Here we publish a notification with the digits and make sure it triggers the right transition in our workflow
            WFClient.PublishNotification(new WorkflowNotification()
            {
                Properties =
                {
                    { "CallerId", CallerId },
                    { "ReceivedDigit", Digit }
                },
                Content = new Dictionary<string, object>()
                {
                }
            });
        }

        public static IDictionary<int, string> GetNextSteps(string callerId, string digit, out string goodbyeStatement)
        {
            goodbyeStatement = null;
            // Here we read the external variable 'UserOptions' from the workflow instance
            // This is a string with multiple lines that we will use to return the different options
            string optionsVariable = "{http://schemas.microsoft.com/workflow/2012/xaml/activities/externalvariable}UserOptions";
            string goodbyeVariable = "{http://schemas.microsoft.com/workflow/2012/xaml/activities/externalvariable}GoodbyeStatement";
            if (_workflows.ContainsKey(callerId))
            {
                string existingWorkflowId = _workflows[callerId];
                // We load the instance for this caller from the workflow manager
                var wfInstance = WFClient.Instances.Get(_workflowName, existingWorkflowId);
                if (wfInstance != null)
                {
                    if (wfInstance.WorkflowStatus == Microsoft.Activities.WorkflowInstanceStatus.Started)
                    {
                        // The workflow is still started, so we know we have to look for the next user options
                        if (wfInstance.MappedVariables.ContainsKey(optionsVariable))
                        {
                            var options = wfInstance.MappedVariables[optionsVariable];
                            return parseOptions(options);
                        }
                    }
                    else
                    {
                        // Since the workflow is finished, the call should finish and we look for the Goodbye message variable
                        if (wfInstance.MappedVariables.ContainsKey(goodbyeVariable))
                        {
                            goodbyeStatement = wfInstance.MappedVariables[goodbyeVariable];
                            DebugWriter.Write(callerId, "Returning no options, but saying goodbye message: " + goodbyeStatement);
                        }
                        // Removing workflow from cache, because he's finished
                        _workflows.Remove(callerId);
                        // We return null (no next steps, but our goodbye statement (output variable) has been assigned
                        return null;
                    }
                }
            }
            return new Dictionary<int, string> { { 0, "No state found" } };
        }

        // In this method, we read the options variable, which is CRLF seperated
        // The first line is option 1, second line option 2, etc
        private static Dictionary<int, string> parseOptions(string options)
        {
            var resultingOptions = new Dictionary<int, string>();
            int idx = 1;
            StringReader reader = new StringReader(options);
            string option = null;
            while ((option = reader.ReadLine()) != null)
            {
                resultingOptions.Add(idx, option);
                idx++;
            }
            return resultingOptions;
        }
    }
}
