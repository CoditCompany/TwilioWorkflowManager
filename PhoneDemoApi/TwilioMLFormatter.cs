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
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;

namespace PhoneDemoApi
{
    public class TwilioMLXmlMediaTypeFormatter : XmlMediaTypeFormatter
    {
        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext)
        {
            try
            {
                var task = Task.Factory.StartNew(() =>
                {
                    var xns = new XmlSerializerNamespaces();
                    var serializer = new XmlSerializer(type);
                    xns.Add(string.Empty, string.Empty);
                    serializer.Serialize(writeStream, value, xns);
                });

                return task;
            }
            catch (Exception)
            {
                return base.WriteToStreamAsync(type, value, writeStream, content, transportContext);
            }
        }
    }
}

