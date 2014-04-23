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


using PhoneDemoApi.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Web.Http;

namespace PhoneDemoApi.Controllers
{
    public class FlightController : ApiController
    {
        // Here we read the twilio action URL from the web.config file
        private string baseUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["PublicTwilioBaseAddress"];
            }
        }

        /// <summary>
        /// This operation will be called by Twilio , passing in the different parameters
        /// GET api/flight
        /// </summary>
        /// <param name="Caller">The phone number that is calling our Twilio number</param>
        /// <param name="Digits">The digits that were entered by the caller (not provided on the first call)</param>
        /// <returns></returns>
        public TwilioResponse Get([FromUri]string Caller = null, [FromUri]string Digits = null)
        {
            // If caller is unknown > reject
            // Here you could also block phone numbers from calling, by returning a reject!
            if (Caller == null)
            {
                return new TwilioResponse { Reject = new TwilioReject { Reason = "rejected" } };
            }


            // Check if there is an active call process for this phone number
            if (!StateMachineInterpreter.HasActiveWorkflow(Caller))
            {
                DebugWriter.Write(Caller, "Starting new workflow");
                StateMachineInterpreter.StartNewWorkflow(Caller);
            }
            else
            {
                DebugWriter.Write(Caller, "Sending input to existing workflow");
                StateMachineInterpreter.SendInputToExistingWorkflow(Caller, Digits);
            }
            
            // I don't like doing this, but I will anyway - taking the time to have the published event being handled
            Thread.Sleep(450);

            // At this step, we have an active workflow, linked with the Caller. 
            // Now get the next input
            string goodbyeStatement = null;

            // We get the next steps.  Null means no next steps and say goodbye
            var nextOptions = StateMachineInterpreter.GetNextSteps(Caller, Digits, out goodbyeStatement);
            return getTwilioResponse(Caller, nextOptions, Digits == null, goodbyeStatement);
        }

        private TwilioResponse getTwilioResponse(string Caller, IDictionary<int, string> nextOptions, bool first, string goodbyeStatement)
        {
            // First check if there are no next options (so we need to finish the call)
            if (nextOptions == null)
            {
                if (string.IsNullOrEmpty(goodbyeStatement))
                {
                    // Hangup, when no goodbye message
                    return new TwilioResponse { SpokenText = getSpokenText("Thank you for calling, we hope you are satisfied with our service") };
                }
                else
                {
                    // Say goodbye.  After that, the call will be hung up
                    return new TwilioResponse { SpokenText = getSpokenText(goodbyeStatement) };
                }
            }

            var response = new TwilioResponse
            {
                CollectInfo = new TwilioCollect
                {
                    FinishingKey = "*", // User has to press * to confirm his choice
                    SpokenText = getSpokenText(buildOptionsText(Caller, nextOptions)),  // This text will be spoken to the user
                    Timeout = 10,  // We wait maximum 10 seconds for user input, otherwise we break the call
                    ActionUri = baseUrl + "api/flight?format=twiml",   // This is the URI to which Twilio will do a get, providing the digits
                    ActionHttpMethod = "GET"    // Specifying get here (this operation)
                }
            };
            if (first)
            {
                // Just for demo purposes, we add an initial message (could be done through an external variable too!)
                response.CollectInfo.SpokenText.Content = "We have flight BA08 for you. " + response.CollectInfo.SpokenText.Content;
                // And we play a tune.  Just because we can and we love it!
                response.CollectInfo.PlayedSound = new TwilioSound
                {
                    SoundUri = baseUrl + "Content/sounds/flightintro.mp3"
                };
            }
            return response;
        }

        // Here we loop through the different options and create a text 
        private static string buildOptionsText(string Caller, IDictionary<int, string> nextOptions)
        {
            StringBuilder responseText;
            responseText = new StringBuilder("Please select one of the following options. ");
            responseText.AppendLine();
            foreach (var option in nextOptions)
            {
                // Press x [to option].
                responseText.Append("Press ").Append(option.Key).Append(" ").Append(option.Value).Append(". ").AppendLine();
            }
            // we end the message by saying: press star to confirm
            responseText.AppendLine("Press star to confirm");

            DebugWriter.Write(Caller, "Sending options\r\n" + responseText.ToString() + "\r\n");
            return responseText.ToString();
        }

        private TwilioText getSpokenText(string text)
        {
            // Here we say it should be a british woman saying our text
            return new TwilioText
            {
                Content = text,
                Language = "en-gb",
                Voice = "woman"
            };
        }


    }
}
