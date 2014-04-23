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
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PhoneDemoApi.Controllers
{
    public class DemoController : ApiController
    {
        // GET api/demo
        public TwilioResponse Get()
        {
            return new TwilioResponse
            {
                // This is a hardcoded test for twilio
                // Just by configuring twilio number to call this URL, you would hear the below message: 
                // http://yourservicename.cloudapp.net/PhoneDemoApi/api/demo?format=twiml
                SpokenText = new TwilioText { Content = "This demo is going to be so cool.", Language = "en-gb", Voice = "alice" }
            };
        }
    }
}
