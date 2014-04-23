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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;

namespace PhoneDemoApi.Controllers
{
    public class DebugWriter
    {
        public static void Write(string phoneNr, string message)
        {
            Debug.WriteLine(string.Format("{0}:\t{1}", phoneNr, message));
        }
    }
}
